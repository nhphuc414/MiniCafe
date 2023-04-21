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
    public class CategoryBUS : ICategoryBUS
    {
        private static ICategoryBUS instance = null;
        private CategoryBUS()
        {
           
        }

        public static ICategoryBUS Instance
        {
            get
            {
                    if (instance == null)
                    {
                        instance = new CategoryBUS();
                    }
                    return instance;
            }
        }

        public void AddCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException("Không có loại sản phẩm để thêm vào");
            }
            if (String.IsNullOrEmpty(category.name)|| String.IsNullOrEmpty(category.description))
            {
                throw new ArgumentException("Vui lòng nhập đầy đủ thông tin");
            }
            if (CategoryDAL.Instance.GetAllCategories().Any(c => c.name.Equals(category.name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Tên Category đã tồn tại.");
            }
            CategoryDAL.Instance.AddCategory(category);
        }

        public void DeleteCategory(int id)
        {
            var category = CategoryDAL.Instance.GetCategoryById(id);
            if (category == null)
            {
                throw new ArgumentException("Category không tồn tại.");
            }
            if (ProductDAL.Instance.GetProductsByCategoryId(id).Any(p => p.categoryId==id))
            {
                throw new ArgumentException("Không thể xóa Category chứa sản phẩm.");
            }
            CategoryDAL.Instance.DeleteCategory(id);
        }

        public List<Category> GetAllCategories()
        {

            return CategoryDAL.Instance.GetAllCategories();
        }

        public Category GetCategoryById(int id)
        {
            var category = CategoryDAL.Instance.GetCategoryById(id);

            if (category == null)
            {
                throw new ArgumentException("Category không tồn tại.");
            }

            return category;
        }

        public void UpdateCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException("Không có loại sản phẩm để thêm vào");
            }
            if (String.IsNullOrEmpty(category.name) || String.IsNullOrEmpty(category.description))
            {
                throw new ArgumentException("Vui lòng nhập đầy đủ thông tin");
            }
            if (CategoryDAL.Instance.GetAllCategories().Any(c => c.id!=category.id && c.name.Equals(category.name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Tên Category đã tồn tại.");
            }

            CategoryDAL.Instance.UpdateCategory(category);
        }
    }
}
