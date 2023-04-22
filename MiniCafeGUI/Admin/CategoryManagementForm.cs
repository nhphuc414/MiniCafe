using MiniCafeBUS.IBUS.BUS;
using MiniCafeDAL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniCafeGUI.Admin
{
    public partial class CategoryManagementForm : Form
    {
        private List<Category> categories;
        int index=-1;
        public CategoryManagementForm()
        {
            categories = new List<Category>();
            categories.AddRange(CategoryBUS.Instance.GetAllCategories());
            InitializeComponent();
            
        }
        private void dataSource()
        {
            dataGridView.DataSource = categories.Select(c => new { 
                ID = c.id, 
                Name =c.name, 
                Description =c.description,
                TotalProduct=ProductBUS.Instance.GetProductsByCategoryId(c.id).Count}).ToList();
            dataGridView.Refresh();
        }
        private void loadColumn()
        {
            // Tạo cột "Sửa"
            var editColumn = new DataGridViewButtonColumn();
            editColumn.HeaderText = "Edit";
            editColumn.Name = "editColumn";
            editColumn.Text = "Sửa";
            editColumn.UseColumnTextForButtonValue = true;
            // Tạo cột "Xóa"
            var deleteColumn = new DataGridViewButtonColumn();
            deleteColumn.HeaderText = "Delete";
            deleteColumn.Name = "deleteColumn";
            deleteColumn.Text = "Xóa";
            deleteColumn.UseColumnTextForButtonValue = true;
            //add vào datagridview

            dataSource();
            dataGridView.Columns.Add(editColumn);
            dataGridView.Columns.Add(deleteColumn);

            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView.ScrollBars = ScrollBars.Both;
        }
        private void CategoryManagementForm_Load(object sender, EventArgs e)
        {
            loadColumn();
            txtName.Focus();
        }
        private void resetField()
        {
            txtDescription.Text = "";
            txtName.Text = "";
            txtName.Focus();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            resetField();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Category category = new Category();
            category.name = txtName.Text;
            category.description = txtDescription.Text;
            try
            {
                CategoryBUS.Instance.AddCategory(category);
                categories.Add(category);
                dataSource();
                MessageBox.Show("Thêm thành công");
                resetField();
            }
            catch (ArgumentException aex)
            {
                txtName.Focus();
                MessageBox.Show(aex.Message);
            }
            catch (Exception ex)
            {
                txtName.Focus();
                MessageBox.Show(ex.Message);
            }
        }
        private void disableAllButton()
        {
            btnAdd.Enabled = false;
        }
        private void enableAllButton()
        {
            
            btnAdd.Enabled=true;
        }
        private void loadOnFields(Category category)
        {
            txtName.Text = categories.FirstOrDefault(c => c.id == category.id).name;
            txtDescription.Text = categories.FirstOrDefault(c => c.id == category.id).description;
            txtName.Focus();
        }
        private void beforeCommit(int index)
        {
            Category selectedCategory = new Category();
            selectedCategory.id = Convert.ToInt32(dataGridView.Rows[index].Cells["id"].Value);
            disableAllButton();
            //Đưa Category lên field để chỉnh sửa
            loadOnFields(selectedCategory);
            dataGridView.CellContentClick -= dataGridViewCategory_CellContentClick;
            dataGridView.CellContentClick += dataGridViewCategory_CommitOrCancel;
            
        }
        private void afterCommitOrCancel()
        {
            enableAllButton();
            dataGridView.CellContentClick -= dataGridViewCategory_CommitOrCancel;
            dataGridView.CellContentClick += dataGridViewCategory_CellContentClick;
            resetField();
        }
        
        private void dataGridViewCategory_CommitOrCancel(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView.Columns["editColumn"].Index && e.RowIndex.GetHashCode() ==index)
            {
                Category selectedCategory = new Category();
                selectedCategory.id = Convert.ToInt32(dataGridView.Rows[e.RowIndex].Cells["id"].Value);
                selectedCategory.name = txtName.Text.Trim();
                selectedCategory.description=txtDescription.Text.Trim();
                try
                {
                    CategoryBUS.Instance.UpdateCategory(selectedCategory);
                    categories[categories.FindIndex(c => c.id == selectedCategory.id)] = selectedCategory;
                    dataSource();
                    MessageBox.Show("Cập nhật thành công");
                    afterCommitOrCancel();
                }
                catch (ArgumentException aex)
                {
                    MessageBox.Show(aex.Message);
                } catch(Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            }
            if (e.ColumnIndex == dataGridView.Columns["deleteColumn"].Index && e.RowIndex.GetHashCode() ==index)
            {
                index = -1;
                afterCommitOrCancel();
            }
        }
        private void dataGridViewCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView.Columns["editColumn"].Index && e.RowIndex >= 0 && e.RowIndex < dataGridView.Rows.Count)
            {
                index = e.RowIndex.GetHashCode();
                beforeCommit(e.RowIndex);
            }
            if (e.ColumnIndex == dataGridView.Columns["deleteColumn"].Index && e.RowIndex >= 0 && e.RowIndex < dataGridView.Rows.Count)
            {
                // Hiển thị hộp thoại xác nhận
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Nếu người dùng xác nhận xóa
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        Category selectedCategory = categories[e.RowIndex];                    
                        CategoryBUS.Instance.DeleteCategory(selectedCategory.id);
                        categories.Remove(selectedCategory);
                        dataSource();
                        MessageBox.Show("Xóa thành công");
                        resetField();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
