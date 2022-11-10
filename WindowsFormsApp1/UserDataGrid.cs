using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class UserDataGrid : Form
    {
       
        public UserDataGrid()
        {
            InitializeComponent();
            this.FormClosing += UserDataGrid_FormClosing;
            
        }

        private void UserDataGrid_FormClosing(object sender, FormClosingEventArgs e)
        {
            uSERTableAdapter.Update(projectOneDataSet);
            projectOneDataSet.AcceptChanges();

        }

        private void UserDataGrid_Load(object sender, EventArgs e)
        {
            // TODO: Bu kod satırı 'projectOneDataSet.USER' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.uSERTableAdapter.Fill(this.projectOneDataSet.USER);

        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            uSERTableAdapter.Update(projectOneDataSet);
            projectOneDataSet.AcceptChanges();
        }
    }
}
