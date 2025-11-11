using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using BCrypt.Net;

namespace PosUmkm
{
    public partial class LoginPage : Form
    {
        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;

        private DataSet ds = new DataSet();
        private string alamat, query;
        public LoginPage()
        {
            alamat = "server=localhost; database=db_pos; username=root; password=;";
            koneksi = new MySqlConnection(alamat);
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            RegisterPage registerPage = new RegisterPage();
            registerPage.Show();
            this.Hide();
            registerPage.FormClosed += (s, args) => this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = txt_username.Text.Trim();
            string password = txt_password.Text.Trim();

            if (username == "" || password == "")
            {
                MessageBox.Show("Username dan Password wajib diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                koneksi.Open();

                // Gunakan parameter agar aman dari SQL Injection
                string query = "SELECT password, role, is_active FROM tbl_user WHERE username = @username LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, koneksi);
                cmd.Parameters.AddWithValue("@username", username);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string hashPassword = reader.GetString("password");
                    string role = reader.GetString("role");
                    int isActive = reader.GetInt32("is_active");

                    // 🔐 Verifikasi password dengan bcrypt
                    bool isValid = BCrypt.Net.BCrypt.Verify(password, hashPassword);

                    if (!isValid)
                    {
                        MessageBox.Show("Password salah!", "Gagal Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        reader.Close();
                        return;
                    }

                    if (isActive != 1)
                    {
                        MessageBox.Show("Akun anda belum aktif!", "Gagal Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        reader.Close();
                        return;
                    }

                    reader.Close();

                        Form1 userForm = new Form1(); // misal dashboard kasir
                        userForm.Show();
                    
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Username tidak ditemukan!", "Gagal Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
            finally
            {
                koneksi.Close();
            }
        }


        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
