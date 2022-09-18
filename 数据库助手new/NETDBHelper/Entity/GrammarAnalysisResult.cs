using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class GrammarAnalysisResult
    {
        public List<GrammarInfo> AnnotationInfos
        {
            get;
            set;
        }

        public List<GrammarInfo> StringInfos
        {
            get;
            set;
        }
    }
}
