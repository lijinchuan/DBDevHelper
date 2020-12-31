using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Biz.WCF
{
    [Serializable]
    public class EnvelopeBody<TValue> : Dictionary<string, TValue>, IXmlSerializable
    {
        private string name;
        public EnvelopeBody()
        {
        }
        public EnvelopeBody(string name)
        {
            this.name = name;
        }

        public EnvelopeBody<object> ConvertToBody(IDictionary<string, object> dic)
        {
            EnvelopeBody<object> ret = new EnvelopeBody<object>();
            foreach (var kv in dic)
            {
                ret.Add(kv.Key, kv.Value);
            }

            return ret;
        }

        public void WriteXml(XmlWriter write)       // Serializer
        {
            XmlSerializer ValueSerializer = new XmlSerializer(typeof(TValue), "");

            if (!string.IsNullOrWhiteSpace(name))
            {
                write.WriteStartElement(name, "http://tempuri.org/");
            }
            foreach (KeyValuePair<string, TValue> kv in this)
            {
                write.WriteStartElement(kv.Key);

                if (kv.Value != null)
                {
                    if (kv.Value is IDictionary<string, object>)
                    {
                        ValueSerializer.Serialize(write, ConvertToBody((IDictionary<string, object>)kv.Value));
                    }
                    else
                    {
                        write.WriteValue(kv.Value);
                    }
                }
                else
                {
                    ValueSerializer.Serialize(write, kv.Value);
                }

                write.WriteEndElement();
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                write.WriteEndElement();
            }
        }
        public void ReadXml(XmlReader reader)       // Deserializer
        {
            reader.Read();
            XmlSerializer ValueSerializer = new XmlSerializer(typeof(TValue));

            name = reader.Name;

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement(name);
                string tk = name;
                TValue vl = (TValue)ValueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(tk, vl);
                reader.MoveToContent();
            }

        }
        public XmlSchema GetSchema()
        {
            return null;
        }
    }
}
