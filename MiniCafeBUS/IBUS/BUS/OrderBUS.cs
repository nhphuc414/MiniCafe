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
    public class OrderBUS : IOrderBUS
    {
        private static IOrderBUS instance = null;
        private static readonly object padlock = new object();
        private OrderBUS()
        {
           
        }

        public static IOrderBUS Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new OrderBUS();
                    }
                    return instance;
                }
            }
        }

        public void AddOrder(Order order)
        {
            OrderDAL.Instance.AddOrder(order);
        }

        public void DeleteOrder(int id)
        {
            OrderDAL.Instance.DeleteOrder(id);
        }

        public List<Order> GetAllOrders()
        {
            return OrderDAL.Instance.GetAllOrders();
        }

        public List<Order> GetOrderByEmployeeId(int employeeId)
        {
            return OrderDAL.Instance.GetOrderByEmployeeId(employeeId);
        }
        public Order GetOrderByTableId(int id)
        {
            return OrderDAL.Instance.GetOrderByTableId(id);
        }
        public Order GetOrderById(int id)
        {
            return OrderDAL.Instance.GetOrderById(id);
        }

        public List<Order> GetOrderFromDayToDay(DateTime fromDate, DateTime toDate)
        {
            return OrderDAL.Instance.GetOrderFromDayToDay(fromDate, toDate);
        }

        public void UpdateOrder(Order order)
        {
            OrderDAL.Instance.UpdateOrder(order);
        }
    }
}
