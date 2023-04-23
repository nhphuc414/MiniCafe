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
using MiniCafeGUI.AdminGUI;
using MiniCafeGUI.EmployeeGUI;
namespace MiniCafeGUI
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }
        private void login(string username, string password)
        {
            var employee = EmployeeBUS.Instance.GetEmployeeByUsernameAndPassword(username, password);
            if (employee.isManager)
            {
                Program.CurrentEmployee = employee;
                AdminForm adminForm = new AdminForm();
                this.Hide();
                adminForm.ShowDialog();
                this.Close();
            }
            else // Ngược lại, hiện EmployeeForm
            {
                Program.CurrentEmployee = employee;
                EmployeeForm employeeForm = new EmployeeForm();
                this.Hide();
                employeeForm.ShowDialog();
                this.Close();
            }
        }
        
        private void btnLogin_Click(object sender, EventArgs e)
        {
           string username = txtUsername.Text.Trim();
           string password = txtPassword.Text.Trim();
           try
            {
                if (username == null || password == null)
                {
                    throw new ArgumentException("Vui lòng nhập đủ thông tin");
                }
                login(username, password);
            }
            catch (ArgumentException aex)
            {
                MessageBox.Show(aex.Message);
                txtUsername.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Nếu người dùng xác nhận đăng xuất
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void loginForm_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
        }
    }
}
