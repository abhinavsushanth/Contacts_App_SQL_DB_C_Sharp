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

namespace Contacts_App
{
    public partial class Form1 : Form
    {
        SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\abganji\DB\C# DB\ContactsDB.mdf;Integrated Security=True;Connect Timeout=30");
        int contactId;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }

                if (btnSave.Text == "Save")
                { 
                    SqlCommand sqlCmd = new SqlCommand("AddOrEditContacts", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    sqlCmd.Parameters.AddWithValue("@mode", "Add");
                    sqlCmd.Parameters.AddWithValue("@Id", 0);
                    sqlCmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    sqlCmd.ExecuteNonQuery();
                    MessageBox.Show("Saved Successfully");
                }
                else if (btnSave.Text == "Update")
                {
                    SqlCommand sqlCmd = new SqlCommand("AddOrEditContacts", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    sqlCmd.Parameters.AddWithValue("@mode", "Edit");
                    sqlCmd.Parameters.AddWithValue("@Id", contactId);
                    sqlCmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    sqlCmd.ExecuteNonQuery();
                    MessageBox.Show("Updated Successfully");
                }
                Reset();
                FillDataGridView();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error Message");
            }
            finally
            {
                sqlCon.Close();
            }
        }

        private void FillDataGridView()
        {
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }

            SqlDataAdapter sqlDa = new SqlDataAdapter("ViewOrSearchContacts", sqlCon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDa.SelectCommand.Parameters.AddWithValue("@Name", txtSearch.Text.Trim());
            DataTable dtTable = new DataTable();
            sqlDa.Fill(dtTable);

            dGVContacts.DataSource = dtTable;
            dGVContacts.Columns[0].Visible = false;

            sqlCon.Close();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                FillDataGridView();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error Message");
            }
        }

        private void dGVContacts_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (dGVContacts.CurrentRow.Index > -1)
                {
                    contactId = Convert.ToInt32(dGVContacts.CurrentRow.Cells[0].Value.ToString());
                    txtName.Text = dGVContacts.CurrentRow.Cells[1].Value.ToString();
                    txtMobile.Text = dGVContacts.CurrentRow.Cells[2].Value.ToString();
                    txtAddress.Text = dGVContacts.CurrentRow.Cells[3].Value.ToString();
                    btnSave.Text = "Update";
                    btnDelete.Enabled = true;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error Message");
            }
        }

        void Reset()
        {
            txtName.Text = txtMobile.Text = txtAddress.Text = "";
            btnSave.Text = "Save";
            contactId = 0;
            btnDelete.Enabled = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Reset();
            FillDataGridView();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }

                SqlCommand sqlCmd = new SqlCommand("DeleteContact", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@Id", contactId);

                sqlCmd.ExecuteNonQuery();
                MessageBox.Show("Deleted Successfully");

                Reset();
                FillDataGridView();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error Message");
            }
        }
    }
}
