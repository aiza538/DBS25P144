using MidProjectDb;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
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
    public partial class Form6: Form
    {
        DatabaseHelper dbHelper = DatabaseHelper.Instance;
        public Form6()
        {
            InitializeComponent();
            LoadDropdowns();
            LoadTasks();
        }
        private void LoadTasks()
        {
            string query = "SELECT * FROM duties";
            DataTable dt = dbHelper.GetData(query);
            dataGridView1.DataSource = dt;
        }
        private void LoadDropdowns()
        {
            
            string committeeQuery = "SELECT committee_id, committee_name FROM committees";
            comboBox1.DataSource = dbHelper.GetData(committeeQuery);
            comboBox1.DisplayMember = "name";
            

            string statusQuery = "SELECT lookup_id, value FROM lookup WHERE category = 'task_status'";
            comboBox3.DataSource = dbHelper.GetData(statusQuery);
            comboBox3.DisplayMember = "value";
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e){ }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e){ }

        private void textBox1_TextChanged(object sender, EventArgs e){ }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e){ }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e){ }

        private void button1_Click(object sender, EventArgs e)
        {
            string committee = comboBox1.Text;
            string assignedTo = comboBox2.Text;
            string taskDescription = textBox1.Text;
            string deadline = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string status = comboBox3.Text;

            if (string.IsNullOrWhiteSpace(committee) || string.IsNullOrWhiteSpace(assignedTo) ||
                string.IsNullOrWhiteSpace(taskDescription) || string.IsNullOrWhiteSpace(status))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            string query = "INSERT INTO duties (committee_id, assigned_to, task_description, deadline, status_id) " +
               "VALUES (@committee_id, @assignedTo, @taskDescription, @deadline, @status_id)";

            MySqlParameter[] parameters =
            {
                new MySqlParameter("@committee_id", comboBox1.SelectedValue),
                new MySqlParameter("@assignedTo", comboBox2.Text),
                new MySqlParameter("@taskDescription", textBox1.Text),
                new MySqlParameter("@deadline", dateTimePicker1.Value.ToString("yyyy-MM-dd")),
                new MySqlParameter("@status_id", comboBox3.SelectedValue)
            };

            if (dbHelper.ExecuteNonQuery(query, parameters))
            {
                MessageBox.Show("Task saved successfully!");
                LoadTasks(); 
            }
            else
            {
                MessageBox.Show("Error saving task.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a task to modify.");
                return;
            }

            int taskId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["duty_id"].Value);
            string committee = comboBox1.Text;
            string assignedTo = comboBox2.Text;
            string taskDescription = textBox1.Text;
            string deadline = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string status = comboBox3.Text;

            string query = "UPDATE duties SET committee_id = @committee_id, assigned_to = @assignedTo, " +
              "task_description = @taskDescription, deadline = @deadline, status_id = @status_id " +
              "WHERE duty_id = @dutyId";

            MySqlParameter[] parameters =
            {
                new MySqlParameter("@committee_id", comboBox1.SelectedValue),
                new MySqlParameter("@assignedTo", comboBox2.Text),
                new MySqlParameter("@taskDescription", textBox1.Text),
                new MySqlParameter("@deadline", dateTimePicker1.Value.ToString("yyyy-MM-dd")),
                new MySqlParameter("@status_id", comboBox3.SelectedValue),
                new MySqlParameter("@dutyId", taskId)
            };

            if (dbHelper.ExecuteNonQuery(query, parameters))
            {
                MessageBox.Show("Task updated successfully!");
                LoadTasks(); 
            }
            else
            {
                MessageBox.Show("Error updating task.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a task to remove.");
                return;
            }

            int taskId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["duty_id"].Value);
            string query = "DELETE FROM duties WHERE duty_id = @taskId";

            MySqlParameter[] parameters =
            {
                new MySqlParameter("@taskId", taskId)
            };

            if (dbHelper.ExecuteNonQuery(query, parameters))
            {
                MessageBox.Show("Task removed successfully!");
                LoadTasks(); 
            }
            else
            {
                MessageBox.Show("Error removing task.");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                comboBox1.Text = row.Cells["committee"].Value.ToString();
                comboBox2.Text = row.Cells["assigned_to"].Value.ToString();
                textBox1.Text = row.Cells["description"].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells["deadline"].Value);
                comboBox3.Text = row.Cells["status"].Value.ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide(); 
            Form1 previousForm = new Form1(); 
            previousForm.Show();
        }
    }
}
