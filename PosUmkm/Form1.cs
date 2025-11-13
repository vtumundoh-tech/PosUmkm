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
    public partial class Form1 : Form
    {
        private int userId;
        public Form1(int id_user)
        {
            InitializeComponent();

            userId = id_user;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click_1(object sender, EventArgs e)
        {
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
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Buka form pembelian
            PembelianPage pembelianPage = new PembelianPage(userId);
            pembelianPage.Show();

            // Sembunyikan form dashboard agar tidak double window
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Buka form dashboard
            ProductPage productPage = new ProductPage(userId);
            productPage.Show();

            // Sembunyikan form pembelian agar tidak double window
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Buka form dashboard
            KasirPage kasirPage = new KasirPage(userId);
            kasirPage.Show();

            // Sembunyikan form product agar tidak double window
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Buka form riwayat transaksi
            RiwayatTransaksi riwayatTransaksi = new RiwayatTransaksi(userId);
            riwayatTransaksi.Show();

            // Sembunyikan form dashboard agar tidak double window
            this.Hide();
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            PengaturanPage pengaturanPage = new PengaturanPage(userId);
            pengaturanPage.Show();

            this.Hide();
        }
    }
}
