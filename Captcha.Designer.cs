namespace _0lymp.us
{
    partial class Captcha
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
            this.PbCaptcha = new System.Windows.Forms.PictureBox();
            this.GbCaptcha = new System.Windows.Forms.GroupBox();
            this.BtCaptcha = new System.Windows.Forms.Button();
            this.LbCaptcha = new System.Windows.Forms.Label();
            this.TxCaptcha = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.PbCaptcha)).BeginInit();
            this.GbCaptcha.SuspendLayout();
            this.SuspendLayout();
            // 
            // PbCaptcha
            // 
            this.PbCaptcha.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PbCaptcha.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(29)))), ((int)(((byte)(34)))));
            this.PbCaptcha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PbCaptcha.Location = new System.Drawing.Point(8, 57);
            this.PbCaptcha.Name = "PbCaptcha";
            this.PbCaptcha.Size = new System.Drawing.Size(266, 90);
            this.PbCaptcha.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PbCaptcha.TabIndex = 0;
            this.PbCaptcha.TabStop = false;
            // 
            // GbCaptcha
            // 
            this.GbCaptcha.BackColor = System.Drawing.Color.Transparent;
            this.GbCaptcha.Controls.Add(this.BtCaptcha);
            this.GbCaptcha.Controls.Add(this.LbCaptcha);
            this.GbCaptcha.Controls.Add(this.TxCaptcha);
            this.GbCaptcha.Controls.Add(this.PbCaptcha);
            this.GbCaptcha.ForeColor = System.Drawing.Color.Salmon;
            this.GbCaptcha.Location = new System.Drawing.Point(16, 38);
            this.GbCaptcha.Margin = new System.Windows.Forms.Padding(4);
            this.GbCaptcha.Name = "GbCaptcha";
            this.GbCaptcha.Padding = new System.Windows.Forms.Padding(4);
            this.GbCaptcha.Size = new System.Drawing.Size(283, 228);
            this.GbCaptcha.TabIndex = 1;
            this.GbCaptcha.TabStop = false;
            this.GbCaptcha.Text = "Captcha";
            // 
            // BtCaptcha
            // 
            this.BtCaptcha.BackColor = System.Drawing.Color.Salmon;
            this.BtCaptcha.FlatAppearance.BorderSize = 0;
            this.BtCaptcha.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtCaptcha.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtCaptcha.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.BtCaptcha.Location = new System.Drawing.Point(88, 188);
            this.BtCaptcha.Margin = new System.Windows.Forms.Padding(4);
            this.BtCaptcha.Name = "BtCaptcha";
            this.BtCaptcha.Size = new System.Drawing.Size(100, 28);
            this.BtCaptcha.TabIndex = 2;
            this.BtCaptcha.Text = "Continuar";
            this.BtCaptcha.UseVisualStyleBackColor = false;
            // 
            // LbCaptcha
            // 
            this.LbCaptcha.AutoSize = true;
            this.LbCaptcha.Location = new System.Drawing.Point(35, 28);
            this.LbCaptcha.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LbCaptcha.Name = "LbCaptcha";
            this.LbCaptcha.Size = new System.Drawing.Size(218, 17);
            this.LbCaptcha.TabIndex = 2;
            this.LbCaptcha.Text = "Introduce los caracteres que ves.";
            // 
            // TxCaptcha
            // 
            this.TxCaptcha.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(29)))), ((int)(((byte)(34)))));
            this.TxCaptcha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxCaptcha.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.TxCaptcha.Location = new System.Drawing.Point(8, 154);
            this.TxCaptcha.Margin = new System.Windows.Forms.Padding(4);
            this.TxCaptcha.Name = "TxCaptcha";
            this.TxCaptcha.Size = new System.Drawing.Size(266, 22);
            this.TxCaptcha.TabIndex = 1;
            this.TxCaptcha.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Captcha
            // 
            this.AcceptButton = this.BtCaptcha;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(29)))), ((int)(((byte)(34)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(315, 276);
            this.Controls.Add(this.GbCaptcha);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Captcha";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "¡Hay un problema!";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Captcha_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PbCaptcha)).EndInit();
            this.GbCaptcha.ResumeLayout(false);
            this.GbCaptcha.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox PbCaptcha;
        private System.Windows.Forms.GroupBox GbCaptcha;
        private System.Windows.Forms.Button BtCaptcha;
        private System.Windows.Forms.Label LbCaptcha;
        public System.Windows.Forms.TextBox TxCaptcha;
    }
}