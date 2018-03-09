using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using System.Data;
using System.Data.SqlClient;

namespace Server_client
{
    public class Server
    {
        class Clients
        {
            public Socket Socket { get; set; } //Сокет клиента
            public int ID { get; set; }        //ID, задается рандомом
            public string Name { get; set; }   //Имя клиента
            public string Pass { get; set; }   //Пароль клента
            public string Message { get; set; }//Сообщение
            public Clients(Socket socket) { Socket = socket; } //Присвоение сокета классу методом
        }
        private string errorCode { get; set; }  //Задается код ошибки и передается в метод ErrorSend()
        public TextBox Chat { get; set; }       //Получает полный доступ к TextBox из Form1

        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //Сокет сервера
        static List<Clients> clientList = new List<Clients>(); //Лист с данными о клиенте

        enum Packet     //Данные о возможных пакетах
        {
            ID,         //0
            Pass,       //1
            Messange,   //2
            Connect,    //3
            Disconnect, //4
            Error,      //5
            Command     //6
        }

        static Random rand = new Random(); //Функция рандома 

        //База данных
        SqlConnection sqlConnect;
        public ListBox sqlList { get; set; }
        public TextBox sqlTextFind { get; set; }
        //База данных
       
        static string code = "null ";//Test
        public string Check() { return code; }//Нужна исключительно для дебага

