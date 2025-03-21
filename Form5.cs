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

    public partial class Form5: Form
    {
        DatabaseHelper dbHelper = DatabaseHelper.Instance;
        public Form5()
        {
            InitializeComponent();
            LoadCommittees();
            LoadRoles();
        }
        private void LoadCommittees()
        {
            string query = "SELECT committee_id, committee_name FROM Committees";
            DataTable dt = dbHelper.GetData(query);

            if (dt != null && dt.Rows.Count > 0)
            {
                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "committee_name";
                comboBox1.ValueMember = "committee_id";

                dataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("No committees found in the database.");
            }

        }
        private void LoadCommitteeMembers(int committeeId)
        {
            string query = @"SELECT cm.member_id, cm.name, l.value AS role 
                     FROM CommitteeMembers cm
                     JOIN lookup l ON cm.role_id = l.lookup_id
                     WHERE cm.committee_id = " + committeeId;
            DataTable dt = dbHelper.GetData(query);

            if (dt != null)
            {
                dataGridView2.DataSource = dt;
            }

        }
        private void LoadRoles()
        {
            string query = "SELECT lookup_id, value FROM lookup WHERE category = 'Role'";
            DataTable dt = dbHelper.GetData(query);

            if (dt != null)
            {
                comboBox2.DataSource = dt;
                comboBox2.DisplayMember = "value";
                comboBox2.ValueMember = "lookup_id";
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e){ }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e){ }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e){ }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                string name = textBox1.Text;
                string query = $"INSERT INTO Committees (committee_name) VALUES ('{name}')";
                dbHelper.ExecuteQuery(query);
                LoadCommittees();
            }
            else
            {
                MessageBox.Show("Please enter a committee name.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["committee_id"].Value);
                string name = textBox1.Text;
                string query = $"UPDATE Committees SET committee_name = '{name}' WHERE committee_id = {id}";
                dbHelper.ExecuteQuery(query);
                LoadCommittees();
            }
            else
            {
                MessageBox.Show("Please select a committee and enter a name.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);
                string query = $"DELETE FROM Committees WHERE ID = {id}";
                dbHelper.ExecuteQuery(query);
                LoadCommittees();
            }
            else
            {
                MessageBox.Show("Please select a committee to delete.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue != null && comboBox2.SelectedValue != null && !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                int committeeId = Convert.ToInt32(comboBox1.SelectedValue);
                int roleId = Convert.ToInt32(comboBox2.SelectedValue);
                string memberName = textBox1.Text;

                string query = "INSERT INTO CommitteeMembers (committee_id, name, role_id) VALUES (@committee_id, @name, @role_id)";

                using (MySqlConnection conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@committee_id", committeeId);
                        cmd.Parameters.AddWithValue("@name", memberName);
                        cmd.Parameters.AddWithValue("@role_id", roleId);
                        cmd.ExecuteNonQuery();
                    }
                }

                LoadCommitteeMembers(committeeId);
            }
            else
            {
                MessageBox.Show("Please fill in all member details.");
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int committeeId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);
                LoadCommitteeMembers(committeeId);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];

                if (row.Cells["name"].Value != null)
                    textBox1.Text = row.Cells["name"].Value.ToString();

                if (row.Cells["role"].Value != null)
                    comboBox2.Text = row.Cells["role"].Value.ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 previousForm = Application.OpenForms["Form1"] as Form1;

            if (previousForm != null)
            {
                previousForm.Show();  
            }
            else
            {
                previousForm = new Form1(); 
                previousForm.Show();
            }

            this.Close();
        }
    }
}
