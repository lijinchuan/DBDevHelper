using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class RespCookie
    {
        public string Domain { get; set; }
        public DateTime Expires { get; set; }
        public bool HasKeys { get; set; }
        public bool HttpOnly { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool Secure { get; set; }
        public string Value { get; set; }
    }
}
