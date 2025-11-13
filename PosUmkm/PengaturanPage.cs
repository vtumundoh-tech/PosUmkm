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

namespace PosUmkm
{
    public partial class PengaturanPage : Form
    {
        private MySqlConnection koneksi;
        private MySqlCommand perintah;
        private string alamat, query;
        private int userId;
        public PengaturanPage(int id_user)
        {
            InitializeComponent();

            alamat = "server=localhost; database=db_pos; username=root; password=;";
            koneksi = new MySqlConnection(alamat);
            userId = id_user;
            TampilkanDataUser();
        }

        private void TampilkanDataUser()
        {
            try
            {
                koneksi.Open();
                query = "SELECT nama_pemilik, nama_umkm, alamat, nomor_kontak " +
                        "FROM tbl_profile_user WHERE id_user = @id";
                perintah = new MySqlCommand(query, koneksi);
                perintah.Parameters.AddWithValue("@id", userId);

                MySqlDataReader reader = perintah.ExecuteReader();

                if (reader.Read())
                {
                    txt_namaPemilik.Text = reader["nama_pemilik"].ToString();
                    txt_namaUmkm.Text = reader["nama_umkm"].ToString();
                    txt_alamat.Text = reader["alamat"].ToString();
                    txt_noKontak.Text = reader["nomor_kontak"].ToString();
                }
                else
                {
                    txt_namaPemilik.Text = "";
                    txt_namaUmkm.Text = "";
                    txt_alamat.Text = "";
                    txt_noKontak.Text = "";
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Gagal menampilkan data user: " + ex.Message);
            }
            finally
            {
                koneksi.Close();
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PengaturanPage_Load(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void btn_ganti_Click_1(object sender, EventArgs e)
        {
            string passwordBaru = txt_gntiPassword.Text.Trim();

            // 🔹 Validasi input kosong
            if (string.IsNullOrEmpty(passwordBaru))
            {
                MessageBox.Show("⚠️ Masukkan password baru terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🔹 Validasi panjang minimum
            if (passwordBaru.Length < 6)
            {
                MessageBox.Show("⚠️ Password minimal 6 karakter!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🔹 Validasi kekuatan password (sama seperti RegisterPage)
            bool hasUpper = passwordBaru.Any(char.IsUpper);
            bool hasLower = passwordBaru.Any(char.IsLower);
            bool hasDigit = passwordBaru.Any(char.IsDigit);
            bool hasSymbol = passwordBaru.Any(ch => !char.IsLetterOrDigit(ch));

            if (!hasUpper || !hasLower || !hasDigit || !hasSymbol)
            {
                MessageBox.Show("⚠️ Password harus mengandung huruf besar, huruf kecil, angka, dan simbol!",
                    "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                koneksi.Open();

                // 🔹 Hash password baru
                string hash = BCrypt.Net.BCrypt.HashPassword(passwordBaru);

                query = "UPDATE tbl_user SET password = @password WHERE id_user = @id";
                perintah = new MySqlCommand(query, koneksi);
                perintah.Parameters.AddWithValue("@password", hash);
                perintah.Parameters.AddWithValue("@id", userId);
                perintah.ExecuteNonQuery();

                MessageBox.Show("✅ Password berhasil diganti!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_gntiPassword.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Gagal mengganti password: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                koneksi.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Buka form dashboard
            Form1 form1 = new Form1(userId);
            form1.Show();

            // Sembunyikan form pengaturan agar tidak double window
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ProductPage productPage = new ProductPage(userId);
            productPage.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Buka form pembelian
            PembelianPage pembelianPage = new PembelianPage(userId);
            pembelianPage.Show();

            // Sembunyikan form pengaturan agar tidak double window
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Buka form kasir
            KasirPage kasirPage = new KasirPage(userId);
            kasirPage.Show();

            // Sembunyikan form pengaturan agar tidak double window
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Buka form riwayat dan laporan
            RiwayatTransaksi riwayatTransaksi = new RiwayatTransaksi(userId);
            riwayatTransaksi.Show();

            // Sembunyikan form pengaturan agar tidak double window
            this.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                    "Apakah Anda yakin ingin logout?",
                    "Konfirmasi Logout",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

            if (result == DialogResult.Yes)
            {
                // Tampilkan kembali form login
                LoginPage loginPage = new LoginPage();
                loginPage.Show();

                // Sembunyikan form utama agar tidak menutup aplikasi
                this.Hide();
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            EditProfilePage editProfilePage = new EditProfilePage(userId);
            editProfilePage.Show();

            // Sembunyikan form utama agar tidak menutup aplikasi
            this.Hide();
        }

        private void txt_gntiPassword_TextChanged(object sender, EventArgs e)
        {
            string password = txt_gntiPassword.Text;
            int score = 0;

            // 🔹 Hitung skor kekuatan password
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

            pgs_strength.Value = score;
            pgs_strength.Style = ProgressBarStyle.Continuous;
            pgs_strength.ForeColor = color;
            pgs_strength.BackColor = Color.White;

            // 🔹 Update label dan progress bar
            lbl_kuat.Text = $"Kekuatan Password: {strengthText}";
            lbl_kuat.ForeColor = color;
        }
    }
}