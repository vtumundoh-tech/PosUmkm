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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using ClosedXML.Excel;

namespace PosUmkm
{
    public partial class ProductPage : Form
    {
        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;

        private DataSet ds = new DataSet();
        private DataTable dt;
        private string alamat, query;
        public ProductPage()
        {
            alamat = "server=localhost; database=db_pos; username=root; password=;";
            koneksi = new MySqlConnection(alamat);

            InitializeComponent();
        }

        private void BersihkanInput()
        {
            dtp_tanggalProduct.Value = DateTime.Now; // reset ke tanggal hari ini
            txt_kodeBarang.Clear();
            txt_namaBarang.Clear();
            txt_hargaBeli.Clear();
            txt_hargaJual.Clear();
            txt_stok.Clear();
            txt_satuan.Clear();
        }

        private void TampilData()
        {
            try
            {
                koneksi.Open();
                string query = "SELECT * FROM tbl_product ORDER BY id_produk DESC";
                adapter = new MySqlDataAdapter(query, koneksi);
                dt = new DataTable();
                adapter.Fill(dt);

                // Tambah kolom baru untuk keuntungan & kerugian
                if (!dt.Columns.Contains("keuntungan"))
                    dt.Columns.Add("keuntungan", typeof(decimal));

                if (!dt.Columns.Contains("total_keuntungan"))
                    dt.Columns.Add("total_keuntungan", typeof(decimal));

                if (!dt.Columns.Contains("kerugian"))
                    dt.Columns.Add("kerugian", typeof(decimal));

                if (!dt.Columns.Contains("total_kerugian"))
                    dt.Columns.Add("total_kerugian", typeof(decimal));

                // Hitung keuntungan & kerugian per item dan totalnya
                foreach (DataRow row in dt.Rows)
                {
                    if (decimal.TryParse(row["harga_beli"].ToString(), out decimal beli) &&
                        decimal.TryParse(row["harga_jual"].ToString(), out decimal jual) &&
                        int.TryParse(row["stok"].ToString(), out int stok))
                    {
                        decimal keuntungan = 0, kerugian = 0;

                        if (jual > beli)
                        {
                            keuntungan = jual - beli;
                        }
                        else if (jual < beli)
                        {
                            kerugian = beli - jual;
                        }

                        row["keuntungan"] = keuntungan;
                        row["total_keuntungan"] = keuntungan * stok;
                        row["kerugian"] = kerugian;
                        row["total_kerugian"] = kerugian * stok;
                    }
                    else
                    {
                        row["keuntungan"] = 0;
                        row["total_keuntungan"] = 0;
                        row["kerugian"] = 0;
                        row["total_kerugian"] = 0;
                    }
                }

                dgv_product.DataSource = dt;

                // Atur header agar rapi
                dgv_product.Columns["keuntungan"].HeaderText = "Keuntungan per Item";
                dgv_product.Columns["total_keuntungan"].HeaderText = "Total Keuntungan";
                dgv_product.Columns["kerugian"].HeaderText = "Kerugian per Item";
                dgv_product.Columns["total_kerugian"].HeaderText = "Total Kerugian";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan data: " + ex.Message);
            }
            finally
            {
                koneksi.Close();
            }
        }

        private void ExportToPDF(string filePath, string namaUmkm, string alamat, string nomorHp)
        {
            try
            {
                // --- import ---
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var doc = new iTextSharp.text.Document(PageSize.A4, 40, 40, 40, 40);
                    PdfWriter.GetInstance(doc, fs);
                    doc.Open();

                    // Judul
                    var judul = new Paragraph("LAPORAN PRODUK", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16, iTextSharp.text.Font.BOLD));
                    judul.Alignment = Element.ALIGN_CENTER;
                    doc.Add(judul);
                    doc.Add(new Paragraph("\n"));

                    // Info UMKM
                    doc.Add(new Paragraph($"Nama UMKM : {namaUmkm}"));
                    doc.Add(new Paragraph($"Alamat     : {alamat}"));
                    doc.Add(new Paragraph($"Nomor HP   : {nomorHp}"));
                    doc.Add(new Paragraph("\n"));

                    // Buat tabel dari DataGridView
                    PdfPTable table = new PdfPTable(dgv_product.Columns.Count);
                    table.WidthPercentage = 100;

                    // Header
                    foreach (DataGridViewColumn column in dgv_product.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            BackgroundColor = new iTextSharp.text.BaseColor(230, 230, 230)
                        };
                        table.AddCell(cell);
                    }

