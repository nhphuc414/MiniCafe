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
        private readonly IProductDAL _producDAL;

        public ProductBUS(IProductDAL producDAL)
        {
            _producDAL = producDAL;
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
