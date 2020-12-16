using Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Biz
{
    public static class RecoverManager
    {
        private static List<Tuple<Type,bool, object[]>> iRecoverAbleLs = new List<Tuple<Type,bool, object[]>>();
        private static string recoverpath = "__recover.bin";

        public static void AddRecoverInstance(TabPage page, bool isSelected)
        {
            if (iRecoverAbleLs == null)
            {
                iRecoverAbleLs = new List<Tuple<Type, bool, object[]>>();
            }
            if (page is IRecoverAble)
            {
                var recoverAble = page as IRecoverAble;
                iRecoverAbleLs.Add(new Tuple<Type,bool, object[]>(recoverAble.GetType(), isSelected, recoverAble.GetRecoverData()));
            }
        }

        public static void SaveRecoverInstance()
        {
            if (iRecoverAbleLs.Count > 0)
            {
                LJC.FrameWorkV3.Comm.SerializerHelper.BinarySave(recoverpath, iRecoverAbleLs);
            }
            else
            {
                if (File.Exists(recoverpath))
                {
                    File.Delete(recoverpath);
                }
            }
        }

        public static IEnumerable<Tuple<TabPage, bool>> Recove()
        {
            if (File.Exists(recoverpath))
            {
                iRecoverAbleLs = (List<Tuple<Type, bool, object[]>>)LJC.FrameWorkV3.Comm.SerializerHelper.BinaryGet(recoverpath);
                if (iRecoverAbleLs != null)
                {
                    foreach (var r in iRecoverAbleLs)
                    {
                        var ctor = r.Item1.GetConstructor(new Type[0]);
                        if (ctor != null)
                        {
                            yield return new Tuple<TabPage, bool>((TabPage)(ctor.Invoke(new Type[0]) as IRecoverAble).Recover(r.Item3), r.Item2);
                        }
                    }
                    iRecoverAbleLs.Clear();
                }
            }

        }

        public static IEnumerable<DBSource> GetDBSources()
        {
            if (File.Exists(recoverpath))
            {
                iRecoverAbleLs = (List<Tuple<Type, bool, object[]>>)LJC.FrameWorkV3.Comm.SerializerHelper.BinaryGet(recoverpath);
                foreach (var r in iRecoverAbleLs)
                {
                    var ctor = r.Item1.GetConstructor(new Type[0]);
                    if (ctor != null)
                    {
                        foreach (var p in r.Item3)
                        {
                            if (p is DBSource)
                            {
                                yield return p as DBSource;
                            }
                        }
                    }
                }
            }
        }
    }
}
