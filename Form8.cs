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
    public partial class Form8: Form
    {
        DatabaseHelper dbHelper = DatabaseHelper.Instance;
        public Form8()
        {
            InitializeComponent();
            LoadEvents();
            LoadVenues();
            LoadAllocations();
        }
        private void LoadEvents()
        {
            string query = "SELECT event_id, event_name FROM itec_events"; 
            DataTable dt = dbHelper.ExecuteQuery(query);
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "event_name";
            comboBox1.ValueMember = "event_id";
        }
        private void LoadVenues()
        {
            string query = "SELECT venue_id, venue_name FROM venues"; 
            DataTable dt = dbHelper.ExecuteQuery(query);
            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "venue_name";
            comboBox2.ValueMember = "venue_id";
        }
        private void LoadAllocations()
        {
            string query = @"
            SELECT va.venue_allocation_id, ev.event_name, v.venue_name, va.assigned_date, va.assigned_time
            FROM venue_allocations va
            JOIN itec_events ev ON va.event_id = ev.event_id
            JOIN venues v ON va.venue_id = v.venue_id";
            dataGridView1.DataSource = dbHelper.GetDataTable(query);

        }
        private void ValidateSchedulingConflicts()
        {
            string eventName = comboBox1.Text;
            int venueId = Convert.ToInt32(comboBox2.SelectedValue);
            string date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string time = dateTimePicker2.Value.ToString("HH:mm:ss");

            string query = $"SELECT COUNT(*) FROM venue_allocations WHERE venue_id={venueId} AND assigned_date='{date}' AND assigned_time='{time}'";

            int count = Convert.ToInt32(dbHelper.ExecuteScalar(query));

            if (count > 0)
            {
                MessageBox.Show("Conflict detected! The venue is already booked at this time.", "Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("No scheduling conflicts detected.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void SaveVenueAssignment()
        {
            int eventId = Convert.ToInt32(comboBox1.SelectedValue);
            int venueId = Convert.ToInt32(comboBox2.SelectedValue);
            string date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string time = dateTimePicker2.Value.ToString("HH:mm:ss");

            string query = $"INSERT INTO venue_allocations (event_id, venue_id, assigned_date, assigned_time) VALUES ({eventId}, {venueId}, '{date}', '{time}')";
            dbHelper.ExecuteNonQueryy(query);

            MessageBox.Show("Venue assigned successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void ModifyExistingAllocation()
        {
            
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string id = dataGridView1.SelectedRows[0].Cells["id"].Value.ToString();
                int eventId = Convert.ToInt32(comboBox1.SelectedValue);
                int venueId = Convert.ToInt32(comboBox2.SelectedValue);
                string date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                string time = dateTimePicker2.Value.ToString("HH:mm:ss");

                string query = $"UPDATE venue_allocations SET event_id={eventId}, venue_id={venueId}, assigned_date='{date}', assigned_time='{time}' WHERE venue_allocation_id={id}";
                dbHelper.ExecuteNonQueryy(query);

                MessageBox.Show("Venue allocation updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Please select a record to modify.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void DeleteAssignment()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string id = dataGridView1.SelectedRows[0].Cells["venue_allocation_id"].Value.ToString();
                string query = $"DELETE FROM venue_allocations WHERE venue_allocation_id={id}";

                dbHelper.ExecuteNonQueryy(query);

                MessageBox.Show("Venue assignment deleted!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Please select a record to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e){ }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e){ }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e){ }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e){ }

        private void button1_Click(object sender, EventArgs e)
        {
            ValidateSchedulingConflicts();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveVenueAssignment();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ModifyExistingAllocation();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DeleteAssignment();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadAllocations();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide(); 
            Form1 previousForm = new Form1();
            previousForm.Show();
        }
    }
}
