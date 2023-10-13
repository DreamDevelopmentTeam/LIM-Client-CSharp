using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LIM
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            if (textBox2.Text == null || textBox2.Text.Equals(""))
            {
                MessageBox.Show("无法加入网络：填写的信息为空！", "LIM Node", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                button1.Enabled = true;
                return;
            }

            if (
                textBox2.Text.Trim().Equals("") ||
                GlobalData.SystemUserNames.Contains(textBox2.Text.Trim()) ||
                GlobalData.SystemUserNames.Contains(textBox2.Text.Trim().Replace(" ", "")) ||
                textBox2.Text.Contains("\t") ||
                textBox2.Text.Contains("\n") ||

                GlobalData.UserNameKeyWords.Any(keyword => textBox2.Text.Contains(keyword))
            )
            {
                MessageBox.Show("无法加入网络：用户名称不符合规范或含有敏感词！", "LIM Node", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                button1.Enabled = true;
                return;
            }

            GlobalData.NetWorkID = (int)numericUpDown1.Value;
            GlobalData.UserName = textBox2.Text;
            if (textBox2.Text.StartsWith("*#*#"))
            {
                GlobalData.UserName = GlobalData.UserName.Replace("*#*#", "");
                GlobalData.ShowAdminOptions = true;
            }
            if (textBox2.Text.EndsWith("#*#*"))
            {
                GlobalData.UserName = GlobalData.UserName.Replace("#*#*", "");
                GlobalData.GetAdminMessage = false;
            }

            GlobalData.EnableEncryption = checkBox1.Checked;
            GlobalData.EncryptionPassword = textBox1.Text;


            try
            {
                GlobalData.UdpCore = new UdpClient(GlobalData.ListenIP);
                this.Hide();
                ChatForm chatForm = new ChatForm();
                try
                {
                    GlobalData.SendMessage(4, "join".ToUpper(), GlobalData.UserName, GlobalData.BroadcastIP);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);  
                }
                chatForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法加入网络：尝试加入网络时发生错误！\n\n" + ex.Message, "LIM Node", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                button1.Enabled = true;
                return;
            }
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = checkBox1.Checked;
            textBox1.ReadOnly = !checkBox1.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string help_data = "LIM Node 节点客户端 (NODE/LIM)(" + Versions.VersionName + " / " + Versions.ProtocalName +  ") 客户端帮助文档：\n\n" +
                               
                               "> 网络编号：\n\t用于在同一局域网划分多个聊天网络，不同网络编号之间的消息不互通。\n" +
                               "> 用户名称：\n\t标记用户所使用的用户名。\n" +
                               "> 加密通信：\n\t在局域网中启用加密的通讯，以防止聊天被监听窃密（接收端需要输入同样的密码才能正确解析消息）。\n";
            MessageBox.Show(help_data, 
                "LIM Node > Config > Help",
                MessageBoxButtons.OK,
                MessageBoxIcon.Question
            );


        }
    }
}
