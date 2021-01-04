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
    public class EnvelopeBody : Dictionary<string, EnvelopeValue>, IXmlSerializable
    {
        private string name;
        public EnvelopeBody()
        {
        }
        public EnvelopeBody(string name)
        {
            this.name = name;
        }

        public void WriteXml(XmlWriter writer)       // Serializer
        {
            XmlSerializer ValueSerializer = new XmlSerializer(typeof(EnvelopeValue), "");
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            if (!string.IsNullOrWhiteSpace(name))
            {
                writer.WriteStartElement(name, "http://tempuri.org/");
            }
            foreach (KeyValuePair<string, EnvelopeValue> kv in this)
            {
                writer.WriteStartElement(kv.Key);

                kv.Value.WriteXml(writer);

                writer.WriteEndElement();
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                writer.WriteEndElement();
            }
        }
        public void ReadXml(XmlReader reader)       // Deserializer
        {

        }
        public XmlSchema GetSchema()
        {
            return null;
        }
    }
}
