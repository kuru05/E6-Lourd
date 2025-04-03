using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using BCrypt.Net;
using System.Linq.Expressions;


namespace E6_lourd
{
    public partial class Form1 : Form
    {
        private MySqlConnection? conn;
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Veuillez remplir toutes les informations.");
                return;
            }

            string servername = "localhost";
            string dbname = "bdd_cpx";
            string dbusername = "root";
            string dbpassword = "";

            string connString = $"Server={servername};Database={dbname};Uid={dbusername};Pwd={dbpassword};";

            try
            {
                conn = new MySqlConnection(connString);
                conn.Open();

                string query = "SELECT password FROM users WHERE email = @Email";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows && reader.Read())
                {
                    string storedPassword = reader.GetString(0);
                    if (BCrypt.Net.BCrypt.Verify(password, storedPassword))
                    {
                        MessageBox.Show("Connexion réussie !");
                        Form2 form2 = new Form2();
                        form2.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Mot de passe incorrect.");
                    }
                }
                else
                {
                    MessageBox.Show("Utilisateur non trouvé.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion : " + ex.Message);
            }
            finally
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
