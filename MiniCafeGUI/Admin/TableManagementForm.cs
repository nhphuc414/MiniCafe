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
namespace MiniCafeGUI.Admin
{
    public partial class TableManagementForm : Form
    {
        private List<Table> tables;
        int index = -1;
        public TableManagementForm()
        {
            tables = new List<Table>();
            tables.AddRange(TableBUS.Instance.GetAllTables());
            InitializeComponent();
        }
        private void dataSource()
        {
            dataGridView.DataSource = tables.Select(t => new
            {
                ID = t.id,
                Name = t.name,
                Status = t.status ? "Có người":"Trống"
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
        private void TableManagementForm_Load(object sender, EventArgs e)
        {
            loadColumn();
            txtName.Focus();
        }
        private void resetFields()
        {
            txtName.Text = "";
            txtName.Focus();
        }
        private void getInField(Table table)
        {
            table.name = txtName.Text.Trim();
        }
        private void loadOnField(Table table)
        {
            Table selectedTable = tables.First(t => t.id == table.id);
            txtName.Text = selectedTable.name;
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
            Table selectedTable = new Table();
            disableAllButton();
            selectedTable.id = Convert.ToInt32(dataGridView.Rows[index].Cells["id"].Value);
            loadOnField(selectedTable);
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
                    Table selectedTable = new Table();
                    selectedTable.id = Convert.ToInt32(dataGridView.Rows[e.RowIndex].Cells["id"].Value);
                    selectedTable.status = tables.FirstOrDefault(t => t.id == selectedTable.id).status;
                    getInField(selectedTable);
                    TableBUS.Instance.UpdateTable(selectedTable);
                    tables[tables.FindIndex(es => es.id == selectedTable.id)] = selectedTable;
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
                        Table selectedTable = tables[e.RowIndex];
                        TableBUS.Instance.DeleteTable(selectedTable.id);
                        tables.Remove(selectedTable);
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
                Table selectedTable = new Table();
                getInField(selectedTable);
                TableBUS.Instance.AddTable(selectedTable);
                tables.Add(selectedTable);
                dataSource();
                MessageBox.Show("Thêm thành công");
                resetFields();
            }
            catch (ArgumentException aex)
            {
                txtName.Focus();
                MessageBox.Show(aex.Message);
            }
            catch (Exception ex)
            {
                txtName.Focus();
                MessageBox.Show(ex.Message);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            resetFields();
        }
    }
}
