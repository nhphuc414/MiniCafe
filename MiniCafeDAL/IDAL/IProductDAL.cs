using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniCafeDAL.Model;
namespace MiniCafeDAL.IDAL
{
    public interface IProductDAL
    {
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
        Product GetProductById(int id);
        List<Product> GetProductsByCategoryId(int categoryId);
        List<Product> GetProductsIsActive();
        List<Product> GetAllProducts();

    }
}
