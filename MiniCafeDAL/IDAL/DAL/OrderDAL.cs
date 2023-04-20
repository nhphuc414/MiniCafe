using MiniCafeDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCafeDAL.IDAL.DAL
{
    internal class OrderDAL : IOrderDAL
    {
        private readonly MiniCafeEntities _context;

        public OrderDAL(MiniCafeEntities context)
        {
            _context = context;
        }
        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }
        public void UpdateOrder(Order order)
        {
            _context.Entry(order).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
        public void DeleteOrder(int id)
        {
            Order orderToDelete = _context.Orders.Find(id);
            _context.Orders.Remove(orderToDelete);
            _context.SaveChanges();
        }

        public List<Order> GetAllOrders()
        {
            return _context.Orders.ToList();
        }

        public List<Order> GetOrderByEmployeeId(int employeeId)
        {
            return _context.Orders.Where(o => o.employeeId == employeeId).ToList();
        }

        public List<Order> GetOrderFromDayToDay(DateTime fromDate, DateTime toDate)
        {
            return _context.Orders.Where(o => o.createDate >= fromDate && o.createDate <= toDate).ToList();
        }
        public Order GetOrderById(int id)
        {
            return _context.Orders.Find(id);
        }

    }
}
