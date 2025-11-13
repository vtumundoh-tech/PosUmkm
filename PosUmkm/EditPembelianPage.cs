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
    public partial class EditPembelianPage : Form
    {
        private int id_pembelian;
        private MySqlConnection koneksi;
        public EditPembelianPage(int id, string kode, string supplier, string total, DateTime tanggal)
        {
            InitializeComponent();
            id_pembelian = id;
            txt_kodeTransaksiEdit.Text = kode;
            txt_supplierEdit.Text = supplier;
            txt_totalEdit.Text = total;
            dtp_tanggalBeliEdit.Value = tanggal;

            koneksi = new MySqlConnection("server=localhost; database=db_pos; username=root; password=;");
        }

        private void EditPembelianPage_Load(object sender, EventArgs e)
        {

        }

        private void btn_simpan_Click(object sender, EventArgs e)
        {
            try
            {
                koneksi.Open();
                string query = "UPDATE tbl_pembelian SET tanggal=@tanggal, kode_transaksi=@kode, supplier=@supplier, total=@total WHERE id_pembelian=@id";

                MySqlCommand cmd = new MySqlCommand(query, koneksi);
                cmd.Parameters.AddWithValue("@tanggal", dtp_tanggalBeliEdit.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@kode", txt_kodeTransaksiEdit.Text.Trim());
                cmd.Parameters.AddWithValue("@supplier", txt_supplierEdit.Text.Trim());
                cmd.Parameters.AddWithValue("@total", txt_totalEdit.Text.Trim());
                cmd.Parameters.AddWithValue("@id", id_pembelian);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Data berhasil diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
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

        private void btn_batal_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btn_batal_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btn_simpan_Click_1(object sender, EventArgs e)
        {
            try
            {
                koneksi.Open();
                string query = "UPDATE tbl_pembelian SET tanggal=@tanggal, kode_transaksi=@kode, supplier=@supplier, total=@total WHERE id_pembelian=@id";

                MySqlCommand cmd = new MySqlCommand(query, koneksi);
                cmd.Parameters.AddWithValue("@tanggal", dtp_tanggalBeliEdit.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@kode", txt_kodeTransaksiEdit.Text.Trim());
                cmd.Parameters.AddWithValue("@supplier", txt_supplierEdit.Text.Trim());
                cmd.Parameters.AddWithValue("@total", txt_totalEdit.Text.Trim());
                cmd.Parameters.AddWithValue("@id", id_pembelian);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Data berhasil diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
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
