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
using MiniCafeDAL.IDAL.DAL;
using MiniCafeDAL.Model;

namespace MiniCafeGUI.AdminGUI
{
    public partial class EmployeeManagementForm : Form
    {
        private List<Employee> employees;
        private List<Shift> shifts;
        int index = -1;
        public EmployeeManagementForm()
        {
            employees = new List<Employee>();
            employees.AddRange(EmployeeBUS.Instance.GetAllEmployees());
            shifts= new List<Shift>();
            shifts.AddRange(ShiftBUS.Instance.GetAllShifts());
            InitializeComponent();
            setDatetimePicker(dtBirthday);
            groupBoxInput.ForeColor = Color.Black;
        }
        private void setDatetimePicker(DateTimePicker dt)
        {
            dt.Format = DateTimePickerFormat.Short;
            dt.Format = DateTimePickerFormat.Custom;
            dt.CustomFormat = "dd-MM-yyyy";
            
        }
        private void dataSource()
        {
            dataGridView.DataSource = employees.Select(e => new
            {
                ID = e.id,
                Name = e.employeeName,Number = e.number,
                Birthday = e.birthday.ToString("dd-MM-yyyy"),
                Username = e.username,
                Role = (e.isManager ? "Manager" : "Employee"),
                Status = e.status? "Busy":"Lazy",
                Active = (e.isFired ? "Fired" : "Active"),
                Shift = (ShiftBUS.Instance.GetShiftById((int)e.shiftId).ToString()),
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
        private void EmployeeManagementForm_Load(object sender, EventArgs e)
        {
            cbShift.DataSource = shifts;
            cbShift.DisplayMember = "ToString";
            cbShift.SelectedIndex = 0;
            cbEmployeeRole.SelectedIndex = 0;
            loadColumn();
            cbShift.Focus();
            cbStatus.SelectedIndex = 0;
            cbStatus.Visible=false;
        }
        private void resetFields()
        {
            txtName.Text = "";
            txtNumber.Text = "";
            txtPassword.Text = "";
            txtUsername.Text = "";
            cbEmployeeRole.SelectedIndex = 0;
            cbShift.SelectedIndex = 0;
            cbStatus.SelectedIndex=0;
            dtBirthday.Value=DateTime.Now;
            cbShift.Focus();
        }
        private void showStatus(bool bol)
        {
            labelStatus.Text = bol ? "Trạng thái" : "Mật khẩu";
            txtPassword.Visible = !bol;
            cbStatus.Visible = bol;
        }
        private void getInField(Employee employee)
        {
            employee.employeeName = txtName.Text;
            employee.number = txtNumber.Text;
            employee.shiftId = shifts[cbShift.SelectedIndex].id;
            employee.hireDay = DateTime.Now;
            employee.birthday = dtBirthday.Value;
            employee.username = txtUsername.Text;
            employee.password = txtPassword.Text;
            employee.isManager = cbEmployeeRole.SelectedIndex == 0 ? false : true;
            employee.isFired = cbStatus.SelectedIndex==0?false:true;
        }
        private void loadOnField(Employee employee)
        {
            Employee selectedEmployee = employees.FirstOrDefault(e => e.id == employee.id);
            cbShift.SelectedIndex = shifts.FindIndex(s =>s.id==selectedEmployee.shiftId);
            txtName.Text = selectedEmployee.employeeName;
            txtNumber.Text = selectedEmployee.number;
            dtBirthday.Value = selectedEmployee.birthday;
            txtUsername.Text = selectedEmployee.username;
            txtUsername.Enabled = false;
            txtPassword.Text = selectedEmployee.password;
            cbEmployeeRole.SelectedIndex = selectedEmployee.isManager ? 1 : 0;
            cbStatus.SelectedIndex = selectedEmployee.isFired ? 1 : 0;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Employee employee = new Employee();
                getInField(employee);
                EmployeeBUS.Instance.AddEmployee(employee);
                employees.Add(employee);
                dataSource();
                MessageBox.Show("Thêm thành công");
                resetFields();
            }
            catch (ArgumentException aex)
            {
                cbShift.Focus();
                MessageBox.Show(aex.Message);
            }
            catch (Exception ex)
            {
                cbShift.Focus();
                MessageBox.Show(ex.Message);
            }
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
            Employee selectedEmployee = new Employee();
            disableAllButton();
            selectedEmployee.id = Convert.ToInt32(dataGridView.Rows[index].Cells["id"].Value);
            loadOnField(selectedEmployee);
            showStatus(true);
            dataGridView.CellContentClick -= dataGridView_CellContentClick;
            dataGridView.CellContentClick += dataGridView_CommitOrCancel;
        }
        private void afterCommitOrCancel()
        {
            enableAllButton();
            showStatus(false);
            txtUsername.Enabled = true;
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
                    Employee selectedEmployee = new Employee();
                    selectedEmployee.id = Convert.ToInt32(dataGridView.Rows[e.RowIndex].Cells["id"].Value);
                    getInField(selectedEmployee);
                    selectedEmployee.status = employees[employees.FindIndex(es => es.id == selectedEmployee.id)].status;
                    EmployeeBUS.Instance.UpdateEmployee(selectedEmployee);
                    employees[employees.FindIndex(es => es.id == selectedEmployee.id)] = selectedEmployee;
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
                        Employee selectedEmployee = employees[e.RowIndex];
                        EmployeeBUS.Instance.DeleteEmployee(selectedEmployee.id);
                        employees.Remove(selectedEmployee);
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
        private void btnDelete_Click(object sender, EventArgs e)
        {

            resetFields();
        }
    }
}
