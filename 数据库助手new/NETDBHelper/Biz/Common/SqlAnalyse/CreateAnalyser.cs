using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public class CreateAnalyser : SqlAnalyser
    {
        private readonly HashSet<string> keys = new HashSet<string> { keyCreate, keyDatabase, keyTable, keyUnique, keyClustered, keyNonclustered, keyIndex, keyView, keyAs, keySelect, keyWith,keyJoin, keyOn, keyFileName, keyLog, keyAsc, keyDesc };

        private ISqlAnalyser selectAnalyser = null;

        public override HashSet<string> GetKeys()
        {
            if (selectAnalyser != null)
            {
                return selectAnalyser.GetKeys();
            }
            return keys;
        }

        public override string GetPrimaryKey()
        {
            return keyCreate;
        }

        protected override bool Accept(ISqlProcessor sqlProcessor, ISqlExpress sqlExpress)
        {
            if (selectAnalyser != null)
            {
                var boo = selectAnalyser.Accept(sqlProcessor, sqlExpress, sqlExpress.AnalyseType == AnalyseType.Key);
                return boo;
            }
            var lastLastKey = PreAcceptKeys(acceptKeys, 1);
            var lastKey = PreAcceptKeys(acceptKeys, 0);

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

        public override void AddAcceptKey(string key)
        {
            selectAnalyser?.AddAcceptKey(key);
            base.AddAcceptKey(key);
        }

        public override void AddAcceptSqlExpress(ISqlExpress sqlExpress)
        {
            selectAnalyser?.AddAcceptSqlExpress(sqlExpress);
            base.AddAcceptSqlExpress(sqlExpress);
        }

        protected override bool AcceptInnerKey(ISqlProcessor sqlProcessor, ISqlExpress sqlExpress)
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
                        selectAnalyser.Accept(sqlProcessor, sqlExpress, true);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return base.AcceptInnerKey(sqlProcessor, sqlExpress);
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
