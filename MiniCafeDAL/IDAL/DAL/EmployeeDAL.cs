using MiniCafeDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCafeDAL.IDAL.DAL
{
    public class EmployeeDAL : IEmployeeDAL
    {
        private static IEmployeeDAL instance = null;
        private static readonly object padlock = new object();
        

        private EmployeeDAL()
        {
        }

        public static IEmployeeDAL Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new EmployeeDAL();
                    }
                    return instance;
                }
            }
        }
        public void AddEmployee(Employee employee)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                entities.Employees.Add(employee);
                entities.SaveChanges();
            }
        }
        public void UpdateEmployee(Employee employee)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                entities.Entry(employee).State = System.Data.Entity.EntityState.Modified;
                entities.SaveChanges();
            }
        }
        public void DeleteEmployee(int id)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                Employee employeeToDelete = entities.Employees.Find(id);
                entities.Employees.Remove(employeeToDelete);
                entities.SaveChanges();
            }
               
        }
        public Employee GetEmployeeById(int id)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                return entities.Employees.Find(id);
            }
        }
        public List<Employee> GetAllEmployees()
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                return entities.Employees.ToList();
            }
        }
        public List<Employee> GetEmployeesByShiftId(int shiftId)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                return entities.Employees.Where(e => e.shiftId == shiftId).ToList();
            }
        }
        public List<Employee> GetEmployeesOnActive()
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                return entities.Employees.Where(e => !e.isFired && e.status==false).ToList();
            }
        }
        public Employee GetEmployeeByUsernameAndPassword(string username, string password)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                return entities.Employees.FirstOrDefault(e => e.username.Equals(username) && e.password.Equals(password));

            }
        }
    }
}
