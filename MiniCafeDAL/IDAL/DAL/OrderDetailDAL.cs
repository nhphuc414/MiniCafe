using MiniCafeDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCafeDAL.IDAL.DAL
{
    public class OrderDetailDAL : IOrderDetailDAL
    {
        private static IOrderDetailDAL instance = null;
        private static readonly object padlock = new object();

        private OrderDetailDAL()
        {
            
        }

        public static IOrderDetailDAL Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDetailDAL();
                    }
                    return instance;
                }
            }
        }
        public void AddOrderDetail(OrderDetail orderDetail)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                entities.OrderDetails.Add(orderDetail);
                entities.SaveChanges();
            }
           
        }
        public void DeleteOrderDetail(OrderDetail orderDetail)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities()) {
                entities.OrderDetails.Remove(orderDetail);
                entities.SaveChanges();
            }
            
        }
        public List<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities()) { 
                return entities.OrderDetails.Where(od => od.orderId == orderId).ToList(); 
            }
            
        }
        
        public void UpdateOrderDetail(OrderDetail orderDetail)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities()) {
                entities.Entry(orderDetail).State = System.Data.Entity.EntityState.Modified;
                entities.SaveChanges();
            }
            
        }
        public List<OrderDetail> GetOrderDetailsByProductId(int productId)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                return entities.OrderDetails.Where(od => od.productId == productId).ToList();
            }

        }

    }
}
