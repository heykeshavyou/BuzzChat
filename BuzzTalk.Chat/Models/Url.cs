﻿using System.Text;

namespace BuzzTalk.Chat.Models
{
    public class Url
    {

        private const bool ISLOCALHOST = false;
        private const bool ISLIVE = false;
        public static string liveUrl = "http://heykeshav-001-site1.ntempurl.com/";
        public static string LocalUrl = ISLOCALHOST?  "http://192.168.1.13:45455/": "https://localhost:7059/";

        public static string baseUrl = ISLIVE ? liveUrl : LocalUrl;

 
    }
}
