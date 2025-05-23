﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.OldVesion.V2
{
    public class APIInvokeLog
    {
        public int Id
        {
            get;
            set;
        }

        public int APIId
        {
            get;
            set;
        }

        public string APIName
        {
            get;
            set;
        }

        public int SourceId
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }

        public APIMethod APIMethod
        {
            get;
            set;
        }

        public BodyDataType BodyDataType
        {
            get;
            set;
        }

        public ApplicationType ApplicationType
        {
            get;
            set;
        }

        public AuthType AuthType
        {
            get;
            set;
        }

        public int ApiEnvId
        {
            get;
            set;
        }

        public Entity.APIData APIData
        {
            get;
            set;
        }

        public APIResonseResult APIResonseResult
        {
            get;
            set;
        }

        public int StatusCode
        {
            get;
            set;
        }

        public string RespMsg
        {
            get;
            set;
        }

        public long RespSize
        {
            get;
            set;
        }

        public long Ms
        {
            get;
            set;
        }

        public string ResponseText
        {
            get;
            set;
        }

        public DateTime CDate
        {
            get;
            set;
        }

        public StringBuilder GetRequestDetail()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"url:{this.Path}");
            sb.AppendLine();

            sb.AppendLine($"method:{this.APIMethod.ToString()}");

            sb.AppendLine();
            sb.AppendLine("headers:");
            if (this.APIData?.Headers?.Count() > 0)
            {
                foreach (var header in this.APIData.Headers)
                {
                    if (header.Name.Equals("User-Agent", StringComparison.OrdinalIgnoreCase))
                    {
                        if (header.Checked)
                        {
                            sb.AppendLine($" UserAgent:{header.Value}");
                        }
                    }
                    else if (header.Name.Equals("Accept", StringComparison.OrdinalIgnoreCase))
                    {
                        if (header.Checked)
                        {
                            sb.AppendLine($" Accept:{header.Value}");
                        }
                    }
                    else if (header.Name.Equals("Connection", StringComparison.OrdinalIgnoreCase))
                    {

                    }
                    else
                    {
                        if (header.Checked)
                        {
                            sb.AppendLine($" {header.Name}:{header.Value}");
                        }
                    }
                }
            }

            var authtype = this.AuthType;
            if (authtype == AuthType.Bearer)
            {
                sb.AppendLine($" Authorization:Bearer {this.APIData?.BearToken}");
            }
            else if (authtype == AuthType.ApiKey)
            {
                if (this.APIData?.ApiKeyAddTo == 0)
                {
                    sb.AppendLine($" {this.APIData?.ApiKeyName}:{this.APIData?.ApiKeyValue}");
                }
            }
            else if (authtype == AuthType.Basic)
            {
                sb.AppendLine($" Authorization:Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{this.APIData?.BasicUserName}:{this.APIData?.BasicUserPwd}"))}");
            }

            sb.AppendLine($" content-type:application/{this.ApplicationType}");
            sb.AppendLine();

            if (this.APIData.Cookies != null)
            {
                sb.AppendLine("cookie:");
                foreach (var cookie in this.APIData.Cookies)
                {
                    sb.AppendLine($" {cookie.Name}:{cookie.Value}");
                }
            }

            sb.AppendLine("body:");
            var bodydataType = this.BodyDataType;
            if (bodydataType == BodyDataType.formdata)
            {
                sb.AppendLine($" formdata:{string.Join("&", this.APIData?.FormDatas.Where(p => p.Checked).Select(p => p.Name + "=" + p.Value))}");
            }
            else if (bodydataType == BodyDataType.xwwwformurlencoded)
            {
                sb.AppendLine($" xwwwformurlencoded:{string.Join("&", this.APIData?.XWWWFormUrlEncoded.Where(p => p.Checked).Select(p => p.Name + "=" + p.Value))}");
            }
            else if (bodydataType == BodyDataType.raw || bodydataType == BodyDataType.wcf)
            {
                sb.AppendLine($"raw:{this.APIData?.RawText}");
            }
            else if (bodydataType == BodyDataType.binary)
            {
                sb.AppendLine($" multipart/form-data:{string.Join("&", this.APIData?.Multipart_form_data.Where(p => p.Checked).Select(p => p.Name + "=" + p.Value))}");
            }
            else
            {
                sb.AppendLine($"raw:");
            }

            return sb;
        }

        public StringBuilder GetRequestBody()
        {
            StringBuilder sb = new StringBuilder();

            var bodydataType = this.BodyDataType;
            if (bodydataType == BodyDataType.formdata)
            {
                sb.AppendLine($"{string.Join("&", this.APIData?.FormDatas.Where(p => p.Checked).Select(p => p.Name + "=" + p.Value))}");
            }
            else if (bodydataType == BodyDataType.xwwwformurlencoded)
            {
                sb.AppendLine($"{string.Join("&", this.APIData?.XWWWFormUrlEncoded.Where(p => p.Checked).Select(p => p.Name + "=" + p.Value))}");
            }
            else if (bodydataType == BodyDataType.raw || bodydataType == BodyDataType.wcf)
            {
                sb.AppendLine($"{this.APIData?.RawText}");
            }
            else if (bodydataType == BodyDataType.binary)
            {
                sb.AppendLine($"{string.Join("&", this.APIData?.Multipart_form_data.Where(p => p.Checked).Select(p => p.Name + "=" + p.Value))}");
            }

            return sb;
        }

        public StringBuilder GetRespDetail()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"status code:{this.StatusCode}");
            sb.AppendLine($"耗时:{this.Ms}毫秒");
            sb.AppendLine($"响应大小:{this.RespSize}B");
            sb.AppendLine();
            sb.AppendLine("headers:");
            if (this.APIResonseResult?.Headers != null)
            {
                foreach (var h in this.APIResonseResult?.Headers)
                {
                    sb.AppendLine($" {h.Key}:{h.Value}");
                }
                sb.AppendLine();
                sb.AppendLine("body:");
                sb.AppendLine(this.ResponseText);
            }

            return sb;
        }

        public StringBuilder GetRespBody()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(this.ResponseText);

            return sb;
        }
    }
}
