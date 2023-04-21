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
        private static IShiftDAL instance = null;
        private static readonly object padlock = new object();

        private ShiftDAL()
        {

        }

        public static IShiftDAL Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ShiftDAL();
                    }
                    return instance;
                }
            }
        }
        public void AddShift(Shift shift)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                entities.Shifts.Add(shift);
                entities.SaveChanges();
            }
        }

        public void UpdateShift(Shift shift)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                entities.Entry(shift).State = System.Data.Entity.EntityState.Modified;
                entities.SaveChanges();
            }

        }
        public void DeleteShift(int id)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                Shift shiftToDelete = entities.Shifts.Find(id);
                entities.Shifts.Remove(shiftToDelete);
                entities.SaveChanges();
            }

        }
        public Shift GetShiftById(int id)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                return entities.Shifts.Find(id);
            }

        }
        public List<Shift> GetAllShifts()
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                return entities.Shifts.ToList();
            }

        }
    }
}
