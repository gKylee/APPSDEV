using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dental_Reservation_System
{
    public partial class Schedule : Form
    {
        OleDbConnection con;
        public Schedule()
        {
            InitializeComponent();
            con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\User\\Desktop\\APPSDEV\\Dental Reservation System\\Dental Reservation System\\Dental.mdb");
        }

        private void Schedule_Load(object sender, EventArgs e)
        {
            con.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Appointments", con);
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.ReadOnly = true;        
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.Columns["Problem"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["AppointmentTime"].DefaultCellStyle.Format = "hh:mm tt";
            con.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            con.Open();
            OleDbCommand cmd = new OleDbCommand(
                "SELECT * FROM Appointments WHERE [PatientName] LIKE ? OR [Dentist] LIKE ? OR [Status] LIKE ?", con);
            cmd.Parameters.AddWithValue("?", "%" + txtSearch.Text + "%");
            cmd.Parameters.AddWithValue("?", "%" + txtSearch.Text + "%");
            cmd.Parameters.AddWithValue("?", "%" + txtSearch.Text + "%");
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to edit!");
                return;
            }

            
            dataGridView1.ReadOnly = false;
            dataGridView1.Columns["AppointmentID"].ReadOnly = true; 

            btnEdit.Enabled = false; 
            btnUpdate.Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row!");
                return;
            }

            DataGridViewRow row = dataGridView1.SelectedRows[0];

            int id = Convert.ToInt32(row.Cells["AppointmentID"].Value);
            string patientName = row.Cells["PatientName"].Value.ToString();
            string dentist = row.Cells["Dentist"].Value.ToString();
            string status = row.Cells["Status"].Value.ToString();
            string problem = row.Cells["Problem"].Value.ToString();
            DateTime date = Convert.ToDateTime(row.Cells["AppointmentDate"].Value);
            DateTime time = Convert.ToDateTime(row.Cells["AppointmentTime"].Value);

            con.Open();
            OleDbCommand cmd = new OleDbCommand(
                "UPDATE Appointments SET [PatientName]=?, [Dentist]=?, [AppointmentDate]=?, [AppointmentTime]=?, [Status]=?, [Problem]=? WHERE [AppointmentID]=?", con);
            cmd.Parameters.AddWithValue("?", patientName);
            cmd.Parameters.AddWithValue("?", dentist);
            cmd.Parameters.AddWithValue("?", date);
            cmd.Parameters.AddWithValue("?", time);
            cmd.Parameters.AddWithValue("?", status);
            cmd.Parameters.AddWithValue("?", problem);
            cmd.Parameters.AddWithValue("?", id);
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Updated successfully!");

            dataGridView1.ReadOnly = true;
            btnEdit.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a row to delete!");
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Are you sure you want to delete this appointment?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["AppointmentID"].Value);

                    con.Open();
                    OleDbCommand cmd = new OleDbCommand(
                        "DELETE FROM Appointments WHERE [AppointmentID]=?", con);
                    cmd.Parameters.AddWithValue("?", id);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Deleted successfully!");
                    
                }
            }
        }
    }
}
