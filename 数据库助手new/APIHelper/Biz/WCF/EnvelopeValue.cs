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
    public struct EnvelopeValue:IXmlSerializable
    {
        object value
        {
            get;
            set;
        }

        public EnvelopeValue(object val)
        {
            this.value = val;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            
        }

        public void WriteXml(XmlWriter writer)
        {
            if (value == null)
            {
                writer.WriteValue("");
            }
            else if (value.GetType() == typeof(string)
                || value.GetType() == typeof(int)
                || value.GetType() == typeof(uint)
                || value.GetType() == typeof(short)
                || value.GetType() == typeof(ushort)
                || value.GetType() == typeof(DateTime)
                || value.GetType() == typeof(decimal)
                || value.GetType() == typeof(double)
                || value.GetType() == typeof(Guid)
                || value.GetType() == typeof(bool))
            {
                writer.WriteValue(value);
            }
            else
            {
                XmlSerializer ValueSerializer = new XmlSerializer(value.GetType(), "");
                ValueSerializer.Serialize(writer, value);
            }
        }

        public static implicit operator EnvelopeValue(int val)
        {
            return new EnvelopeValue(val);
        }
    }
}
