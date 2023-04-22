using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniCafeBUS.IBUS.BUS;
using MiniCafeDAL.Model;

namespace MiniCafeGUI.Admin
{
    public partial class ProductManagementForm : Form
    {
        private List<Category> categories;
        private List<Product> products;
        int index = -1;
        public ProductManagementForm()
        {
            categories = new List<Category>();
            categories.AddRange(CategoryBUS.Instance.GetAllCategories());
            products = new List<Product>();
            products.AddRange(ProductBUS.Instance.GetAllProducts());
            InitializeComponent();
        }
        private void dataSource()
        {
            dataGridView.DataSource = products.Select(p => new
            {
                ID = p.id,
                Category=CategoryBUS.Instance.GetCategoryById(p.categoryId).ToString(),
                Name = p.name,
                Price = decimal.Round(p.unitPrice,2),
                Quantity = p.quantity,
                Status = p.discontinued==true? "Ngừng bán":"Hoạt động"

            }).ToList();
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
        private void ProductManagementForm_Load(object sender, EventArgs e)
        {
            cbCategory.DataSource = categories;
            cbCategory.SelectedIndex = 0;
            loadColumn();
            cbCategory.Focus();
            cbStatus.SelectedIndex = 0;
            cbStatus.Visible = false;
            lbStatus.Visible = false;
        }
        private void resetFields() {
            txtName.Text = "";
            txtPrice.Text = "";
            txtQuantity.Text = "";
            cbCategory.SelectedIndex = 0;
            cbStatus.SelectedIndex = 0;
            cbCategory.Focus();
        }
        private void showStatus(bool bol)
        {
            lbStatus.Visible = bol ;
            cbStatus.Visible = bol;
        }
        private void getInField(Product product)
        {
            product.name = txtName.Text.Trim();
            product.categoryId = categories[cbCategory.SelectedIndex].id;
            if (Decimal.TryParse(txtPrice.Text, out decimal price))
            {
                product.unitPrice = price;
            }
            else {
                txtPrice.Focus();
                throw new Exception("Vui lòng nhập giá tiền chính xác"); 
            }
            if (Int32.TryParse(txtQuantity.Text,out int quantity))
            {
                product.quantity = quantity;
            }
            else throw new Exception("Vui lòng nhập số lượng chính xác");
            product.discontinued = cbStatus.SelectedIndex==0?false:true;
        }
        private void loadOnField(Product product)
        {
            Product selectedProduct = products.First(p => p.id == product.id);
            cbCategory.SelectedIndex = categories.FindIndex(c =>c.id==selectedProduct.categoryId);
            txtName.Text = selectedProduct.name;
            txtPrice.Text = selectedProduct.unitPrice.ToString();
            txtQuantity.Text = selectedProduct.quantity.ToString();
            cbStatus.SelectedIndex = selectedProduct.discontinued ? 1:0;
        }
        private void disableAllButton()
        {

            btnAdd.Enabled = false;
        }
        private void enableAllButton()
        {
            btnAdd.Enabled = true;
        }
        private void beforeCommit(int index)
        {
            Product selectedProduct = new Product();
            disableAllButton();
            selectedProduct.id = Convert.ToInt32(dataGridView.Rows[index].Cells["id"].Value);
            loadOnField(selectedProduct);
            showStatus(true);
            dataGridView.CellContentClick -= dataGridView_CellContentClick;
            dataGridView.CellContentClick += dataGridView_CommitOrCancel;
        }
        private void afterCommitOrCancel()
        {
            enableAllButton();
            showStatus(false);
            dataGridView.CellContentClick -= dataGridView_CommitOrCancel;
            dataGridView.CellContentClick += dataGridView_CellContentClick;
            resetFields();
        }
        private void dataGridView_CommitOrCancel(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView.Columns["editColumn"].Index && e.RowIndex.GetHashCode() == index)
            {
                try
                {
                    Product selectedProduct = new Product();
                    selectedProduct.id = Convert.ToInt32(dataGridView.Rows[e.RowIndex].Cells["id"].Value);
                    getInField(selectedProduct);
                    ProductBUS.Instance.UpdateProduct(selectedProduct);
                    products[products.FindIndex(es => es.id == selectedProduct.id)] = selectedProduct;
                    dataSource();
                    MessageBox.Show("Cập nhật thành công");
                    afterCommitOrCancel();
                }
                catch (ArgumentException aex)
                {
                    MessageBox.Show(aex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (e.ColumnIndex == dataGridView.Columns["deleteColumn"].Index && e.RowIndex.GetHashCode() == index)
            {
                index = 0;
                afterCommitOrCancel();
            }
        }
        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
                        Product selectedProduct = products[e.RowIndex];
                        ProductBUS.Instance.DeleteProduct(selectedProduct.id);
                        products.Remove(selectedProduct);
                        dataSource();
                        MessageBox.Show("Xóa thành công");
                        resetFields();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);

                    }
                }
            }
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Product selectedProduct = new Product();
                getInField(selectedProduct);
                ProductBUS.Instance.AddProduct(selectedProduct);
                products.Add(selectedProduct);
                dataSource();
                MessageBox.Show("Thêm thành công");
                resetFields();
            }
            catch (ArgumentException aex)
            {
                cbCategory.Focus();
                MessageBox.Show(aex.Message);
            }
            catch (Exception ex)
            {
                cbCategory.Focus();
                MessageBox.Show(ex.Message);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            resetFields();
        }
    }
}
