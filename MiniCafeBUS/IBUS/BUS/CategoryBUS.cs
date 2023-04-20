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
        private readonly ICategoryDAL _categoryDAL;

        public CategoryBUS(ICategoryDAL categoryDAL)
        {
            _categoryDAL = categoryDAL;
        }

        public void AddCategory(Category category)
        {
            if (_categoryDAL.GetAllCategories().Any(c => c.name.Equals(category.name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Tên Category đã tồn tại.");
            }
            _categoryDAL.AddCategory(category);
        }

        public void DeleteCategory(int id)
        {
            var category = _categoryDAL.GetCategoryById(id);
            if (category == null)
            {
                throw new ArgumentException("Category không tồn tại.");
            }
            if (category.Products.Any())
            {
                throw new ArgumentException("Không thể xóa Category chứa sản phẩm.");
            }
            _categoryDAL.DeleteCategory(id);
        }

        public List<Category> GetAllCategories()
        {

            return _categoryDAL.GetAllCategories();
        }

        public Category GetCategoryById(int id)
        {
            var category = _categoryDAL.GetCategoryById(id);

            if (category == null)
            {
                throw new ArgumentException("Category không tồn tại.");
            }

            return category;
        }

        public void UpdateCategory(Category category)
        {
            if (_categoryDAL.GetAllCategories().Any(c => c.id != category.id && c.name.Equals(category.name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Tên Category đã tồn tại.");
            }
            _categoryDAL.UpdateCategory(category);
        }
    }
}
