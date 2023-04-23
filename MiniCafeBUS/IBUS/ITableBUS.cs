using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniCafeDAL.Model;
namespace MiniCafeBUS.IBUS
{
    public interface ITableBUS
    {
        void AddTable(Table table);
        void UpdateTable(Table table);
        void DeleteTable(int id);
        Table GetTableById(int id);
        List<Table> GetAllTables();
        List<Table> GetTablesOnActive();
    }
}
