﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EdgyCore.Credientals
{
    public class Credientals
    {
        public string token { get; set; }
        public string dblToken { get; set; }
        public string GJP { get; set; }

        public string prefix { get; set; }
        public ulong clientID { get; set; }
        public string invLink { get; set; }
        public string accID { get; set; }
        public ulong ownerID { get; set; }
    }
}