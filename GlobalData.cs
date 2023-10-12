using System;


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

        public static int NetWorkID = 0;
        public static string UserName = "";

        
        public static UdpClient UdpCore = new UdpClient();


    }

}

