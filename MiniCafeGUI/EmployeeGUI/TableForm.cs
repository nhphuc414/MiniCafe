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
namespace MiniCafeGUI.EmployeeGUI
{
    public partial class TableForm : Form
    {
        private List<Category> categories;
        private List<Product> products;
        private List<OrderDetail> orderDetails;
        private List<Table> tables;
        private decimal total;
        public TableForm()
        {
            InitializeComponent();
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
        private void onLoad()
        {
            dataGridView.DataSource = null;
            dataGridView.Refresh();
            flowLayoutPanel.Controls.Clear();
            flowLayoutPanel.Refresh();  
            tables = new List<Table>();
            tables.AddRange(TableBUS.Instance.GetAllTables());
            categories = new List<Category>();
            categories.AddRange(CategoryBUS.Instance.GetAllCategories());
            products = new List<Product>();
            products.AddRange(ProductBUS.Instance.GetProductsIsActive());
            cbCategory.DataSource = categories;
            cbCategory.SelectedIndex = 0;
            foreach (Table table in tables)
            {
               
                Button button = new Button();
                button.Text = table.name + "\n"+ (table.status?"Có người":"Trống");
                button.Size = new System.Drawing.Size(80, 80);
                button.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                button.ForeColor = table.status ? System.Drawing.Color.White : System.Drawing.Color.Black;
                button.BackColor = table.status ? System.Drawing.Color.Maroon : System.Drawing.Color.Silver;
                if (table.status)
                {
                    button.Click += new EventHandler(button_Click);
                }
                flowLayoutPanel.Controls.Add(button);
            }
           
        }
        private void button_Click(object sender, EventArgs e)
        {
            // Lấy thông tin về đối tượng Table được chọn từ Text của Button
            int i =flowLayoutPanel.Controls.IndexOf((Button)sender);
            orderDetails = new List<OrderDetail>();
            Order order = OrderBUS.Instance.GetOrderByTableId(tables[i].id);
            orderDetails.AddRange(OrderDetailBUS.Instance.GetOrderDetailsByOrderId(order.id));
            loadColumn();
        }
        private void dataSource()
        {
            dataGridView.DataSource = orderDetails.Select((d, index) => new
            {
                STT = index + 1,
                Category = CategoryBUS.Instance.GetCategoryById(ProductBUS.Instance.GetProductById(d.productId).categoryId),
                Product = ProductBUS.Instance.GetProductById(d.productId),
                Quantity = d.quantity,
                Price = d.unitPrice,
                TotalPrice = decimal.Round(d.unitPrice * d.quantity, 2)
            }).ToList();
            dataGridView.Refresh();
            total = 0;
            foreach (var od in orderDetails)
            {
                total += od.unitPrice * od.quantity;
            }
            lbTotal.Text = Decimal.Round(total, 2).ToString();
        }
        private void loadColumn()
        {
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView.ScrollBars = ScrollBars.Both;
            dataSource();
        }
        private void TableForm_Load(object sender, EventArgs e)
        {
            onLoad();
        }
       
        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbProduct.DataSource = products.Where(p => p.quantity != 0 && p.categoryId == ((Category)cbCategory.SelectedValue).id).ToList();
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

            if (orderDetails != null && orderDetails.Count > 0)
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
                        OrderDetail existOrderDetail = orderDetails.First(od => od.productId == orderDetail.productId);
                        existOrderDetail.quantity += orderDetail.quantity;
                        Product product = ProductBUS.Instance.GetProductById(existOrderDetail.productId);
                        product.quantity -= orderDetail.quantity;
                        ProductBUS.Instance.UpdateProduct(product);
                        OrderDetailBUS.Instance.UpdateOrderDetail(existOrderDetail);
                    }
                    else
                    {
                        orderDetail.orderId = orderDetails[0].orderId;
                        Product product = ProductBUS.Instance.GetProductById(orderDetail.productId);
                        product.quantity -= orderDetail.quantity;
                        ProductBUS.Instance.UpdateProduct(product);
                        OrderDetailBUS.Instance.AddOrderDetail(orderDetail);
                        orderDetails.Add(orderDetail);
                    }
                    MessageBox.Show("Thêm thành công");
                    dataSource();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn bàn");
            }
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            if (orderDetails!=null && orderDetails.Count > 0)
            {
                TablePayForm tablePayForm = new TablePayForm(orderDetails, total);
                tablePayForm.StartPosition = FormStartPosition.CenterScreen;
                tablePayForm.ShowDialog(this);
                if (tablePayForm.DialogResult == DialogResult.OK)
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
    }
}
