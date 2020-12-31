using Microsoft.VisualStudio.TestTools.UnitTesting;
using Biz.WCF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace Biz.WCF.Tests
{
    [TestClass()]
    public class WCFClientTests
    {
        [TestMethod()]
        public void GetInterfaceInfosTest()
        {
            Envelope envelope = new Envelope("SearchFreeCourseHoursOneVThree");
            var dic = envelope.Body;
            dic.Add("one", 1);
            dic.Add("two", 2);
            envelope.Body= dic;

            var xml = SerializerToXML(envelope);

            

            var client = WCFClient.CreateClient("http://10.252.254.104:13113/Service.svc");

            var ops = client.GetInterfaceInfos();
        }

        /// <summary>
        /// 序列化保存成文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToSerializer"></param>
        /// <param name="savePath"></param>
        public static string SerializerToXML<T>(T objectToSerializer, string savePath = null, bool catchErr = false)
        {
            try
            {
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add("s", "http://schemas.xmlsoap.org/soap/envelope/");

                XmlSerializer ser = new XmlSerializer(typeof(T));

                StringBuilder sb = new StringBuilder();
                //StringWriter sw = new StringWriter(sb);



                XmlWriterSettings settings = new XmlWriterSettings();
                // Remove the <?xml version="1.0" encoding="utf-8"?> 
                settings.OmitXmlDeclaration = true;
                
                XmlWriter sw = XmlWriter.Create(sb, settings);

                ser.Serialize(sw, objectToSerializer, namespaces);

                return sb.ToString();
            }
            catch
            {
                if (catchErr)
                    throw;
                else
                    return string.Empty;
            }
        }


        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T DeserializerXML<T>(string xml)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));

            StringReader sr = new StringReader(xml);
            return (T)ser.Deserialize(sr);
        }
    }
}