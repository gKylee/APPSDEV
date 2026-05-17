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
    public partial class Admin : Form
    {
        OleDbConnection con;
        public Admin()
        {
            InitializeComponent();
            con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\User\\Desktop\\APPSDEV\\Dental Reservation System\\Dental Reservation System\\Dental.mdb");
        }
        private void LoadNotifications()
        {
            con.Open();
            OleDbCommand cmd = new OleDbCommand(
                "SELECT * FROM Appointments ORDER BY [AppointmentDate] ASC", con);

            OleDbDataReader reader = cmd.ExecuteReader();
            notifPanel.Controls.Clear();
            int yPos = 10;

            while (reader.Read())
            {
                if (reader["Status"].ToString() != "Pending") continue;
                Panel card = new Panel();
                card.Size = new Size(notifPanel.Width - 20, 70);
                card.Location = new Point(10, yPos);
                card.BackColor = Color.FromArgb(235, 245, 255);
                card.Padding = new Padding(10);
                card.Cursor = Cursors.Hand;
                int appointmentID = Convert.ToInt32(reader["AppointmentID"]);
                card.Tag = appointmentID;

                Label lblMessage = new Label();
                lblMessage.Text = "🦷 " + reader["Problem"].ToString();
                lblMessage.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lblMessage.Location = new Point(10, 8);
                lblMessage.Size = new Size(card.Width - 20, 20);
                lblMessage.Cursor = Cursors.Hand; 

                Label lblDate = new Label();
                lblDate.Text = "Status: " + reader["Status"].ToString();
                lblDate.Font = new Font("Segoe UI", 8);
                lblDate.ForeColor = Color.Gray;
                lblDate.Location = new Point(10, 35);
                lblDate.Size = new Size(card.Width - 20, 20);
                lblDate.Cursor = Cursors.Hand;
                string appointmentName = (reader["PatientName"]).ToString();

                EventHandler openForm = (sender, e) =>
                {
                    con.Close();
                    MessageBox.Show("Clicked Name: " + appointmentID); 
                    AdminAppointment adminAppointment = new AdminAppointment(appointmentName,appointmentID);
                    adminAppointment.ShowDialog();

                };

                card.Click += openForm;
                lblMessage.Click += openForm;
                lblDate.Click += openForm;

                card.Controls.Add(lblMessage);
                card.Controls.Add(lblDate);
                notifPanel.Controls.Add(card);
                yPos += 80;
            }
            con.Close();
        }
        private void Admin_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = " Authenticated as: " + Login.userName+"!";
            LoadNotifications();
        }

        private void appointmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Schedule sched = new Schedule();
            sched.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadNotifications();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void doctorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Doctor dc = new Doctor();
            this.Hide();
            dc.ShowDialog();
            this.Show();

        }
    }
}
