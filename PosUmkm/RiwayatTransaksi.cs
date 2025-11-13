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
    public partial class RiwayatTransaksi : Form
    {
        string connStr = "server=localhost;uid=root;pwd=;database=db_pos;";

        public RiwayatTransaksi()
        {
            InitializeComponent();

            // Event saat form load
            this.Load += RiwayatTransaksi_Load;

            // Hubungkan tombol dari designer dengan event handler
            this.btn_cetakSemua.Click += new EventHandler(this.btn_cetakSemua_Click);
            this.btn_cetak.Click += new EventHandler(this.btn_cetak_Click);
        }


        // Export ke PDF berdasarkan isi DataGridView dgv_riwayat
        private void ExportToPDF(string filePath, string namaUmkm, string alamat, string nomorHp)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var doc = new iTextSharp.text.Document(PageSize.A4.Rotate(), 40, 40, 40, 40);
                    PdfWriter.GetInstance(doc, fs);
                    doc.Open();

                    // Judul
                    var judul = new Paragraph("LAPORAN TRANSAKSI", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16));
                    judul.Alignment = Element.ALIGN_CENTER;
                    doc.Add(judul);
                    doc.Add(new Paragraph("\n"));

                    // Info UMKM
                    doc.Add(new Paragraph($"Nama UMKM : {namaUmkm}"));
                    doc.Add(new Paragraph($"Alamat     : {alamat}"));
                    doc.Add(new Paragraph($"Nomor HP   : {nomorHp}"));
                    doc.Add(new Paragraph($"Tanggal Cetak : {DateTime.Now:dd-MM-yyyy HH:mm:ss}"));
                    doc.Add(new Paragraph("\n"));

                    // Buat tabel dari DataGridView dgv_riwayat
                    int colCount = dgv_riwayat.Columns.Count;
                    PdfPTable table = new PdfPTable(colCount);
                    table.WidthPercentage = 100f;

                    // Header
                    iTextSharp.text.Font headerFont = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10);
                    for (int i = 0; i < colCount; i++)
                    {
                        string headerText = dgv_riwayat.Columns[i].HeaderText;
                        PdfPCell cell = new PdfPCell(new Phrase(headerText, headerFont))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            BackgroundColor = new BaseColor(230, 230, 230)
                        };
                        table.AddCell(cell);
                    }

                    // Data isi
                    iTextSharp.text.Font bodyFont = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 9);
                    foreach (DataGridViewRow row in dgv_riwayat.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            for (int i = 0; i < colCount; i++)
                            {
                                var value = row.Cells[i].Value?.ToString() ?? "";
                                PdfPCell dataCell = new PdfPCell(new Phrase(value, bodyFont));
                                table.AddCell(dataCell);
                            }
                        }
                    }

                    doc.Add(table);
                    doc.Add(new Paragraph("\n\n\n"));

                    // tanda tangan
                    Paragraph mengetahui = new Paragraph("Mengetahui,", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12));
                    mengetahui.Alignment = Element.ALIGN_CENTER;
                    doc.Add(mengetahui);

                    Paragraph pemilik = new Paragraph("Pemilik UMKM", FontFactory.GetFont(FontFactory.HELVETICA, 12));
                    pemilik.Alignment = Element.ALIGN_CENTER;
                    doc.Add(pemilik);

                    doc.Add(new Paragraph("\n\n\n(........................................)", FontFactory.GetFont(FontFactory.HELVETICA, 12)) { Alignment = Element.ALIGN_CENTER });

                    doc.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal membuat PDF: " + ex.Message);
            }
        }

        // Export ke Excel berdasarkan isi DataGridView dgv_riwayat
        private void ExportToExcel(string filePath, string namaUmkm, string alamat, string nomorHp)
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add("Laporan Transaksi");

                    // Header UMKM
                    ws.Cell(1, 1).Value = "Nama UMKM:";
                    ws.Cell(1, 2).Value = namaUmkm;
                    ws.Cell(2, 1).Value = "Alamat:";
                    ws.Cell(2, 2).Value = alamat;
                    ws.Cell(3, 1).Value = "Nomor HP:";
                    ws.Cell(3, 2).Value = nomorHp;
                    ws.Cell(4, 1).Value = "Tanggal Cetak:";
                    ws.Cell(4, 2).Value = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

                    // Tulis header tabel mulai baris 6
                    int startRow = 6;
                    for (int c = 0; c < dgv_riwayat.Columns.Count; c++)
                    {
                        ws.Cell(startRow, c + 1).Value = dgv_riwayat.Columns[c].HeaderText;
                        ws.Cell(startRow, c + 1).Style.Font.Bold = true;
                        ws.Cell(startRow, c + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                    }

                    // Isi data
                    int excelRow = startRow + 1;
                    for (int r = 0; r < dgv_riwayat.Rows.Count; r++)
                    {
                        var row = dgv_riwayat.Rows[r];
                        if (row.IsNewRow) continue;
                        for (int c = 0; c < dgv_riwayat.Columns.Count; c++)
                        {
                            ws.Cell(excelRow, c + 1).Value = row.Cells[c].Value?.ToString() ?? "";
                        }
                        excelRow++;
                    }

                    ws.Columns().AdjustToContents();
                    workbook.SaveAs(filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal membuat Excel: " + ex.Message);
            }
        }

        private void TampilkanDataTransaksi()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            t.no_transaksi AS 'No Transaksi',
                            t.tanggal AS 'Tanggal',
                            t.jam AS 'Jam',
                            d.kode_barang AS 'Kode Barang',
                            d.nama_barang AS 'Nama Barang',
                            d.qty AS 'Jumlah',
                            d.harga AS 'Harga',
                            d.total AS 'Total'
                        FROM tbl_transaksi t
                        JOIN tbl_detail_transaksi d ON t.no_transaksi = d.no_transaksi
                        ORDER BY t.tanggal DESC, t.jam DESC";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgv_riwayat.DataSource = dt;

                    // Format kolom angka agar lebih rapi (cek dulu apakah kolom ada)
                    if (dgv_riwayat.Columns.Contains("Harga"))
                        dgv_riwayat.Columns["Harga"].DefaultCellStyle.Format = "N0";
                    if (dgv_riwayat.Columns.Contains("Total"))
                        dgv_riwayat.Columns["Total"].DefaultCellStyle.Format = "N0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Gagal menampilkan data transaksi: " + ex.Message);
            }
        }

        // Tampil data riwayat berdasarkan tanggal (filter untuk dgv_riwayat)
        private void TampilDataBerdasarkanTanggal(DateTime tanggalAwal, DateTime tanggalAkhir)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            t.no_transaksi AS 'No Transaksi',
                            t.tanggal AS 'Tanggal',
                            t.jam AS 'Jam',
                            d.kode_barang AS 'Kode Barang',
                            d.nama_barang AS 'Nama Barang',
                            d.qty AS 'Jumlah',
                            d.harga AS 'Harga',
                            d.total AS 'Total'
                        FROM tbl_transaksi t
                        JOIN tbl_detail_transaksi d ON t.no_transaksi = d.no_transaksi
                        WHERE DATE(t.tanggal) BETWEEN @awal AND @akhir
                        ORDER BY t.tanggal ASC, t.jam ASC";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@awal", tanggalAwal.ToString("yyyy-MM-dd"));
                    adapter.SelectCommand.Parameters.AddWithValue("@akhir", tanggalAkhir.ToString("yyyy-MM-dd"));

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgv_riwayat.DataSource = dt;

                    if (dgv_riwayat.Columns.Contains("Harga"))
                        dgv_riwayat.Columns["Harga"].DefaultCellStyle.Format = "N0";
                    if (dgv_riwayat.Columns.Contains("Total"))
                        dgv_riwayat.Columns["Total"].DefaultCellStyle.Format = "N0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan data berdasarkan tanggal: " + ex.Message);
            }
        }

        private void HitungSemuaTotal()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // Harian
                    string qHarian = @"
                        SELECT 
                            COALESCE(SUM(d.total), 0) AS TotalHarian,
                            COALESCE(SUM(d.qty), 0) AS JumlahHarian
                        FROM tbl_detail_transaksi d
                        WHERE d.no_transaksi IN (
                            SELECT no_transaksi 
                            FROM tbl_transaksi 
                            WHERE DATE(tanggal) = CURDATE() AND status = 'Berhasil'
                        )";

                    using (MySqlCommand cmd = new MySqlCommand(qHarian, conn))
                    using (MySqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            txt_totalHarian.Text = Convert.ToDecimal(r["TotalHarian"]).ToString("N0");
                            txt_jumlahHarian.Text = r["JumlahHarian"].ToString();
                        }
                    }

                    // Mingguan
                    string qMingguan = @"
                        SELECT 
                            COALESCE(SUM(d.total), 0) AS TotalMingguan,
                            COALESCE(SUM(d.qty), 0) AS JumlahMingguan
                        FROM tbl_detail_transaksi d
                        WHERE d.no_transaksi IN (
                            SELECT no_transaksi 
                            FROM tbl_transaksi 
                            WHERE YEARWEEK(tanggal, 1) = YEARWEEK(CURDATE(), 1)
                            AND status = 'Berhasil'
                        )";

                    using (MySqlCommand cmd2 = new MySqlCommand(qMingguan, conn))
                    using (MySqlDataReader r2 = cmd2.ExecuteReader())
                    {
                        if (r2.Read())
                        {
                            txt_totalMingguan.Text = Convert.ToDecimal(r2["TotalMingguan"]).ToString("N0");
                            txt_jumlahMingguan.Text = r2["JumlahMingguan"].ToString();
                        }
                    }

                    // Keseluruhan
                    string qKeseluruhan = @"
                        SELECT 
                            COALESCE(SUM(d.total), 0) AS TotalKeseluruhan,
                            COALESCE(SUM(d.qty), 0) AS JumlahKeseluruhan
                        FROM tbl_detail_transaksi d
                        WHERE d.no_transaksi IN (
                            SELECT no_transaksi 
                            FROM tbl_transaksi 
                            WHERE status = 'Berhasil'
                        )";

                    using (MySqlCommand cmd3 = new MySqlCommand(qKeseluruhan, conn))
                    using (MySqlDataReader r3 = cmd3.ExecuteReader())
                    {
                        if (r3.Read())
                        {
                            txt_totalKeseluruhan.Text = Convert.ToDecimal(r3["TotalKeseluruhan"]).ToString("N0");
                            txt_JumlahKeseluruhan.Text = r3["JumlahKeseluruhan"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Gagal menghitung total penjualan: " + ex.Message);
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
        }

        private void RiwayatTransaksi_Load(object sender, EventArgs e)
        {
            TampilkanDataTransaksi();
            HitungSemuaTotal();
        }

        private void dgv_riwayat_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            TampilkanDataTransaksi();
            HitungSemuaTotal();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                TampilkanDataTransaksi(); // memanggil ulang data dari database
                HitungSemuaTotal();
                MessageBox.Show("Data berhasil diperbarui!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat ulang data: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Buka form product
            ProductPage productPage = new ProductPage();
            productPage.Show();

            // Sembunyikan form riwayat agar tidak double window
            this.Hide();
        }

        private void dtp_tglAkhir_ValueChanged(object sender, EventArgs e)
        {
        }

        // Tombol Cetak berdasarkan tanggal (menggunakan datepickers dtp_riwayat & dtp_tglAkhir)
        private void btn_cetak_Click(object sender, EventArgs e)
        {
            DateTime tanggalAwal = dtp_riwayat.Value.Date;
            DateTime tanggalAkhir = dtp_tglAkhir.Value.Date;

            if (tanggalAwal > tanggalAkhir)
            {
                MessageBox.Show("Tanggal awal tidak boleh lebih besar dari tanggal akhir.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string namaUmkm = "UMKM Contoh";
            string alamat = "Jl. Samratulangi No. 123, Manado";
            string nomorHp = "081234567890";

            try
            {
                // Filter di grid terlebih dahulu
                TampilDataBerdasarkanTanggal(tanggalAwal, tanggalAkhir);

                // Kemudian ekspor hasil yang tampil di dgv_riwayat
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF Files (*.pdf)|*.pdf|Excel Files (*.xlsx)|*.xlsx";
                sfd.FileName = $"Laporan_Transaksi_{tanggalAwal:yyyyMMdd}_{tanggalAkhir:yyyyMMdd}";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string filePath = sfd.FileName;

                    if (Path.GetExtension(filePath).ToLower() == ".pdf")
                    {
                        ExportToPDF(filePath, namaUmkm, alamat, nomorHp);
                    }
                    else
                    {
                        ExportToExcel(filePath, namaUmkm, alamat, nomorHp);
                    }

                    MessageBox.Show("Laporan berdasarkan tanggal berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            finally
            {
                // Setelah ekspor, tampilkan kembali semua data
                TampilkanDataTransaksi();
            }
        }

        // Tombol Cetak Semua (ekspor seluruh data)
        private void btn_cetakSemua_Click(object sender, EventArgs e)
        {
            string namaUmkm = "UMKM Contoh";
            string alamat = "Jl. Samratulangi No. 123, Manado";
            string nomorHp = "081234567890";

            // Pastikan grid menampilkan seluruh data
            TampilkanDataTransaksi();

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PDF Files (*.pdf)|*.pdf|Excel Files (*.xlsx)|*.xlsx";
            sfd.FileName = "Laporan_Transaksi_Semua_" + DateTime.Now.ToString("yyyyMMdd");

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string filePath = sfd.FileName;

                if (Path.GetExtension(filePath).ToLower() == ".pdf")
                {
                    ExportToPDF(filePath, namaUmkm, alamat, nomorHp);
                }
                else
                {
                    ExportToExcel(filePath, namaUmkm, alamat, nomorHp);
                }

                MessageBox.Show("Laporan semua data berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Buka form dashboard
            Form1 form1Page = new Form1();
            form1Page.Show();

            // Sembunyikan form riwayat agar tidak double window
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Buka form pembelian
            PembelianPage pembelianPage = new PembelianPage();
            pembelianPage.Show();

            // Sembunyikan form riwayat agar tidak double window
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Buka form kasir
            KasirPage kasirPage = new KasirPage();
            kasirPage.Show();

            // Sembunyikan form riwayat agar tidak double window
            this.Hide();
        }
    }
}
