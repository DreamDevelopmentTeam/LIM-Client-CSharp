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
            button4.Enabled = false;
            checkBox2.Enabled = false;
            checkBox3.Enabled = false;
            checkBox4.Enabled = false;
            checkBox5.Enabled = false; 
            button4.Hide();
            checkBox2.Hide();
            checkBox3.Hide();
            checkBox4.Hide();
            checkBox5.Hide();
            if (GlobalData.GetAdminMessage == false)
            {
                checkBox4.Enabled = true;
                checkBox5.Enabled = true;
                checkBox4.Show();
                checkBox5.Show();
            }
            if (GlobalData.ShowAdminOptions == true)
            {
                button4.Enabled = true;
                checkBox2.Enabled = true;
                checkBox3.Enabled = true;
                checkBox4.Enabled = true;
                checkBox5.Enabled = true;
                button4.Show();
                checkBox2.Show();
                checkBox3.Show();
                checkBox4.Show();
                checkBox5.Show();
            }

            Thread thread = new Thread(new ThreadStart(() =>
            {
                GlobalData.MessagesGetter(listBox1, checkBox2, checkBox4, checkBox5, textBox3);
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
                if (!checkBox3.Checked)
                {
                    GlobalData.SendMessage(0, GlobalData.UserName, textBox1.Text, ip);
                }
                else
                {
                    GlobalData.SendMessage(2, "run".ToUpper(), " /c " + textBox1.Text, ip);
                }

                if (!checkBox6.Checked)
                {
                    textBox1.Text = "";
                }
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法发送消息：尝试发送消息时发生错误！\n\n" + ex.Message, "LIM Node", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //GlobalData.SendMessage(1, "find".ToUpper(), GlobalData.UserName, GlobalData.BroadcastIP);
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
                GlobalData.SendMessage(1, "find".ToUpper(), GlobalData.UserName, ip);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法发送消息：尝试发送消息时发生错误！\n\n" + ex.Message, "LIM Node", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            textBox2.Enabled = checkBox1.Checked;
            textBox2.ReadOnly = !checkBox1.Checked;

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
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
                GlobalData.SendMessage(2, "exit".ToUpper(), "", ip);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法发送消息：尝试发送消息时发生错误！\n\n" + ex.Message, "LIM Node", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }
        }
    }
}
