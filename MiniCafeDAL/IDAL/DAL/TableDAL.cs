using MiniCafeDAL.Model;
using System.Collections.Generic;
using System.Linq;

namespace MiniCafeDAL.IDAL.DAL
{
    public class TableDAL : ITableDAL
    {
        private readonly MiniCafeEntities _context;

        public TableDAL(MiniCafeEntities context)
        {
            _context = context;
        }
        public void AddTable(Table table)
        {
            _context.Tables.Add(table);
            _context.SaveChanges();
        }
        public void UpdateTable(Table table)
        {
            _context.Entry(table).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
        public void DeleteTable(int id)
        {
            Table tableToDelete = _context.Tables.Find(id);
            _context.Tables.Remove(tableToDelete);
            _context.SaveChanges();
        }
        public Table GetTableById(int id)
        {
            return _context.Tables.Find(id);
        }
        public List<Table> GetAllTables()
        {
            return _context.Tables.ToList();
        }
    }
}