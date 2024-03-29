﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Common
{
    [Serializable]
    public class AuthyApplications
    {
        public AuthyApplication app { get; set; }
    }
    [Serializable]
    public class AuthyApplication
    {
        public string name;
        public string plan;
        public bool sms_enabled;
        public string phone_calls_enabled;
        public int app_id;
        public bool onetouch_enabled;
    }
}
