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
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IPbox.Text = "127.0.0.1";
            Namebox.Text = "Den";
            string ipClient = IPbox.Text;
            const int portClient = 900;
            string nameClient = Namebox.Text;

            Client cl = new Client();
            ChatBox.Text += cl.Check();
            cl.Connect(ipClient,portClient,nameClient);
        }

        private void button1_Click(object sender, EventArgs e)
        {       
            IPbox.Text = "127.0.0.1";
            Server server = new Server();
            server.StartServer();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Client cl = new Client();
            cl.Send(SendText.Text);
            ChatBox.Text += cl.Check();
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
