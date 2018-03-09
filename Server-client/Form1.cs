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
        Server server = new Server();
        Client cl = new Client();
        private void button2_Click(object sender, EventArgs e)
        {
            
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
            server.Chat = ChatBox;
            server.StartServer();
            server.sqlList = sqlList;
            server.sqlTextFind = sqlFind;
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
            cl.Send(SendText.Text);
            SendText.Text = "";
            //cl.SendMessage(Sendtext.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ChatBox.Text += Environment.NewLine + "Server - " + server.Check() + " || Client - " + cl.Check();
            //Hello! =)
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //server.sqlList = sqlList;
            //server.sqlUpdate();
            //server.CloseSQL();
            server.sqlFindUser();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            server.sqlUpdate();
            
        }
    }
}
