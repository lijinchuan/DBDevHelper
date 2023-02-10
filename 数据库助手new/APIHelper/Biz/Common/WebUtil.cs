using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common
{
    public class WebUtil
    {
        public static readonly string ContentTypes_Html = "text/html";
        public static readonly string ContentTypes_Josn = "application/json";

        public static readonly string[] ContentTypes = new[]
        {
            ContentTypes_Html,
            ContentTypes_Josn,
            "application/xml",
            "application/xhtml+xml",
            "application/atom+xml",
            "application/octet-stream",
            "image/bmp",
            "application/svg+xml",
            "image/gif",
            "image/jpeg",
            "image/png",
            "image/tiff",
            "image/webp",
            "application/pdf",
            "application/msword",
            "application/zip"
        };

        public static readonly string Charsets_UTF8 = "utf-8";

        public static readonly string[] Charsets = new[]
        {
            Charsets_UTF8,
            "gb2312",
            "big5",
            "iso-8859-1" //西欧的编码，英文编码
        };
    }
}
