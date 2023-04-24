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
using System.Windows.Forms.DataVisualization.Charting;

namespace MiniCafeGUI.AdminGUI
{
    public partial class StatisticManagementForm : Form
    {

        public StatisticManagementForm()
        {
            InitializeComponent();
            setDatetimePicker(dtFromDay);
            setDatetimePicker(dtToDay);
            cbRevenueBy.SelectedIndex = 0;
        }
        private void loadOrderDetail(List<OrderDetail> orderDetails)
        {
            dataGridView.DataSource = null;
            dataGridView.Refresh();
            List<Order> orders = new List<Order>();

            orders.AddRange(OrderBUS.Instance.GetOrderFromDayToDay(dtFromDay.Value, dtToDay.Value));
            foreach (Order o in orders)
            {
                orderDetails.AddRange(OrderDetailBUS.Instance.GetOrderDetailsByOrderId(o.id));
            }
        }
        private void btnCount_Click(object sender, EventArgs e)
        {
            if (cbRevenueBy.SelectedIndex == 1)
            {
                dataGridView.DataSource = null;

                dataGridView.Refresh();
                List<Order> orders = new List<Order>();
                orders.AddRange(OrderBUS.Instance.GetOrderFromDayToDay(dtFromDay.Value, dtToDay.Value));
                dataGridView.DataSource = orders.GroupBy(o => o.employeeId).Select(g => new
                {
                    Name = EmployeeBUS.Instance.GetEmployeeById(g.Key),
                    Revenue = Decimal.Round(g.Sum(o => o.totalPrice),2),

                }).ToList();

                dataGridView.Refresh();
            }
            else if (cbRevenueBy.SelectedIndex == 0)
            {
                List<OrderDetail> orderDetails = new List<OrderDetail>(); ;
                loadOrderDetail(orderDetails);
                dataGridView.DataSource = orderDetails.Select(od => new
                {
                    Name = ProductBUS.Instance.GetProductById(od.productId),
                    Revenue = Decimal.Round(od.quantity * od.unitPrice,2)
                }).ToList();

                dataGridView.Refresh();
            }
            else { throw new Exception("Vui lòng chọn loại Thống kê"); }
        }
        private void setDatetimePicker(DateTimePicker dt)
        {
            dt.Format = DateTimePickerFormat.Short;
            dt.Format = DateTimePickerFormat.Custom;
            dt.CustomFormat = "dd-MM-yyyy";
        }
    }
}
