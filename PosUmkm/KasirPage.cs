using MySql.Data;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Bcpg;
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
    public partial class KasirPage : Form
    {
        string connStr = "server=localhost;uid=root;pwd=;database=db_pos;";
        private int userId;
        public KasirPage(int id_user)
        {
            userId = id_user;

            InitializeComponent();

            // Hubungkan event load
            this.Load += KasirPage_Load;

            dgv_transaksi.Columns.Clear();
            dgv_transaksi.Columns.Add("Kode", "Kode Barang");
            dgv_transaksi.Columns.Add("Nama", "Nama Barang");
            dgv_transaksi.Columns.Add("Qty", "Jumlah");
            dgv_transaksi.Columns.Add("Harga", "Harga Satuan");
            dgv_transaksi.Columns.Add("Total", "Total");
            dgv_transaksi.Columns.Add("Keterangan", "Keterangan");
            dgv_transaksi.Columns.Add("Request", "Request");
        }

        private void TampilDataProduk()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = "SELECT kode_barang, nama_barang, harga_jual, stok FROM tbl_product";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgv_produkKasir.DataSource = dt;

                    // Atur header kolom agar rapi
                    dgv_produkKasir.Columns["kode_barang"].HeaderText = "Kode Barang";
                    dgv_produkKasir.Columns["nama_barang"].HeaderText = "Nama Barang";
                    dgv_produkKasir.Columns["harga_jual"].HeaderText = "Harga";
                    dgv_produkKasir.Columns["stok"].HeaderText = "Stok";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Gagal menampilkan data produk: " + ex.Message);
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void HitungTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dgv_transaksi.Rows)
            {
                if (row.Cells["Total"].Value != null)
                    total += Convert.ToDecimal(row.Cells["Total"].Value);
            }
            lbl_total.Text = "Rp. " + total.ToString("N0");
        }

        private void BuatTransaksiBaru()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // 🔹 Cek transaksi terakhir (kalau mau disimpan urutan)
                    string query = "SELECT no_transaksi FROM tbl_transaksi ORDER BY id_transaksi DESC LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    object result = cmd.ExecuteScalar();

                    // 🔹 Bagian yang kamu minta
                    // Kode dasar transaksi (bisa tetap, contoh: 0027070)
                    string kodeDasar = "0027070";

                    // 🔹 Ambil tanggal, bulan, dan tahun saat ini
                    string tanggal = DateTime.Now.ToString("dd");  // contoh: 25
                    string bulan = DateTime.Now.ToString("MM");    // contoh: 12
                    string tahun = DateTime.Now.ToString("yyyy");  // contoh: 2025

                    // 🔹 Susun kode transaksi sesuai format
                    string nextNo = $"{kodeDasar}{tanggal}{bulan}/INV/VT/{tahun}";

                    // 🔹 (Opsional) Jika ingin menambahkan nomor urut tambahan di akhir
                    // Bisa aktifkan baris berikut:
                    
                    if (result != null && result != DBNull.Value)
                    {
                        int lastId = 1;
                        int.TryParse(result.ToString().Split('/')[0].Substring(kodeDasar.Length), out lastId);
                        nextNo = $"{kodeDasar}{tanggal}{bulan}/INV/VT/{tahun}-{lastId + 1}";
                    }
                    

                    txt_noTransaksi.Text = nextNo;
                }

                // 🔹 Set tanggal dan jam otomatis
                dtp_tanggalKasir.Value = DateTime.Now;
                txt_jam.Text = DateTime.Now.ToString("HH:mm:ss");
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Gagal generate nomor transaksi: " + ex.Message);
            }
        }



        private void LoadDataProduk()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = "SELECT kode_barang, nama_barang, harga_jual, stok, satuan FROM tbl_product";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgv_produkKasir.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data produk: " + ex.Message);
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            MessageBox.Show("Timer jalan!");

            txt_jam.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Buka form dashboard
            Form1 form1Page = new Form1();
            form1Page.Show();

            // Sembunyikan form product agar tidak double window
            this.Hide();
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void KasirPage_Load(object sender, EventArgs e)
        {
            BuatTransaksiBaru();
            TampilDataProduk();

            // Timer untuk update jam real-time
            tmr_kasir.Interval = 1000;
            tmr_kasir.Tick += (s, ev) =>
            {
                txt_jam.Text = DateTime.Now.ToString("HH:mm:ss");
            };
            tmr_kasir.Start();
        }


        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txt_noTransaksi_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string query = "SELECT * FROM tbl_product";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                TampilDataProduk();
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgv_produkKasir.DataSource = dt;
            }
        }

        private void btn_inputPesanan_Click(object sender, EventArgs e)
        {
            if (dgv_produkKasir.SelectedRows.Count > 0)
            {
                string kode = dgv_produkKasir.SelectedRows[0].Cells["kode_barang"].Value.ToString();
                string nama = dgv_produkKasir.SelectedRows[0].Cells["nama_barang"].Value.ToString();
                decimal harga = Convert.ToDecimal(dgv_produkKasir.SelectedRows[0].Cells["harga_jual"].Value);

                if (string.IsNullOrWhiteSpace(txt_jumlah.Text))
                {
                    MessageBox.Show("Masukkan jumlah terlebih dahulu!");
                    return;
                }

                int qty = int.Parse(txt_jumlah.Text);
                decimal total = harga * qty;

                dgv_transaksi.Rows.Add(kode, nama, qty, harga, total, txt_keterangan.Text, txt_request.Text);
                HitungTotal();

                // Hapus input setelah dimasukkan
                txt_jumlah.Clear();
                txt_keterangan.Clear();
                txt_request.Clear();
            }
            else
            {
                MessageBox.Show("Pilih produk terlebih dahulu!");
            }
        }

        private void txt_cariProduk_TextChanged(object sender, EventArgs e)
        {
            string cari = txt_cariProduk.Text;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT kode_barang, nama_barang, harga_jual, stok FROM tbl_product WHERE nama_barang LIKE @cari OR kode_barang LIKE @cari";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@cari", "%" + cari + "%");
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgv_produkKasir.DataSource = dt; // bisa pakai DataGridView terpisah khusus pencarian
            }
        }

        private void txt_bayar_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txt_bayar.Text, out decimal bayar))
            {
                decimal total = 0;
                foreach (DataGridViewRow row in dgv_transaksi.Rows)
                {
                    if (row.Cells["Total"].Value != null)
                        total += Convert.ToDecimal(row.Cells["Total"].Value);
                }

                decimal kembali = bayar - total;
                txt_kembalian.Text = kembali.ToString("N0");
            }
        }

        private void btn_selesaiTransaksi_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string queryTrans = "INSERT INTO tbl_transaksi (no_transaksi, tanggal, jam, total, bayar, kembalian, status) " +
                        "VALUES (@no, @tgl, @jam, @total, @bayar, @kembali, @status)";
                    MySqlCommand cmd = new MySqlCommand(queryTrans, conn, trans);
                    cmd.Parameters.AddWithValue("@no", txt_noTransaksi.Text);
                    cmd.Parameters.AddWithValue("@tgl", dtp_tanggalKasir.Value.Date);
                    cmd.Parameters.AddWithValue("@jam", DateTime.Now.ToString("HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@total", lbl_total.Text.Replace("Rp. ", "").Replace(".", ""));
                    cmd.Parameters.AddWithValue("@bayar", string.IsNullOrWhiteSpace(txt_bayar.Text) ? 0 : Convert.ToDecimal(txt_bayar.Text));
                    cmd.Parameters.AddWithValue("@kembali", string.IsNullOrWhiteSpace(txt_kembalian.Text) ? 0 : Convert.ToDecimal(txt_kembalian.Text));
                    cmd.Parameters.AddWithValue("@status", "Berhasil");

                    cmd.ExecuteNonQuery();

                    long lastIdTransaksi = cmd.LastInsertedId; // 🔹 ambil id transaksi

                    foreach (DataGridViewRow row in dgv_transaksi.Rows)
                    {
                        if (row.Cells["Kode"].Value != null)
                        {
                            string queryDetail = "INSERT INTO tbl_detail_transaksi (id_transaksi, no_transaksi, kode_barang, nama_barang, qty, harga, total, keterangan, request, status) " +
                                                 "VALUES (@id_transaksi, @no, @kode, @nama, @qty, @harga, @total, @ket, @req, @status)";
                            MySqlCommand cmdDetail = new MySqlCommand(queryDetail, conn, trans);

                            cmdDetail.Parameters.AddWithValue("@id_transaksi", lastIdTransaksi);
                            cmdDetail.Parameters.AddWithValue("@no", txt_noTransaksi.Text);
                            cmdDetail.Parameters.AddWithValue("@kode", row.Cells["Kode"].Value);
                            cmdDetail.Parameters.AddWithValue("@nama", row.Cells["Nama"].Value);
                            cmdDetail.Parameters.AddWithValue("@qty", row.Cells["Qty"].Value);
                            cmdDetail.Parameters.AddWithValue("@harga", row.Cells["Harga"].Value);
                            cmdDetail.Parameters.AddWithValue("@total", row.Cells["Total"].Value);
                            cmdDetail.Parameters.AddWithValue("@ket", row.Cells["Keterangan"].Value);
                            cmdDetail.Parameters.AddWithValue("@req", row.Cells["Request"].Value);
                            cmdDetail.Parameters.AddWithValue("@status", "Berhasil");
                            cmdDetail.ExecuteNonQuery();
                        }
                    }

                    trans.Commit();
                    MessageBox.Show("✅ Transaksi berhasil disimpan!");
                    MessageBox.Show("Terima kasih 😊\nSilakan datang kembali!", "Kasir", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    // 🔹 Bersihkan semua input dan buat transaksi baru
                    dgv_transaksi.Rows.Clear();
                    txt_bayar.Clear();
                    txt_kembalian.Clear();
                    lbl_total.Text = "Rp. 0";

                    // 🔹 Generate nomor transaksi berikutnya otomatis
                    BuatTransaksiBaru();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("❌ Gagal menyimpan transaksi: " + ex.Message);
                }
            }
        }

        private void dgv_transaksi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lbl_total_Click(object sender, EventArgs e)
        {

        }

        private void btn_hapusItem_Click(object sender, EventArgs e)
        {
            if (dgv_transaksi.SelectedRows.Count > 0)
            {
                // Konfirmasi sebelum hapus
                DialogResult result = MessageBox.Show("Apakah Anda yakin ingin menghapus item ini?",
                                                      "Konfirmasi Hapus",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Hapus baris yang dipilih
                    dgv_transaksi.Rows.RemoveAt(dgv_transaksi.SelectedRows[0].Index);
                    HitungTotal(); // update total setelah hapus
                }
            }
            else
            {
                MessageBox.Show("Pilih item yang ingin dihapus terlebih dahulu!");
            }
        }

        private void btn_batalTransaksi_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Apakah Anda yakin ingin membatalkan transaksi ini?",
                        "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();
                        string query = "INSERT INTO tbl_transaksi (no_transaksi, tanggal, jam, total, bayar, kembalian, status) " +
                                       "VALUES (@no, @tgl, @jam, 0, 0, 0, @status)";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@no", txt_noTransaksi.Text);
                        cmd.Parameters.AddWithValue("@tgl", dtp_tanggalKasir.Value.Date);
                        cmd.Parameters.AddWithValue("@jam", DateTime.Now.ToString("HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@status", "Batal");
                        cmd.ExecuteNonQuery();
                    }

                    // 🔹 Reset tampilan form
                    dgv_transaksi.Rows.Clear();
                    txt_bayar.Clear();
                    txt_kembalian.Clear();
                    lbl_total.Text = "Rp. 0";

                    // 🔹 Buat transaksi baru
                    BuatTransaksiBaru();

                    MessageBox.Show("❌ Transaksi dibatalkan.", "Batal", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal membatalkan transaksi: " + ex.Message);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Buka form dashboard pembelian
            PembelianPage pembelianPage = new PembelianPage(userId);
            pembelianPage.Show();

            // Sembunyikan form kasir agar tidak double window
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Buka form product
            ProductPage productPage = new ProductPage(userId);
            productPage.Show();

            // Sembunyikan form kasir agar tidak double window
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Buka form riwayat dan laporan transaksi
            RiwayatTransaksi riwayatTransaksi = new RiwayatTransaksi(userId);
            riwayatTransaksi.Show();

            // Sembunyikan form kasir agar tidak double window
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

        private void button9_Click(object sender, EventArgs e)
        {
            // Buka form riwayat dan laporan transaksi
            PengaturanPage pengaturanPage = new PengaturanPage(userId);
            pengaturanPage.Show();

            // Sembunyikan form pembelian agar tidak double window
            this.Hide();
        }

        private void KasirPage_Load_1(object sender, EventArgs e)
        {

        }
    }
}
