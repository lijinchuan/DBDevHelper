using Entity.WCF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

        public Dictionary<string,List<InterfaceInfo>> GetInterfaceInfos()
        {
            if (_interfaceInfos.Count > 0)
            {
                return _interfaceInfos;
            }

            LJC.FrameWorkV3.Comm.HttpRequestEx httpRequestEx = new LJC.FrameWorkV3.Comm.HttpRequestEx();
            var resp = httpRequestEx.DoRequest($"{this._url}?singleWsdl", new byte[0], method: LJC.FrameWorkV3.Comm.WebRequestMethodEnum.GET);
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(resp.ResponseContent);

            XmlNamespaceManager xnm = new XmlNamespaceManager(doc.NameTable);
            xnm.AddNamespace("wsdl", "http://schemas.xmlsoap.org/wsdl/");
            xnm.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            xnm.AddNamespace("soap", "http://schemas.xmlsoap.org/wsdl/soap/");
            xnm.AddNamespace("wsaw", "http://www.w3.org/2006/05/addressing/wsdl");

            //解析类型
            Dictionary<string, ParamInfo> paraminfodic = new Dictionary<string, ParamInfo>();
            var typeelementlist = doc.SelectNodes("//wsdl:types/xs:schema/xs:element", xnm);
            foreach(XmlNode typeele in typeelementlist)
            {
                ParamInfo paraminfo = new ParamInfo();
                paraminfo.Name =TrimNameSpace(typeele.Attributes["name"].Value);
                paraminfo.ChildParamInfos = new List<ParamInfo>();
                if (typeele.ChildNodes.Count > 0)
                {
                    paraminfo.Type = typeele.ChildNodes[0].Name;
                    
                    var ele = typeele.ChildNodes[0].SelectNodes("xs:sequence/xs:element", xnm);
                    foreach(XmlNode e in ele)
                    {
                        paraminfo.ChildParamInfos.Add(new ParamInfo
                        {
                            ChildParamInfos = new List<ParamInfo>(),
                            Name = TrimNameSpace(e.Attributes["name"].Value),
                            Nillable = e.Attributes["nillable"] == null ? false : bool.Parse(e.Attributes["nillable"].Value),
                            Type = TrimNameSpace(e.Attributes["type"].Value)
                        });
                    }

                    paraminfodic.Add(paraminfo.Name, paraminfo);
                }
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
                    foreach (System.Xml.XmlNode childnode in node.ChildNodes)
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
                        newinterface.SoapAction = binddic[servicename].OperatorBindDic[newinterface.OperationName].SoapAction;
                        newinterface.BindName = binddic[servicename].Name;
                        oplist.Add(newinterface);
                    }

                }
                _interfaceInfos.Add(servicename, oplist);
            }

            return _interfaceInfos;
        }
    }
}
