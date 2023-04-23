using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace MiniCafeGUI.EmployeeGUI
{
    public partial class EmployeeForm : Form
    {
        public EmployeeForm()
        {
            InitializeComponent();
        }

        private void employeeForm_Load(object sender, EventArgs e)
        {
            lbUser.Text = Program.CurrentEmployee.employeeName;
        }
        private void Logout()
        {
            Program.CurrentEmployee = null;
            // Hiển thị lại form đăng nhập
            LoginForm loginForm = new LoginForm();
            this.Hide();
            loginForm.ShowDialog();
            this.Close();

        }
        private void showForm(Form formToShow)
        {
            panelForms.Controls.Clear();
            // Thiết lập kích thước và vị trí của Form1
            formToShow.TopLevel = false;
            formToShow.FormBorderStyle = FormBorderStyle.None;
            formToShow.Dock = DockStyle.Fill;
            formToShow.BackColor = panelForms.BackColor;
            // Thêm Form1 vào panelForms
            panelForms.Controls.Add(formToShow);
            // Hiển thị Form1 trong panelForms
            formToShow.Show();
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Nếu người dùng xác nhận đăng xuất
            if (result == DialogResult.Yes)
            {
                // Gọi hàm đăng xuất
                Logout();
            }
        }

        private void btnTable_Click(object sender, EventArgs e)
        {
            // Tạo một instance mới của Form
            TableForm tableForm = new TableForm();
            showForm((TableForm)tableForm);
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            // Tạo một instance mới của Form
            OrderForm orderForm = new OrderForm();
            showForm((OrderForm)orderForm);
        }
    }
}
