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
    public partial class Form7: Form
    {
        DatabaseHelper dbHelper = DatabaseHelper.Instance;
        public Form7()
        {
            InitializeComponent();
            LoadTransactions();
        }
        private void LoadTransactions()
        {
            string query = @"
            SELECT 
                f.transaction_id,
                l.value AS transaction_type,
                f.amount,
                s1.sponsor_name AS from_sponsor,
                v1.vendor_name AS from_vendor,
                c1.committee_name AS from_committee,
                s2.sponsor_name AS to_sponsor,
                v2.vendor_name AS to_vendor,
                c2.committee_name AS to_committee,
                f.description,
                f.date_recorded
            FROM finances f
            LEFT JOIN lookup l ON f.type_id = l.type_id
            LEFT JOIN sponsors s1 ON f.from_entity_type = 'Sponsor' AND f.from_entity_id = s1.sponsor_id
            LEFT JOIN vendors v1 ON f.from_entity_type = 'Vendor' AND f.from_entity_id = v1.vendor_id
            LEFT JOIN committees c1 ON f.from_entity_type = 'Committee' AND f.from_entity_id = c1.committee_id
            LEFT JOIN sponsors s2 ON f.to_entity_type = 'Sponsor' AND f.to_entity_id = s2.sponsor_id
            LEFT JOIN vendors v2 ON f.to_entity_type = 'Vendor' AND f.to_entity_id = v2.vendor_id
            LEFT JOIN committees c2 ON f.to_entity_type = 'Committee' AND f.to_entity_id = c2.committee_id
            ";

            dataGridView1.DataSource = dbHelper.GetData(query);
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            DataTable typeData = dbHelper.GetData("SELECT lookup_id AS type_id, value FROM lookup WHERE category = 'TransactionType'");
            comboBox1.DataSource = typeData;
            comboBox1.DisplayMember = "value";

            DataTable sponsorData = dbHelper.GetData("SELECT sponsor_id, sponsor_name FROM sponsors");
            comboBox2.DataSource = sponsorData;
            comboBox2.DisplayMember = "sponsor_name";
            comboBox2.ValueMember = "sponsor_id";

            DataTable committeeData = dbHelper.GetData("SELECT committee_id, committee_name FROM committees");
            comboBox3.DataSource = committeeData;
            comboBox3.DisplayMember = "committee_name";
            comboBox3.ValueMember = "committee_id";

            LoadTransactions();
        }

        private void textBox1_TextChanged(object sender, EventArgs e){ }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e){ }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e){ }

        private void textBox2_TextChanged(object sender, EventArgs e){ }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e){ }

        private void button1_Click(object sender, EventArgs e)
        {
            string transactionType = comboBox1.SelectedValue?.ToString();
            string amount = textBox1.Text.Trim();
            string fromEntityType = "Sponsor";
            string fromEntityId = comboBox2.SelectedValue?.ToString();
            string toEntityType = "Committee";
            string toEntityId = comboBox3.SelectedValue?.ToString();
            string description = textBox2.Text.Trim();
            string dateRecorded = dateTimePicker1.Value.ToString("yyyy-MM-dd");

            string query = @"
                INSERT INTO finances 
                (itec_id, event_id, type_id, amount, from_entity_type, from_entity_id, to_entity_type, to_entity_id, description, date_recorded)
                VALUES (1, 1, @type_id, @amount, @from_entity_type, @from_entity_id, @to_entity_type, @to_entity_id, @description, @date_recorded)";

            MySqlParameter[] parameters =
            {
                new MySqlParameter("@type_id", transactionType),
                new MySqlParameter("@amount", amount),
                new MySqlParameter("@from_entity_type", fromEntityType),
                new MySqlParameter("@from_entity_id", fromEntityId),
                new MySqlParameter("@to_entity_type", toEntityType),
                new MySqlParameter("@to_entity_id", toEntityId),
                new MySqlParameter("@description", description),
                new MySqlParameter("@date_recorded", dateRecorded),
            };

            if (dbHelper.ExecuteNonQuery(query, parameters))
            {
                MessageBox.Show("Financial record saved successfully!");
                LoadTransactions();
            }
            else
            {
                MessageBox.Show("Error saving record.");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a record to modify.");
                return;
            }

            int recordId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["transaction_id"].Value);
            string transactionType = comboBox1.SelectedValue.ToString();
            string amount = textBox1.Text;
            string fromEntityType = "Sponsor";
            string fromEntityId = comboBox2.SelectedValue.ToString();
            string toEntityType = "Committee";
            string toEntityId = comboBox3.SelectedValue.ToString();
            string description = textBox2.Text;
            string dateRecorded = dateTimePicker1.Value.ToString("yyyy-MM-dd");

            string query = "UPDATE finances SET type_id = @type_id, amount = @amount, from_entity_type = @from_entity_type, from_entity_id = @from_entity_id, " +
               "to_entity_type = @to_entity_type, to_entity_id = @to_entity_id, description = @description, date_recorded = @date_recorded " +
               "WHERE transaction_id = @transaction_id";

            MySqlParameter[] parameters =
            {
                new MySqlParameter("@type_id", transactionType),
                new MySqlParameter("@amount", amount),
                new MySqlParameter("@from_entity_type", fromEntityType),
                new MySqlParameter("@from_entity_id", fromEntityId),
                new MySqlParameter("@to_entity_type", toEntityType),
                new MySqlParameter("@to_entity_id", toEntityId),
                new MySqlParameter("@description", description),
                new MySqlParameter("@date_recorded", dateRecorded),
                new MySqlParameter("@transaction_id", recordId),
            };

            if (dbHelper.ExecuteNonQuery(query, parameters))
            {
                MessageBox.Show("Record updated successfully!");
                LoadTransactions();
            }
            else
            {
                MessageBox.Show("Error updating record.");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }

            int recordId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["transaction_id"].Value);
            string query = "DELETE FROM finances WHERE transaction_id = @transaction_id";

            MySqlParameter[] parameters =
            {
                new MySqlParameter("@transaction_id", recordId)
            };

            if (dbHelper.ExecuteNonQuery(query, parameters))
            {
                MessageBox.Show("Record deleted successfully!");
                LoadTransactions();
            }
            else
            {
                MessageBox.Show("Error deleting record.");
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textBox1.Text = row.Cells["amount"].Value.ToString();

                string fromEntity = row.Cells["from_sponsor"].Value?.ToString();
                if (string.IsNullOrEmpty(fromEntity))
                    fromEntity = row.Cells["from_vendor"].Value?.ToString();
                if (string.IsNullOrEmpty(fromEntity))
                    fromEntity = row.Cells["from_committee"].Value?.ToString();

                string toEntity = row.Cells["to_sponsor"].Value?.ToString();
                if (string.IsNullOrEmpty(toEntity))
                    toEntity = row.Cells["to_vendor"].Value?.ToString();
                if (string.IsNullOrEmpty(toEntity))
                    toEntity = row.Cells["to_committee"].Value?.ToString();

                comboBox1.SelectedValue = GetComboBoxValue(comboBox1, row.Cells["transaction_type"].Value.ToString());
                comboBox2.SelectedValue = GetComboBoxValue(comboBox2, fromEntity);
                comboBox3.SelectedValue = GetComboBoxValue(comboBox3, toEntity);

                textBox2.Text = row.Cells["description"].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells["date_recorded"].Value);

            }
        }
        private object GetComboBoxValue(ComboBox comboBox, string value)
        {
            foreach (DataRowView item in comboBox.Items)
            {
                if (item[comboBox.DisplayMember].ToString() == value)
                    return item[comboBox.ValueMember];
            }
            return null;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide(); 
            Form1 previousForm = new Form1(); 
            previousForm.Show();
        }
    }
}