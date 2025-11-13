namespace PosUmkm
{
    partial class EditPembelianPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtp_tanggalBeliEdit = new System.Windows.Forms.DateTimePicker();
            this.btn_batal = new System.Windows.Forms.Button();
            this.txt_totalEdit = new System.Windows.Forms.TextBox();
            this.txt_supplierEdit = new System.Windows.Forms.TextBox();
            this.txt_kodeTransaksiEdit = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_simpan = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Gray;
            this.groupBox1.Controls.Add(this.dtp_tanggalBeliEdit);
            this.groupBox1.Controls.Add(this.btn_batal);
            this.groupBox1.Controls.Add(this.txt_totalEdit);
            this.groupBox1.Controls.Add(this.txt_supplierEdit);
            this.groupBox1.Controls.Add(this.txt_kodeTransaksiEdit);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btn_simpan);
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 293);
            this.groupBox1.TabIndex = 58;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "EDIT DATA";
            // 
            // dtp_tanggalBeliEdit
            // 
            this.dtp_tanggalBeliEdit.Location = new System.Drawing.Point(144, 24);
            this.dtp_tanggalBeliEdit.Name = "dtp_tanggalBeliEdit";
            this.dtp_tanggalBeliEdit.Size = new System.Drawing.Size(256, 22);
            this.dtp_tanggalBeliEdit.TabIndex = 66;
            // 
            // btn_batal
            // 
            this.btn_batal.Font = new System.Drawing.Font("Segoe UI", 8.3F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_batal.Location = new System.Drawing.Point(179, 138);
            this.btn_batal.Name = "btn_batal";
            this.btn_batal.Size = new System.Drawing.Size(134, 30);
            this.btn_batal.TabIndex = 65;
            this.btn_batal.Text = "Batal";
            this.btn_batal.UseVisualStyleBackColor = true;
            this.btn_batal.Click += new System.EventHandler(this.btn_batal_Click_1);
            // 
            // txt_totalEdit
            // 
            this.txt_totalEdit.Location = new System.Drawing.Point(144, 100);
            this.txt_totalEdit.Name = "txt_totalEdit";
            this.txt_totalEdit.Size = new System.Drawing.Size(256, 22);
            this.txt_totalEdit.TabIndex = 64;
            // 
            // txt_supplierEdit
            // 
            this.txt_supplierEdit.Location = new System.Drawing.Point(144, 74);
            this.txt_supplierEdit.Name = "txt_supplierEdit";
            this.txt_supplierEdit.Size = new System.Drawing.Size(621, 22);
            this.txt_supplierEdit.TabIndex = 63;
            // 
            // txt_kodeTransaksiEdit
            // 
            this.txt_kodeTransaksiEdit.Location = new System.Drawing.Point(144, 48);
            this.txt_kodeTransaksiEdit.Name = "txt_kodeTransaksiEdit";
            this.txt_kodeTransaksiEdit.Size = new System.Drawing.Size(256, 22);
            this.txt_kodeTransaksiEdit.TabIndex = 62;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 8.3F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 19);
            this.label6.TabIndex = 61;
            this.label6.Text = "TOTAL";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8.3F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 19);
            this.label5.TabIndex = 60;
            this.label5.Text = "SUPPLIER";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8.3F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 19);
            this.label4.TabIndex = 59;
            this.label4.Text = "KODE TRANSAKSI";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.3F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 19);
            this.label3.TabIndex = 58;
            this.label3.Text = "TANGGAL";
            // 
            // btn_simpan
            // 
            this.btn_simpan.Font = new System.Drawing.Font("Segoe UI", 8.3F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_simpan.Location = new System.Drawing.Point(39, 138);
            this.btn_simpan.Name = "btn_simpan";
            this.btn_simpan.Size = new System.Drawing.Size(134, 30);
            this.btn_simpan.TabIndex = 57;
            this.btn_simpan.Text = "Simpan";
            this.btn_simpan.UseVisualStyleBackColor = true;
            this.btn_simpan.Click += new System.EventHandler(this.btn_simpan_Click_1);
            // 
            // EditPembelianPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox1);
            this.Name = "EditPembelianPage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EditPembelianPage";
            this.Load += new System.EventHandler(this.EditPembelianPage_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtp_tanggalBeliEdit;
        private System.Windows.Forms.Button btn_batal;
        private System.Windows.Forms.TextBox txt_totalEdit;
        private System.Windows.Forms.TextBox txt_supplierEdit;
        private System.Windows.Forms.TextBox txt_kodeTransaksiEdit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_simpan;
    }
}