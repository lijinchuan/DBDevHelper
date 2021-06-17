using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APIHelper.UC
{
    public class TextBoxEx:TextBox
    {
        #region 禁止粘贴
        /// <summary>
        /// 重写基类的WndProc方法
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0302)            // 0x0302是粘贴消息
            {
                base.WndProc(ref m);
                try
                {
                    var text = Text;
                    var obj = JsonConvert.DeserializeObject<dynamic>(text);
                    Text = JsonConvert.SerializeObject(obj, Formatting.Indented);
                }
                catch
                {

                }
                return;
            }
            base.WndProc(ref m);            // 若此消息不是粘贴消息，则交给其基类去处理
        }
        #endregion
    }
}
