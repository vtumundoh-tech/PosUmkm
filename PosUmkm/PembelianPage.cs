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
    public partial class PembelianPage : Form
    {
        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;

        private DataSet ds = new DataSet();
        private DataTable dt;
        private string alamat, query;
        public PembelianPage()
        {
            alamat = "server=localhost; database=db_pos; username=root; password=;";
            koneksi = new MySqlConnection(alamat);

            InitializeComponent();
        }

        private void BersihkanInput()
        {
            txt_kodeTransaksi.Clear();
            txt_supplier.Clear();
            txt_total.Clear();
            dtp_tanggalBeli.Value = DateTime.Now; // reset ke tanggal hari ini
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
                    var judul = new Paragraph("LAPORAN PEMBELIAN", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16, iTextSharp.text.Font.BOLD));
                    judul.Alignment = Element.ALIGN_CENTER;
                    doc.Add(judul);
                    doc.Add(new Paragraph("\n"));

                    // Info UMKM
                    doc.Add(new Paragraph($"Nama UMKM : {namaUmkm}"));
                    doc.Add(new Paragraph($"Alamat     : {alamat}"));
                    doc.Add(new Paragraph($"Nomor HP   : {nomorHp}"));
                    doc.Add(new Paragraph("\n"));

                    // Buat tabel dari DataGridView
                    PdfPTable table = new PdfPTable(dgv_pembelian.Columns.Count);
                    table.WidthPercentage = 100;

                    // Header
                    foreach (DataGridViewColumn column in dgv_pembelian.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            BackgroundColor = new iTextSharp.text.BaseColor(230, 230, 230)
                        };
                        table.AddCell(cell);
                    }

                    // Data isi
                    foreach (DataGridViewRow row in dgv_pembelian.Rows)
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
                    ws.Range(5, 1, 5, dgv_pembelian.Columns.Count).Merge().Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;

                    // Header tabel
                    for (int i = 0; i < dgv_pembelian.Columns.Count; i++)
                    {
                        ws.Cell(7, i + 1).Value = dgv_pembelian.Columns[i].HeaderText;
                        ws.Cell(7, i + 1).Style.Font.Bold = true;
                        ws.Cell(7, i + 1).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightGray;
                    }

                    // Data isi
                    for (int i = 0; i < dgv_pembelian.Rows.Count; i++)
                    {
                        for (int j = 0; j < dgv_pembelian.Columns.Count; j++)
                        {
                            ws.Cell(i + 8, j + 1).Value = dgv_pembelian.Rows[i].Cells[j].Value?.ToString() ?? "";
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



        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Buka form dashboard
            Form1 form1Page = new Form1();
            form1Page.Show();

            // Sembunyikan form pembelian agar tidak double window
            this.Hide();
        }

        private void dgv_pembelian_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                koneksi.Open();
                string query = "SELECT * FROM tbl_pembelian ORDER BY id_pembelian DESC";
                adapter = new MySqlDataAdapter(query, koneksi);
                dt = new DataTable();
                adapter.Fill(dt);
                dgv_pembelian.DataSource = dt;
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
        private void TampilData()
        {
            try
            {
                koneksi.Open();
                string query = "SELECT * FROM tbl_pembelian ORDER BY id_pembelian DESC";
                adapter = new MySqlDataAdapter(query, koneksi);
                dt = new DataTable();
                adapter.Fill(dt);
                dgv_pembelian.DataSource = dt;
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_kodeTransaksi.Text) ||
        string.IsNullOrWhiteSpace(txt_supplier.Text) ||
        string.IsNullOrWhiteSpace(txt_total.Text))
            {
                MessageBox.Show("Semua field wajib diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                koneksi.Open();

                string query = "INSERT INTO tbl_pembelian (tanggal, kode_transaksi, supplier, total) " +
                               "VALUES (@tanggal, @kode, @supplier, @total)";

                perintah = new MySqlCommand(query, koneksi);
                perintah.Parameters.AddWithValue("@tanggal", dtp_tanggalBeli.Value.ToString("yyyy-MM-dd"));
                perintah.Parameters.AddWithValue("@kode", txt_kodeTransaksi.Text.Trim());
                perintah.Parameters.AddWithValue("@supplier", txt_supplier.Text.Trim());
                perintah.Parameters.AddWithValue("@total", txt_total.Text.Trim());

                int hasil = perintah.ExecuteNonQuery();

                if (hasil > 0)
                {
                    MessageBox.Show("Data pembelian berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Gagal menambahkan data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
            finally
            {
                koneksi.Close();
                TampilData();     // 🔁 sekarang dipanggil setelah koneksi ditutup
                BersihkanInput();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                TampilData(); // memanggil ulang data dari database
                MessageBox.Show("Data berhasil diperbarui!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat ulang data: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dgv_pembelian.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang ingin diedit!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgv_pembelian.SelectedRows[0];
            int id = Convert.ToInt32(row.Cells["id_pembelian"].Value);
            string kode = row.Cells["kode_transaksi"].Value.ToString();
            string supplier = row.Cells["supplier"].Value.ToString();
            string total = row.Cells["total"].Value.ToString();
            DateTime tanggal = Convert.ToDateTime(row.Cells["tanggal"].Value);

            EditPembelianPage editForm = new EditPembelianPage (id, kode, supplier, total, tanggal);

            if (editForm.ShowDialog() == DialogResult.OK)
            {
                TampilData(); // refresh data grid
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (dgv_pembelian.SelectedRows.Count > 0)
            {
                // Ambil nilai ID dari baris yang dipilih
                int id = Convert.ToInt32(dgv_pembelian.SelectedRows[0].Cells["id_pembelian"].Value);
                string kode = dgv_pembelian.SelectedRows[0].Cells["kode_transaksi"].Value.ToString();

                DialogResult result = MessageBox.Show(
                    $"Apakah Anda yakin ingin menghapus transaksi dengan kode transaksi {kode}?",
                    "Konfirmasi Hapus",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        koneksi.Open();
                        string query = "DELETE FROM tbl_pembelian WHERE id_pembelian = @id";
                        MySqlCommand cmd = new MySqlCommand(query, koneksi);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal menghapus data: " + ex.Message);
                    }
                    finally
                    {
                        koneksi.Close();
                        TampilData(); // refresh datagridview setelah hapus
                    }
                }
            }
            else
            {
                MessageBox.Show("Silakan pilih data yang ingin dihapus terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txt_pencarianBeli_TextChanged(object sender, EventArgs e)
        {
            try
            {
                koneksi.Open();
                string query = "SELECT * FROM tbl_pembelian WHERE supplier LIKE @cari OR kode_transaksi LIKE @cari";
                adapter = new MySqlDataAdapter(query, koneksi);
                adapter.SelectCommand.Parameters.AddWithValue("@cari", "%" + txt_pencarianBeli.Text + "%");
                dt = new DataTable();
                adapter.Fill(dt);
                dgv_pembelian.DataSource = dt;
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

        private void button13_Click(object sender, EventArgs e)
        {
            // --- Placeholder data profil UMKM ---
            string namaUmkm = "UMKM Contoh"; // TODO: ambil dari halaman profil
            string alamat = "Jl. Samratulangi No. 123, Manado"; // TODO: ambil dari halaman profil
            string nomorHp = "081234567890"; // TODO: ambil dari halaman profil

            // Pilihan simpan file
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PDF Files (*.pdf)|*.pdf|Excel Files (*.xlsx)|*.xlsx";
            sfd.FileName = "Laporan_Pembelian_" + DateTime.Now.ToString("yyyyMMdd");

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

                MessageBox.Show("Laporan berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void PembelianPage_Load(object sender, EventArgs e)
        {
            TampilData();
        }
    }
}
