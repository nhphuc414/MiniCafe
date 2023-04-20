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
    public class EmployeeBUS : IEmployeeBUS
    {
        private readonly IEmployeeDAL _employeeDAL;

        public EmployeeBUS(IEmployeeDAL employeeDAL)
        {
            _employeeDAL = employeeDAL;
        }

        public void AddEmployee(Employee employee)
        {
            _employeeDAL.AddEmployee(employee);
        }

        public void DeleteEmployee(int id)
        {
            _employeeDAL.DeleteEmployee(id);
        }

        public List<Employee> GetAllEmployees()
        {
            return _employeeDAL.GetAllEmployees();
        }

        public Employee GetEmployeeById(int id)
        {
            return _employeeDAL.GetEmployeeById(id);
        }

        public List<Employee> GetEmployeesByShiftId(int shiftId)
        {
            return _employeeDAL.GetEmployeesByShiftId(shiftId);
        }

        public List<Employee> GetEmployeesOnActive()
        {
            return _employeeDAL.GetEmployeesOnActive();
        }

        public void UpdateEmployee(Employee employee)
        {
            _employeeDAL.UpdateEmployee(employee);
        }
    }
}
