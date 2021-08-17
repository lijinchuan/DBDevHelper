using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Entity
{
    public class DBSourceCollection:ICollection<DBSource>,IEnumerator,IEnumerator<DBSource>
    {
        private List<DBSource> container;
        public DBSourceCollection()
        {
            container = new List<DBSource>();
        }
        public DBSourceCollection(List<DBSource> dbSourceList)
        {
            this.container = dbSourceList;
        }
        public DBSource this[int index]
        {
            get
            {
                if (container.Count() > index)
                    return container[index];
                return null;
            }
        }
 
        public void Add(DBSource item)
        {
            if (item == null)
                return;
            
            if (!Contains(item))
            {
                container.Add(item);
            }
        }

        public void Clear()
        {
            container.Clear();
        }

        private DBSource Find(DBSource item)
        {
            if (item == null)
                return null;

            return container.Find(p=>p.ServerName.Equals(item.ServerName,StringComparison.OrdinalIgnoreCase)
                &&p.IDType==item.IDType&&
                (p.IDType==IDType.windows||p.LoginName.Equals(item.LoginName,StringComparison.OrdinalIgnoreCase)));
        }

        public bool Contains(DBSource item)
        {
            if (item == null)
                return true;
            return Find(item)!=null;
        }

        public bool Upsert(DBSource item,bool isForPasswordAndAccount)
        {
            var old = Find(item);
            if (old == null)
            {
                container.Add(item);
                return true;
            }
            else
            {
                if (isForPasswordAndAccount && !string.Equals(item.LoginPassword, old.LoginPassword, StringComparison.OrdinalIgnoreCase))
                {
                    old.LoginName = item.LoginName;
                    old.LoginPassword = item.LoginPassword;
                    return true;
                }
                else
                {
                    old.ConnDB = item.ConnDB;
                    old.ExDBList = item.ExDBList;
                    old.ExDBRegex = item.ExDBRegex;
                    old.ExTBList = item.ExTBList;
                    old.IDType = item.IDType;
                    old.ID = item.ID;
                    old.LoginName = item.LoginName;
                    old.LoginPassword = item.LoginPassword;
                    old.ServerName = item.ServerName;
                    return true;
                }
            }
        }

        public void CopyTo(DBSource[] array, int arrayIndex)
        {
            if (array == null)
                return;
            for (int i = arrayIndex; i < container.Count(); i++)
            {
                Add(array[i]);
            }
        }

        public int Count
        {
            get
            {
                return container.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Remove(DBSource item)
        {
            if (item == null)
                return false;

            var removeItem = Find(item);
            if (removeItem == null)
                return false;

            return container.Remove(removeItem);
        }

        public IEnumerator<DBSource> GetEnumerator()
        {
            return (IEnumerator<DBSource>)(new DBSourceCollection(this.container));
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (IEnumerator)(new DBSourceCollection(this.container));
        }

        private int _currentIdx=-1;
        public object Current
        {
            get
            {
                if(container.Count()>_currentIdx)
                    return container[_currentIdx];
                return default(DBSource);
            }
        }

        public bool MoveNext()
        {
            if (_currentIdx < container.Count - 1)
            {
                _currentIdx++;
                return true;
            }
            else
            {
                _currentIdx = -1;
                return false;
            }
        }

        public void Reset()
        {
            _currentIdx = -1;
        }

        DBSource IEnumerator<DBSource>.Current
        {
            get
            {
                return (DBSource)Current;
            }
        }

        public void Dispose()
        {
            
        }
    }
}
