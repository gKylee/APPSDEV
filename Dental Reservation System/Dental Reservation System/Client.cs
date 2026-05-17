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
    public partial class Client : Form
    {
        OleDbConnection con;
        public Client()
        {
            InitializeComponent();
            con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\User\\Desktop\\APPSDEV\\Dental Reservation System\\Dental Reservation System\\Dental.mdb");
        }

        private void LoadNotifications()
        {
            con.Open();
            OleDbCommand cmd = new OleDbCommand(
                "SELECT * FROM Notification WHERE [Username]=? ORDER BY [DateCreated] DESC", con);
            cmd.Parameters.AddWithValue("?", Login.userName);

            OleDbDataReader reader = cmd.ExecuteReader();

           
            notifPanel.Controls.Clear();
            int yPos = 10;

            while (reader.Read())
            {
                string message = reader["Message"].ToString().Trim();

                string status = "";
               
                if (message.ToLower().Contains("cancelled") || message.ToLower().Contains("cancel"))
                    status = "Cancelled";
                else if (message.ToLower().Contains("visit"))
                    status = "On-Going";



                Color bgColor, textColor;
                switch (status)
                {
                    case "Cancelled":
                        bgColor = Color.FromArgb(255, 235, 235);
                        textColor = Color.FromArgb(180, 30, 30);
                        break;
                    case "On-Going":
                        bgColor = Color.FromArgb(225, 255, 230); 
                        textColor = Color.FromArgb(20, 130, 50);
                        break;
                    default:
                        bgColor = Color.FromArgb(235, 245, 255);
                        textColor = Color.FromArgb(30, 80, 160);
                        break;
                }

                Panel card = new Panel();
                card.Size = new Size(notifPanel.Width - 20, 70);
                card.Location = new Point(10, yPos);
                card.BackColor = bgColor; 
                card.Padding = new Padding(10);

                Label lblMessage = new Label();
                lblMessage.Text = "🦷 " + reader["Message"].ToString();
                lblMessage.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lblMessage.ForeColor = textColor;
                lblMessage.Location = new Point(10, 8);
                lblMessage.Size = new Size(card.Width - 20, 20);


                Label lblDate = new Label();
                lblDate.Text = Convert.ToDateTime(reader["DateCreated"]).ToString("MMM dd, yyyy");
                lblDate.Font = new Font("Segoe UI", 8);
                lblDate.ForeColor = Color.Gray;
                lblDate.Location = new Point(10, 35);
                lblDate.Size = new Size(card.Width - 20, 20);

                card.Controls.Add(lblMessage);
                card.Controls.Add(lblDate);
                notifPanel.Controls.Add(card);

                yPos += 80; 
            }

            con.Close();
        }

        private void Client_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = Login.userName+" to iCare Dental!";
            LoadNotifications();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadNotifications();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();

        }

        private void appointmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyAppointment aAppointment = new ApplyAppointment();
            this.Hide();
            aAppointment.ShowDialog();
            this.Show();
            con.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0); 
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }
    }
}
