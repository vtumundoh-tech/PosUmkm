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
using BCrypt.Net;

namespace PosUmkm
{
    public partial class RegisterPage : Form
    {
        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;

        private DataSet ds = new DataSet();
        private string alamat, query;
        public RegisterPage()
        {
            alamat = "server=localhost; database=db_pos; username=root; password=;";
            koneksi = new MySqlConnection(alamat);
            InitializeComponent();
        }

        private void RegisterPage_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_password_TextChanged(object sender, EventArgs e)
        {
            {
                string password = txt_password.Text;
                int score = 0;

                // 🔹 Kriteria kekuatan
                if (password.Length >= 8)
                    score++;
                if (password.Any(char.IsUpper))
                    score++;
                if (password.Any(char.IsLower))
                    score++;
                if (password.Any(char.IsDigit))
                    score++;
                if (password.Any(ch => !char.IsLetterOrDigit(ch)))
                    score++;

                // 🔹 Tentukan teks & warna berdasarkan score
                string strengthText = "";
                Color color = Color.Gray;

                switch (score)
                {
                    case 0:
                    case 1:
                        strengthText = "Sangat Lemah";
                        color = Color.Red;
                        break;
                    case 2:
                        strengthText = "Lemah";
                        color = Color.OrangeRed;
                        break;
                    case 3:
                        strengthText = "Cukup";
                        color = Color.Orange;
                        break;
                    case 4:
                        strengthText = "Kuat";
                        color = Color.ForestGreen;
                        break;
                    case 5:
                        strengthText = "Sangat Kuat";
                        color = Color.Green;
                        break;
                }

                // 🔹 Update label dan progress bar
                lbl_passwordStrength.Text = $"Kekuatan Password: {strengthText}";
                lbl_passwordStrength.ForeColor = color;

                pgsBar_Strength.Value = score;
                pgsBar_Strength.ForeColor = color;

                // 🔹 Ubah warna progress bar (sedikit tricky karena WinForms default tidak mendukung warna langsung)
                pgsBar_Strength.Style = ProgressBarStyle.Continuous;
                pgsBar_Strength.BackColor = Color.White;
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void btn_daftar_Click(object sender, EventArgs e)
        {
            string username = txt_username.Text.Trim();
            string password = txt_password.Text.Trim();


            if (username == "" || password == "")
            {
                MessageBox.Show("Username dan Password wajib diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (password.Length < 6)
            {
                MessageBox.Show("Password minimal 6 karakter!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSymbol = password.Any(ch => !char.IsLetterOrDigit(ch));

            if (!hasUpper || !hasLower || !hasDigit || !hasSymbol)
            {
                MessageBox.Show("Password harus mengandung huruf besar, huruf kecil, angka, dan simbol!",
                    "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(alamat))
            {
                try
                {
                    conn.Open();

                    // cek username sudah ada
                    string checkQuery = "SELECT COUNT(*) FROM tbl_user WHERE username = @username";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@username", username);
                    int exists = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (exists > 0)
                    {
                        MessageBox.Show("Username sudah digunakan, silakan pilih yang lain.", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // buat kode_umkm
                    string lastCodeQuery = "SELECT kode_umkm FROM tbl_user ORDER BY id_user DESC LIMIT 1";
                    MySqlCommand lastCmd = new MySqlCommand(lastCodeQuery, conn);
                    object lastCodeObj = lastCmd.ExecuteScalar();
                    string newCode = "U001";
                    if (lastCodeObj != null)
                    {
                        string lastCode = lastCodeObj.ToString();
                        int number = int.Parse(lastCode.Substring(1));
                        newCode = "U" + (number + 1).ToString("D3");
                    }

                    // HASH password dengan bcrypt
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                    // INSERT
                    string insertQuery = @"INSERT INTO tbl_user 
                                   (kode_umkm, username, password, role, is_active, date_created) 
                                   VALUES (@kode, @user, @pass, 'kasir', 1, NOW())";
                    MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@kode", newCode);
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", hashedPassword);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Registrasi berhasil! Silakan login.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                }
            }
        }
    }
}
