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
            DBSource db = container.FirstOrDefault(p => p.ServerName.Equals(item.ServerName,StringComparison.OrdinalIgnoreCase));
            if (db == null)
            {
                db = item;
                container.Add(db);
            }
            else
            {
                db = item;
            }
        }

        public void Clear()
        {
            container.Clear();
        }

        public bool Contains(DBSource item)
        {
            if (item == null)
                return true;
            return container.Exists(p => p.ServerName.Equals(item.ServerName,StringComparison.OrdinalIgnoreCase));
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
            var db = container.FirstOrDefault(p => p.ServerName.Equals(item.ServerName, StringComparison.OrdinalIgnoreCase));
            if (db == null)
                return false;
            return container.Remove(db);
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
