using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace LIM
{
    public partial class ChatForm : Form
    {
        public ChatForm()
        {
            InitializeComponent();
        }

        private void contextMenuStrip1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private async void ChatForm_Load(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                GlobalData.MessagesGetter(listBox1, checkBox2);
            }));
            thread.Start(); 


        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                int index = listBox1.IndexFromPoint(e.Location);
                if (index >= 0)
                {
                    listBox1.SelectedIndex = index;
                    this.contextMenuStrip1.Show(this.listBox1, e.Location);
                }
            }
        }

        private void ChatForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalData.UdpCore.Close();
            Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void 清除记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null || textBox1.Text.Trim().Equals(""))
            {
                return;
            }
            try
            {
                IPEndPoint ip = GlobalData.BroadcastIP;
                if (checkBox1.Checked)
                {
                    string str = "255.255.255.255";
                    if (textBox2.Text == null || textBox2.Text.Trim().Equals(""))
                    {
                        str = "255.255.255.255";
                    }
                    else
                    {
                        str = textBox2.Text;
                    }

                    ip = new IPEndPoint(IPAddress.Parse(str), GlobalData.ProtocalPort);
                }
                GlobalData.SendMessage(0, GlobalData.UserName, textBox1.Text, ip);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法发送消息：尝试发送消息时发生错误！\n" + ex.Message, "LIM Node", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GlobalData.SendMessage(1, "find", GlobalData.UserName, GlobalData.BroadcastIP);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            textBox2.Enabled = checkBox1.Checked;
            textBox2.ReadOnly = !checkBox1.Checked;

        }
    }
}
