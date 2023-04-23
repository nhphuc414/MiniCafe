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
    public partial class PayForm : Form
    {
        private List<OrderDetail> orderDetails;
        decimal total;
        private List<Table> tables;
        private List<Employee> employees;
        public PayForm(List<OrderDetail> orderDetails, decimal total)
        {
            this.orderDetails = new List<OrderDetail>();
            this.orderDetails.AddRange(orderDetails);
            this.total = total;
            InitializeComponent();
        }

        private void radioButtonChecked_Change(object sender, EventArgs e)
        {
            if (radPayNow.Checked)
            {
                lbEmployee.Text = "Nhân viên lập hóa đơn";
                lbRad.Text = "Tiền nhận từ khách";
                txtCustomerPay.Text = "";
                txtCustomerPay.Visible = true;
                lbTotalText.Visible = true;
                lbChange.Visible = true;
                lbChange.Text = "0";
                cbTable.Visible = false;
            }
            else
            {
                lbEmployee.Text = "Nhân viên phục vụ";
                lbRad.Text = "Bàn";
                txtCustomerPay.Text = "";
                txtCustomerPay.Visible = false;
                lbTotalText.Visible = false;
                lbChange.Visible = false;
                lbChange.Text = "0";
                cbTable.Visible = true;
            }
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            try
            {
                if (radPayNow.Checked)
                {
                    if (!checkChange()) throw new Exception("Tiền khách trả nhỏ hơn tiền thanh toán");
                    Order order = new Order();
                    order.employeeId = ((Employee)cbEmployee.SelectedValue).id;
                    order.totalPrice = this.total;
                    order.createDate = DateTime.Now;
                    order.status = false;
                    OrderBUS.Instance.AddOrder(order);
                    foreach (var od in orderDetails)
                    {
                        od.orderId = order.id;
                        OrderDetailBUS.Instance.AddOrderDetail(od);
                        Product updateProduct = ProductBUS.Instance.GetProductById(od.productId);
                        updateProduct.quantity -= od.quantity;
                        ProductBUS.Instance.UpdateProduct(updateProduct);
                    }
                    MessageBox.Show("Xuất hóa đơn thành công");
                    // Nếu người dùng đã nhập đủ thông tin, đóng form xuất hóa đơn và trả về kết quả xác thực là OK
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    Order order = new Order();
                    order.employeeId = ((Employee)cbEmployee.SelectedValue).id;
                    order.totalPrice = this.total;
                    order.createDate = DateTime.Now;
                    order.status = true;
                    order.tableId = ((Table)cbTable.SelectedValue).id;
                    
                    Employee employee= EmployeeBUS.Instance.GetEmployeeById(order.employeeId);
                    employee.status = true;
                    Table table= TableBUS.Instance.GetTableById((int)order.tableId);
                    table.status = true;
                    EmployeeBUS.Instance.UpdateEmployee(employee);
                    TableBUS.Instance.UpdateTable(table);
                    OrderBUS.Instance.AddOrder(order);
                    foreach (var od in orderDetails)
                    {
                        Product updateProduct = ProductBUS.Instance.GetProductById(od.productId);
                        updateProduct.quantity -= od.quantity;
                        ProductBUS.Instance.UpdateProduct(updateProduct);
                        od.orderId = order.id;
                        OrderDetailBUS.Instance.AddOrderDetail(od);
                    }
                    MessageBox.Show("Đặt bàn thành công");
                    // Nếu người dùng đã nhập đủ thông tin, đóng form xuất hóa đơn và trả về kết quả xác thực là OK
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn hủy đơn hàng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Hủy đơn thành công");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {

            this.Close();
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
        }
        private void loadColumn()
        {
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView.ScrollBars = ScrollBars.Both;
            dataSource();
        }
        private void PayForm_Load(object sender, EventArgs e)
        {
            lbTotal.Text = Decimal.Round(total, 2).ToString();
            loadColumn();
            tables = new List<Table>();
            employees = new List<Employee>();
            tables.AddRange(TableBUS.Instance.GetTablesOnActive());
            employees.AddRange(EmployeeBUS.Instance.GetEmployeesOnActive());
            cbTable.DataSource = tables;
            cbEmployee.DataSource = employees;
        }
        private bool checkChange()
        {
            if (Decimal.TryParse(txtCustomerPay.Text, out total))
            {
                if (total - this.total >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void txtCustomerPay_TextChanged(object sender, EventArgs e)
        {
            if (Decimal.TryParse(txtCustomerPay.Text,out decimal change))
            {
                if ((change - this.total) >= 0)
                {
                    lbChange.Text = Decimal.Round(change - this.total, 2).ToString();
                }
                else lbChange.Text = "Tiền nhận từ khách nhỏ hơn tiền thanh toán";
            }
            else lbChange.Text = "Vui lòng nhập đúng số tiền";
        }
    }
}
