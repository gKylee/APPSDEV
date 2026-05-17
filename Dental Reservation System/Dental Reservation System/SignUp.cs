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
using System.Text.RegularExpressions;

namespace Dental_Reservation_System
{
    public partial class SignUp : Form
    {
        OleDbConnection con;

        public SignUp()
        {
            InitializeComponent();
            con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\User\\Desktop\\APPSDEV\\Dental Reservation System\\Dental Reservation System\\Dental.mdb");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirm = txtConfirmation.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();

            // Empty
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirm) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(phone))
            {
                lblIncorrect.ForeColor = Color.Red;
                lblIncorrect.Text = "Please fill in all fields.";
                return;
            }

            // Username
            if (user.Length < 4 || user.Length > 30)
            {
                lblIncorrect.ForeColor = Color.Red;
                lblIncorrect.Text = "Username must be 4-30 characters.";
                return;
            }

            // Password
            if (password.Length < 6 || !Regex.IsMatch(password, @"\d"))
            {
                lblIncorrect.ForeColor = Color.Red;
                lblIncorrect.Text = "Password must be 6+ characters with at least one number.";
                return;
            }

            // Match
            if (password != confirm)
            {
                lblIncorrect.ForeColor = Color.Red;
                lblIncorrect.Text = "Passwords do not match.";
                return;
            }

            // Email
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                lblIncorrect.ForeColor = Color.Red;
                lblIncorrect.Text = "Enter a valid email address.";
                return;
            }

            // Phone
            if (!Regex.IsMatch(phone, @"^\d{7,15}$"))
            {
                lblIncorrect.ForeColor = Color.Red;
                lblIncorrect.Text = "Phone must be 7-15 digits only.";
                return;
            }

            try
            {
                con.Open();

                // Duplicate
                OleDbCommand checkCmd = new OleDbCommand("SELECT COUNT(*) FROM Users WHERE Username = ?", con);
                checkCmd.Parameters.AddWithValue("?", user);
                int exists = (int)checkCmd.ExecuteScalar();
                if (exists > 0)
                {
                    lblIncorrect.ForeColor = Color.Red;
                    lblIncorrect.Text = "Username is already taken.";
                    return;
                }

                // Insert
                OleDbCommand addCmd = new OleDbCommand("INSERT INTO Users ([Username], [Password], [Role], [PhoneNumber], [Email]) VALUES (?,?,?,?,?)", con);
                addCmd.Parameters.AddWithValue("?", user);
                addCmd.Parameters.AddWithValue("?", password);
                addCmd.Parameters.AddWithValue("?", "Client");
                addCmd.Parameters.AddWithValue("?", phone);
                addCmd.Parameters.AddWithValue("?", email);
                addCmd.ExecuteNonQuery();

                MessageBox.Show("Account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                lblIncorrect.ForeColor = Color.Red;
                lblIncorrect.Text = "Error: " + ex.Message;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void txtConfirmation_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtConfirmation.Text)) { lblIncorrect.Text = ""; return; }

            if (txtConfirmation.Text != txtPassword.Text)
            {
                lblIncorrect.ForeColor = Color.Red;
                lblIncorrect.Text = "Passwords do not match.";
            }
            else
            {
                lblIncorrect.ForeColor = Color.Green;
                lblIncorrect.Text = "Passwords match.";
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) lblIncorrect.Text = ""; else MessageBox.Show("You must agree to our policy! Please check the box", "Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
        }
    }
}