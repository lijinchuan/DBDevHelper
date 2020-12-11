using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Biz
{
    public class TreeNodeEx:TreeNode
    {
        public int CollapseImgIndex
        {
            get;
            set;
        }

        public int ExpandImgIndex
        {
            get;
            set;
        }

        public TreeNodeEx()
        {

        }

        public TreeNodeEx(string text, int imageIndex, int selectedImageIndex,
            int collapseImgIndex,int expandImgIndex)
            :base(text,imageIndex,selectedImageIndex)
        {
            CollapseImgIndex = collapseImgIndex;
            ExpandImgIndex = expandImgIndex;
        }

        public TreeNodeEx(string text, int imageIndex, int selectedImageIndex)
            : base(text, imageIndex, selectedImageIndex)
        {
            CollapseImgIndex = imageIndex;
            ExpandImgIndex = selectedImageIndex;
        }
    }
}
