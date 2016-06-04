using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Biz.Common
{
    public static class XMLHelper
    {
        public static string Serialize(object obj,string file=null)
        {
            if (obj == null)
                return string.Empty;
            XmlSerializer ser = new XmlSerializer(obj.GetType());
            byte[] buffer;
            using (MemoryStream ms = new MemoryStream())
            {
                ser.Serialize(ms, obj);
                buffer = ms.GetBuffer();
            }
            string s = Encoding.UTF8.GetString(buffer);
            if (!string.IsNullOrWhiteSpace(file))
            {
                File.WriteAllText(file, s, Encoding.UTF8);
            }
            return s;
        }

        public static T DeSerialize<T>(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return default(T);
            XmlSerializer ser = new XmlSerializer(typeof(T));
            byte[] buffer=Encoding.UTF8.GetBytes(s);
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                return (T)ser.Deserialize(ms);
            }
        }

        public static T DeSerializeFromFile<T>(string path)
        {
            if (!File.Exists(path))
                return default(T);
            string s = File.ReadAllText(path, Encoding.UTF8);
            return DeSerialize<T>(s);
        }
    }
}
