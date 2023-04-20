using MiniCafeDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCafeDAL.IDAL.DAL
{
    internal class ProductDAL : IProductDAL
    {
        private readonly MiniCafeEntities _context;

        public ProductDAL(MiniCafeEntities context)
        {
            _context = context;
        }
        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }
        public void UpdateProduct(Product product)
        {
            _context.Entry(product).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
        public void DeleteProduct(int id)
        {
            Product productToDelete = _context.Products.Find(id);
            _context.Products.Remove(productToDelete);
            _context.SaveChanges();
        }
        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }
        public Product GetProductById(int id)
        {
            return _context.Products.Find(id);
        }
        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            return _context.Products.Where(p => p.categoryId == categoryId).ToList();
        }
        public List<Product> GetProductsIsActive()
        {
            return _context.Products.Where(p => !p.discontinued || p.quantity != 0).ToList();
        }
    }
}
