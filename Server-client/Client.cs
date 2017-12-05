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
        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static MemoryStream ms = new MemoryStream(new byte[1024], 0, 256, true, true);
        static BinaryWriter write = new BinaryWriter(ms);
        static BinaryReader read = new BinaryReader(ms);

        enum PacketData { ID, Text };

        static string textBox;//test
        public string Check() { return textBox; }//Test

        static Form1 form = new Form1();
        static string name;
        static string id;

        public void Connect(string IP, int port, string nameV)
        {

            try
            {
                name = nameV;
                socket.Connect(IP, port);
                SendPacket(PacketData.ID);
                id = Convert.ToString(ReceivePacket());
                textBox = "Youre ID:"+id;
                Task.Run(() => { while (true) ReceivePacket(); });
            }
            catch (Exception)
            {
                textBox = "Connection Failed!";
            }

        }

        public void Send(string text)
        {
            textBox = text;
            form.ChatBox.Text = "ksk";
            SendPacket(PacketData.Text);
            //textBox = Environment.NewLine + name + ":" + textBox;
        }

        static void SendPacket(PacketData data)
        {
            ms.Position = 0;
            switch (data)
            {
                case PacketData.ID:
                    write.Write(0);
                    socket.Send(ms.GetBuffer());
                    break;
                case PacketData.Text:
                    write.Write(1);
                    write.Write(name);
                    write.Write(textBox);
                    socket.Send(ms.GetBuffer());    
                    break;
            }
        }

        static int ReceivePacket()
        {
            
            ms.Position = 0;

            socket.Receive(ms.GetBuffer());

            int s = read.ReadInt32();

            switch(s)
            {
                case 0: return read.ReadInt32();
                case 1:
                    textBox = read.ReadString() + read.ReadString();
                    break;

            }
            return -1;
        }

    }
}
