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
    [XmlRoot(ElementName = "Envelope",Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    
    public class Envelope
    {
        public Envelope()
        {

        }

        public Envelope(string opname)
        {
            this.Body = new EnvelopeBody(opname);
        }

        public EnvelopeBody Body
        {
            get;
            set;
        }
    }
}
