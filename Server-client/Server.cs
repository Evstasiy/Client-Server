using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Server_client
{
    public class Server
    {
        class Clients
        {
            public Socket Socket { get; set; } //Сокет клиента
            public int ID { get; set; }        //ID, задается рандомом
            public string Name { get; set; }   //Имя клиента
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
            Messange,   //1
            Connect,    //2
            Disconnect, //3
            Error       //4
        }

        static Random rand = new Random(); //Функция рандома 
        
       
        static string code = "null ";//Test
        public string Check() { return code; }//Нужна исключительно для дебага
        public void StartServer() //Функция старта сервера
        {
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
                switch (dataCode) {
                    case 0://Пользователь подключился
                        ReceivePacket(dataCode,client,ms);  //Метод получения данных 
                        SendPacket(Packet.Connect, client); //Метод отправки данных
                        SendPacket(Packet.ID, client);
                        break;
                    case 1://Пользователь отправил сообщение
                        ReceivePacket(dataCode, client, ms);
                        SendPacket(Packet.Messange, client);
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
            switch (packet)
            {
                case Packet.ID: 
                    while (true)
                    {
                        foreach (var c in clientList) 
                        {
                            //try { Chat.Text += Environment.NewLine + "Name: " + c.Name; } catch { }
                            if (c != client) //Проверка на совпадение имен
                                if (c.Name == client.Name)
                                {
                                    ErrorSend(client, "404"); //Отправляем ошибку 404 клиенту
                                    break;
                                }

                        }
                        int id = rand.Next(0, 101); //Создаем id 
                        try { Chat.Text += Environment.NewLine + "ID : " + id; } catch { } //Вывод на стороне сервера
                        if (clientList.Find(cl => cl.ID == id) == null) //Проверка на схожесть id
                        {
                            client.ID = id;
                            write.Write(0);
                            write.Write(id);
                            break;
                        }//Конец проверки id 
                    }//Конец цикла while
                    try
                    {
                    client.Socket.Send(ms.GetBuffer()); 
                    Chat.Text += Environment.NewLine + "New connect : " + "Name -" + client.Name + " ID - " + client.ID + " IP: " + ((IPEndPoint)client.Socket.RemoteEndPoint).Address.ToString(); } catch { }
                    break;

                case Packet.Messange: 
                    foreach (var c in clientList) //Перечислением отсылаем клиентам входящее сообщение
                    {
                        if (c != client)
                        {
                            write.Write(1);
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
                            write.Write(2);
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
                            write.Write(3);
                            write.Write(client.Name);
                            c.Socket.Send(ms.GetBuffer());
                        }
                    }
                    break;

                case Packet.Error:
                    write.Write(4);
                    write.Write(errorCode);
                    client.Socket.Send(ms.GetBuffer());
                    ClientDisconnect(client);
                    break;

            }//Switch
        }//void SendPacket

        private void ReceivePacket(int code,Clients client,MemoryStream ms) //Метод принятия данных
        {
            BinaryReader read = new BinaryReader(ms);
            //Chat.Text += Environment.NewLine + "Code - " + code;
            switch (code)
            {
                case 0:
                    client.Name = read.ReadString();
                    break;

                case 1:
                    client.Message = read.ReadString();
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
