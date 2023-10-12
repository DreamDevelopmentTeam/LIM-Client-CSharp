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
                textBox2.Text.Contains("\n") 
            )
            {
                MessageBox.Show("无法加入网络：用户名称不符合规范！", "LIM Node", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                button1.Enabled = true;
                return;
            }

            GlobalData.NetWorkID = (int)numericUpDown1.Value;
            GlobalData.UserName = textBox2.Text;
            if (textBox2.Text.StartsWith("*#*#"))
            {
                GlobalData.UserName = textBox2.Text.Replace("*#*#", "");
                GlobalData.ShowAdminOptions = true;
            }
            if (textBox2.Text.EndsWith("#*#*"))
            {
                GlobalData.UserName = textBox2.Text.Replace("#*#*", "");
                GlobalData.GetAdminMessage = false;
            }


            try
            {
                GlobalData.UdpCore = new UdpClient(GlobalData.ListenIP);

            }
            catch (Exception ex)
            {
                MessageBox.Show("无法加入网络：尝试加入网络时发生错误！\n" + ex.Message, "LIM Node", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                button1.Enabled = true;
                return;
            }




        }
    }
}
