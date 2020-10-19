using Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz
{
    public static class RecoverManager
    {
        private static List<Tuple<Type, object[]>> iRecoverAbleLs = new List<Tuple<Type, object[]>>();
        private static string recoverpath = "__recover.bin";

        public static void AddRecoverInstance(IRecoverAble recoverAble)
        {
            iRecoverAbleLs.Add(new Tuple<Type, object[]>(recoverAble.GetType(), recoverAble.GetRecoverData()));
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

        public static IEnumerable<IRecoverAble> Recove()
        {
            if (File.Exists(recoverpath))
            {
                iRecoverAbleLs = (List<Tuple<Type, object[]>>)LJC.FrameWorkV3.Comm.SerializerHelper.BinaryGet(recoverpath);
                foreach(var r in iRecoverAbleLs)
                {
                    var ctor = r.Item1.GetConstructor(new Type[0]);
                    if (ctor != null)
                    {
                        yield return (ctor.Invoke(new Type[0]) as IRecoverAble).Recover(r.Item2);
                    }
                }
                iRecoverAbleLs.Clear();
            }
            
        }

        public static IEnumerable<DBSource> GetDBSources()
        {
            if (File.Exists(recoverpath))
            {
                iRecoverAbleLs = (List<Tuple<Type, object[]>>)LJC.FrameWorkV3.Comm.SerializerHelper.BinaryGet(recoverpath);
                foreach (var r in iRecoverAbleLs)
                {
                    var ctor = r.Item1.GetConstructor(new Type[0]);
                    if (ctor != null)
                    {
                        foreach(var p in r.Item2)
                        {
                            if(p is DBSource)
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
