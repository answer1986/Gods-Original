using System;
using System.Drawing;
using System.Windows.Forms;

namespace _0lymp.us
{
    partial class Enter
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
            this.GbEnter = new System.Windows.Forms.GroupBox();
            this.BtnEnter = new System.Windows.Forms.Button();
            this.TxtLicense = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PbEnter = new System.Windows.Forms.PictureBox();
            this.LbInfoEnter = new System.Windows.Forms.Label();
            this.LbVersion = new System.Windows.Forms.Label();
            this.GbEnter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbEnter)).BeginInit();
            this.SuspendLayout();
            // 
            // GbEnter
            // 
            this.GbEnter.BackColor = System.Drawing.Color.Transparent;
            this.GbEnter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.GbEnter.Controls.Add(this.BtnEnter);
            this.GbEnter.Controls.Add(this.TxtLicense);
            this.GbEnter.Controls.Add(this.label1);
            this.GbEnter.Controls.Add(this.PbEnter);
            this.GbEnter.Controls.Add(this.LbInfoEnter);
            this.GbEnter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GbEnter.ForeColor = System.Drawing.Color.Salmon;
            this.GbEnter.Location = new System.Drawing.Point(17, 43);
            this.GbEnter.Name = "GbEnter";
            this.GbEnter.Size = new System.Drawing.Size(193, 169);
            this.GbEnter.TabIndex = 3;
            this.GbEnter.TabStop = false;
            this.GbEnter.Text = "Iniciar Sesión";
            // 
            // BtnEnter
            // 
            this.BtnEnter.BackColor = System.Drawing.Color.Salmon;
            this.BtnEnter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnEnter.FlatAppearance.BorderColor = System.Drawing.Color.Salmon;
            this.BtnEnter.FlatAppearance.BorderSize = 0;
            this.BtnEnter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnEnter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnEnter.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.BtnEnter.Location = new System.Drawing.Point(54, 101);
            this.BtnEnter.Name = "BtnEnter";
            this.BtnEnter.Size = new System.Drawing.Size(84, 32);
            this.BtnEnter.TabIndex = 4;
            this.BtnEnter.Text = "Ingresar";
            this.BtnEnter.UseVisualStyleBackColor = false;
            this.BtnEnter.Click += new System.EventHandler(this.BtnEnter_Click);
            // 
            // TxtLicense
            // 
            this.TxtLicense.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.RecentlyUsedList;
            this.TxtLicense.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(29)))), ((int)(((byte)(34)))));
            this.TxtLicense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtLicense.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.TxtLicense.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.TxtLicense.Location = new System.Drawing.Point(15, 59);
            this.TxtLicense.Name = "TxtLicense";
            this.TxtLicense.PasswordChar = '*';
            this.TxtLicense.Size = new System.Drawing.Size(162, 22);
            this.TxtLicense.TabIndex = 1;
            this.TxtLicense.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            //this.TxtLicense.KeyPress += new System.KeyPressEventHandler(this.TxtLicense_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Ingrese su licencia:";
            // 
            // PbEnter
            // 
            this.PbEnter.Location = new System.Drawing.Point(89, 84);
            this.PbEnter.Name = "PbEnter";
            this.PbEnter.Size = new System.Drawing.Size(16, 11);
            this.PbEnter.TabIndex = 24;
            this.PbEnter.TabStop = false;
            // 
            // LbInfoEnter
            // 
            this.LbInfoEnter.Location = new System.Drawing.Point(0, 136);
            this.LbInfoEnter.Name = "LbInfoEnter";
            this.LbInfoEnter.Size = new System.Drawing.Size(193, 25);
            this.LbInfoEnter.TabIndex = 25;
            this.LbInfoEnter.Text = "...";
            this.LbInfoEnter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LbVersion
            // 
            this.LbVersion.AutoSize = true;
            this.LbVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.LbVersion.Location = new System.Drawing.Point(93, 215);
            this.LbVersion.Name = "LbVersion";
            this.LbVersion.Size = new System.Drawing.Size(40, 17);
            this.LbVersion.TabIndex = 13;
            this.LbVersion.Text = "0.0.0";
            this.LbVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Enter
            // 
            this.AcceptButton = this.BtnEnter;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BackgroundImage = global::_0lymp.us.Properties.Resources.WhatsApp_Image_2020_01_14_at_3_14_04_PM;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(226, 235);
            this.Controls.Add(this.LbVersion);
            this.Controls.Add(this.GbEnter);
            this.Name = "Enter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.GbEnter.ResumeLayout(false);
            this.GbEnter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbEnter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox GbEnter;
        private Label LbVersion;
        private PictureBox PbEnter;
        private Label LbInfoEnter;
        private TextBox TxtLicense;
        private Label label1;
        private Button BtnEnter;
    }
}