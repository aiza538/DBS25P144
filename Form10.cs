using MidProjectDb;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MidProjectDb
{
    public partial class Form10 : Form
    {
        DatabaseHelper dbHelper = DatabaseHelper.Instance;
        public Form10()
        {
            InitializeComponent();
        }
        private void Form10_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(new string[] {
            "Event Report", "Participant Report",
            "Committee Report", "Financial Report", "Event Schedule Report"
            });
            comboBox2.Items.AddRange(GetEventNames());
            comboBox3.Items.AddRange(GetCommitteeNames());
        }
        private string[] GetEventNames()
        {
            string query = "SELECT DISTINCT event_name FROM itec_events";
            DataTable dt = dbHelper.GetDataTable(query);
            ArrayList eventNames = new ArrayList();

            foreach (DataRow row in dt.Rows)
            {
                eventNames.Add(row["event_name"]); 
            }

            return (string[])eventNames.ToArray(typeof(string)); 
        }
        private string[] GetCommitteeNames()
        {
            string query = "SELECT DISTINCT committee_name FROM committees";
            DataTable dt = dbHelper.GetDataTable(query);
            ArrayList committeeNames = new ArrayList();

            foreach (DataRow row in dt.Rows)
            {
                committeeNames.Add(row["committee_name"]); 
            }

            return (string[])committeeNames.ToArray(typeof(string)); 
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            string reportType = comboBox1.Text;
            string eventFilter = comboBox2.Text;
            string committeeFilter = comboBox3.Text;
            string startDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string endDate = dateTimePicker2.Value.ToString("yyyy-MM-dd");

            string query = "";
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            switch (reportType)
            {
                case "Participant Report":
                    query = "SELECT p.name, p.email, p.institute, ep.event_id, e.event_name " +
                            "FROM participants p " +
                            "JOIN event_participants ep ON p.participant_id = ep.participant_id " +
                            "JOIN itec_events e ON ep.event_id = e.event_id " +
                            "WHERE e.event_date BETWEEN @start AND @end";
                    parameters.Add(new MySqlParameter("@start", startDate));
                    parameters.Add(new MySqlParameter("@end", endDate));
                    if (!string.IsNullOrEmpty(eventFilter))
                    {
                        query += " AND e.event_name = @event";
                        parameters.Add(new MySqlParameter("@event", eventFilter));
                    }
                    break;

                case "Financial Report":
                    query = "SELECT f.transaction_id, f.amount, f.type_id, f.description, f.date_recorded, l.value AS type " +
                            "FROM finances f " +
                            "JOIN lookup l ON f.type_id = l.lookup_id " +
                            "WHERE f.date_recorded BETWEEN @start AND @end";
                    parameters.Add(new MySqlParameter("@start", startDate));
                    parameters.Add(new MySqlParameter("@end", endDate));
                    break;

                case "Committee Report":
                    query = "SELECT c.committee_name, p.name AS member_name, l.value AS role " +
                            "FROM committees c " +
                            "JOIN committee_members cm ON c.committee_id = cm.committee_id " +
                            "JOIN participants p ON cm.participant_id = p.participant_id " +
                            "JOIN lookup l ON cm.role_id = l.lookup_id " +
                            "WHERE 1=1";
                    if (!string.IsNullOrEmpty(committeeFilter))
                    {
                        query += " AND c.committee_name = @committee";
                        parameters.Add(new MySqlParameter("@committee", committeeFilter));
                    }
                    break;

                case "Event Schedule Report":
                    query = "SELECT event_name, event_date, location, description " +
                            "FROM itec_events " +
                            "WHERE event_date BETWEEN @start AND @end";
                    parameters.Add(new MySqlParameter("@start", startDate));
                    parameters.Add(new MySqlParameter("@end", endDate));
                    break;

                default:
                    MessageBox.Show("Please select a valid report type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
            }

            try
            {
                DataTable dt = dbHelper.GetDataTablee(query, parameters.ToArray());
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading report:\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No data to export!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFile = new SaveFileDialog
            {
                Filter = "PDF Files|*.pdf",
                Title = "Save as PDF"
            };

            
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No data to export!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFile = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Save as Excel"
            };   
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e){ }
        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 previousForm = new Form1(); 
            previousForm.Show();
        }
    }
}