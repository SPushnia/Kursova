using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace collageDB
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;

        private SqlCommandBuilder sqlBuilder = null;

        private SqlDataAdapter sqlDataAdapter = null;

        private DataSet dataSet = null;

        private bool newRowAdding = false;
        public Form1()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT *,'Delete' AS [Command] From Student", sqlConnection);

                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();
                sqlBuilder.GetDeleteCommand();

                dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "Student");

                dataGridView1.DataSource = dataSet.Tables["Student"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[9, i] = linkCell;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void UpdateData()
        {
            try
            {
                dataSet.Tables["Student"].Clear();

                sqlDataAdapter.Fill(dataSet, "Student");

                dataGridView1.DataSource = dataSet.Tables["Student"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[9, i] = linkCell;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\lolol\source\repos\collageDB\collageDB\Database1.mdf;Integrated Security=True");
            sqlConnection.Open();
            LoadData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 9)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
                    if (task == "Delete")
                    {
                        if (MessageBox.Show("Видалити цю стрічку?", "Видалити", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;
                            dataGridView1.Rows.RemoveAt(rowIndex);
                            dataSet.Tables["Student"].Rows[rowIndex].Delete();
                            sqlDataAdapter.Update(dataSet, "Student");
                        }
                    }
                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;
                        DataRow row = dataSet.Tables["Student"].NewRow();

                        row["Surname"] = dataGridView1.Rows[rowIndex].Cells["Surname"].Value;
                        row["Name"] = dataGridView1.Rows[rowIndex].Cells["Name"].Value;
                        row["Fname"] = dataGridView1.Rows[rowIndex].Cells["Fname"].Value;
                        row["DateBirth"] = dataGridView1.Rows[rowIndex].Cells["DateBirth"].Value;
                        row["NumSpec"] = dataGridView1.Rows[rowIndex].Cells["NumSpec"].Value;
                        row["NameSpec"] = dataGridView1.Rows[rowIndex].Cells["NameSpec"].Value;
                        row["Math"] = dataGridView1.Rows[rowIndex].Cells["Math"].Value;
                        row["Ukr"] = dataGridView1.Rows[rowIndex].Cells["Ukr"].Value;

                        dataSet.Tables["Student"].Rows.Add(row);

                        dataSet.Tables["Student"].Rows.RemoveAt(dataSet.Tables["Student"].Rows.Count - 1);

                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);

                        dataGridView1.Rows[e.RowIndex].Cells[9].Value = "Delete";

                        sqlDataAdapter.Update(dataSet, "Student");

                        newRowAdding = false;

                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;

                        dataSet.Tables["Student"].Rows[r]["Surname"] = dataGridView1.Rows[r].Cells["Surname"].Value;
                        dataSet.Tables["Student"].Rows[r]["Name"] = dataGridView1.Rows[r].Cells["Name"].Value;
                        dataSet.Tables["Student"].Rows[r]["Fname"] = dataGridView1.Rows[r].Cells["Fname"].Value;
                        dataSet.Tables["Student"].Rows[r]["DateBirth"] = dataGridView1.Rows[r].Cells["DateBirth"].Value;
                        dataSet.Tables["Student"].Rows[r]["NumSpec"] = dataGridView1.Rows[r].Cells["NumSpec"].Value;
                        dataSet.Tables["Student"].Rows[r]["NameSpec"] = dataGridView1.Rows[r].Cells["NameSpec"].Value;
                        dataSet.Tables["Student"].Rows[r]["Math"] = dataGridView1.Rows[r].Cells["Math"].Value;
                        dataSet.Tables["Student"].Rows[r]["Ukr"] = dataGridView1.Rows[r].Cells["Ukr"].Value;

                        sqlDataAdapter.Update(dataSet, "Student");

                        dataGridView1.Rows[e.RowIndex].Cells[9].Value = "Delete";
                    }
                    UpdateData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    newRowAdding = true;

                    int lastRow = dataGridView1.Rows.Count - 2;

                    DataGridViewRow row = dataGridView1.Rows[lastRow];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[9, lastRow] = linkCell;

                    row.Cells["Command"].Value = "Insert";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;

                    DataGridViewRow editingRow = dataGridView1.Rows[rowIndex];
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[9, rowIndex] = linkCell;

                    editingRow.Cells["Command"].Value = "Update";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column_KeyPress);

            if (dataGridView1.CurrentCell.ColumnIndex == 5)
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    textBox.KeyPress += new KeyPressEventHandler(Column_KeyPress);
                }

            }
            if (dataGridView1.CurrentCell.ColumnIndex == 7)
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    textBox.KeyPress += new KeyPressEventHandler(Column_KeyPress);
                }
            }
            if (dataGridView1.CurrentCell.ColumnIndex == 8)
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    textBox.KeyPress += new KeyPressEventHandler(Column_KeyPress);
                }
            }
        }
        private void Column_KeyPress(object sender,KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void вихідToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close();
            Close();
        }
    }
}
