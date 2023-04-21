using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniCafeDAL.Model;

namespace MiniCafeDAL.IDAL
{
    public interface IEmployeeDAL
    {
        void AddEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(int id);
        Employee GetEmployeeById(int id);
        Employee GetEmployeeByUsernameAndPassword(string username, string password);
        List<Employee> GetAllEmployees();
        List<Employee> GetEmployeesByShiftId(int shiftId);
        List<Employee> GetEmployeesOnActive();
    }
}
