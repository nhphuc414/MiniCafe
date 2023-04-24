using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniCafeDAL.Model;
namespace MiniCafeDAL.IDAL
{
    public interface IOrderDetailDAL
    {
        List<OrderDetail> GetOrderDetailsByOrderId(int orderId);
        void AddOrderDetail(OrderDetail orderDetail);
        void UpdateOrderDetail(OrderDetail orderDetail);
        void DeleteOrderDetail(OrderDetail orderDetail);
        List<OrderDetail> GetOrderDetailsByProductId(int productId);
    }
}
