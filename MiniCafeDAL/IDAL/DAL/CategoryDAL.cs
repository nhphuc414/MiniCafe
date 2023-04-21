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
        private static ICategoryDAL instance = null;
        private static readonly object padlock = new object();
        private CategoryDAL()
        {
        }

        public static ICategoryDAL Instance
        {
            get
            {
                    if (instance == null)
                    {
                        instance = new CategoryDAL();
                    }
                    return instance;
            }
        }

        public void AddCategory(Category category)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                entities.Categories.Add(category);
                entities.SaveChanges();
            }
        }
        public void UpdateCategory(Category category)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                entities.Entry(category).State = System.Data.Entity.EntityState.Modified;
                entities.SaveChanges();
            }
        }
        public void DeleteCategory(int id)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                Category categoryToDelete = entities.Categories.Find(id);
                entities.Categories.Remove(categoryToDelete);
                entities.SaveChanges();
            }
        }
        public Category GetCategoryById(int id)
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                return entities.Categories.Find(id);
            }
        }
        public List<Category> GetAllCategories()
        {
            using (MiniCafeEntities entities = new MiniCafeEntities())
            {
                return entities.Categories.ToList();
            }
        }

    }
}
