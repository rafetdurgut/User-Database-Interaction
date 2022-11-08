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
    public partial class UserPanel : Form
    {
        public static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ProjectOne;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public SqlConnection sqlConnection = new SqlConnection(connectionString);
        public UserPanel()
        {
            InitializeComponent();
            LoadFromDatabase();
            
        }

        private void LoadFromDatabase()
        {
            try
            {
                listView1.Items.Clear();
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.[USER]", sqlConnection);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ListViewItem li = new ListViewItem(dr["Id"].ToString());
                    li.SubItems.Add(dr["Name"].ToString());
                    li.SubItems.Add(dr["Surname"].ToString());
                    li.SubItems.Add(dr["Email"].ToString());
                    li.SubItems.Add(dr["LastLoginDate"].ToString());

                    listView1.Items.Add(li);
                }
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                sqlConnection.Close();

                MessageBox.Show(e.Message);
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtSurname.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                return;
            }

            SqlCommand sqlCommand = new SqlCommand("INSERT INTO dbo.[USER] (Name, Surname, Password, Email, LastLoginDate) " +
                "VALUES('" + txtUserName.Text + "', '" + txtSurname.Text + "', '" + txtPassword.Text + "', '" + txtPassword.Text + "' ,'" + DateTime.Now.ToString("s") + "')", sqlConnection);
            try
            {

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                LoadFromDatabase();
            }
            catch (SqlException ex)
            {

                sqlConnection.Close();
                MessageBox.Show(ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                if(item.Selected)
                {
                    SqlCommand sqlCommand = new SqlCommand("DELETE FROM dbo.[USER] WHERE Id = " + item.Text, sqlConnection);
                    try
                    {

                        sqlConnection.Open();
                        sqlCommand.ExecuteNonQuery();
                        sqlConnection.Close();
                        LoadFromDatabase();
                    }
                    catch (SqlException ex)
                    {

                        sqlConnection.Close();
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUpdateEmail.Text) || string.IsNullOrEmpty(txtUpdateName.Text) || string.IsNullOrEmpty(txtUpdateSurname.Text))
            {
                return;
            }

            SqlCommand sqlCommand = new SqlCommand("UPDATE dbo.[USER] SET Name=@name, Surname=@surname, Email=@email, Password=@password WHERE Id="+ listView1.SelectedItems[0].Text, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@name",txtUpdateName.Text);
            sqlCommand.Parameters.AddWithValue("@surname", txtUpdateSurname.Text);
            sqlCommand.Parameters.AddWithValue("@email", txtUpdateEmail.Text);
            sqlCommand.Parameters.AddWithValue("@password", txtUpdatePass.Text);


            try
            {

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                LoadFromDatabase();
            }
            catch (SqlException ex)
            {

                sqlConnection.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count > 0)
            {
                groupBox2.Show();
                string id = listView1.SelectedItems[0].Text;

                try
                {
                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.[USER] WHERE Id = "+ id + "", sqlConnection);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        txtUpdateEmail.Text = dr["Email"].ToString();
                        txtUpdateName.Text = dr["Name"].ToString();
                        txtUpdateSurname.Text = dr["Surname"].ToString();
                        txtUpdatePass.Text = dr["Password"].ToString();
                    }
                    sqlConnection.Close();
                }
                catch (Exception ee)
                {
                    sqlConnection.Close();

                    MessageBox.Show(ee.Message);
                }
            }
            else
            {
                groupBox2.Hide();
            }
        }
    }
}
