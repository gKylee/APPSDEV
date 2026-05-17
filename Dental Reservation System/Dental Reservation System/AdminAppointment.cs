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
    public partial class AdminAppointment : Form
    {
        string name;
        int id;
        OleDbConnection con;

        public AdminAppointment(String name, int id)
        {
            InitializeComponent();
            con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\User\\Desktop\\APPSDEV\\Dental Reservation System\\Dental Reservation System\\Dental.mdb");
            this.name = name;
            this.id = id;
            lblPatientName.Text = name;
        }

        private void AdminAppointment_Load(object sender, EventArgs e)
        {
            LoadDentists();
        }

        private void LoadDentists()
        {
            try
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT [Name] FROM Doctors", con);
                OleDbDataReader reader = cmd.ExecuteReader();

                cmbDentist.Items.Clear();
                while (reader.Read())
                {
                    cmbDentist.Items.Add(reader["Name"].ToString());
                }
                con.Close();

                checkInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dentists: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        public void checkInfo()
        {
            try
            {
                con.Open();
                OleDbCommand checkCmd = new OleDbCommand(
                    "SELECT [Dentist],[Status] FROM Appointments WHERE [AppointmentID]=?", con);
                checkCmd.Parameters.AddWithValue("?", id);
                OleDbDataReader checkRd = checkCmd.ExecuteReader();

                if (checkRd.Read())
                {
                    cmbDentist.Text = checkRd["Dentist"].ToString();
                    cmbStatus.Text = checkRd["Status"].ToString();
                }
                else
                {
                    cmbDentist.Text = "";
                    cmbStatus.Text = "";
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading appointment info: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void sendNotif(String message)
        {
            try
            {
                con.Open();
                OleDbCommand notifCmd = new OleDbCommand(
                    "INSERT INTO Notification ([Username], [Message], [DateCreated]) VALUES (?,?,?)", con);
                notifCmd.Parameters.AddWithValue("?", name);
                notifCmd.Parameters.AddWithValue("?", message);
                notifCmd.Parameters.AddWithValue("?", Convert.ToDateTime(DateTime.Today));
                notifCmd.ExecuteNonQuery();
                con.Close();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending notification: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                MessageBox.Show("Please enter a message before proceeding.", "Message Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMessage.Focus();
                return;
            }

            string Time = time.Value.ToString("hh:mm tt");

            try
            {
                con.Open();
                OleDbCommand sendCmd = new OleDbCommand(
                    "UPDATE Appointments SET [Dentist]=?, [AppointmentDate]=?, [AppointmentTime]=?, [Status]=? WHERE [AppointmentID]=?", con);
                sendCmd.Parameters.AddWithValue("?", cmbDentist.Text);
                sendCmd.Parameters.AddWithValue("?", date);
                sendCmd.Parameters.AddWithValue("?", Time);
                sendCmd.Parameters.AddWithValue("?", cmbStatus.Text);
                sendCmd.Parameters.AddWithValue("?", id);
                sendCmd.ExecuteNonQuery();

                OleDbCommand getDate = new OleDbCommand(
                    "SELECT [AppointmentDate], [AppointmentTime], [Status] FROM Appointments WHERE [AppointmentID]=?", con);
                getDate.Parameters.AddWithValue("?", id);

                OleDbDataReader reader = getDate.ExecuteReader();
                string appointmentDate = "";
                string appointmentTime = "";
                if (reader.Read())
                {
                    appointmentDate = reader["AppointmentDate"].ToString();
                    appointmentTime = reader["AppointmentTime"].ToString();
                }
                con.Close();

                string formattedDate = Convert.ToDateTime(appointmentDate).ToString("MM/dd/yyyy");
                string formattedTime = Convert.ToDateTime(appointmentTime).ToString("hh:mm tt");

                if (cmbStatus.Text == "Cancelled")
                {
                    string message = "Hello Dear Patient " + name + " " + txtMessage.Text;
                    sendNotif(message);
                }
                else
                {
                    string message = "Hello Dear Patient " + name + " You can visit on Date: " + formattedDate + " Time: " + formattedTime;
                    sendNotif(message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
    }
}