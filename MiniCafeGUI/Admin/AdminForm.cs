using MiniCafeGUI.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniCafeGUI
{
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
        }

        private void adminForm_Load(object sender, EventArgs e)
        {
            lbUser.Text = Program.CurrentEmployee.employeeName;
            MainManagementForm mainManagementForm = new MainManagementForm();
            showForm(mainManagementForm);
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

        private void btnCategory_Click(object sender, EventArgs e)
        {
            // Tạo một instance mới của Form
            CategoryManagementForm categoryManagementForm = new CategoryManagementForm();
            showForm((CategoryManagementForm)categoryManagementForm);
        }

        private void btnShift_Click(object sender, EventArgs e)
        {
            // Tạo một instance mới của Form
            ShiftManagementForm shiftManagementForm = new ShiftManagementForm();
            showForm((ShiftManagementForm)shiftManagementForm);
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            // Tạo một instance mới của Form1
            EmployeeManagementForm employeeManagementForm = new EmployeeManagementForm();
            showForm((EmployeeManagementForm)employeeManagementForm);
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            // Tạo một instance mới của Form1
            ProductManagementForm productManagementForm = new ProductManagementForm();
            showForm((ProductManagementForm)productManagementForm);
        }

        private void btnTable_Click(object sender, EventArgs e)
        {
            TableManagementForm tableManagementForm = new TableManagementForm();
            showForm((TableManagementForm)tableManagementForm);
        }

        private void btnStatistic_Click(object sender, EventArgs e)
        {
            try
            {
                MainManagementForm mainManagementForm = new MainManagementForm();
                showForm(mainManagementForm);
                throw new Exception("Chưa tạo chức năng");     
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MainManagementForm mainManagementForm = new MainManagementForm();
            showForm(mainManagementForm);
        }
    }
}
