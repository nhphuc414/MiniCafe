using MiniCafeDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCafeDAL.IDAL.DAL
{
    public class CategoryDAL : ICategoryDAL
    {
        private readonly MiniCafeEntities _context;

        public CategoryDAL(MiniCafeEntities context)
        {
            _context = context;
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }
        public void UpdateCategory(Category category)
        {
            _context.Entry(category).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
        public void DeleteCategory(int id)
        {
            Category categoryToDelete = _context.Categories.Find(id);
            _context.Categories.Remove(categoryToDelete);
            _context.SaveChanges();
        }
        public Category GetCategoryById(int id)
        {
            return _context.Categories.Find(id);
        }
        public List<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

    }
}
