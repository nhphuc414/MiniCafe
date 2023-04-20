using MiniCafeDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCafeDAL.IDAL.DAL
{
    internal class EmployeeDAL : IEmployeeDAL
    {
        private readonly MiniCafeEntities _context;

        public EmployeeDAL(MiniCafeEntities context)
        {
            _context = context;
        }
        public void AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }
        public void UpdateEmployee(Employee employee)
        {
            _context.Entry(employee).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
        public void DeleteEmployee(int id)
        {
            Employee employeeToDelete = _context.Employees.Find(id);
            _context.Employees.Remove(employeeToDelete);
            _context.SaveChanges();
        }
        public Employee GetEmployeeById(int id)
        {
            return _context.Employees.Find(id);
        }
        public List<Employee> GetAllEmployees()
        {
            return _context.Employees.ToList();
        }
        public List<Employee> GetEmployeesByShiftId(int shiftId)
        {
            return _context.Employees.Where(e => e.shiftId == shiftId).ToList();
        }
        public List<Employee> GetEmployeesOnActive()
        {
            return _context.Employees.Where(e => !e.isFired).ToList();
        }


    }
}
