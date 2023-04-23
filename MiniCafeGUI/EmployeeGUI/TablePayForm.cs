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
    public partial class TablePayForm : Form
    {
        private List<OrderDetail> orderDetails;
        decimal total;
        public TablePayForm(List<OrderDetail> orderDetails,decimal total)
        {
            this.orderDetails = new List<OrderDetail>();
            this.orderDetails.AddRange(orderDetails);
            this.total = total;
            InitializeComponent();
        }

        private void TablePayForm_Load(object sender, EventArgs e)
        {
            lbTotal.Text = Decimal.Round(total, 2).ToString();
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
        }
        private void loadColumn()
        {
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView.ScrollBars = ScrollBars.Both;
            dataSource();
        }
        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            try
            {
                if (!checkChange()) throw new Exception("Tiền khách trả nhỏ hơn tiền thanh toán");
                Order order = OrderBUS.Instance.GetOrderById(orderDetails[0].orderId);
                Employee employee = EmployeeBUS.Instance.GetEmployeeById(order.employeeId);
                Table table = TableBUS.Instance.GetTableById((int)order.tableId);
                order.totalPrice = this.total;
                order.createDate = DateTime.Now;
                order.status = false;
                employee.status = false;
                table.status = false;

                EmployeeBUS.Instance.UpdateEmployee(employee);
                TableBUS.Instance.UpdateTable(table);
                OrderBUS.Instance.UpdateOrder(order);
                MessageBox.Show("Thanh toán thành công");
                // Nếu người dùng đã nhập đủ thông tin, đóng form xuất hóa đơn và trả về kết quả xác thực là OK
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
            if (Decimal.TryParse(txtCustomerPay.Text, out decimal change))
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
