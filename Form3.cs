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
    public partial class Form3: Form
    {
        DatabaseHelper dbHelper = DatabaseHelper.Instance;
        public Form3()
        {
            InitializeComponent();
            LoadComboBoxes();
            LoadEvents();
        }
        
        private void label1_Click(object sender, EventArgs e){ }

        private void label2_Click(object sender, EventArgs e){ }

        private void textBox2_TextChanged_1(object sender, EventArgs e){ }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e){ }

        private void textBox1_TextChanged(object sender, EventArgs e){ }
        private void Events_Load(object sender, EventArgs e)
        {
            string query = "SELECT * FROM itec_events";
            DataTable dt = dbHelper.ExecuteQuery(query);
            dataGridView1.DataSource = dt;
        }
        private void LoadComboBoxes()
        {
           try
            {
                DataTable dtCategories = dbHelper.ExecuteQuery("SELECT event_category_id, category_name FROM event_categories");
                if (dtCategories != null && dtCategories.Rows.Count > 0)
                {
                    comboBox1.DataSource = dtCategories;
                    comboBox1.DisplayMember = "category_name";
                    comboBox1.ValueMember = "event_category_id";
                }
                else
                {
                    MessageBox.Show("No categories found.");
                }

                DataTable dtVenues = dbHelper.ExecuteQuery("SELECT venue_id, venue_name FROM venues");
                if (dtVenues != null && dtVenues.Rows.Count > 0)
                {
                    comboBox2.DataSource = dtVenues;
                    comboBox2.DisplayMember = "venue_name";
                    comboBox2.ValueMember = "venue_id";
                }
                else
                {
                    MessageBox.Show("No venues found.");
                }

                DataTable dtCommittees = dbHelper.ExecuteQuery("SELECT committee_id, committee_name FROM committees");
                if (dtCommittees != null && dtCommittees.Rows.Count > 0)
                {
                    comboBox3.DataSource = dtCommittees;
                    comboBox3.DisplayMember = "committee_name";
                    comboBox3.ValueMember = "committee_id";
                }
                else
                {
                    MessageBox.Show("No committees found.");
                }
           }
           catch (Exception ex)
           {
                 MessageBox.Show("Error Loading ComboBoxes: " + ex.Message);
           }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e){ }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e){ }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e){ }
        private void LoadEvents()
        {
            string query = @"
                SELECT 
                    e.event_id, 
                    e.event_name, 
                    ec.category_name AS category, 
                    e.description, 
                    e.event_date, 
                    v.venue_name AS venue, 
                    c.committee_name AS committee,
                    e.event_category_id, e.venue_id, e.committee_id
                FROM itec_events e
                JOIN event_categories ec ON e.event_category_id = ec.event_category_id
                JOIN venues v ON e.venue_id = v.venue_id
                JOIN committees c ON e.committee_id = c.committee_id";

            try
            {
                DataTable dt = dbHelper.ExecuteQuery(query);
                if (dt != null && dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("No events found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null || comboBox2.SelectedValue == null || comboBox3.SelectedValue == null)
            {
                MessageBox.Show("Please select a valid Category, Venue, and Committee.");
                return;
            }

            string query = "INSERT INTO itec_events (event_name, event_category_id, description, event_date, venue_id, committee_id) VALUES (@name, @category, @desc, @date, @venue, @committee)";

            MySqlParameter[] parameters = {
            new MySqlParameter("@name", textBox1.Text),
            new MySqlParameter("@category", comboBox1.SelectedValue),
            new MySqlParameter("@desc", textBox2.Text),
            new MySqlParameter("@date", dateTimePicker1.Value),
            new MySqlParameter("@venue", comboBox2.SelectedValue),
            new MySqlParameter("@committee", comboBox3.SelectedValue)
            };

            try
            {
                dbHelper.ExecuteNonQuery(query, parameters);
                LoadEvents();
                MessageBox.Show("Event added successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int eventId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["event_id"].Value);
                string query = @"UPDATE itec_events SET 
                event_name = @name, 
                event_category_id = @category, 
                description = @desc, 
                event_date = @date, 
                venue_id = @venue, 
                committee_id = @committee 
                WHERE event_id = @id";

                MySqlParameter[] parameters = {
                    new MySqlParameter("@id", eventId),
                    new MySqlParameter("@name", textBox1.Text),
                    new MySqlParameter("@category", comboBox1.SelectedValue),
                    new MySqlParameter("@desc", textBox2.Text),
                    new MySqlParameter("@date", dateTimePicker1.Value),
                    new MySqlParameter("@venue", comboBox2.SelectedValue),
                    new MySqlParameter("@committee", comboBox3.SelectedValue)
                };

                try
                {
                    dbHelper.ExecuteNonQuery(query, parameters);
                    LoadEvents();
                    MessageBox.Show("Event updated successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select an event to update.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int eventId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["event_id"].Value);
                string query = "DELETE FROM itec_events WHERE event_id = @id";
                MySqlParameter[] parameters = { new MySqlParameter("@id", eventId) };

                try
                {
                    dbHelper.ExecuteNonQuery(query, parameters);
                    LoadEvents(); 
                    MessageBox.Show("Event deleted successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select an event to delete.");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["event_name"].Value.ToString();
                comboBox1.SelectedValue = row.Cells["event_category_id"].Value; 
                textBox2.Text = row.Cells["description"].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells["event_date"].Value);
                comboBox2.SelectedValue = row.Cells["venue_id"].Value; 
                comboBox3.SelectedValue = row.Cells["committee_id"].Value; 
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
