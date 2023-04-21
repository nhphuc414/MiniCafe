using MiniCafeDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCafeDAL.IDAL.DAL
{
    public class ProductDAL : IProductDAL
    {
        private static IProductDAL instance = null;
        private static readonly object padlock = new object();
        private ProductDAL()
        {
            
        }

        public static IProductDAL Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ProductDAL();
                    }
                    return instance;
                }
            }
        }
        public void AddProduct(Product product)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities()) {
                entities.Products.Add(product);
                entities.SaveChanges();
            }
           
        }
        public void UpdateProduct(Product product)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities()) {
                entities.Entry(product).State = System.Data.Entity.EntityState.Modified;
                entities.SaveChanges();
            }
            
        }
        public void DeleteProduct(int id)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities()) {
                Product productToDelete = entities.Products.Find(id);
                entities.Products.Remove(productToDelete);
                entities.SaveChanges();
            }
           
        }
        public List<Product> GetAllProducts()
        {
            using (MiniCafeEntities entities = new MiniCafeEntities()) {
                return entities.Products.ToList();
            }
            
        }
        public Product GetProductById(int id)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities()) {
                return entities.Products.Find(id);
            }
            
        }
        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities()) {
                return entities.Products.Where(p => p.categoryId == categoryId).ToList();
            }
           
        }
        public List<Product> GetProductsIsActive()
        {
            using (MiniCafeEntities entities = new MiniCafeEntities()) {
                return entities.Products.Where(p => !p.discontinued || p.quantity != 0).ToList();
            }
            
        }
    }
}
