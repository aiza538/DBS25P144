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
    public partial class Form2: Form
    {
        DatabaseHelper dbHelper =  DatabaseHelper.Instance;
        public Form2()
        {
            InitializeComponent();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e){ }
        private void textBox1_TextChanged(object sender, EventArgs e){ }
        private void textBox3_TextChanged(object sender, EventArgs e){ }
        private void textBox2_TextChanged(object sender, EventArgs e){ }
        private void button1_Click(object sender, EventArgs e)
        {
            string editionYear = textBox1.Text.Trim();
            if (!int.TryParse(editionYear, out _))
            {
                MessageBox.Show("Year must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string theme = textBox2.Text.Trim();
            string description = textBox3.Text.Trim();

            if (string.IsNullOrEmpty(editionYear) || string.IsNullOrEmpty(theme) || string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Please fill all fields!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO itec_editions (year, theme, description) VALUES (@year, @theme, @desc)";

            dbHelper.ExecuteParameterizedQuery(query, new MySqlParameter[]
            {
                new MySqlParameter("@year", editionYear),
                new MySqlParameter("@theme", theme),
                new MySqlParameter("@desc", description)
            });

            MessageBox.Show("Edition Added Successfully!");
            LoadGridData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Please select an edition to update.");
                return;
            }

            int itec_Id = Convert.ToInt32(comboBox1.SelectedValue);
            string editionYear = textBox1.Text.Trim();
            string theme = textBox2.Text.Trim();
            string description = textBox3.Text.Trim();

            string query = "UPDATE itec_editions SET year = @year, theme = @theme, description = @desc WHERE itec_id = @id";

            dbHelper.ExecuteParameterizedQuery(query, new MySqlParameter[]
            {
                new MySqlParameter("@year", editionYear),
                   new MySqlParameter("@theme", theme),
                new MySqlParameter("@desc", description),
                new MySqlParameter("@id", itec_Id)
            });

            MessageBox.Show("Edition Updated Successfully!");
            LoadGridData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Please select an edition to delete.");
                return;
            }

            int itec_Id = Convert.ToInt32(comboBox1.SelectedValue);
            string query = "DELETE FROM itec_editions WHERE itec_id = @id";

            dbHelper.ExecuteParameterizedQuery(query, new MySqlParameter[]
            {
                new MySqlParameter("@id", itec_Id)
            });

            MessageBox.Show("Edition Deleted Successfully!");
            LoadGridData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                comboBox1.SelectedValue = row.Cells["itec_id"].Value;
                textBox1.Text = row.Cells["year"].Value.ToString();
                textBox2.Text = row.Cells["theme"].Value.ToString();
                textBox3.Text = row.Cells["description"].Value.ToString();
            }

        }
        private void LoadEditions()
        {
            string query = "SELECT itec_id, year FROM itec_editions";
            DataTable dt = dbHelper.GetDataTable(query);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("No editions found in the database.");
                return;
            }

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "year";  
            comboBox1.ValueMember = "itec_id";
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.SelectedIndex = -1;
            comboBox1.Refresh();

        }
        private void LoadGridData()
        {
            try
            {
                string query = "SELECT * FROM itec_editions ORDER BY year DESC";
                DataTable dt = dbHelper.GetDataTable(query);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No editions found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading editions: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            LoadEditions(); 
            LoadGridData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide(); 
            Form1 previousForm = new Form1(); 
            previousForm.Show();
        }
    }
}
