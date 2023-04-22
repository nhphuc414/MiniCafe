using MiniCafeDAL.Model;
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
namespace MiniCafeGUI.Admin
{
    public partial class ShiftManagementForm : Form
    {
        private List<Shift> shifts;
        int index = -1;
        public ShiftManagementForm()
        {
            shifts = new List<Shift>();
            shifts.AddRange(ShiftBUS.Instance.GetAllShifts());
            InitializeComponent();
            setDatetimePicker(dtStartTime);
            setDatetimePicker(dtEndTime);
        }
        private void setDatetimePicker(DateTimePicker dt)
        {
            dt.Format = DateTimePickerFormat.Custom;
            dt.ShowUpDown = true;
            dt.CustomFormat = "HH:mm:ss";
            dt.Format = DateTimePickerFormat.Time;
        }
        private void dataSource()
        {
            dataGridView.DataSource = shifts.Select(s => new {
                ID = s.id,
                StartTime = s.startTime,
                EndTime = s.endTime,
                EmployeeInShift = EmployeeBUS.Instance.GetEmployeesByShiftId(s.id).Count,
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

        private void ShiftManagementForm_Load(object sender, EventArgs e)
        {
            loadColumn();
            dtStartTime.Focus();
        }
        private void resetField()
        {
            dtStartTime.Value = DateTime.Now;
            dtEndTime.Value = DateTime.Now;
            dtStartTime.Focus();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            resetField();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Shift shift = new Shift();
            shift.startTime = dtStartTime.Value.TimeOfDay;
            shift.endTime = dtEndTime.Value.TimeOfDay;
            try
            {
                ShiftBUS.Instance.AddShift(shift);
                shifts.Add(shift);
                dataSource();
                MessageBox.Show("Thêm thành công");
                resetField();
            }
            catch (ArgumentException aex)
            {
                dtStartTime.Focus();
                MessageBox.Show(aex.Message);
            }
            catch (Exception ex)
            {
                dtStartTime.Focus();
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
        private void loadOnFields(Shift shift)
        {
            Shift selectedShift = shifts.FirstOrDefault(s => s.id == shift.id);
            DateTime dt = DateTime.Today; // Lấy ngày hôm nay
            DateTime resultStartTime = dt.Add(selectedShift.startTime);
            DateTime resultEndTime = dt.Add(selectedShift.endTime);
            dtStartTime.Value = resultStartTime;
            dtEndTime.Value = resultEndTime;
            dtStartTime.Focus();
        }
        private void beforeCommit(int index)
        {
            Shift selectedShift = new Shift();
            selectedShift.id = Convert.ToInt32(dataGridView.Rows[index].Cells["id"].Value);
            disableAllButton();
            //Đưa Category lên field để chỉnh sửa
            loadOnFields(selectedShift);
            //Set sự kiện
            dataGridView.CellContentClick -= dataGridViewCategory_CellContentClick;
            dataGridView.CellContentClick += dataGridViewCategory_CommitOrCancel;

        }
        private void afterCommitOrCancel(int index)
        {
            enableAllButton();
            dataGridView.CellContentClick -= dataGridViewCategory_CommitOrCancel;
            dataGridView.CellContentClick += dataGridViewCategory_CellContentClick;
            resetField();
        }
        private void dataGridViewCategory_CommitOrCancel(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView.Columns["editColumn"].Index && e.RowIndex == index)
            {
                Shift selectedShift = new Shift();
                selectedShift.id = Convert.ToInt32(dataGridView.Rows[e.RowIndex].Cells["id"].Value);
                selectedShift.startTime = dtStartTime.Value.TimeOfDay;
                selectedShift.endTime = dtEndTime.Value.TimeOfDay;
                try
                {
                    ShiftBUS.Instance.UpdateShift(selectedShift);
                    shifts.FirstOrDefault(s => s.id == selectedShift.id).startTime = selectedShift.startTime;
                    shifts.FirstOrDefault(s => s.id == selectedShift.id).endTime = selectedShift.endTime;
                    dataSource();
                    MessageBox.Show("Cập nhật thành công");
                    afterCommitOrCancel(e.RowIndex);
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
            if (e.ColumnIndex == dataGridView.Columns["deleteColumn"].Index && e.RowIndex == index)
            {
                index = -1;
                afterCommitOrCancel(e.RowIndex);
            }
        }
        private void dataGridViewCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView.Columns["editColumn"].Index && e.RowIndex >= 0 && e.RowIndex < dataGridView.Rows.Count)
            {
                index = e.RowIndex;
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
                        Shift selectedShift = shifts[e.RowIndex];
                        ShiftBUS.Instance.DeleteShift(selectedShift.id);
                        shifts.Remove(selectedShift);
                        dataSource();
                        MessageBox.Show("Xóa thành công");
                        resetField();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
