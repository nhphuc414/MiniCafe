using MiniCafeDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCafeDAL.IDAL.DAL
{
    public class ShiftDAL : IShiftDAL
    {
        private readonly MiniCafeEntities _context;

        public ShiftDAL(MiniCafeEntities context)
        {
            _context = context;
        }
        public void AddShift(Shift shift)
        {
            _context.Shifts.Add(shift);
            _context.SaveChanges();
        }
        public void UpdateShift(Shift shift)
        {
            _context.Entry(shift).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
        public void DeleteShift(int id)
        {
            Shift shiftToDelete = _context.Shifts.Find(id);
            _context.Shifts.Remove(shiftToDelete);
            _context.SaveChanges();
        }
        public Shift GetShiftById(int id)
        {
            return _context.Shifts.Find(id);
        }
        public List<Shift> GetAllShifts()
        {
            return _context.Shifts.ToList();
        }
    }
}
