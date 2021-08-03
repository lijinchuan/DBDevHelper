using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Biz
{
    public static class IEUtil
    {
        /// <summary>
        /// 引用wininet.dll + 定义InternetSetCookie
        /// </summary>
        /// <param name="lpszUrlName">需要设置Cookie的URL</param>
        /// <param name="lbszCookieName">Cookie名称</param>
        /// <param name="lpszCookieData">Cookie数据</param>
        /// <returns>设置Cookie是否成功</returns>
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

        [DllImport("wininet.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        private static extern bool InternetSetOption(int hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);

        /// <summary>
        /// 定义IE版本的枚举
        /// </summary>
        public enum IeVersion
        {
            NONE,
            强制ie11,//11001（0x2AF）
            标准ie11,//11000（0x2AF8）
            强制ie10,//10001 (0x2711) Internet Explorer 10。网页以IE 10的标准模式展现，页面!DOCTYPE无效
            标准ie10,//10000 (0x02710) Internet Explorer 10。在IE 10标准模式中按照网页上!DOCTYPE指令来显示网页。Internet Explorer 10 默认值。
            强制ie9,//9999 (0x270F) Windows Internet Explorer 9. 强制IE9显示，忽略!DOCTYPE指令
            标准ie9,//9000 (0x2328) Internet Explorer 9. Internet Explorer 9默认值，在IE9标准模式中按照网页上!DOCTYPE指令来显示网页。
            强制ie8,//8888 (0x22B8) Internet Explorer 8，强制IE8标准模式显示，忽略!DOCTYPE指令
            标准ie8,//8000 (0x1F40) Internet Explorer 8默认设置，在IE8标准模式中按照网页上!DOCTYPE指令展示网页
            标准ie7//7000 (0x1B58) 使用WebBrowser Control控件的应用程序所使用的默认值，在IE7标准模式中按照网页上!DOCTYPE指令来展示网页
        }

        public static IeVersion GetIEVersion()
        {
            string productName = AppDomain.CurrentDomain.SetupInformation.ApplicationName;//获取程序名称

            RegistryKey key = Registry.CurrentUser;
            RegistryKey software =
                key.CreateSubKey(
                    @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION\" + productName);
            if (software != null)
            {
                software.Close();
                software.Dispose();
            }
            RegistryKey wwui =
                key.OpenSubKey(
                    @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);
            //该项必须已存在
            if (wwui != null)
            {
                var var=(int?)wwui.GetValue(productName);
                switch (var)
                {
                    case 0x1B58:
                        return IeVersion.标准ie7;
                    case 0x1F40:
                        return IeVersion.标准ie8;
                    case 0x22B8:
                        return IeVersion.强制ie8;
                    case 0x2328:
                        return IeVersion.标准ie9;
                    case 0x270F:
                        return IeVersion.强制ie9;
                    case 0x02710:
                        return IeVersion.标准ie10;
                    case 0x2711:
                        return IeVersion.强制ie10;
                    case 0x2AF8:
                        return IeVersion.标准ie11;
                    case 0x2AF9:
                        return IeVersion.强制ie11;
                }
            }

            return IeVersion.NONE;
        }

        /// <summary>
        /// 设置WebBrowser的默认版本
        /// </summary>
        /// <param name="ver">IE版本</param>
        public static bool SetIE(IeVersion ver)
        {
            string productName = AppDomain.CurrentDomain.SetupInformation.ApplicationName;//获取程序名称

            object version;
            switch (ver)
            {
                case IeVersion.标准ie7:
                    version = 0x1B58;
                    break;
                case IeVersion.标准ie8:
                    version = 0x1F40;
                    break;
                case IeVersion.强制ie8:
                    version = 0x22B8;
                    break;
                case IeVersion.标准ie9:
                    version = 0x2328;
                    break;
                case IeVersion.强制ie9:
                    version = 0x270F;
                    break;
                case IeVersion.标准ie10:
                    version = 0x02710;
                    break;
                case IeVersion.强制ie10:
                    version = 0x2711;
                    break;
                case IeVersion.标准ie11:
                    version = 0x2AF8;
                    break;
                case IeVersion.强制ie11:
                    version = 0x2AF9;
                    break;
                default:
                    version = 0x1F40;
                    break;
            }

            RegistryKey key = Registry.CurrentUser;
            RegistryKey software =
                key.CreateSubKey(
                    @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION\" + productName);
            if (software != null)
            {
                software.Close();
                software.Dispose();
            }
            RegistryKey wwui =
                key.OpenSubKey(
                    @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);
            //该项必须已存在
            if (wwui != null)
            {
                wwui.SetValue(productName, version, RegistryValueKind.DWord);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 使用InternetSetOption操作wininet.dll清除webbrowser里的cookie
        /// </summary>
        public static unsafe bool SuppressWininetBehavior()
        {
            /* SOURCE: http://msdn.microsoft.com/en-us/library/windows/desktop/aa385328%28v=vs.85%29.aspx
                * INTERNET_OPTION_SUPPRESS_BEHAVIOR (81):
                *      A general purpose option that is used to suppress behaviors on a process-wide basis. 
                *      The lpBuffer parameter of the function must be a pointer to a DWORD containing the specific behavior to suppress. 
                *      This option cannot be queried with InternetQueryOption. 
                *      
                * INTERNET_SUPPRESS_COOKIE_PERSIST (3):
                *      Suppresses the persistence of cookies, even if the server has specified them as persistent.
                *      Version:  Requires Internet Explorer 8.0 or later.
                */
            int option = (int)3/* INTERNET_SUPPRESS_COOKIE_PERSIST*/;
            int* optionPtr = &option;

            bool success = InternetSetOption(0, 81/*INTERNET_OPTION_SUPPRESS_BEHAVIOR*/, new IntPtr(optionPtr), sizeof(int));

            return success;

        }
    }
}
