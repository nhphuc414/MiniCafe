using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniCafeDAL.Model;
using MiniCafeBUS.IBUS.BUS;
namespace MiniCafeGUI.EmployeeGUI
{
    public partial class OrderForm : Form
    {
        private List<Category> categories;
        private List<Product> products;
        private List<OrderDetail> orderDetails;
        int index = -1;
        public OrderForm()
        {
            
            InitializeComponent();
        }
        private void dataSource()
        {
            dataGridView.DataSource = orderDetails.Select((d,index) => new
            {
                STT = index+1,
                Category=CategoryBUS.Instance.GetCategoryById(ProductBUS.Instance.GetProductById(d.productId).categoryId),
                Product=ProductBUS.Instance.GetProductById(d.productId),
                Quantity = d.quantity,
                Price = d.unitPrice,
                TotalPrice = decimal.Round(d.unitPrice * d.quantity, 2)
            }).ToList();
            dataGridView.Refresh();
        }
        private void loadColumn()
        {
            dataGridView.Columns.Clear();
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
        private void OrderForm_Load(object sender, EventArgs e)
        {
            onLoad();
        }
        private void onLoad()
        {
            orderDetails = new List<OrderDetail>();
            categories = new List<Category>();
            categories.AddRange(CategoryBUS.Instance.GetAllCategories());
            products = new List<Product>();
            products.AddRange(ProductBUS.Instance.GetProductsIsActive());
            cbCategory.DataSource = categories;
            loadColumn();
            cbCategory.Focus();
            resetFields();
        }
        private void resetFields()
        {

            cbCategory.SelectedIndex = 0;
            cbCategory.Focus();
            txtQuantity.Text = "";
        }
        private void getInField(OrderDetail orderDetail)
        {
            if (txtQuantity.Text == "" || Convert.ToInt32(txtQuantity.Text)<=0) throw new Exception("Vui lòng nhập số lượng");
            orderDetail.quantity = Convert.ToInt32(txtQuantity.Text);
            orderDetail.productId = ((Product)cbProduct.SelectedValue).id;
            orderDetail.unitPrice = ((Product)cbProduct.SelectedValue).unitPrice;
            orderDetail.discount = 0;
        }
        private void loadOnField(OrderDetail orderDetail)
        {
            Category selectedCategory=CategoryBUS.Instance.GetCategoryById(ProductBUS.Instance.GetProductById(orderDetail.productId).categoryId);
            cbCategory.SelectedIndex = categories.FindIndex(c=>c.id==selectedCategory.id);
            cbProduct.SelectedIndex = products.Where(p => p.quantity != 0 && p.categoryId == ((Category)cbCategory.SelectedValue).id).ToList().FindIndex(p=>p.id==orderDetail.productId);
            txtQuantity.Text=orderDetail.quantity.ToString();
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
            OrderDetail selectedorderDetail = new OrderDetail();
            disableAllButton();
            selectedorderDetail = orderDetails[index];
            loadOnField(selectedorderDetail);
            dataGridView.CellContentClick -= dataGridView_CellContentClick;
            dataGridView.CellContentClick += dataGridView_CommitOrCancel;
        }
        private void afterCommitOrCancel()
        {
            enableAllButton();
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
                    OrderDetail selectedorderDetail = new OrderDetail();
                    getInField(selectedorderDetail);
                    if (selectedorderDetail.quantity >= ProductBUS.Instance.GetProductById(selectedorderDetail.productId).quantity)
                    {
                        throw new ArgumentException("Không đủ hàng để bán");
                    }
                    orderDetails[orderDetails.FindIndex(od => od.productId == selectedorderDetail.productId)] = selectedorderDetail;
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
                        OrderDetail selectedorderDetail = orderDetails[e.RowIndex];
                        orderDetails.Remove(selectedorderDetail);
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
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                OrderDetail orderDetail = new OrderDetail();
                getInField(orderDetail);
                if (orderDetail.quantity > ProductBUS.Instance.GetProductById(orderDetail.productId).quantity)
                {
                    throw new Exception("Số lượng trong kho không đủ");
                }
                if (orderDetails.Any(od => od.productId == orderDetail.productId))
                {
                    orderDetails.First(od=>od.productId==orderDetail.productId).quantity+=orderDetail.quantity;
                }
                else
                {
                    orderDetails.Add(orderDetail);
                }
                dataSource();
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }
        private void btnPay_Click(object sender, EventArgs e)
        {
            if (orderDetails.Count > 0)
            {
                decimal total = 0;
                foreach(var od in orderDetails)
                {
                    total += od.unitPrice * od.quantity;
                }
                PayForm payForm = new PayForm(orderDetails,total);
                payForm.StartPosition = FormStartPosition.CenterScreen;
                payForm.ShowDialog(this);
                if (payForm.DialogResult == DialogResult.OK)
                {
                    // Nếu người dùng xác thực, reset lại form hiện tại
                    this.onLoad();
                }
            }
            else
            {
                MessageBox.Show("Không có đơn hàng để thanh toán");
            }
        }
        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbProduct.DataSource = products.Where(p=>p.quantity!=0 && p.categoryId== ((Category)cbCategory.SelectedValue).id).ToList();
        }
    }
}
