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
            try
            {
                GlobalData.SendMessage(4, "quit".ToUpper(), GlobalData.UserName, GlobalData.BroadcastIP);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
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

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.Control)
                {
                    if (checkBox7.Checked)
                    {
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        button2_Click(null, null);
                        return;
                    }
                }
                if (checkBox8.Checked)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    button2_Click(null, null);
                    return;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string help_data = "LIM Node 节点客户端 (NODE/LIM)(" + Versions.VersionName + " / " + Versions.ProtocalName + ") 客户端帮助文档：\n\n" +

                               "> 清除记录：\n\t清空消息接收区域现有的聊天记录。\n" +
                               "> 查询客户端：\n\t通过此节点向网络发送‘客户端发现协议’报文，如果客户端在线，则会向此节点发送回复报文。\n" +
                               "> 地址黑名单：\n\t使用‘|’符号或‘;’符号（不可同时使用多种符号）分割的IP地址列表。节点会屏蔽从这些IP地址发出的数据报文。\n" +
                               "> 消息接收目标：\n\t如果开启此选项，并且在文本框键入目标IP地址，则数据报文将仅发送给目标IP，否则数据报文将向整个局域网广播。如果开启此选项且输入框为空，数据报文仍然会被广播到整个局域网。\n";

            MessageBox.Show(help_data,
                "LIM Node > Chat > Help",
                MessageBoxButtons.OK,
                MessageBoxIcon.Question
            );
        }
    }
}
