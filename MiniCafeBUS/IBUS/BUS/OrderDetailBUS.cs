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

        private OrderDetailBUS()
        {
           
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
            OrderDetailDAL.Instance.AddOrderDetail(orderDetail);
        }

        public void DeleteOrderDetail(OrderDetail orderDetail)
        {
            OrderDetailDAL.Instance.DeleteOrderDetail(orderDetail);
        }

        public List<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            return OrderDetailDAL.Instance.GetOrderDetailsByOrderId(orderId);
        }

        public void UpdateOrderDetail(OrderDetail orderDetail)
        {
            OrderDetailDAL.Instance.UpdateOrderDetail(orderDetail);
        }
        public List<OrderDetail> GetOrderDetailsByProductId(int productId)
        {
           return  OrderDetailDAL.Instance.GetOrderDetailsByProductId(productId);
        }


    }
}
