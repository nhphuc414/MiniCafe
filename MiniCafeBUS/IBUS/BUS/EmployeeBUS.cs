using MiniCafeDAL.Model;
using MiniCafeDAL.IDAL;
using MiniCafeDAL.IDAL.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniCafeBUS.Utils;

namespace MiniCafeBUS.IBUS.BUS
{
    public class EmployeeBUS : IEmployeeBUS
    {
        private static IEmployeeBUS instance = null;
        private static readonly object padlock = new object();
        private readonly IEmployeeDAL _employeeDAL;

        private EmployeeBUS()
        {
            _employeeDAL = EmployeeDAL.Instance;
        }

        public static IEmployeeBUS Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new EmployeeBUS();
                    }
                    return instance;
                }
            }
        }

        public void AddEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), "Thông tin nhân viên không được để trống.");
            }
            if (Utils.Utils.checkName(employee.employeeName)
                && Utils.Utils.checkUsername(employee.username)
                && Utils.Utils.checkBirthday(employee.birthday)
                && Utils.Utils.checkNumber(employee.number)){
                if (string.IsNullOrEmpty(employee.password))
                {
                    throw new ArgumentException("Mật khẩu không được để trống.");
                }

                if (_employeeDAL.GetAllEmployees().Any(e => e.username.Equals(employee.username)))
                {
                    throw new ArgumentException("Tên đăng nhập đã tồn tại trong hệ thống.");
                }
                _employeeDAL.AddEmployee(employee);
            }
        }

        public void DeleteEmployee(int id)
        {
            var employee = _employeeDAL.GetEmployeeById(id);
            if (employee == null)
            {
                throw new ArgumentException("Nhân viên không tồn tại.");
            }
            _employeeDAL.DeleteEmployee(id);
        }

        public List<Employee> GetAllEmployees()
        {
            return _employeeDAL.GetAllEmployees();
        }

        public Employee GetEmployeeById(int id)
        {
            var employee = _employeeDAL.GetEmployeeById(id);
            if (employee == null)
            {
                throw new ArgumentException("Nhân viên không tồn tại.");
            }
            return _employeeDAL.GetEmployeeById(id);
        }

        public Employee GetEmployeeByUsernameAndPassword(string username, string password)
        {
            if (String.IsNullOrEmpty(username)|| String.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Vui lòng điền hết thông tin");
            }
            Employee employee = _employeeDAL.GetEmployeeByUsernameAndPassword(username, password);
            if (employee == null)
            {
                throw new ArgumentException("Sai tài khoản hoặc mật khẩu.");
            }
            return employee;
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
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), "Thông tin nhân viên không được để trống.");
            }
            if (Utils.Utils.checkName(employee.employeeName)
                && Utils.Utils.checkUsername(employee.username)
                && Utils.Utils.checkBirthday(employee.birthday)
                && Utils.Utils.checkNumber(employee.number))
            {
                _employeeDAL.UpdateEmployee(employee);
            }
        }
    }
}
