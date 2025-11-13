namespace PosUmkm
{
    partial class RegisterPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterPage));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_username = new System.Windows.Forms.TextBox();
            this.btn_daftar = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_password = new System.Windows.Forms.TextBox();
            this.btn_kembali = new System.Windows.Forms.Button();
            this.lbl_passwordStrength = new System.Windows.Forms.Label();
            this.pgsBar_Strength = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(431, 361);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(514, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 23);
            this.label1.TabIndex = 19;
            this.label1.Text = "Regist your Account";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(486, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 19);
            this.label3.TabIndex = 22;
            this.label3.Text = "Username";
            // 
            // txt_username
            // 
            this.txt_username.Location = new System.Drawing.Point(490, 88);
            this.txt_username.Name = "txt_username";
            this.txt_username.Size = new System.Drawing.Size(259, 22);
            this.txt_username.TabIndex = 23;
            // 
            // btn_daftar
            // 
            this.btn_daftar.BackColor = System.Drawing.Color.Black;
            this.btn_daftar.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_daftar.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btn_daftar.Location = new System.Drawing.Point(490, 235);
            this.btn_daftar.Name = "btn_daftar";
            this.btn_daftar.Size = new System.Drawing.Size(259, 36);
            this.btn_daftar.TabIndex = 25;
            this.btn_daftar.Text = "Daftar";
            this.btn_daftar.UseVisualStyleBackColor = false;
            this.btn_daftar.Click += new System.EventHandler(this.btn_daftar_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(486, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 19);
            this.label4.TabIndex = 26;
            this.label4.Text = "Password";
            // 
            // txt_password
            // 
            this.txt_password.Location = new System.Drawing.Point(490, 147);
            this.txt_password.Name = "txt_password";
            this.txt_password.Size = new System.Drawing.Size(259, 22);
            this.txt_password.TabIndex = 27;
            this.txt_password.TextChanged += new System.EventHandler(this.txt_password_TextChanged);
            // 
            // btn_kembali
            // 
            this.btn_kembali.BackColor = System.Drawing.Color.Black;
            this.btn_kembali.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_kembali.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btn_kembali.Location = new System.Drawing.Point(490, 277);
            this.btn_kembali.Name = "btn_kembali";
            this.btn_kembali.Size = new System.Drawing.Size(259, 30);
            this.btn_kembali.TabIndex = 28;
            this.btn_kembali.Text = "Kembali";
            this.btn_kembali.UseVisualStyleBackColor = false;
            this.btn_kembali.Click += new System.EventHandler(this.button2_Click);
            // 
            // lbl_passwordStrength
            // 
            this.lbl_passwordStrength.AutoSize = true;
            this.lbl_passwordStrength.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_passwordStrength.ForeColor = System.Drawing.Color.Gray;
            this.lbl_passwordStrength.Location = new System.Drawing.Point(486, 178);
            this.lbl_passwordStrength.Name = "lbl_passwordStrength";
            this.lbl_passwordStrength.Size = new System.Drawing.Size(0, 19);
            this.lbl_passwordStrength.TabIndex = 29;
            // 
            // pgsBar_Strength
            // 
            this.pgsBar_Strength.Location = new System.Drawing.Point(490, 200);
            this.pgsBar_Strength.Maximum = 5;
            this.pgsBar_Strength.Name = "pgsBar_Strength";
            this.pgsBar_Strength.Size = new System.Drawing.Size(259, 23);
            this.pgsBar_Strength.TabIndex = 30;
            this.pgsBar_Strength.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // RegisterPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SkyBlue;
            this.ClientSize = new System.Drawing.Size(800, 385);
            this.Controls.Add(this.pgsBar_Strength);
            this.Controls.Add(this.lbl_passwordStrength);
            this.Controls.Add(this.btn_kembali);
            this.Controls.Add(this.txt_password);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_daftar);
            this.Controls.Add(this.txt_username);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "RegisterPage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RegisterPage";
            this.Load += new System.EventHandler(this.RegisterPage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_username;
        private System.Windows.Forms.Button btn_daftar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_password;
        private System.Windows.Forms.Button btn_kembali;
        private System.Windows.Forms.Label lbl_passwordStrength;
        private System.Windows.Forms.ProgressBar pgsBar_Strength;
    }
}