        public void CloseSQL()
        {/*
            if ((sqlConnect != null) && (sqlConnect.State != ConnectionState.Closed))
                sqlConnect.Close();*/
            //Chat.Text += sqlConnect.Database.ToString();
        }
        private void sqlNewUser(int id, string Name, string Pass)  //Вносит в базу данных нового пользователя
        {
            SqlCommand command = new SqlCommand("INSERT INTO [Table] (Id,Name,Pass)VALUES(@Id,@Name,@Pass)", sqlConnect);  //Отправляет команду для создания элементов в таблице
            command.Parameters.AddWithValue("Id", id);   
            command.Parameters.AddWithValue("Name", Name);
            command.Parameters.AddWithValue("Pass", Pass);
            command.ExecuteNonQuery();
        }
        //Конец метода sqlNewUser
        public async void sqlUpdate() //Метод получения данных из таблицы 
        {
            sqlList.Items.Clear();
            sqlList.Items.Add("ID" + "  " + "Name" + "  " + "Password");
            SqlDataReader sqlRead = null;
            SqlCommand command = new SqlCommand("SELECT * FROM [Table]", sqlConnect);
            try
            {
                sqlRead = await command.ExecuteReaderAsync();
                while (await sqlRead.ReadAsync())
                {
                    try { sqlList.Items.Add(sqlRead["Id"] + " " + sqlRead["Name"] + " " + sqlRead["Pass"]); } catch { }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlRead != null)
                    sqlRead.Close();
            }
        }
        //Конец метода sqlUpdate
        public async void sqlFindUser() //Метод получения данных из таблицы с конкретным запросом по поиску пользователя 
        {
            string findString = sqlTextFind.Text; 
            if (findString != "")
            {
                sqlList.Items.Clear();
                sqlList.Items.Add("ID" + "  " + "Name" + "  " + "Password");
                SqlDataReader sqlRead = null;
                SqlCommand command = new SqlCommand("SELECT * FROM [Table] WHERE [Name]='" + findString + "'", sqlConnect);
                try
                {
                    sqlRead = await command.ExecuteReaderAsync();
                    while (await sqlRead.ReadAsync())
                    {
                        sqlList.Items.Add(sqlRead["Id"] + " " + sqlRead["Name"] +" "+ sqlRead["Pass"]);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (sqlRead != null)
                        sqlRead.Close();
                }
            }
            else  //Если поиск пустой
            {
                MessageBox.Show("Text is null", "Finder", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        //Конец метода sqlFindUser
        private bool sqlIdentificationUser(bool way, Clients client) //Метод для проверки пароля пользователя
        {
            if (way) 
            {
                SqlDataReader read = null;
                SqlCommand command = new SqlCommand("SELECT [Name] FROM [Table] WHERE[Name] = '" + client.Name + "'", sqlConnect);
                read = command.ExecuteReader();
                bool findUser = false;
                while (read.Read())
                {
                    if (read["Name"] != null) findUser = true;
                }
                read.Close();
                return findUser;
            }
            else 
            {
                SqlDataReader read = null;
                SqlCommand command = new SqlCommand("SELECT [Pass] FROM [Table] WHERE[Name] ='" + client.Name + "'", sqlConnect);
                read = command.ExecuteReader();
                while (read.Read())
                {
                    if (read["Pass"].ToString() == client.Pass)
                    {
                        read.Close();
                        return false;
                    }
                    else { read.Close(); return true; }
                    }
            }
            
            return false;
        }
        //Конец метода sqlIdentificationUser
        private void sqlReplacePass(string Name, string Pass) //Метод изменения пароля в таблице 

        {
            SqlDataReader sqlRead = null;
            SqlCommand command = new SqlCommand("SELECT [Id] FROM [Table] WHERE[Name]='" + Name + "'", sqlConnect);
            int id = 0;
            sqlRead = command.ExecuteReader();
            while (sqlRead.Read())
            {
                id = Convert.ToInt32(sqlRead["Id"].ToString());
            }
            sqlRead.Close();
            SqlCommand commandDel = new SqlCommand("DELETE FROM [Table] WHERE[Name]='" + Name + "'", sqlConnect);
            commandDel.ExecuteNonQuery();
            sqlNewUser(id, Name, Pass);
        }
        //Конец метода sqlReplacePass
        public async void DataConnect() //Метод подключения к базе данных 

        {
            string connect = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\User\source\repos\Server-client\Server-client\dataBase.mdf;Integrated Security=True";
            sqlConnect = new SqlConnection(connect);

            await sqlConnect.OpenAsync();
        }
        //Конец метода DataConnect
        public void StartServer() //Функция старта сервера
        {
            DataConnect();
            socket.Bind(new IPEndPoint(IPAddress.Any, 900));//Связь сервера с конечной точкой
            socket.Listen(0); //Прослушивание входящих соединений
            socket.BeginAccept(AcceptCallback, null); //Асинхронный метод для начала работы с клиентом *идет вызов AcceptCallback()
        }
        //Конец функции StartServer

        private void AcceptCallback(IAsyncResult ar)//Функция для асинхронной работы с клиентом
        {
            Clients clients = new Clients(socket.EndAccept(ar)); //Передаем сокет клиента в метод Clients() в одноименном классе и закрываем подключение методом EndAccept()
            Thread thread = new Thread(BrainClient); //Создаем поток для клиента *в качестве параметра запускаем метод BrainClient,который будет работать в этом потоке  
            thread.Start(clients); //Запускаем поток *передаем в качесве аргумента сокет клиента
            thread.Name = "ClientThread";

            socket.BeginAccept(AcceptCallback, null); //Запускаем метод для принятия подключений заново 
            clientList.Add(clients); //Добавляем клиента в конец списка
        }
        //Конец функции AcceptCallback

        private void BrainClient(object ob)// Функция для основной работы с клиентом 
        {
            Clients client = (Clients)ob; //Получаем сокет клиента
            MemoryStream ms = new MemoryStream(new byte[1024], 0, 256, true, true); //Создаем поток для работы с памятью
            BinaryReader read = new BinaryReader(ms); //Чтение с потока 
            BinaryWriter write = new BinaryWriter(ms);//Запись в поток 

            while (true) //Запускаем бесконечный цикл для обработки данных 
            {
                ms.Position = 0; //Задаем начальное положение в потоке для корректной записи и чтения данных

                try{ //Обработка исключения
                    client.Socket.Receive(ms.GetBuffer()); //Получаем данные от клиента
                }  
                catch
                {
                    ClientDisconnect(client); //Запускаем метод для отключения клиента
                    return;
                }
                int dataCode = read.ReadInt32(); //Получаем код пакета
                code = dataCode.ToString(); //Задаем значение тестовой переменной
                //Chat.Text += Environment.NewLine + "Code " + code;
                switch (dataCode) {
                    case 0://Пользователь подключился
                        ReceivePacket(dataCode,client,ms);  //Метод получения данных 
                        SendPacket(Packet.ID, client);
                        SendPacket(Packet.Connect, client); //Метод отправки данных
                        break;
                    case 1://Проверка пароля пользователя
                        ReceivePacket(dataCode, client, ms);
                        break;
                    case 2://Пользователь отправил сообщение
                        ReceivePacket(dataCode, client, ms);
                        SendPacket(Packet.Messange, client);
                        break;
                    case 3:
                        ReceivePacket(dataCode, client, ms);
                        SendPacket(Packet.Command, client);
                        break;
                    default:
                        code = "Error";
                        break;
                } //Конец Switch

            }  //Конец цикла while
        }
        //Конец функции BrainClient

        private void SendPacket(Packet packet, Clients client) //Функция отправки данных 
        {
            MemoryStream ms = new MemoryStream(new byte[1024], 0, 256, true, true);
            BinaryWriter write = new BinaryWriter(ms);
            ms.Position = 0;
            switch (packet)
            {
                case Packet.ID:
                    bool userConnect = true;
                    foreach (var c in clientList)
                    {
                       // Chat.Text += Environment.NewLine + "Name : " + c.Name;
                        try { Chat.Text += Environment.NewLine + "Name: " + c.Name; } catch { }
                        if (c != client) //Проверка на совпадение имен
                            if (c.Name == client.Name)
                            {
                                ErrorSend(client, "404"); //Отправляем ошибку 404 клиенту
                                userConnect = false;
                                break;
                            }
                    }
                    if(userConnect)
                    if (!sqlIdentificationUser(true, client))
                    {
                        while (true)
                        {
                            int id = rand.Next(0, 99); //Создаем id 
                            try { Chat.Text += Environment.NewLine + "ID : " + id; } catch { } //Вывод на стороне сервера
                            if (clientList.Find(cl => cl.ID == id) == null) //Проверка на схожесть id
                            {
                                client.ID = id;
                                write.Write(0);
                                write.Write(id);
                                break;
                            }//Конец проверки id 
                        }//Конец цикла while
                        client.Socket.Send(ms.GetBuffer());
                        sqlNewUser(client.ID, client.Name.ToString(), "321");
                    }
                    else
                    {
                        write.Write(1);
                        write.Write(true);
                        write.Write("Please,write youre pass");
                        client.Socket.Send(ms.GetBuffer());
                    }
                    //Chat.Text += Environment.NewLine + "New connect : " + "Name -" + client.Name + " ID - " + client.ID + " IP: " + ((IPEndPoint)client.Socket.RemoteEndPoint).Address.ToString(); 
                    
                    break;

                case Packet.Pass:
                    ms.Position = 0;
                    write.Write(1);
                    write.Write(false);
                    write.Write("Successful entry!");
                    client.Socket.Send(ms.GetBuffer());
                    break;

                case Packet.Messange: 
                    foreach (var c in clientList) //Перечислением отсылаем клиентам входящее сообщение
                    {
                        if (c != client)
                        {
                            write.Write(2);
                            write.Write(client.Name);
                            write.Write(client.Message);
                            c.Socket.Send(ms.GetBuffer());
                        }
                        client.Socket.Send(ms.GetBuffer());
                    }
                    break;

                case Packet.Connect:
                     foreach (var c in clientList) //Перечислением сообщяем о подключении клиента
                     {
                        if (c != client)
                         {
                             write.Write(3);
                             write.Write(client.Name);
                             c.Socket.Send(ms.GetBuffer());
                         }
                     }
                    break;

                case Packet.Disconnect:
                    foreach (var c in clientList)//Перечислением сообщяем об отключении клиента
                    {
                        if (c != client)
                        {
                            write.Write(4);
                            write.Write(client.Name);
                            c.Socket.Send(ms.GetBuffer());
                        }
                    }
                    break;

                case Packet.Error:
                    write.Write(5);
                    write.Write(errorCode);
                    client.Socket.Send(ms.GetBuffer());
                    ClientDisconnect(client);
                    break;

                case Packet.Command: //В разработке
                    write.Write(6);
                    write.Write("Your pass is replace!");
                    client.Socket.Send(ms.GetBuffer());
                    break;

            }//Switch
        }//void SendPacket

        private void ReceivePacket(int code,Clients client,MemoryStream ms) //Метод принятия данных
        {
            //ms.Position = 0;
            BinaryReader read = new BinaryReader(ms);
            //Chat.Text += Environment.NewLine + "Code " + code;
            switch (code)
            {
                case 0:
                    client.Name = read.ReadString();
                    break;
                case 1:
                    client.Pass = read.ReadString();
                    if (!sqlIdentificationUser(false, client))
                    {
                        Chat.Text += Environment.NewLine + "New connect : " + "Name -" + client.Name + " ID - " + client.ID + " IP: " + ((IPEndPoint)client.Socket.RemoteEndPoint).Address.ToString();
                        SendPacket(Packet.Pass, client);
                    }
                    else
                    {
                        Chat.Text += Environment.NewLine + "Login false : " + "Name : " + client.Name + " Pass: " + client.Pass;
                    }
                    break;
                case 2:
                    client.Message = read.ReadString();
                    break;
                case 3:
                    string command = read.ReadString();
                    switch (command)
                    {
                        case "/userPass_Replace":
                            sqlReplacePass(client.Name, read.ReadString());
                            break;
                    }
                    break;
            }//Конец switch

        }
        //Конец метода ReceivePacket

        private void ErrorSend(Clients client,string code) //Метод отправки ошибки
        {
            errorCode = code; //Передаем код ошибки
            SendPacket(Packet.Error, client); 
        }
        //Конец метода ErrorSend

        private void ClientDisconnect(Clients client) //Метод отключения клиента
        {
            try
            {
                client.Socket.Shutdown(SocketShutdown.Both); //Блокировка получения данных от клиента
                client.Socket.Disconnect(true); //Закрывает подключение клиента
                SendPacket(Packet.Disconnect, client); 
                clientList.Remove(client); //Удаляет клиента из списка 
                Chat.Text += Environment.NewLine + "Disconnect : " + "Name -" + client.Name + " id: " + client.ID + " IP: " + ((IPEndPoint)client.Socket.RemoteEndPoint).Address.ToString();
            } catch { }
        }
        //Конец метода ClientDisconnect

    }//Конец класса Server
}//Конец пространства имен Server-client