                    // Data isi
                    foreach (DataGridViewRow row in dgv_product.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                table.AddCell(cell.Value?.ToString() ?? "");
                            }
                        }
                    }

                    doc.Add(table);
                    doc.Add(new Paragraph("\n\n\n"));

                    // --- Tambah bagian tanda tangan ---
                    Paragraph mengetahui = new Paragraph("Mengetahui,", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD));
                    mengetahui.Alignment = Element.ALIGN_CENTER;
                    doc.Add(mengetahui);

                    Paragraph pemilik = new Paragraph("Pemilik UMKM", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12));
                    pemilik.Alignment = Element.ALIGN_CENTER;
                    doc.Add(pemilik);

                    doc.Add(new Paragraph("\n\n\n(........................................)", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12)) { Alignment = Element.ALIGN_CENTER });

                    doc.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal membuat PDF: " + ex.Message);
            }
        }

        private void ExportToExcel(string filePath, string namaUmkm, string alamat, string nomorHp)
        {
            try
            {
                using (var workbook = new ClosedXML.Excel.XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add("Laporan Pembelian");

                    // Info UMKM
                    ws.Cell(1, 1).Value = "Nama UMKM:";
                    ws.Cell(1, 2).Value = namaUmkm;
                    ws.Cell(2, 1).Value = "Alamat:";
                    ws.Cell(2, 2).Value = alamat;
                    ws.Cell(3, 1).Value = "Nomor HP:";
                    ws.Cell(3, 2).Value = nomorHp;

                    ws.Cell(5, 1).Value = "Laporan Pembelian";
                    ws.Cell(5, 1).Style.Font.Bold = true;
                    ws.Cell(5, 1).Style.Font.FontSize = 14;
                    ws.Range(5, 1, 5, dgv_product.Columns.Count).Merge().Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;

                    // Header tabel
                    for (int i = 0; i < dgv_product.Columns.Count; i++)
                    {
                        ws.Cell(7, i + 1).Value = dgv_product.Columns[i].HeaderText;
                        ws.Cell(7, i + 1).Style.Font.Bold = true;
                        ws.Cell(7, i + 1).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightGray;
                    }

                    // Data isi
                    for (int i = 0; i < dgv_product.Rows.Count; i++)
                    {
                        for (int j = 0; j < dgv_product.Columns.Count; j++)
                        {
                            ws.Cell(i + 8, j + 1).Value = dgv_product.Rows[i].Cells[j].Value?.ToString() ?? "";
                        }
                    }

                    // --- Auto-fit dan wrap text ---
                    ws.Columns().AdjustToContents();
                    ws.Style.Alignment.WrapText = true;

                    workbook.SaveAs(filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal membuat Excel: " + ex.Message);
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Buka form dashboard
            Form1 form1Page = new Form1();
            form1Page.Show();

            // Sembunyikan form product agar tidak double window
            this.Hide();
        }

        private void txt_cariBarangProduct_TextChanged(object sender, EventArgs e)
        {
            try
            {
                koneksi.Open();
                string query = "SELECT * FROM tbl_product WHERE kode_barang LIKE @cari OR nama_barang LIKE @cari";
                adapter = new MySqlDataAdapter(query, koneksi);
                adapter.SelectCommand.Parameters.AddWithValue("@cari", "%" + txt_cariBarangProduct.Text + "%");
                dt = new DataTable();
                adapter.Fill(dt);
                dgv_product.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal mencari data: " + ex.Message);
            }
            finally
            {
                koneksi.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dgv_product.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang ingin diedit!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgv_product.SelectedRows[0];
            int id = Convert.ToInt32(row.Cells["id_produk"].Value);
            string kode = row.Cells["kode_barang"].Value.ToString();
            string nama = row.Cells["nama_barang"].Value.ToString();
            string beli = row.Cells["harga_beli"].Value.ToString();
            string jual = row.Cells["harga_jual"].Value.ToString();
            string stok = row.Cells["stok"].Value.ToString();
            string satuan = row.Cells["satuan"].Value.ToString();
            DateTime tanggal = Convert.ToDateTime(row.Cells["tanggal"].Value);

            EditProduct editForm = new EditProduct(id, kode, nama, beli, jual, stok, satuan, tanggal);
            if (editForm.ShowDialog() == DialogResult.OK)
                TampilData();
        }

        private void btn_hapus_Click(object sender, EventArgs e)
        {
            if (dgv_product.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang ingin dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgv_product.SelectedRows[0].Cells["id_produk"].Value);
            string nama = dgv_product.SelectedRows[0].Cells["nama_barang"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Apakah Anda yakin ingin menghapus produk '{nama}'?",
                "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    koneksi.Open();
                    string query = "DELETE FROM tbl_product WHERE id_produk=@id";
                    MySqlCommand cmd = new MySqlCommand(query, koneksi);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TampilData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menghapus data: " + ex.Message);
                }
                finally
                {
                    koneksi.Close();
                }
            }
        }
private void label9_Click(object sender, EventArgs e)
        {

        }

        private void dtp_tanggalProduct_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txt_satuan_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_stok_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_hargaJual_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_hargaBeli_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_namaBarang_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_kodeBarang_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            {
                TampilData();
                MessageBox.Show("Data berhasil diperbarui!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            string namaUmkm = "UMKM Contoh";
            string alamat = "Jl. Samratulangi No. 123, Manado";
            string nomorHp = "081234567890";

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PDF Files (*.pdf)|*.pdf|Excel Files (*.xlsx)|*.xlsx";
            sfd.FileName = "Laporan_Produk_" + DateTime.Now.ToString("yyyyMMdd");

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string filePath = sfd.FileName;

                if (Path.GetExtension(filePath).ToLower() == ".pdf")
                    ExportToPDF(filePath, namaUmkm, alamat, nomorHp);
                else
                    ExportToExcel(filePath, namaUmkm, alamat, nomorHp);

                MessageBox.Show("Laporan berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            // Pastikan user sudah memilih data
            if (dgv_product.SelectedRows.Count == 0)
            {
                MessageBox.Show("Silakan pilih data yang ingin dihapus terlebih dahulu!",
                                "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ambil id_produk dari baris yang dipilih
            int id_produk = Convert.ToInt32(dgv_product.SelectedRows[0].Cells["id_produk"].Value);
            string nama_produk = dgv_product.SelectedRows[0].Cells["nama_barang"].Value.ToString();

            // Konfirmasi sebelum hapus
            DialogResult konfirmasi = MessageBox.Show(
                $"Apakah Anda yakin ingin menghapus data '{nama_produk} ini'?",
                "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (konfirmasi == DialogResult.Yes)
            {
                string connString = "server=localhost; database=db_pos; username=root; password=;";
                using (MySqlConnection koneksi = new MySqlConnection(connString))
                {
                    try
                    {
                        koneksi.Open();
                        string query = "DELETE FROM tbl_product WHERE id_produk = @id";
                        using (MySqlCommand cmd = new MySqlCommand(query, koneksi))
                        {
                            cmd.Parameters.AddWithValue("@id", id_produk);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Data produk berhasil dihapus!",
                                        "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Refresh data grid setelah hapus
                        TampilData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal menghapus data: " + ex.Message,
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ProductPage_Load(object sender, EventArgs e)
        {
            TampilData();
        }

        private void dgv_product_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Buka form pembelian
            PembelianPage pembelianPage = new PembelianPage();
            pembelianPage.Show();

            // Sembunyikan form product agar tidak double window
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Buka form kasir
            KasirPage kasirPage = new KasirPage();
            kasirPage.Show();

            // Sembunyikan form product agar tidak double window
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Buka form riwayat dan laporan
            RiwayatTransaksi riwayatTransaksi = new RiwayatTransaksi();
            riwayatTransaksi.Show();

            // Sembunyikan form product agar tidak double window
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_kodeBarang.Text) ||
                string.IsNullOrWhiteSpace(txt_namaBarang.Text) ||
                string.IsNullOrWhiteSpace(txt_hargaBeli.Text) ||
                string.IsNullOrWhiteSpace(txt_hargaJual.Text) ||
                string.IsNullOrWhiteSpace(txt_stok.Text) ||
                string.IsNullOrWhiteSpace(txt_satuan.Text))
            {
                MessageBox.Show("Semua field wajib diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                koneksi.Open();
                string query = @"INSERT INTO tbl_product (tanggal, kode_barang, nama_barang, harga_beli, harga_jual, stok, satuan)
                                 VALUES (@tanggal, @kode, @nama, @beli, @jual, @stok, @satuan)";
                perintah = new MySqlCommand(query, koneksi);
                perintah.Parameters.AddWithValue("@tanggal", dtp_tanggalProduct.Value.ToString("yyyy-MM-dd"));
                perintah.Parameters.AddWithValue("@kode", txt_kodeBarang.Text.Trim());
                perintah.Parameters.AddWithValue("@nama", txt_namaBarang.Text.Trim());
                perintah.Parameters.AddWithValue("@beli", txt_hargaBeli.Text.Trim());
                perintah.Parameters.AddWithValue("@jual", txt_hargaJual.Text.Trim());
                perintah.Parameters.AddWithValue("@stok", txt_stok.Text.Trim());
                perintah.Parameters.AddWithValue("@satuan", txt_satuan.Text.Trim());
                perintah.ExecuteNonQuery();

                MessageBox.Show("Data produk berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BersihkanInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menambah data: " + ex.Message);
            }
            finally
            {
                koneksi.Close();
                TampilData();
            }
        }
    }
    
}
