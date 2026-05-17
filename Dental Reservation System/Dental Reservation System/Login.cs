using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Dental_Reservation_System
{
    public partial class Login : Form
    {
        OleDbConnection con;
        public static string userName = "";
        public Login()
        {
            InitializeComponent();
            con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\User\\Desktop\\APPSDEV\\Dental Reservation System\\Dental Reservation System\\Dental.mdb");
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
            // Empty
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                lblIncorrect.ForeColor = Color.Red;
                lblIncorrect.Text = "Please fill in all fields.";
                return;
            }

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
