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
    public partial class Form4: Form
    {
         DatabaseHelper dbHelper =  DatabaseHelper.Instance;
         
        public Form4()
        {
            InitializeComponent();
            LoadData();
        }

        private void label1_Click(object sender, EventArgs e){ }

        private void textBox1_TextChanged(object sender, EventArgs e){ }

        private void textBox2_TextChanged(object sender, EventArgs e){ }

        private void textBox3_TextChanged(object sender, EventArgs e){ }

        private void textBox4_TextChanged(object sender, EventArgs e){ }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e){ }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e){ }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e){ }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string insertParticipantQuery = "INSERT INTO participants (itec_id, name, email, contact, institute, role_id) " +
                                                "VALUES (@itec_id, @name, @email, @contact, @institute, @role_id)";

                using (var conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(insertParticipantQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@itec_id", 1);
                        cmd.Parameters.AddWithValue("@name", textBox1.Text);
                        cmd.Parameters.AddWithValue("@email", textBox2.Text);
                        cmd.Parameters.AddWithValue("@contact", textBox3.Text);
                        cmd.Parameters.AddWithValue("@institute", textBox4.Text);
                        cmd.Parameters.AddWithValue("@role_id", GetRoleId(comboBox1.SelectedItem?.ToString()));

                        cmd.ExecuteNonQuery();

                        long participantId = cmd.LastInsertedId;

                        foreach (var selectedItem in checkedListBox1.CheckedItems)
                        {
                            int eventId = GetEventId(selectedItem.ToString());
                            int paymentStatusId = GetPaymentStatusId(comboBox2.SelectedItem?.ToString());

                            string insertEventParticipant = "INSERT INTO event_participants (event_id, participant_id, payment_status_id, fee_amount) " +
                                                            "VALUES (@event_id, @participant_id, @payment_status_id, @fee_amount)";

                            using (MySqlCommand eventCmd = new MySqlCommand(insertEventParticipant, conn))
                            {
                                eventCmd.Parameters.AddWithValue("@event_id", eventId);
                                eventCmd.Parameters.AddWithValue("@participant_id", participantId);
                                eventCmd.Parameters.AddWithValue("@payment_status_id", paymentStatusId);
                                eventCmd.Parameters.AddWithValue("@fee_amount", 0);

                                eventCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                MessageBox.Show("Participant Added Successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting participant: " + ex.Message); // 🔴 Now it will show the actual error message!
            }
            finally
            {
                LoadData();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string query = "UPDATE participants SET email=@email, contact=@contact, institute=@institute, role_id=@role_id " +
                           "WHERE name=@name";

            MySqlParameter[] parameters =
            {
                new MySqlParameter("@name", textBox1.Text),
                new MySqlParameter("@email", textBox2.Text),
                new MySqlParameter("@contact", textBox3.Text),
                new MySqlParameter("@institute", textBox4.Text),
                new MySqlParameter("@role_id", GetRoleId(comboBox1.SelectedItem?.ToString()))
            };

            dbHelper.ExecuteNonQuery(query, parameters);
            MessageBox.Show("Participant Updated!");
            LoadData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM participants WHERE name=@name";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@name", textBox1.Text)
            };

            dbHelper.ExecuteNonQuery(query, parameters);
            MessageBox.Show("Participant Deleted!");
            LoadData();
        }
        private void LoadData() 
        {
            string query = "SELECT * FROM participants";
            dataGridView1.DataSource = dbHelper.GetDataTable(query);
        }
        
        private void LoadComboBoxes()
        {
            comboBox1.DataSource = dbHelper.GetDataTable("SELECT value FROM lookup WHERE category = 'role'");
            comboBox1.DisplayMember = "value";

            comboBox2.DataSource = dbHelper.GetDataTable("SELECT value FROM lookup WHERE category = 'payment_status'");
            comboBox2.DisplayMember = "value";
        }

        private void LoadCheckedListBox()
        {
            DataTable dt = dbHelper.GetDataTable("SELECT event_name FROM itec_events");
            checkedListBox1.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                checkedListBox1.Items.Add(row["event_name"].ToString());
            }
        }

        private string GetSelectedEvents() 
        {
            string selectedEvents = "";
            foreach (var item in checkedListBox1.CheckedItems)
            {
                selectedEvents += item.ToString() + ", ";
            }
            return selectedEvents.TrimEnd(',', ' ');
        }
        private void Form4_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadComboBoxes();
            LoadCheckedListBox();
        }
        private int GetRoleId(string roleName)
        {
            return GetLookupId("SELECT lookup_id FROM lookup WHERE category = 'role' AND value = @value", roleName);
        }

        private int GetPaymentStatusId(string status)
        {
            return GetLookupId("SELECT lookup_id FROM lookup WHERE category = 'payment_status' AND value = @value", status);
        }

        private int GetEventId(string eventName)
        {
            return GetLookupId("SELECT event_id FROM itec_events WHERE event_name = @value", eventName);
        }

        private int GetLookupId(string query, string value)
        {
            MySqlParameter[] parameters = { new MySqlParameter("@value", value) };
            object result = dbHelper.ExecuteScalar(query, parameters);
            return result != null ? Convert.ToInt32(result) : -1;
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
