using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Server_client
{
    public partial class Form1 : Form
    {
        string text;
        bool connect = false;
        public Form1()
        {
            InitializeComponent();
            button3.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Client cl = new Client();
            if (!connect)
            {
                IPbox.Text = "127.0.0.1";
                Namebox.Text = Namebox.Text;
                string ipClient = IPbox.Text;
                const int portClient = 900;
                string nameClient = Namebox.Text;
                
                cl.ChatCl = ChatBox;
                ChatBox.Text += Environment.NewLine + "Connecting...";
                cl.Connect(ipClient, portClient, nameClient);

                button1.Enabled = false;
                IPbox.Enabled = false;
                Namebox.Enabled = false;
                button2.Text = "Disconnect";
                connect = true;
                button3.Enabled = true;
            }
            else
            {
                button1.Enabled = true;
                IPbox.Enabled = true;
                Namebox.Enabled = true;

                cl.Disconnect();
                button2.Text = "Client";
                connect = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {       
            IPbox.Text = "127.0.0.1";
            Server server = new Server();
            server.Chat = ChatBox;
            server.StartServer();
            button1.Enabled = false;
            button2.Enabled = false;
            IPbox.Enabled = false;
            Namebox.Enabled = false;
            button4.Visible = true;
            SendText.Enabled = false;
            button3.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Client cl = new Client();
            cl.Send(SendText.Text);
            SendText.Text = "";
            //cl.SendMessage(Sendtext.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Server server = new Server();
            Client cl = new Client();
            ChatBox.Text += Environment.NewLine + "Server - " + server.Check() + " ||Client - " + cl.Check();
            //Hello! =)
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Client cl = new Client();
            Server server = new Server();
            textBox1.Text += Environment.NewLine + "Server - " + server.Check() + "  ||  Client - " + cl.Check();
        }
    }
}
