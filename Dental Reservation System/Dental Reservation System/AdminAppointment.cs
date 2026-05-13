using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Deployment.Application;
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
            con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\User\\Desktop\\Dental Reservation System\\Dental Reservation System\\Dental.mdb");
            this.name = name;
            this.id = id;
            checkInfo();
            lblPatientName.Text = name;
        }
        
        public void checkInfo()
        {
            con.Open();
            OleDbCommand checkCmd = new OleDbCommand("SELECT [Dentist],[Status] FROM Appointments WHERE [AppointmentID]=?", con);
            checkCmd.Parameters.AddWithValue("?",id);
            checkCmd.ExecuteNonQuery();
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
       

        private void AdminAppointment_Load(object sender, EventArgs e)
        {
            
        }

        private void sendNotif(String message)
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

        private void button1_Click(object sender, EventArgs e)
        {
            string Time =time.Value.ToString("hh:mm tt");
            
            con.Open();
            OleDbCommand sendCmd = new OleDbCommand(
                "UPDATE Appointments SET [Dentist]=?, [AppointmentDate]=?, [AppointmentTime]=?, [Status]=? WHERE [AppointmentID]=?", con);
            sendCmd.Parameters.AddWithValue("?",cmbDentist.Text);
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
                string message = "Hello Dear Patient " + name + " Your application is cancelled please apply again. Thank you.";
                sendNotif(message);
            }
            else
            {
                string message = "Hello Dear Patient " + name + " You can visit on Date: " + formattedDate + " Time: " + formattedTime;
                sendNotif(message);
            }


            
            
        }
    }
}
