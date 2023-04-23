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
    public class OrderDetailBUS : IOrderDetailBUS
    {
        private static IOrderDetailBUS instance = null;
        private static readonly object padlock = new object();
        private readonly IOrderDetailDAL _orderDetailDAL;

        private OrderDetailBUS()
        {
            _orderDetailDAL = OrderDetailDAL.Instance;
        }

        public static IOrderDetailBUS Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDetailBUS();
                    }
                    return instance;
                }
            }
        }

        public void AddOrderDetail(OrderDetail orderDetail)
        {
            _orderDetailDAL.AddOrderDetail(orderDetail);
        }

        public void DeleteOrderDetail(OrderDetail orderDetail)
        {
            _orderDetailDAL.DeleteOrderDetail(orderDetail);
        }

        public List<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            return _orderDetailDAL.GetOrderDetailsByOrderId(orderId);
        }

        public void UpdateOrderDetail(OrderDetail orderDetail)
        {
            _orderDetailDAL.UpdateOrderDetail(orderDetail);
        }
        
    }
}
