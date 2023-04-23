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

        private EmployeeBUS()
        {
            
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
                && Utils.Utils.checkNumber(employee.number)
                && Utils.Utils.checkBirthday(employee.birthday)
                && Utils.Utils.checkUsername(employee.username)
               )
            {
                if (string.IsNullOrEmpty(employee.password))
                {
                    throw new ArgumentException("Mật khẩu không được để trống.");
                }

                if (EmployeeDAL.Instance.GetAllEmployees().Any(e => e.username.Equals(employee.username)))
                {
                    throw new ArgumentException("Tên đăng nhập đã tồn tại trong hệ thống.");
                }
                employee.status = false;
                EmployeeDAL.Instance.AddEmployee(employee);
            }
        }

        public void DeleteEmployee(int id)
        {
            var employee = EmployeeDAL.Instance.GetEmployeeById(id);
            if (employee == null)
            {
                throw new ArgumentException("Nhân viên không tồn tại.");
            }
            if (OrderDAL.Instance.GetOrderByEmployeeId(id).Any())
            {
                throw new ArgumentException("Nhân viên đã từng lập hóa đơn không thể xóa");
            }
            EmployeeDAL.Instance.DeleteEmployee(id);
        }

        public List<Employee> GetAllEmployees()
        {
            return EmployeeDAL.Instance.GetAllEmployees();
        }

        public Employee GetEmployeeById(int id)
        {
            var employee = EmployeeDAL.Instance.GetEmployeeById(id);
            if (employee == null)
            {
                throw new ArgumentException("Nhân viên không tồn tại.");
            }
            return EmployeeDAL.Instance.GetEmployeeById(id);
        }

        public Employee GetEmployeeByUsernameAndPassword(string username, string password)
        {
            if (String.IsNullOrEmpty(username)|| String.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Vui lòng điền hết thông tin");
            }
            Employee employee = EmployeeDAL.Instance.GetEmployeeByUsernameAndPassword(username, password);
            if (employee == null)
            {
                throw new ArgumentException("Sai tài khoản hoặc mật khẩu.");
            }
            return employee;
        }

        public List<Employee> GetEmployeesByShiftId(int shiftId)
        {

            return EmployeeDAL.Instance.GetEmployeesByShiftId(shiftId);
        }

        public List<Employee> GetEmployeesOnActive()
        {
            return EmployeeDAL.Instance.GetEmployeesOnActive();
        }

        public void UpdateEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), "Thông tin nhân viên không được để trống.");
            }
            if (Utils.Utils.checkName(employee.employeeName)
                && Utils.Utils.checkBirthday(employee.birthday)
                && Utils.Utils.checkNumber(employee.number))
            {
                if(EmployeeDAL.Instance.GetAllEmployees().Any(e =>e.id != employee.id && e.number == employee.number))
                {
                    throw new ArgumentNullException(nameof(employee), "Trùng số điện thoại.");
                } 
                if (employee.status && employee.isFired)
                {
                    throw new ArgumentException("Không thể sa thải nhân viên đang làm việc");
                }
                EmployeeDAL.Instance.UpdateEmployee(employee);
            }
        }
    }
}
