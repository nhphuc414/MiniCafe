using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniCafeDAL.Model;
namespace MiniCafeBUS.IBUS
{
    public interface IOrderBUS
    {
        List<Order> GetAllOrders();
        List<Order> GetOrderByEmployeeId(int employeeId);
        List<Order> GetOrderFromDayToDay(DateTime fromDate, DateTime toDate);
        Order GetOrderById(int id);
        void AddOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(int id);
    }
}
