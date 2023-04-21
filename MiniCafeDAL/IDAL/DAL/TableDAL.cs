using MiniCafeDAL.Model;
using System.Collections.Generic;
using System.Linq;

namespace MiniCafeDAL.IDAL.DAL
{
    public class TableDAL : ITableDAL
    {
        private static ITableDAL instance = null;
        private static readonly object padlock = new object();

        private TableDAL()
        {

        }

        public static ITableDAL Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new TableDAL();
                    }
                    return instance;
                }
            }
        }
        public void AddTable(Table table)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                entities.Tables.Add(table);
                entities.SaveChanges();
            }

        }
        public void UpdateTable(Table table)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                entities.Entry(table).State = System.Data.Entity.EntityState.Modified;
                entities.SaveChanges();
            }

        }
        public void DeleteTable(int id)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                Table tableToDelete = entities.Tables.Find(id);
                entities.Tables.Remove(tableToDelete);
                entities.SaveChanges();
            }

        }
        public Table GetTableById(int id)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                return entities.Tables.Find(id);
            }

        }
        public List<Table> GetAllTables()
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                return entities.Tables.ToList();
            }

        }
    }
}