using MiniCafeDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCafeDAL.IDAL.DAL
{
    public class OrderDAL : IOrderDAL
    {
        private static IOrderDAL instance = null;
        private static readonly object padlock = new object();

        private OrderDAL()
        {
        }

        public static IOrderDAL Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAL();
                    }
                    return instance;
                }
            }
        }
        public void AddOrder(Order order)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                entities.Orders.Add(order);
                entities.SaveChanges();
            }

        }
        public void UpdateOrder(Order order)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                entities.Entry(order).State = System.Data.Entity.EntityState.Modified;
                entities.SaveChanges();
            }

        }
        public void DeleteOrder(int id)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                Order orderToDelete = entities.Orders.Find(id);
                entities.Orders.Remove(orderToDelete);
                entities.SaveChanges();
            }

        }

        public List<Order> GetAllOrders()
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                return entities.Orders.ToList();
            }

        }

        public List<Order> GetOrderByEmployeeId(int employeeId)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                return entities.Orders.Where(o => o.employeeId == employeeId).ToList();
            }

        }

        public List<Order> GetOrderFromDayToDay(DateTime fromDate, DateTime toDate)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                return entities.Orders.Where(o => o.createDate >= fromDate && o.createDate <= toDate).ToList();
            }

        }
        public Order GetOrderById(int id)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                return entities.Orders.Find(id);
            }

        }

    }
}
