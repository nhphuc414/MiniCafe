using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniCafeDAL.Model;
namespace MiniCafeBUS.IBUS
{
    public interface IShiftBUS
    {
        void AddShift(Shift shift);
        void DeleteShift(int id);
        void UpdateShift(Shift shift);
        Shift GetShiftById(int id);
        List<Shift> GetAllShifts();
    }
}
