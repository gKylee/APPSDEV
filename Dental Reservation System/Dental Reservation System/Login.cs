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
using System.Data.OleDb;

namespace Dental_Reservation_System
{
    public partial class Login : Form
    {
        OleDbConnection con;
        public static string userName = "";
        public Login()
        {
            InitializeComponent();
            con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\User\\Desktop\\Dental Reservation System\\Dental Reservation System\\Dental.mdb");
        }

        //public void Validation()
        //{
        //    string.IsNullOrEmpty(txtUsername.Text)
        //}

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            OleDbCommand loginCmd = new OleDbCommand("SELECT * FROM Users WHERE [Username]=? AND [Password]=?",con);
            loginCmd.Parameters.AddWithValue("?", txtUsername.Text);
            loginCmd.Parameters.AddWithValue("?", txtPassword.Text);
            OleDbDataReader rd = loginCmd.ExecuteReader();

            if (rd.Read())
            {
                string role = rd["Role"].ToString();

                
                userName = txtUsername.Text;
                if(role == "Admin")
                {
                    Admin admin = new Admin();
                    this.Hide();
                    admin.ShowDialog();
                    con.Close();
                    this.Show();
                }
                else
                {
                    Client c = new Client();
                    this.Hide();
                    c.ShowDialog();
                    con.Close();
                    this.Show();
                }
                   
            }
            else
            {
                lblIncorrect.Text = "Invalid Credentials!";
                con.Close();
            }
            con.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            SignUp signUp = new SignUp();
            this.Hide();
            signUp.ShowDialog();
            this.Show();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
