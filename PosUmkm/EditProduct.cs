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
    public partial class EditProduct : Form
    {
        private int id_produk;
        private MySqlConnection koneksi;
        private string alamat = "server=localhost; database=db_pos; username=root; password=;";
        public EditProduct(int id, string kode, string nama, string beli, string jual, string stok, string satuan, DateTime tanggal)
        {
            InitializeComponent();
            koneksi = new MySqlConnection(alamat);

            // simpan id untuk update nanti
            id_produk = id;

            // tampilkan data ke form edit
            txt_kodeBarangEdit.Text = kode;
            txt_namaBarangEdit.Text = nama;
            txt_hargaBeliEdit.Text = beli;
            txt_hargaJualEdit.Text = jual;
            txt_stokEdit.Text = stok;
            txt_satuanEdit.Text = satuan;
            dtp_tanggalProductEdit.Value = tanggal;
        }

        private void EditProduct_Load(object sender, EventArgs e)
        {

        }

        private void btn_BatalEdit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_simpanEdit_Click(object sender, EventArgs e)
        {
            try
            {
                koneksi.Open();
                string query = @"UPDATE tbl_product 
                                 SET tanggal=@tanggal, kode_barang=@kode, nama_barang=@nama, 
                                     harga_beli=@beli, harga_jual=@jual, stok=@stok, satuan=@satuan
                                 WHERE id_produk=@id";
                MySqlCommand cmd = new MySqlCommand(query, koneksi);
                cmd.Parameters.AddWithValue("@id", id_produk);
                cmd.Parameters.AddWithValue("@tanggal", dtp_tanggalProductEdit.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@kode", txt_kodeBarangEdit.Text.Trim());
                cmd.Parameters.AddWithValue("@nama", txt_namaBarangEdit.Text.Trim());
                cmd.Parameters.AddWithValue("@beli", txt_hargaBeliEdit.Text.Trim());
                cmd.Parameters.AddWithValue("@jual", txt_hargaJualEdit.Text.Trim());
                cmd.Parameters.AddWithValue("@stok", txt_stokEdit.Text.Trim());
                cmd.Parameters.AddWithValue("@satuan", txt_satuanEdit.Text.Trim());
                cmd.ExecuteNonQuery();

                MessageBox.Show("Data produk berhasil diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK; // agar form utama refresh data
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memperbarui data: " + ex.Message);
            }
            finally
            {
                koneksi.Close();
            }
        }
    }
}