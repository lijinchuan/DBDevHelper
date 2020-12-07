using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace APIHelper.UC
{
    public partial class Scale : UserControl
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
        private Dictionary<int, Point> _lineNos;
        public Dictionary<int, Point> LineNos
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
            if (_lineNos == null)
            {
                _lineNos = new Dictionary<int, Point>();
                _lineNos.Add(1, new Point(0, 0));
            }
            if (_lineNos.Count == 0)
            {
                _lineNos.Add(1, new Point(0, 0));
            }
            foreach (KeyValuePair<int, Point> kv in _lineNos)
            {
                e.Graphics.DrawString(kv.Key.ToString(), this.Font, new SolidBrush(Color.Gray), kv.Value);
            }

        }
    }
}
