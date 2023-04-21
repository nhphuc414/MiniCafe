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
    public class ProductBUS : IProductBUS
    {
        
        private static IProductBUS instance = null;
        private static readonly object padlock = new object();
        private readonly IProductDAL _producDAL;

        private ProductBUS()
        {
            _producDAL = ProductDAL.Instance;
        }

        public static IProductBUS Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ProductBUS();
                    }
                    return instance;
                }
            }
        }

        public void AddProduct(Product product)
        {
            _producDAL.AddProduct(product);
        }

        public void DeleteProduct(int id)
        {
            _producDAL.DeleteProduct(id);
        }

        public List<Product> GetAllProducts()
        {
            return _producDAL.GetAllProducts();
        }

        public Product GetProductById(int id)
        {
            return _producDAL.GetProductById(id);
        }

        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            return _producDAL.GetProductsByCategoryId(categoryId);
        }

        public List<Product> GetProductsIsActive()
        {
            return _producDAL.GetProductsIsActive();
        }

        public void UpdateProduct(Product product)
        {
            _producDAL.UpdateProduct(product);
        }
    }
}
