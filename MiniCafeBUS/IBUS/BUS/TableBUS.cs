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
        private readonly ITableDAL _tableDAL;

        public TableBUS(ITableDAL tableDAL)
        {
            _tableDAL = tableDAL;
        }

        public void AddTable(Table table)
        {
            _tableDAL.AddTable(table);
        }

        public void DeleteTable(int id)
        {
            _tableDAL.DeleteTable(id);
        }

        public List<Table> GetAllTables()
        {
           return _tableDAL.GetAllTables();
        }

        public Table GetTableById(int id)
        {
            return _tableDAL.GetTableById(id);
        }

        public void UpdateTable(Table table)
        {
            _tableDAL.UpdateTable(table);
        }
    }
}
