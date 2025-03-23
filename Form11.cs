using MidProjectDb;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MidProjectDb
{
    public partial class Form11: Form
    {
        DatabaseHelper dbHelper = DatabaseHelper.Instance;

        public Form11()
        {
            InitializeComponent();
        }
        private void Form11_Load(object sender, EventArgs e)
        {
            LoadRoles();
            LoadUsers();
        }
        private void LoadRoles()
        {
            comboBox1.Items.Clear();
            string query = "SELECT role_id, role_name FROM roles";
            DataTable dt = dbHelper.GetDataTable(query);
            comboBox1.DisplayMember = "role_name";
            comboBox1.ValueMember = "role_id";
            comboBox1.DataSource = dt;
        }
        private void LoadUsers()
        {
            string query = "SELECT user_id, username, email, role_id FROM users";
            DataTable dt = dbHelper.GetDataTable(query);
            dataGridView1.DataSource = dt;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 previousForm = new Form1();
            previousForm.Show();
        }
        private void textBox1_TextChanged(object sender, EventArgs e){ }

        private void textBox2_TextChanged(object sender, EventArgs e){ }

        private void textBox3_TextChanged(object sender, EventArgs e){ }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e){ }
        private void ClearFields(params Control[] controls)
        {
            foreach (var control in controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.Clear();
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.SelectedIndex = -1;
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string username = textBox1.Text.Trim();
                string email = textBox2.Text.Trim();
                string password = textBox3.Text.Trim();
                int roleId = Convert.ToInt32(comboBox1.SelectedValue);

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please fill all fields!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string query = "INSERT INTO users (username, email, password_hash, role_id) VALUES (@username, @email, @password, @role_id)";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@username", username),
                    new MySqlParameter("@email", email),
                    new MySqlParameter("@password", password),
                    new MySqlParameter("@role_id", roleId)
                };

                if (dbHelper.ExecuteNonQuery(query, parameters))
                {
                    MessageBox.Show("User added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields(textBox1, textBox2, textBox3, comboBox1);
                    LoadUsers();
                }
                else
                {
                    MessageBox.Show("Error adding user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string email = textBox2.Text.Trim();
            string password = textBox3.Text.Trim();
            int roleId = Convert.ToInt32(comboBox1.SelectedValue);

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Enter the username to modify!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE users SET email = @email, password_hash = @password, role_id = @role_id WHERE username = @username";
            MySqlParameter[] parameters = {
                new MySqlParameter("@username", username),
                new MySqlParameter("@email", email),
                new MySqlParameter("@password", password),
                new MySqlParameter("@role_id", roleId)
            };

            if (dbHelper.ExecuteNonQuery(query, parameters))  
            {
                MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields(textBox1, textBox2, textBox3, comboBox1);
                LoadUsers();
            }
            else
            {
                MessageBox.Show("Error updating user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Enter the username to delete!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "DELETE FROM users WHERE username = @username";
            MySqlParameter[] parameters = { new MySqlParameter("@username", username) };

            if (dbHelper.ExecuteNonQuery(query, parameters))
            {
                MessageBox.Show("User removed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
                LoadUsers();
            }
            else
            {
                MessageBox.Show("Error removing user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0) 
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                    textBox1.Text = row.Cells["username"].Value?.ToString() ?? "";
                    textBox2.Text = row.Cells["email"].Value?.ToString() ?? "";
                    textBox3.Text = row.Cells["password_hash"].Value?.ToString() ?? "";
                    comboBox1.SelectedValue = row.Cells["role_id"].Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error selecting user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
