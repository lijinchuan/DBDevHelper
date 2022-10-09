using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public class CreateAnalyser : SqlAnalyser
    {
        private HashSet<string> keys = new HashSet<string> { keyCreate, keyDatabase, keyTable, keyUnique, keyClustered, keyNonclustered, keyIndex, keyView, keyAs, keySelect, keyWith, keyOn, keyFileName, keyLog, keyAsc, keyDesc };

        private ISqlAnalyser selectAnalyser = null;

        public override HashSet<string> GetKeys()
        {
            return keys;
        }

        public override string GetPrimaryKey()
        {
            return keyCreate;
        }

        protected override bool Accept(ISqlExpress sqlExpress)
        {
            if (selectAnalyser != null)
            {
                var boo= selectAnalyser.Accept(sqlExpress, sqlExpress.AnalyseType == AnalyseType.Key);
                return boo;
            }
            var lastLastKey = PreAcceptKeys(acceptKeys, 1);
            var lastKey=PreAcceptKeys(acceptKeys, 0);

            if (lastLastKey == keyCreate && sqlExpress.ExpressType == SqlExpressType.Token)
            {
                if (lastKey == keyTable)
                {
                    sqlExpress.AnalyseType = AnalyseType.Table;
                    tables.Add(sqlExpress);
                }
                else if (lastKey == keyView)
                {
                    sqlExpress.AnalyseType = AnalyseType.View;
                }
            }

            return true;
        }

        protected override bool AcceptInnerKey(ISqlExpress sqlExpress)
        {
            if (sqlExpress.Val == keySelect)
            {
                var lastLastKey = PreAcceptKeys(acceptKeys, 1);
                var lastKey = PreAcceptKeys(acceptKeys, 0);
                if (lastLastKey == keyView && lastKey == keyAs)
                {
                    if (selectAnalyser == null)
                    {
                        selectAnalyser = new SelectAnalyser();
                        selectAnalyser.Accept(sqlExpress, true);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return base.AcceptInnerKey(sqlExpress);
        }

        public override void Print(string sql)
        {
            if (selectAnalyser != null)
            {
                foreach(var tb in selectAnalyser.GetTables())
                {
                    tables.Add(tb);
                }

                foreach (var col in selectAnalyser.GetColumns())
                {
                    colums.Add(col);
                }

                foreach(var alias in selectAnalyser.GetAliasTables())
                {
                    aliasTables.Add(alias);
                }
            }
            base.Print(sql);
        }
    }
}
