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
            Message
        }

        static string textBox;//test
        public string Check() { return textBox; }//Test

        
        static string name;
        int id;

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
            SendPacket(PacketData.Message);
        }

        static void SendPacket(PacketData data)
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
                case PacketData.Message:
                    write.Write(1);
                    write.Write(textBox);
                    socket.Send(ms.GetBuffer());    
                    break;
            }
        }

        private void ReceivePacket()
        {
            ms.Position = 0;
            try { socket.Receive(ms.GetBuffer()); } catch { }

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
                    string name = read.ReadString();
                    string message = read.ReadString();
                    ChatCl.Text += Environment.NewLine + name +" : " + message;
                    //MessageBox.Show("name - " + name);
                    break;
                case 2:
                    ChatCl.Text += Environment.NewLine + "New connect : " + read.ReadString();
                    break;
                case 3:
                    ChatCl.Text += Environment.NewLine + "Disconnect : " + read.ReadString();
                    break;
                case 4:
                    ChatCl.Text += Environment.NewLine + "Error " + read.ReadString();
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
