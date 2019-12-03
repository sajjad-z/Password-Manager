using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Models
{
    public class DataBase<TEntity> where TEntity : class
    {
        LiteDatabase db;
        LiteCollection<TEntity> collection;

        public DataBase()
        {
            // Open database (or create if not exits)
            db = new LiteDatabase(@"MyDB.db");

            // Get table collection
            if (typeof(TEntity) == typeof(tbl_Password))
            {
                collection = db.GetCollection<TEntity>("tbl_passwords");
            }
            else if (typeof(TEntity) == typeof(tbl_Setting))
            {
                collection = db.GetCollection<TEntity>("tbl_settings");
            }
        }

        public bool Insert(TEntity record)
        {
            try
            {
                collection.Insert(record);
                return true;
            }
            catch { return false; }
        }

        public bool Delete(Expression<Func<TEntity, bool>> where = null)
        {
            try
            {
                collection.Delete(where);
                return true;
            }
            catch { return false; }
        }

        public bool Delete(int id)
        {
            try
            {
                collection.Delete(id);
                return true;
            }
            catch { return false; }
        }

        public TEntity Select(int id)
        {
            try
            {
                return collection.FindById(id);
            }
            catch { return null; }
        }

        public IEnumerable<TEntity> Select(Expression<Func<TEntity, bool>> where = null)
        {
            try
            {
                if (where != null)
                {
                    return collection.Find(where);
                }
                return collection.FindAll();
            }
            catch { return null; }
        }

        public int Count(Expression<Func<TEntity, bool>> where = null)
        {
            try
            {
                if (where != null)
                {
                    return collection.Count(where);
                }
                return collection.Count();
            }
            catch { return 0; }
        }

        public bool Update(TEntity record)
        {
            try
            {
                collection.Update(record);
                return true;
            }
            catch { return false; }
        }

    }
}
