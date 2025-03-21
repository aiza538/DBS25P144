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
    public partial class Form9: Form
    {
        DatabaseHelper dbHelper = DatabaseHelper.Instance;
        public Form9()
        {
            InitializeComponent();
            LoadEvents();
            LoadTeams();
            LoadResults();
        }
        private void LoadEvents()
        {
            string query = "SELECT event_id, event_name FROM itec_events";
            DataTable dt = dbHelper.GetDataTable(query);
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "event_name";
            comboBox1.ValueMember = "event_id";
        }
        private void LoadTeams()
        {
            string query = "SELECT team_id, team_name FROM teams";
            DataTable dt = dbHelper.GetDataTable(query);
            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "team_name";
            comboBox2.ValueMember = "team_id";
        }
        private void LoadResults()
        {
            string query = @"
                SELECT 
                    er.result_id,
                    ev.event_id,
                    ev.event_name,
                    t.team_id,
                    t.team_name,
                    er.position,
                    er.score,
                    er.remarks
                FROM event_results er
                JOIN itec_events ev ON er.event_id = ev.event_id
                JOIN teams t ON er.team_id = t.team_id";
            dataGridView1.DataSource = dbHelper.GetDataTable(query);

        }
        private void SaveResult()
        {
            try
            {
                int eventId = Convert.ToInt32(comboBox1.SelectedValue);
                int teamId = Convert.ToInt32(comboBox2.SelectedValue);
                string position = comboBox3.Text.Trim();
                string score = textBox1.Text.Trim();
                string remarks = textBox2.Text.Trim();

                if (string.IsNullOrEmpty(position) || string.IsNullOrEmpty(score))
                {
                    MessageBox.Show("Position and Score are required!");
                    return;
                }

                string query = @"INSERT INTO event_results 
                        (event_id, team_id, position, score, remarks) 
                         VALUES (@event_id, @team_id, @position, @score, @remarks)";
                MySqlParameter[] parameters =
                {
                    new MySqlParameter("@event_id", eventId),
                    new MySqlParameter("@team_id", teamId),
                    new MySqlParameter("@position", position),
                    new MySqlParameter("@score", score),
                    new MySqlParameter("@remarks", remarks)
                };

                if (dbHelper.ExecuteNonQuery(query, parameters))
                {
                    MessageBox.Show("Result saved successfully!");
                    LoadResults();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ModifyResult()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string resultId = dataGridView1.SelectedRows[0].Cells["result_id"].Value.ToString();
                int eventId = Convert.ToInt32(comboBox1.SelectedValue);
                int teamId = Convert.ToInt32(comboBox2.SelectedValue);
                string position = comboBox3.Text.Trim();
                string score = textBox1.Text.Trim();
                string remarks = textBox2.Text.Trim();

                string query = @"UPDATE event_results 
                                 SET event_id=@event_id, team_id=@team_id, 
                                     position=@position, score=@score, remarks=@remarks 
                                 WHERE result_id=@result_id";

                MySqlParameter[] parameters =
                 {
                    new MySqlParameter("@event_id", eventId),
                    new MySqlParameter("@team_id", teamId),
                    new MySqlParameter("@position", position),
                    new MySqlParameter("@score", score),
                    new MySqlParameter("@remarks", remarks),
                    new MySqlParameter("@result_id", resultId)
                };

                if (dbHelper.ExecuteNonQuery(query, parameters))
                {
                    MessageBox.Show("Result updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadResults(); 
                }
                else
                {
                    MessageBox.Show("Failed to update result.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a record to modify.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void DeleteResult()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string resultId = dataGridView1.SelectedRows[0].Cells["result_id"].Value.ToString();

                DialogResult confirm = MessageBox.Show("Are you sure you want to delete this result?",
                                                       "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    string query = "DELETE FROM event_results WHERE result_id=@result_id";
                    MySqlParameter[] parameters = { new MySqlParameter("@result_id", resultId) };

                    if (dbHelper.ExecuteNonQuery(query, parameters))
                    {
                        MessageBox.Show("Result deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadResults(); 
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete result.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a record to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e){ }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e){ }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e){ }

        private void textBox1_TextChanged(object sender, EventArgs e){ }

        private void textBox2_TextChanged(object sender, EventArgs e){ }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveResult();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ModifyResult();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DeleteResult();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadResults(); 
            MessageBox.Show("Results displayed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                comboBox1.SelectedValue = row.Cells["event_id"].Value;
                comboBox2.SelectedValue = row.Cells["team_id"].Value;
                comboBox3.Text = row.Cells["position"].Value.ToString();
                textBox1.Text = row.Cells["score"].Value.ToString();
                textBox2.Text = row.Cells["remarks"].Value.ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide(); 
            Form1 previousForm = new Form1(); 
            previousForm.Show();
        }
    }
}
