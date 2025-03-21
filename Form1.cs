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
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e){ }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 eventForm = new Form3();
            eventForm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You are already on the Dashboard!");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form5 committeeForm = new Form5();
            committeeForm.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form7 financialForm = new Form7();
            financialForm.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form4 participantForm = new Form4();
            participantForm.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form6 dutyAssignmentForm = new Form6();
            dutyAssignmentForm.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form8 venueAllocationForm = new Form8();
            venueAllocationForm.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form9 eventResultForm = new Form9();
            eventResultForm.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form10 reportForm = new Form10();
            reportForm.Show();
            this.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Form11 userForm = new Form11();
            userForm.Show();
            this.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Form2 editionForm = new Form2();
            editionForm.Show();
            this.Hide();
        }
    }
}
