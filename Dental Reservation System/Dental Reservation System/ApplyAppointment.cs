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
    public partial class ApplyAppointment : Form
    {
        OleDbConnection con;
        public ApplyAppointment()
        {
            InitializeComponent();
            con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\working\\Desktop\\APPSDEV\\Dental Reservation System\\Dental Reservation System\\Dental.mdb");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            OleDbCommand cmdRequest = new OleDbCommand("INSERT INTO Appointments ([PatientName],[Problem],[Status]) VALUES (?,?,?)", con);
            cmdRequest.Parameters.AddWithValue("?", Login.userName);
            cmdRequest.Parameters.AddWithValue("?", txtProblem.Text);
            cmdRequest.Parameters.AddWithValue("?", "Pending");
            cmdRequest.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Your Application is now sent, please wait in your notification box for response","Important",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            this.Close();
        }

        private void ApplyAppointment_Load(object sender, EventArgs e)
        {

        }
    }
}
