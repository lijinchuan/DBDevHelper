using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Biz.Common
{
    public static class XMLHelper
    {
        public static string Serialize(object obj, string file = null)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            XmlSerializer ser = new XmlSerializer(obj.GetType());
            using (StringWriter sw = new StringWriter())
            {
                ser.Serialize(sw, obj);
                string xml = sw.ToString();
                if (!string.IsNullOrWhiteSpace(file))
                {
                    File.WriteAllText(file, xml, Encoding.UTF8);
                }

                return xml;
            }
        }

        public static T DeSerialize<T>(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return default(T);
            }

            XmlSerializer ser = new XmlSerializer(typeof(T));
            using (StringReader sr = new StringReader(s))
            {
                return (T)ser.Deserialize(sr);
            }
        }

        public static T DeSerializeFromFile<T>(string path)
        {
            if (!File.Exists(path))
            {
                return default(T);
            }

            return DeSerialize<T>(File.ReadAllText(path, Encoding.UTF8));
        }
    }
}
