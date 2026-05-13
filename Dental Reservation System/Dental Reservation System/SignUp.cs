using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;


namespace Dental_Reservation_System
{
    public partial class SignUp : Form
    {
        OleDbConnection con;
        public SignUp()
        {
            InitializeComponent();
            con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\User\\Desktop\\Dental Reservation System\\Dental Reservation System\\Dental.mdb");
            
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text != txtConfirmation.Text)
            {
                button1.Enabled = false;
                lblIncorrect.Text = "Password didn't match";
                button1.Enabled = true;
                return;
            }
            else
            {
                con.Open();
                string user = txtUsername.Text;
                string password = txtPassword.Text;
                string email = txtEmail.Text;
                string num = txtPhone.Text;
                OleDbCommand addCmd = new OleDbCommand("INSERT INTO Users ([Username], [Password], [Role], [PhoneNumber], [Email]) VALUES (?,?,?,?,?)", con);
                addCmd.Parameters.AddWithValue("?", user);
                addCmd.Parameters.AddWithValue("?", password);
                addCmd.Parameters.AddWithValue("?", "Client");
                addCmd.Parameters.AddWithValue("?", num);
                addCmd.Parameters.AddWithValue("?", email);
                addCmd.ExecuteNonQuery();
                con.Close();
                Close();
            }
                
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            this.Hide();
            login.Show();
        }

        private void SignUp_Load(object sender, EventArgs e)
        {
           
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
