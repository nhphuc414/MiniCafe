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
        private readonly IOrderDAL _orderDAL;

        public OrderBUS(IOrderDAL orderDAL)
        {
            _orderDAL = orderDAL;
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
