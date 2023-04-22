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
        private static IShiftBUS instance = null;
        private static readonly object padlock = new object();

        private ShiftBUS()
        {
            
        }

        public static IShiftBUS Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ShiftBUS();
                    }
                    return instance;
                }
            }
        }

        public void AddShift(Shift shift)
        {
            if (shift == null)
            {
                throw new ArgumentNullException("Không được để trống");
            }
            if (shift.endTime <= shift.startTime)
            {
                throw new InvalidOperationException("Vui lòng chọn thời gian chính xác");
            }
            if (ShiftDAL.Instance.GetAllShifts().Any(
                s => (shift.startTime >= s.startTime && shift.startTime < s.endTime) ||
            (shift.endTime > s.startTime && shift.endTime <= s.endTime) ||
            (shift.startTime < s.startTime && shift.endTime > s.endTime)))
            {
                throw new ArgumentException("Trùng giờ với ca làm khác");
            }
            ShiftDAL.Instance.AddShift(shift);
        }

        public void DeleteShift(int id)
        {
            if (EmployeeDAL.Instance.GetEmployeesByShiftId(id).Any(e =>e.shiftId==id))
            {
                throw new ArgumentException("Vẫn còn nhân viên trong ca");
            }
            ShiftDAL.Instance.DeleteShift(id);
        }

        public List<Shift> GetAllShifts()
        {
            return ShiftDAL.Instance.GetAllShifts();
        }

        public Shift GetShiftById(int id)
        {
            return ShiftDAL.Instance.GetShiftById(id);
        }

        public void UpdateShift(Shift shift)
        {
            if (shift == null)
            {
                throw new ArgumentNullException("Không được để trống");
            }
            if (shift.endTime <= shift.startTime)
            {
                throw new InvalidOperationException("Vui lòng chọn thời gian chính xác");
            }
            if (ShiftDAL.Instance.GetAllShifts().Any(
                s => ((s.id != shift.id) && ((shift.startTime >= s.startTime && shift.startTime < s.endTime) ||
            (shift.endTime > s.startTime && shift.endTime <= s.endTime) ||
            (shift.startTime < s.startTime && shift.endTime > s.endTime)))))
            {
                throw new ArgumentException("Trùng giờ với ca làm khác");
            }
            ShiftDAL.Instance.UpdateShift(shift);
        }
    }
}
