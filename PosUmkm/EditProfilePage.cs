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

namespace PosUmkm
{
    public partial class EditProfilePage : Form
    {
        private MySqlConnection koneksi;
        private MySqlCommand perintah;

        private string alamat, query;
        private int userId;
        // Konstruktor parameterless — diperlukan Designer
        public EditProfilePage()
        {
            InitializeComponent();
            alamat = "server=localhost; database=db_pos; username=root; password=;";
            koneksi = new MySqlConnection(alamat);
            userId = 0; // default, artinya belum di-set dari luar
        }

        // Konstruktor runtime yang menerima id_user
        public EditProfilePage(int id_user) : this() // panggil konstruktor parameterless dulu
        {
            userId = id_user;
            // jika ingin memuat data profil yang sudah ada ke textbox saat membuka edit:
            LoadProfileIfExists();
        }

        private void LoadProfileIfExists()
        {
            try
            {
                koneksi.Open();
                query = "SELECT nama_pemilik, nama_umkm, alamat, nomor_kontak " +
                        "FROM tbl_profile_user WHERE id_user = @id";
                using (perintah = new MySqlCommand(query, koneksi))
                {
                    perintah.Parameters.AddWithValue("@id", userId);
                    using (var reader = perintah.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txt_namaPemilik.Text = reader["nama_pemilik"].ToString();
                            txt_namaUmkm.Text = reader["nama_umkm"].ToString();
                            txt_alamat.Text = reader["alamat"].ToString();
                            txt_nmrKontak.Text = reader["nomor_kontak"].ToString();
                        }
                        else
                        {
                            // kosongkan jika belum ada
                            txt_namaPemilik.Clear();
                            txt_namaUmkm.Clear();
                            txt_alamat.Clear();
                            txt_nmrKontak.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat profil: " + ex.Message);
            }
            finally
            {
                koneksi.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                koneksi.Open();

                // cek apakah profil sudah ada
                query = "SELECT COUNT(*) FROM tbl_profile_user WHERE id_user = @id";
                using (perintah = new MySqlCommand(query, koneksi))
                {
                    perintah.Parameters.AddWithValue("@id", userId);
                    int count = Convert.ToInt32(perintah.ExecuteScalar());

                    if (count > 0)
                    {
                        // update
                        query = @"UPDATE tbl_profile_user 
                                  SET nama_pemilik = @pemilik, nama_umkm = @umkm, alamat = @alamat, nomor_kontak = @kontak 
                                  WHERE id_user = @id";
                    }
                    else
                    {
                        // insert
                        query = @"INSERT INTO tbl_profile_user (id_user, nama_pemilik, nama_umkm, alamat, nomor_kontak)
                                  VALUES (@id, @pemilik, @umkm, @alamat, @kontak)";
                    }
                }

                using (perintah = new MySqlCommand(query, koneksi))
                {
                    perintah.Parameters.AddWithValue("@id", userId);
                    perintah.Parameters.AddWithValue("@pemilik", txt_namaPemilik.Text.Trim());
                    perintah.Parameters.AddWithValue("@umkm", txt_namaUmkm.Text.Trim());
                    perintah.Parameters.AddWithValue("@alamat", txt_alamat.Text.Trim());
                    perintah.Parameters.AddWithValue("@kontak", txt_nmrKontak.Text.Trim());

                    perintah.ExecuteNonQuery();
                }

                MessageBox.Show("✅ Data profil berhasil disimpan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK; // agar pemanggil tahu ada perubahan
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menyimpan data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                koneksi.Close();
            }
        }

        private void btn_batal_Click(object sender, EventArgs e)
        {
            PengaturanPage pengaturanPage = new PengaturanPage(userId);
            pengaturanPage.Show();
            this.Hide();
        }

        private void EditProfilePage_Load(object sender, EventArgs e)
        {

        }
    }
}
