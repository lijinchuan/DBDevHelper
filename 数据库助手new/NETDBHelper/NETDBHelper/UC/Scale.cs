using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NETDBHelper.UC
{
    public partial class Scale : BaseUserControl
    {
        public int FirstLine
        {
            get
            {
                return _lineNos.First().Key;
            }
        }
        public int LastLine
        {
            get
            {
                return _lineNos.Last().Key;
            }
        }
        private Dictionary<int, PointF> _lineNos;
        public Dictionary<int, PointF> LineNos
        {
            get
            {
                return this._lineNos;
            }
            set
            {
                this._lineNos = value;
                this.Invalidate();
            }
        }
        public Scale()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            LockPaint = true;
            if (_lineNos == null)
            {
                _lineNos = new Dictionary<int, PointF>();
                _lineNos.Add(1, new PointF(0, 0));
            }
            if (_lineNos.Count == 0)
            {
                _lineNos.Add(1, new PointF(0, 0));
            }
            foreach (KeyValuePair<int, PointF> kv in _lineNos)
            {
                e.Graphics.DrawString(kv.Key.ToString(), this.Font, new SolidBrush(Color.Gray), kv.Value);
            }
            LockPaint = false;
        }
    }
}
