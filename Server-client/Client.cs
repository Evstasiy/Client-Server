using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Server_client
{
    public class Client
    {
        public TextBox ChatCl{ get; set; }

        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static MemoryStream ms = new MemoryStream(new byte[1024], 0, 256, true, true);
        static BinaryWriter write = new BinaryWriter(ms);
        static BinaryReader read = new BinaryReader(ms);

        bool connect;

        enum PacketData {
            ID,
            Pass,
            Message,
            Command
        }

        static string textBox;//test
        public string Check() { return textBox; }//Test

        
        static string name;
        int id;
        bool pass = true;

        public void Connect(string IP, int port, string nameV)
        {
            try
            {
                connect = true;
                name = nameV;
                socket.Connect(IP, port);
                SendPacket(PacketData.ID);
                ReceivePacket();
                
                Task.Run(() => { while (connect) ReceivePacket(); });
            }
            catch (Exception)
            {
                ChatCl.Text += Environment.NewLine + "Connection Failed!";
            }
        }

        public void Send(string text)
        {
            textBox = text;
            //textBox = Environment.NewLine + name + ":" + textBox;
            if (textBox.Substring(0, 1) == "/") SendPacket(PacketData.Command);
            else
                if (!pass) SendPacket(PacketData.Message);
            else SendPacket(PacketData.Pass);
            
        }

        void SendPacket(PacketData data)
        {
            ms.Position = 0;
            switch (data)
            {
                case PacketData.ID:
                    write.Write(0);
                    write.Write(name);
                    //MessageBox.Show("name - " + name);
                    socket.Send(ms.GetBuffer());
                    break;
                case PacketData.Pass:
                    write.Write(1);
                    write.Write(textBox);
                    socket.Send(ms.GetBuffer());
                    //pass = false;
                    break;
                case PacketData.Message:
                    write.Write(2);
                    write.Write(textBox);
                    socket.Send(ms.GetBuffer());    
                    break;
                case PacketData.Command:
                    write.Write(3);
                    switch (textBox.Split()[0])
                    {
                        case "/userPass_Replace":
                            write.Write(textBox.Split()[0]);
                            write.Write(textBox.Split()[1]);
                            socket.Send(ms.GetBuffer());
                            break;
                    }
                    break;
            }
        }

        private void ReceivePacket()
        {
            try { socket.Receive(ms.GetBuffer()); } catch { }
            ms.Position = 0;
            int code = read.ReadInt32();
            //ChatCl.Text += Environment.NewLine + "Code : " + code;
            switch (code)
            {
                case 0:
                    if (id == 0)
                    {
                        id = read.ReadInt32();
                        ChatCl.Text += Environment.NewLine + "Youre ID :" + id;
                    }
                    break;
                case 1:
                    pass = read.ReadBoolean();
                    ChatCl.Text += Environment.NewLine + read.ReadString();
                    break;
                case 2:
                    string name = read.ReadString();
                    string message = read.ReadString();
                    ChatCl.Text += Environment.NewLine + name +" : " + message;
                    //MessageBox.Show("name - " + name);
                    break;
                case 3:
                    ChatCl.Text += Environment.NewLine + "New connect : " + read.ReadString();
                    break;
                case 4:
                    ChatCl.Text += Environment.NewLine + "Disconnect : " + read.ReadString();
                    break;
                case 5:
                    ChatCl.Text += Environment.NewLine + "Error " + read.ReadString() + Environment.NewLine + "Try connect later.";
                    Disconnect();
                    break;
                case 6:
                    ChatCl.Text += Environment.NewLine + read.ReadString();
                    break;

            }
        }

        public void Disconnect()
        {
            connect = false;
            /*socket.Shutdown(SocketShutdown.Both);
            socket.Disconnect(true);*/
            socket.Close();
            //MessageBox.Show("Connect: " + socket.Connected);
        }

    }
}
