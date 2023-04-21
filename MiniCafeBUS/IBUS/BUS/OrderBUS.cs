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
        private readonly IOrderDAL _orderDAL;
        private OrderBUS()
        {
            _orderDAL = OrderDAL.Instance;
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
            _orderDAL.AddOrder(order);
        }

        public void DeleteOrder(int id)
        {
            _orderDAL.DeleteOrder(id);
        }

        public List<Order> GetAllOrders()
        {
            return _orderDAL.GetAllOrders();
        }

        public List<Order> GetOrderByEmployeeId(int employeeId)
        {
            return _orderDAL.GetOrderByEmployeeId(employeeId);
        }

        public Order GetOrderById(int id)
        {
            return _orderDAL.GetOrderById(id);
        }

        public List<Order> GetOrderFromDayToDay(DateTime fromDate, DateTime toDate)
        {
            return _orderDAL.GetOrderFromDayToDay(fromDate, toDate);
        }

        public void UpdateOrder(Order order)
        {
            _orderDAL.UpdateOrder(order);
        }
    }
}
