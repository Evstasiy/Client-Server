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
    public class Server : Form1
    {
        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static List<Socket> clients = new List<Socket>();

        static Random rand = new Random();


        
        static string code="null ";//Test
        public string Check() { return code; }//Test
        public void StartServer()
        {
            socket.Bind(new IPEndPoint(IPAddress.Any, 900));
            socket.Listen(0);
            socket.BeginAccept(AcceptCallback, null);
        }

        static void AcceptCallback(IAsyncResult ar)
        {
            Socket client = socket.EndAccept(ar);
            Thread thread = new Thread(BrainClient);
            thread.Start(client);

            socket.BeginAccept(AcceptCallback, null);
            clients.Add(client);
        }

        static void BrainClient(object ob)
        {
            Socket client = (Socket)ob;
            MemoryStream ms = new MemoryStream(new byte[1024], 0, 256, true, true);
            BinaryReader read = new BinaryReader(ms);
            BinaryWriter write = new BinaryWriter(ms);
            
            while (true)
            {
                ms.Position = 0;
                
                try
                {
                    client.Receive(ms.GetBuffer());
                }
                catch
                {
                    client.Shutdown(SocketShutdown.Both);
                    client.Disconnect(true);
                    clients.Remove(client);
                    return;
                }
                int dataCode = read.ReadInt32();
                code = dataCode.ToString();
                switch (dataCode) {
                    case 0:
                        int id = rand.Next(1, 100);
                        //code = "Sending! " + id + "   ";
                        write.Write(id);
                        client.Send(ms.GetBuffer());
                        break;
                    case 1:
                        foreach (var c in clients)
                        {
                            //if (c != client)
                            //{
                                string name = read.ReadString();
                                string message = read.ReadString();
                                code = "1 " + name + ":" + message;
                                ms.Position = 0;
                               
                                write.Write(code);
                                c.Send(ms.GetBuffer());
                            //}
                        }
                        break;
                    default:
                        code = "!!!!!WARNING!!!!!";
                        break;
                }   
                
            }
        }

        static void SendPacket()
        {

        }


    }
}
