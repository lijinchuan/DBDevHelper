using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Biz.WCF
{
    [Serializable]
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelopes
    {
        public Envelopes()
        {

        }

        public Envelopes(string opname)
        {
            this.Body.Add(new EnvelopeBody(opname));

        }

        public void Add(string opname)
        {
            this.Body.Add(new EnvelopeBody(opname));
        }

        public List<EnvelopeBody> Body
        {
            get;
            set;
        } = new List<EnvelopeBody>();
    }
}
