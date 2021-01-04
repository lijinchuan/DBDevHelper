using Entity.WCF;
using LJC.FrameWorkV3.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Biz.WCF
{
    public class WCFClient
    {
        private string _url = "";

        private Dictionary<string, List<InterfaceInfo>> _interfaceInfos;

        private WCFClient(string url)
        {
            _url = url;
            _interfaceInfos = new Dictionary<string, List<InterfaceInfo>>();
        }

        public static WCFClient CreateClient(string url)
        {
            return new WCFClient(url);
        }

        private string TrimNameSpace(string tag)
        {
            return tag.Split(new[] { ':' }).Last();
        }

        private void Analysis(XmlNode schemaNode, XmlNamespaceManager xnm, ref Dictionary<string, ParamInfo> paraminfodic)
        {
            if (paraminfodic == null)
            {
                paraminfodic = new Dictionary<string, ParamInfo>();
            }

            var importnode = schemaNode.SelectNodes("xs:import", xnm);
            if (importnode.Count > 0)
            {
                foreach (XmlNode node in importnode)
                {
                    var ns = node.Attributes["namespace"].Value;

                    var importschemanode = schemaNode.OwnerDocument.SelectSingleNode("//wsdl:types/xs:schema[@targetNamespace='" + ns + "']", xnm);

                    Analysis(importschemanode, xnm, ref paraminfodic);
                }
            }

            foreach (XmlNode node in schemaNode.ChildNodes)
            {
                if (node.Name == "xs:import")
                {
                    continue;
                }

                ParamInfo paraminfo = new ParamInfo();
                paraminfo.Name = TrimNameSpace(node.Attributes["name"].Value);
                paraminfo.ChildParamInfos = new List<ParamInfo>();

                XmlNodeList ele = null;
                if (node.Name == "xs:element")
                {
                    if (node.ChildNodes.Count > 0)
                    {
                        ele = node.SelectNodes("xs:complexType/xs:sequence/xs:element", xnm);
                    }
                    else
                    {
                        paraminfo.Nillable = node.Attributes["nillable"] == null ? false : bool.Parse(node.Attributes["nillable"].Value);
                        paraminfo.Type = TrimNameSpace(node.Attributes["type"].Value);
                        paraminfo.IsMany = node.Attributes["maxOccurs"]?.Value == "unbounded";
                        if (paraminfo.Type != null && paraminfodic.ContainsKey(paraminfo.Type))
                        {
                            paraminfo.ChildParamInfos = paraminfodic[paraminfo.Type].ChildParamInfos;
                        }
                    }
                }
                else
                {
                    ele = node.SelectNodes("xs:sequence/xs:element", xnm);
                }
                if (ele != null)
                {
                    foreach (XmlNode e in ele)
                    {
                        var p = new ParamInfo
                        {
                            ChildParamInfos = new List<ParamInfo>(),
                            Name = TrimNameSpace(e.Attributes["name"].Value),
                            Nillable = e.Attributes["nillable"] == null ? false : bool.Parse(e.Attributes["nillable"].Value),
                            Type = TrimNameSpace(e.Attributes["type"].Value),
                            IsMany = e.Attributes["maxOccurs"]?.Value == "unbounded"
                        };
                        if (p.Type != null && paraminfodic.ContainsKey(p.Type))
                        {
                            p.ChildParamInfos = paraminfodic[p.Type].ChildParamInfos;
                        }
                        paraminfo.ChildParamInfos.Add(p);
                    }
                }

                if (!paraminfodic.ContainsKey(paraminfo.Name))
                {
                    paraminfodic.Add(paraminfo.Name, paraminfo);
                }
            }

        }

        public Dictionary<string,List<InterfaceInfo>> GetInterfaceInfos()
        {
            if (_interfaceInfos.Count > 0)
            {
                return _interfaceInfos;
            }

            LJC.FrameWorkV3.Comm.HttpRequestEx httpRequestEx = new LJC.FrameWorkV3.Comm.HttpRequestEx();
            var resp = httpRequestEx.DoRequest($"{this._url}?singleWsdl", new byte[0], method: LJC.FrameWorkV3.Comm.WebRequestMethodEnum.GET);
            System.Xml.XmlDocument doc = new XmlDocument();
            doc.LoadXml(resp.ResponseContent);

            XmlNamespaceManager xnm = new XmlNamespaceManager(doc.NameTable);
            xnm.AddNamespace("wsdl", "http://schemas.xmlsoap.org/wsdl/");
            xnm.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            xnm.AddNamespace("soap", "http://schemas.xmlsoap.org/wsdl/soap/");
            xnm.AddNamespace("wsaw", "http://www.w3.org/2006/05/addressing/wsdl");

            //解析类型
            Dictionary<string, ParamInfo> paraminfodic = new Dictionary<string, ParamInfo>();
            var typeelementlist = doc.SelectNodes("//wsdl:types/xs:schema", xnm);
            
            foreach (XmlNode typeele in typeelementlist)
            {
                Analysis(typeele,xnm, ref paraminfodic);
            }

            //解析Message
            Dictionary<string, string> msgdic = new Dictionary<string, string>();
            var messagenodes = doc.SelectNodes("//wsdl:message",xnm);
            foreach (XmlNode msgnode in messagenodes)
            {
                msgdic.Add(TrimNameSpace(msgnode.Attributes["name"].Value), TrimNameSpace(msgnode.SelectSingleNode("wsdl:part[@name='parameters']", xnm).Attributes["element"].Value));
            }

            //解析wsdl:binding
            var bindingnodes = doc.SelectNodes("//wsdl:binding", xnm);
            var binddic = new Dictionary<string, Binding>();
            foreach(XmlNode bindnode in bindingnodes)
            {
                Binding binding = new Binding();
                binding.OperatorBindDic = new Dictionary<string, OperationBind>();
                binding.Name =TrimNameSpace(bindnode.Attributes["name"].Value);
                binding.Type =TrimNameSpace(bindnode.Attributes["type"].Value);

                foreach(XmlNode node in bindnode.SelectNodes("wsdl:operation", xnm))
                {
                    OperationBind operationBind = new OperationBind();
                    operationBind.OpeartionName =TrimNameSpace(node.Attributes["name"].Value);
                    var soapnode = node.SelectSingleNode("soap:operation", xnm);
                    operationBind.SoapAction = soapnode.Attributes["soapAction"].Value;
                    operationBind.Style = soapnode.Attributes["style"].Value;

                    foreach (XmlNode n in node.ChildNodes)
                    {
                        if (n.Name == "wsdl:input")
                        {
                            operationBind.InputUse =TrimNameSpace(n.SelectSingleNode("soap:body", xnm).Attributes["use"].Value);
                        }
                        else if (n.Name == "wsdl:output")
                        {
                            operationBind.InputUse =TrimNameSpace(n.SelectSingleNode("soap:body", xnm).Attributes["use"].Value);
                        }
                    }

                    binding.OperatorBindDic.Add(operationBind.OpeartionName, operationBind);
                    
                }
                binddic.Add(binding.Type, binding);
            }

            //解析服务
            var servicenodes = doc.SelectNodes("//wsdl:portType", xnm);
            foreach (XmlNode servicenode in servicenodes)
            {
                var servicename = TrimNameSpace(servicenode.Attributes["name"].Value);
                var oplist = new List<InterfaceInfo>();
                foreach (XmlNode node in servicenode.ChildNodes)
                {
                    var newinterface = new InterfaceInfo();
                    newinterface.OperationName = node.Attributes["name"].Value;
                    foreach (XmlNode childnode in node.ChildNodes)
                    {
                        if (childnode.Name == "wsdl:input")
                        {
                            newinterface.InputWsawAction = childnode.Attributes["wsaw:Action"].Value;
                            newinterface.InputMessage = TrimNameSpace(childnode.Attributes["message"].Value);

                            newinterface.InputParams = paraminfodic[msgdic[newinterface.InputMessage]].ChildParamInfos;
                        }
                        else if (childnode.Name == "wsdl:output")
                        {
                            newinterface.OutputWsawAction = childnode.Attributes["wsaw:Action"].Value;
                            newinterface.OutputMessage = TrimNameSpace(childnode.Attributes["message"].Value);

                            newinterface.OutputParams = paraminfodic[msgdic[newinterface.OutputMessage]].ChildParamInfos;
                        }  
                    }
                    newinterface.SoapAction = binddic[servicename].OperatorBindDic[newinterface.OperationName].SoapAction;
                    newinterface.BindName = binddic[servicename].Name;
                    oplist.Add(newinterface);

                }
                _interfaceInfos.Add(servicename, oplist);
            }

            return _interfaceInfos;
        }

        /// <summary>
        /// 序列化保存成文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToSerializer"></param>
        /// <param name="savePath"></param>
        public static string Serializer<T>(T objectToSerializer)
        {
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("s", "http://schemas.xmlsoap.org/soap/envelope/");

            XmlSerializer ser = new XmlSerializer(typeof(T));

            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            // Remove the <?xml version="1.0" encoding="utf-8"?> 
            settings.OmitXmlDeclaration = true;

            XmlWriter sw = XmlWriter.Create(sb, settings);

            ser.Serialize(sw, objectToSerializer, namespaces);

            return sb.ToString();
        }

        public HttpResponseEx Invoke(string service, string operationname, string xml)
        {
            var interfacedic = GetInterfaceInfos();
            if (!interfacedic.ContainsKey(service))
            {
                throw new NotImplementedException($"查找服务失败:{service}");
            }
            var op = interfacedic[service].Find(p => p.OperationName.ToLower() == operationname.ToLower());
            if (op == null)
            {
                throw new NotImplementedException($"查找操作方法失败:{operationname}");
            }

            HttpRequestEx httpRequestEx = new HttpRequestEx();

            httpRequestEx.Headers.Add("SOAPAction", $"\"{op.SoapAction}\"");
            httpRequestEx.Expect = "100";

            var resp = httpRequestEx.DoRequest(this._url, Encoding.UTF8.GetBytes(xml), WebRequestMethodEnum.POST,
                false, true, "text/xml; charset=utf-8");

            return resp;

        }

        public HttpResponseEx Invoke(string service,string operationname,Envelope data)
        {
            var interfacedic = GetInterfaceInfos();
            if (!interfacedic.ContainsKey(service))
            {
                throw new NotImplementedException($"查找服务失败:{service}");
            }
            var op = interfacedic[service].Find(p => p.OperationName.ToLower() == operationname.ToLower());
            if (op == null)
            {
                throw new NotImplementedException($"查找操作方法失败:{operationname}");
            }

            HttpRequestEx httpRequestEx = new HttpRequestEx();
            
            httpRequestEx.Headers.Add("SOAPAction", op.SoapAction);
            httpRequestEx.Headers.Add("Expect", "100-continue");

            var resp = httpRequestEx.DoRequest(this._url, Encoding.UTF8.GetBytes(Serializer(data)), WebRequestMethodEnum.POST,
                false, true, "text/xml; charset=utf-8");

            return resp;

        }
    }
}
