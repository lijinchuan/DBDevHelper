using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouchBaseDevHelper.UI
{
    public class Utils
    {
        public static IEnumerable<string> GetHostAndPoint(string connstr)
        {
            var hps = connstr.Split(',');
            foreach (var hp in hps)
            {
                if (string.IsNullOrWhiteSpace(hp))
                {
                    continue;
                }

                if (hp.IndexOf(':') == -1)
                {
                    yield return hp.Trim() + ":8091";
                }
                else
                {

                    yield return hp.Trim();
                }
            }
        }
    }
}
