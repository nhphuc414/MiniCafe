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

        private ProductBUS()
        {
           
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
            if (product.name == "") { throw new Exception("Tên sản phẩm không được để trống"); }
            if (ProductDAL.Instance.GetAllProducts().Any(p=>p.name==product.name &&p.categoryId==product.categoryId)) { 
                throw new Exception("Đã có sản phẩm này"); 
            }
            ProductDAL.Instance.AddProduct(product);
        }

        public void DeleteProduct(int id)
        { 
            if (OrderDetailDAL.Instance.GetOrderDetailsByProductId(id).Any())
            {
                throw new Exception("Không thể xóa sản phẩm đã từng bán");
            }
            ProductDAL.Instance.DeleteProduct(id);
        }

        public List<Product> GetAllProducts()
        {
            return ProductDAL.Instance.GetAllProducts();
        }

        public Product GetProductById(int id)
        {
            return ProductDAL.Instance.GetProductById(id);
        }

        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            return ProductDAL.Instance.GetProductsByCategoryId(categoryId);
        }

        public List<Product> GetProductsIsActive()
        {
            return ProductDAL.Instance.GetProductsIsActive();
        }

        public void UpdateProduct(Product product)
        {
            if (product.name == "") { throw new Exception("Tên sản phẩm không được để trống"); }
            if (ProductDAL.Instance.GetAllProducts().Any(p =>!(p.id==product.id )&& p.name.Equals(product.name) && p.categoryId == product.categoryId))
            {
                throw new Exception("Đã có sản phẩm này");
            }
            ProductDAL.Instance.UpdateProduct(product);
        }

        public dynamic GetRevenue()
        {
            return ProductDAL.Instance.GetRevenue();
        }
    }
}
