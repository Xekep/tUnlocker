// -----------------------------------------------------------------------
//    tUnlocker - by atom0s (c) 2013 [atom0s@live.com]
//
//    tUnlocker is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    tUnlocker is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with tUnlocker.  If not, see <http://www.gnu.org/licenses/>.
// -----------------------------------------------------------------------

namespace tUnlocker
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.btnPatchTerraria = new System.Windows.Forms.Button();
            this.btnLaunchTerraria = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstLog);
            this.groupBox1.Location = new System.Drawing.Point(12, 108);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(451, 249);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "tUnlocker Log";
            // 
            // lstLog
            // 
            this.lstLog.FormattingEnabled = true;
            this.lstLog.Location = new System.Drawing.Point(6, 19);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(439, 225);
            this.lstLog.TabIndex = 0;
            // 
            // btnPatchTerraria
            // 
            this.btnPatchTerraria.Image = global::tUnlocker.Properties.Resources.patch;
            this.btnPatchTerraria.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPatchTerraria.Location = new System.Drawing.Point(304, 12);
            this.btnPatchTerraria.Name = "btnPatchTerraria";
            this.btnPatchTerraria.Size = new System.Drawing.Size(159, 42);
            this.btnPatchTerraria.TabIndex = 1;
            this.btnPatchTerraria.Text = "Patch Terraria";
            this.btnPatchTerraria.UseVisualStyleBackColor = true;
            this.btnPatchTerraria.Click += new System.EventHandler(this.btnPatchTerraria_Click);
            // 
            // btnLaunchTerraria
            // 
            this.btnLaunchTerraria.Image = global::tUnlocker.Properties.Resources.launch;
            this.btnLaunchTerraria.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLaunchTerraria.Location = new System.Drawing.Point(304, 60);
            this.btnLaunchTerraria.Name = "btnLaunchTerraria";
            this.btnLaunchTerraria.Size = new System.Drawing.Size(159, 42);
            this.btnLaunchTerraria.TabIndex = 2;
            this.btnLaunchTerraria.Text = "Launch Terraria";
            this.btnLaunchTerraria.UseVisualStyleBackColor = true;
            this.btnLaunchTerraria.Click += new System.EventHandler(this.btnLaunchTerraria_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::tUnlocker.Properties.Resources.unlock;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(75, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "tUnlocker - by atom0s";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(75, 34);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(81, 13);
            this.lblVersion.TabIndex = 5;
            this.lblVersion.Text = "Version: 1.0.0.0";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 371);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnLaunchTerraria);
            this.Controls.Add(this.btnPatchTerraria);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "tUnlocker :: by atom0s";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.Button btnPatchTerraria;
        private System.Windows.Forms.Button btnLaunchTerraria;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblVersion;


    }
}

