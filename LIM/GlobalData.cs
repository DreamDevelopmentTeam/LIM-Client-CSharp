using System;
using System.Net;
using System.Net.Sockets;


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
        public static UdpClient UdpCore;// = new UdpClient(ListenIP);

    }

}

