using MiniCafeDAL.IDAL;
using MiniCafeDAL.IDAL.DAL;
using MiniCafeDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MiniCafeBUS.IBUS.BUS
{
    public class TableBUS : ITableBUS
    {
        private static ITableBUS instance = null;
        private static readonly object padlock = new object();

        private TableBUS()
        {
        }

        public static ITableBUS Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new TableBUS();
                    }
                    return instance;
                }
            }
        }
        public void AddTable(Table table)
        {
            if (TableDAL.Instance.GetAllTables().Any(t => t.name.Equals(table.name)))
            {
                throw new Exception("Tên bàn đã có");
            }
            TableDAL.Instance.AddTable(table);
        }

        public void DeleteTable(int id)
        {
            TableDAL.Instance.DeleteTable(id);
        }

        public List<Table> GetAllTables()
        {
           return TableDAL.Instance.GetAllTables();
        }

        public Table GetTableById(int id)
        {
            return TableDAL.Instance.GetTableById(id);
        }

        public void UpdateTable(Table table)
        {
            if (TableDAL.Instance.GetAllTables().Any(t => t.id!=table.id && t.name.Equals(table.name)))
            {
                throw new Exception("Tên bàn đã có");
            }
            TableDAL.Instance.UpdateTable(table);
        }
        public List<Table> GetTablesOnActive()
        {
            return TableDAL.Instance.GetTablesOnActive();
        }
    }
}
