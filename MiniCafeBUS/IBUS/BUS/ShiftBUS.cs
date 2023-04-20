using MiniCafeDAL.Model;
using MiniCafeDAL.IDAL;
using MiniCafeDAL.IDAL.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCafeBUS.IBUS.BUS
{
    public class ShiftBUS : IShiftBUS
    {
        private readonly IShiftDAL _shiftDAL;

        public ShiftBUS(IShiftDAL shiftDAL)
        {
            _shiftDAL = shiftDAL;
        }

        public void AddShift(Shift shift)
        {
            _shiftDAL.AddShift(shift);
        }

        public void DeleteShift(int id)
        {
            _shiftDAL.DeleteShift(id);
        }

        public List<Shift> GetAllShifts()
        {
            return _shiftDAL.GetAllShifts();
        }

        public Shift GetShiftById(int id)
        {
            return _shiftDAL.GetShiftById(id);
        }

        public void UpdateShift(Shift shift)
        {
            _shiftDAL.UpdateShift(shift);
        }
    }
}
