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
    public partial class Doctor : Form
    {
        OleDbConnection con;
        int selectedDoctorId = -1;

        public Doctor()
        {
            InitializeComponent();
            con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\User\\Desktop\\APPSDEV\\Dental Reservation System\\Dental Reservation System\\Dental.mdb");
        }

        private void Doctor_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = false; 
            LoadDoctorCards();
        }

        private void SetEditingMode(bool isEditing)
        {
            btnSend.Enabled = !isEditing;
            btnSave.Enabled = isEditing;


            if (isEditing)
            {
                btnSave.BackColor = Color.FromArgb(31, 97, 141);
                btnSave.ForeColor = Color.White;
                btnSend.BackColor = Color.LightGray;
                btnSend.ForeColor = Color.DarkGray;
            }
            else
            {
                btnSend.BackColor = Color.FromArgb(192, 57, 43);
                btnSend.ForeColor = Color.White;
                btnSave.BackColor = Color.LightGray;
                btnSave.ForeColor = Color.DarkGray;
                selectedDoctorId = -1;
                txtDoctorName.Clear();
                txtContact.Clear();
            }
        }
        private void LoadDoctorCards(string search = "")
        {
            panelDoctor.Controls.Clear();
            panelDoctor.AutoScroll = true;

            try
            {
                con.Open();

                OleDbCommand cmd;

                if (string.IsNullOrWhiteSpace(search))
                {
                    cmd = new OleDbCommand("SELECT [DoctorID], [Name], [Contact] FROM Doctors", con);
                }
                else
                {
                    cmd = new OleDbCommand("SELECT [DoctorID], [Name], [Contact] FROM Doctors WHERE [Name] LIKE ?", con);
                    cmd.Parameters.AddWithValue("?", "%" + search + "%");
                }

                OleDbDataReader reader = cmd.ExecuteReader();
                int cardIndex = 0;

                while (reader.Read())
                {
                    int doctorId = Convert.ToInt32(reader["DoctorID"]);
                    string doctorName = reader["Name"].ToString();
                    string contact = reader["Contact"].ToString();

                    Panel card = new Panel();
                    card.Size = new Size(panelDoctor.Width - 40, 80);
                    card.Location = new Point(10, 10 + cardIndex * 95);
                    card.BackColor = Color.White;
                    card.BorderStyle = BorderStyle.FixedSingle;
                    card.Padding = new Padding(10);
                    card.Tag = new object[] { doctorId, doctorName, contact };
                    card.Cursor = Cursors.Hand;

                    Label lblIcon = new Label();
                    lblIcon.Text = "👨‍⚕️";
                    lblIcon.Font = new Font("Segoe UI", 20);
                    lblIcon.Size = new Size(50, 60);
                    lblIcon.Location = new Point(10, 10);
                    lblIcon.TextAlign = ContentAlignment.MiddleCenter;
                    lblIcon.Tag = card.Tag;
                    lblIcon.Cursor = Cursors.Hand;

                    Label lblName = new Label();
                    lblName.Text = doctorName;
                    lblName.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                    lblName.ForeColor = Color.FromArgb(31, 97, 141);
                    lblName.Size = new Size(250, 25);
                    lblName.Location = new Point(70, 15);
                    lblName.AutoEllipsis = true;
                    lblName.Tag = card.Tag;
                    lblName.Cursor = Cursors.Hand;

                    Label lblContact = new Label();
                    lblContact.Text = "📞 " + contact;
                    lblContact.Font = new Font("Segoe UI", 9);
                    lblContact.ForeColor = Color.Gray;
                    lblContact.Size = new Size(250, 20);
                    lblContact.Location = new Point(70, 45);
                    lblContact.Tag = card.Tag;
                    lblContact.Cursor = Cursors.Hand;

                    card.Click += Card_Click;
                    lblIcon.Click += Card_Click;
                    lblName.Click += Card_Click;
                    lblContact.Click += Card_Click;

                    card.Controls.Add(lblIcon);
                    card.Controls.Add(lblName);
                    card.Controls.Add(lblContact);

                    panelDoctor.Controls.Add(card);
                    cardIndex++;
                }
                if (cardIndex == 0)
                {
                    Label lblEmpty = new Label();
                    lblEmpty.Text = "No doctors found.";
                    lblEmpty.Font = new Font("Segoe UI", 10);
                    lblEmpty.ForeColor = Color.Gray;
                    lblEmpty.Size = new Size(panelDoctor.Width - 40, 40);
                    lblEmpty.Location = new Point(10, 10);
                    lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
                    panelDoctor.Controls.Add(lblEmpty);
                }

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading doctors: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void LoadDoctorCards()
        {
            panelDoctor.Controls.Clear();
            panelDoctor.AutoScroll = true;

            try
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT [DoctorID], [Name], [Contact] FROM Doctors", con);
                OleDbDataReader reader = cmd.ExecuteReader();

                int cardIndex = 0;

                while (reader.Read())
                {
                    int doctorId = Convert.ToInt32(reader["DoctorID"]);
                    string doctorName = reader["Name"].ToString();
                    string contact = reader["Contact"].ToString();

                    Panel card = new Panel();
                    card.Size = new Size(panelDoctor.Width - 40, 80);
                    card.Location = new Point(10, 10 + cardIndex * 95);
                    card.BackColor = Color.White;
                    card.BorderStyle = BorderStyle.FixedSingle;
                    card.Padding = new Padding(10);
                    card.Tag = new object[] { doctorId, doctorName, contact }; // Store doctor data
                    card.Cursor = Cursors.Hand;

                    Label lblIcon = new Label();
                    lblIcon.Text = "👨‍⚕️";
                    lblIcon.Font = new Font("Segoe UI", 20);
                    lblIcon.Size = new Size(50, 60);
                    lblIcon.Location = new Point(10, 10);
                    lblIcon.TextAlign = ContentAlignment.MiddleCenter;
                    lblIcon.Tag = card.Tag;
                    lblIcon.Cursor = Cursors.Hand;


                    Label lblName = new Label();
                    lblName.Text = doctorName;
                    lblName.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                    lblName.ForeColor = Color.FromArgb(31, 97, 141);
                    lblName.Size = new Size(250, 25);
                    lblName.Location = new Point(70, 15);
                    lblName.AutoEllipsis = true;
                    lblName.Tag = card.Tag;
                    lblName.Cursor = Cursors.Hand;

             
                    Label lblContact = new Label();
                    lblContact.Text = "📞 " + contact;
                    lblContact.Font = new Font("Segoe UI", 9);
                    lblContact.ForeColor = Color.Gray;
                    lblContact.Size = new Size(250, 20);
                    lblContact.Location = new Point(70, 45);
                    lblContact.Tag = card.Tag;
                    lblContact.Cursor = Cursors.Hand;

           
                    card.Click += Card_Click;
                    lblIcon.Click += Card_Click;
                    lblName.Click += Card_Click;
                    lblContact.Click += Card_Click;

                    card.Controls.Add(lblIcon);
                    card.Controls.Add(lblName);
                    card.Controls.Add(lblContact);

                    panelDoctor.Controls.Add(card);
                    cardIndex++;
                }

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading doctors: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void Card_Click(object sender, EventArgs e)
        {
         
            object[] data = sender is Panel
                ? (object[])((Panel)sender).Tag
                : (object[])((Control)sender).Tag;

            selectedDoctorId = Convert.ToInt32(data[0]);
            txtDoctorName.Text = data[1].ToString();
            txtContact.Text = data[2].ToString();

            SetEditingMode(true);

         
            foreach (Control ctrl in panelDoctor.Controls)
            {
                if (ctrl is Panel p)
                    p.BackColor = Color.White;
            }

            Panel selectedCard = sender is Panel
                ? (Panel)sender
                : (Panel)((Control)sender).Parent;

            selectedCard.BackColor = Color.FromArgb(214, 234, 248);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDoctorName.Text))
            {
                MessageBox.Show("Please enter the doctor's name.", "Name Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDoctorName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtContact.Text))
            {
                MessageBox.Show("Please enter the contact number.", "Contact Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContact.Focus();
                return;
            }

            try
            {
                con.Open();
                OleDbCommand insertCmd = new OleDbCommand(
                    "INSERT INTO Doctors ([Name], [Contact]) VALUES (?, ?)", con);
                insertCmd.Parameters.AddWithValue("?", txtDoctorName.Text.Trim());
                insertCmd.Parameters.AddWithValue("?", txtContact.Text.Trim());
                insertCmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Doctor added successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                SetEditingMode(false);
                LoadDoctorCards();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (selectedDoctorId == -1)
            {
                MessageBox.Show("No doctor selected for editing.", "Selection Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDoctorName.Text))
            {
                MessageBox.Show("Please enter the doctor's name.", "Name Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDoctorName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtContact.Text))
            {
                MessageBox.Show("Please enter the contact number.", "Contact Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContact.Focus();
                return;
            }

            try
            {
                con.Open();
                OleDbCommand updateCmd = new OleDbCommand(
                    "UPDATE Doctors SET [Name]=?, [Contact]=? WHERE [DoctorID]=?", con);
                updateCmd.Parameters.AddWithValue("?", txtDoctorName.Text.Trim());
                updateCmd.Parameters.AddWithValue("?", txtContact.Text.Trim());
                updateCmd.Parameters.AddWithValue("?", selectedDoctorId);
                updateCmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Doctor updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                SetEditingMode(false);
                LoadDoctorCards();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadDoctorCards(txtSearch.Text.Trim());
        }

        
    }
}