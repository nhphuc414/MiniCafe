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
            dataGridView.DataSource = categories.Select(c => new {c.id, c.name,c.description}).ToList();
            dataGridView.Columns.Add(editColumn);
            dataGridView.Columns.Add(deleteColumn);

            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView.ScrollBars = ScrollBars.Both;
        }
        private void CategoryManagementForm_Load(object sender, EventArgs e)
        {
            loadColumn();
        }
        private void resetField()
        {
            txtDescription.Text = "";
            txtName.Text = "";
            dataGridView.Refresh();
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
                dataGridView.DataSource=categories.Select(c => new { c.id, c.name, c.description }).ToList();
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
        private void disableAllButton(int indexRow)
        {
            dataGridView.ReadOnly = true;
            dataGridView.Rows[indexRow].Cells["editColumn"].ReadOnly = false;
            dataGridView.Rows[indexRow].Cells["deleteColumn"].ReadOnly = false;
            btnAdd.Enabled = false;
        }
        private void enableAllButton()
        {
            dataGridView.Enabled=true;
            btnAdd.Enabled=true;
        }
        private void beforeCommit(int index)
        {
            Category selectedCategory = new Category();
            selectedCategory.id = Convert.ToInt32(dataGridView.Rows[index].Cells["id"].Value);
            disableAllButton(index);
            //Đưa Category lên field để chỉnh sửa
            txtName.Text = categories.FirstOrDefault(c => c.id== selectedCategory.id).name;
            txtDescription.Text = categories.FirstOrDefault(c => c.id == selectedCategory.id).description;
            txtName.Focus();
            dataGridView.Rows[index].Cells["editColumn"].Value = "Cập nhật";
            dataGridView.Rows[index].Cells["deleteColumn"].Value = "Hủy bỏ";
           
            dataGridView.CellContentClick -= dataGridViewCategory_CellContentClick;
            dataGridView.CellContentClick += dataGridViewCategory_CommitOrCancel;
            
        }
        private void afterCommitOrCancel(int index)
        {
            
            enableAllButton();
            dataGridView.Rows[index].Cells["editColumn"].Value = "Sửa";
            dataGridView.Rows[index].Cells["deleteColumn"].Value = "Xóa";
            dataGridView.CellContentClick -= dataGridViewCategory_CommitOrCancel;
            dataGridView.CellContentClick += dataGridViewCategory_CellContentClick;
            resetField();
        }
        
        private void dataGridViewCategory_CommitOrCancel(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView.Columns["editColumn"].Index && e.RowIndex ==index)
            {
                Category selectedCategory = new Category();
                selectedCategory.id = Convert.ToInt32(dataGridView.Rows[e.RowIndex].Cells["id"].Value);
                selectedCategory.name = txtName.Text.Trim();
                selectedCategory.description=txtDescription.Text.Trim();
                try
                {
                    CategoryBUS.Instance.UpdateCategory(selectedCategory);  
                    categories.FirstOrDefault(c => c.id == selectedCategory.id).name = selectedCategory.name;
                    categories.FirstOrDefault(c => c.id == selectedCategory.id).description = selectedCategory.description;
                    dataGridView.DataSource = categories.Select(c => new { c.id, c.name, c.description }).ToList();   
                    MessageBox.Show("Cập nhật thành công");
                    afterCommitOrCancel(e.RowIndex);
                }
                catch (ArgumentException aex)
                {
                    MessageBox.Show(aex.Message);
                } catch(Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            }
            if (e.ColumnIndex == dataGridView.Columns["deleteColumn"].Index && e.RowIndex ==index)
            {
                index = -1;
                afterCommitOrCancel(e.RowIndex);
            }
        }
        private void dataGridViewCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView.Columns["editColumn"].Index && e.RowIndex >= 0 && e.RowIndex < dataGridView.Rows.Count)
            {
                index = e.RowIndex;
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
                        dataGridView.DataSource= categories.Select(c => new { c.id, c.name, c.description }).ToList();
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
