using MiniCafeDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCafeDAL.IDAL.DAL
{
    internal class OrderDetailDAL : IOrderDetailDAL
    {
        private readonly MiniCafeEntities _context;

        public OrderDetailDAL(MiniCafeEntities context)
        {
            _context = context;
        }
        public void AddOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            _context.SaveChanges();
        }
        public void DeleteOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetails.Remove(orderDetail);
            _context.SaveChanges();
        }
        public List<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            return _context.OrderDetails.Where(od => od.orderId == orderId).ToList();
        }
        public void UpdateOrderDetail(OrderDetail orderDetail)
        {
            _context.Entry(orderDetail).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

    }
}
