namespace ManageCloudDrive
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
            this.olvFiles = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn4 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn3 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.btnSaveHash = new System.Windows.Forms.Button();
            this.btnCheckDup = new System.Windows.Forms.Button();
            this.lbDir = new System.Windows.Forms.Label();
            this.btnCompare = new System.Windows.Forms.Button();
            this.cbDrive = new System.Windows.Forms.ComboBox();
            this.btnLoadDrive = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbUserinfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbStorageamount = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnLogout = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnClearLocal = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbSource = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.olvFiles)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // olvFiles
            // 
            this.olvFiles.AllColumns.Add(this.olvColumn1);
            this.olvFiles.AllColumns.Add(this.olvColumn2);
            this.olvFiles.AllColumns.Add(this.olvColumn4);
            this.olvFiles.AllColumns.Add(this.olvColumn3);
            this.olvFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvFiles.CellEditUseWholeCell = false;
            this.olvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1,
            this.olvColumn2,
            this.olvColumn4,
            this.olvColumn3});
            this.olvFiles.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvFiles.FullRowSelect = true;
            this.olvFiles.GridLines = true;
            this.olvFiles.Location = new System.Drawing.Point(12, 55);
            this.olvFiles.Name = "olvFiles";
            this.olvFiles.ShowGroups = false;
            this.olvFiles.Size = new System.Drawing.Size(452, 381);
            this.olvFiles.TabIndex = 0;
            this.olvFiles.UseCompatibleStateImageBehavior = false;
            this.olvFiles.View = System.Windows.Forms.View.Details;
            this.olvFiles.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.olvFiles_FormatRow);
            this.olvFiles.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.olvFiles_ItemDrag);
            this.olvFiles.DoubleClick += new System.EventHandler(this.olvFiles_DoubleClick);
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "Name";
            this.olvColumn1.Text = "Name";
            this.olvColumn1.Width = 180;
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "Size";
            this.olvColumn2.AspectToStringFormat = "{0:n0}";
            this.olvColumn2.Text = "Size";
            this.olvColumn2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvColumn2.Width = 84;
            // 
            // olvColumn4
            // 
            this.olvColumn4.AspectName = "CountFile";
            this.olvColumn4.AspectToStringFormat = "{0:n0}";
            this.olvColumn4.Text = "No Files";
            this.olvColumn4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvColumn4.Width = 89;
            // 
            // olvColumn3
            // 
            this.olvColumn3.Text = "Operation";
            // 
            // btnSaveHash
            // 
            this.btnSaveHash.Location = new System.Drawing.Point(308, 5);
            this.btnSaveHash.Name = "btnSaveHash";
            this.btnSaveHash.Size = new System.Drawing.Size(75, 27);
            this.btnSaveHash.TabIndex = 1;
            this.btnSaveHash.Text = "Save hash";
            this.btnSaveHash.UseVisualStyleBackColor = true;
            this.btnSaveHash.Click += new System.EventHandler(this.btnSaveHash_Click);
            // 
            // btnCheckDup
            // 
            this.btnCheckDup.Location = new System.Drawing.Point(227, 5);
            this.btnCheckDup.Name = "btnCheckDup";
            this.btnCheckDup.Size = new System.Drawing.Size(75, 27);
            this.btnCheckDup.TabIndex = 3;
            this.btnCheckDup.Text = "Check Dup";
            this.btnCheckDup.UseVisualStyleBackColor = true;
            this.btnCheckDup.Click += new System.EventHandler(this.btnCheckDup_Click);
            // 
            // lbDir
            // 
            this.lbDir.AutoSize = true;
            this.lbDir.Location = new System.Drawing.Point(12, 37);
            this.lbDir.Name = "lbDir";
            this.lbDir.Size = new System.Drawing.Size(52, 15);
            this.lbDir.TabIndex = 4;
            this.lbDir.Text = "Local dir";
            // 
            // btnCompare
            // 
            this.btnCompare.Location = new System.Drawing.Point(389, 5);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(75, 27);
            this.btnCompare.TabIndex = 1;
            this.btnCompare.Text = "Compare";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // cbDrive
            // 
            this.cbDrive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDrive.Items.AddRange(new object[] {
            "Google Drive",
            "One Drive"});
            this.cbDrive.Location = new System.Drawing.Point(15, 8);
            this.cbDrive.Name = "cbDrive";
            this.cbDrive.Size = new System.Drawing.Size(92, 23);
            this.cbDrive.TabIndex = 5;
            // 
            // btnLoadDrive
            // 
            this.btnLoadDrive.Location = new System.Drawing.Point(113, 5);
            this.btnLoadDrive.Name = "btnLoadDrive";
            this.btnLoadDrive.Size = new System.Drawing.Size(75, 27);
            this.btnLoadDrive.TabIndex = 6;
            this.btnLoadDrive.Text = "Load Drive";
            this.btnLoadDrive.UseVisualStyleBackColor = true;
            this.btnLoadDrive.Click += new System.EventHandler(this.btnLoadDrive_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbUserinfo,
            this.lbStorageamount,
            this.btnLogout,
            this.btnClearLocal,
            this.toolStripStatusLabel3,
            this.lbSource});
            this.statusStrip1.Location = new System.Drawing.Point(0, 437);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.ShowItemToolTips = true;
            this.statusStrip1.Size = new System.Drawing.Size(476, 24);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbUserinfo
            // 
            this.lbUserinfo.Name = "lbUserinfo";
            this.lbUserinfo.Size = new System.Drawing.Size(37, 19);
            this.lbUserinfo.Text = "Login";
            // 
            // lbStorageamount
            // 
            this.lbStorageamount.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.lbStorageamount.Name = "lbStorageamount";
            this.lbStorageamount.Size = new System.Drawing.Size(30, 19);
            this.lbStorageamount.Text = "size";
            // 
            // btnLogout
            // 
            this.btnLogout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLogout.Image = global::ManageCloudDrive.Properties.Resources.logout;
            this.btnLogout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.ShowDropDownArrow = false;
            this.btnLogout.Size = new System.Drawing.Size(20, 22);
            this.btnLogout.Text = "Log out";
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_ClickAsync);
            // 
            // btnClearLocal
            // 
            this.btnClearLocal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClearLocal.Image = global::ManageCloudDrive.Properties.Resources.bin;
            this.btnClearLocal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClearLocal.Name = "btnClearLocal";
            this.btnClearLocal.ShowDropDownArrow = false;
            this.btnClearLocal.Size = new System.Drawing.Size(20, 22);
            this.btnClearLocal.Text = "Clear cache";
            this.btnClearLocal.Click += new System.EventHandler(this.btnClearLocal_Click);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(350, 19);
            this.toolStripStatusLabel3.Spring = true;
            // 
            // lbSource
            // 
            this.lbSource.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lbSource.Name = "lbSource";
            this.lbSource.Size = new System.Drawing.Size(4, 19);
            // 
            // frmMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 461);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnLoadDrive);
            this.Controls.Add(this.cbDrive);
            this.Controls.Add(this.lbDir);
            this.Controls.Add(this.btnCheckDup);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.btnSaveHash);
            this.Controls.Add(this.olvFiles);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::ManageCloudDrive.Properties.Resources.drive;
            this.Name = "frmMain";
            this.Text = "ManageCloudDrive";
            this.Load += new System.EventHandler(this.Form1_LoadAsync);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmMain_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.frmMain_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.olvFiles)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView olvFiles;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private BrightIdeasSoftware.OLVColumn olvColumn4;
        private System.Windows.Forms.Button btnSaveHash;
        private System.Windows.Forms.Button btnCheckDup;
        private System.Windows.Forms.Label lbDir;
        private System.Windows.Forms.Button btnCompare;
        private BrightIdeasSoftware.OLVColumn olvColumn3;
        private System.Windows.Forms.ComboBox cbDrive;
        private System.Windows.Forms.Button btnLoadDrive;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lbUserinfo;
        private System.Windows.Forms.ToolStripStatusLabel lbStorageamount;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel lbSource;
        private System.Windows.Forms.ToolStripDropDownButton btnLogout;
        private System.Windows.Forms.ToolStripDropDownButton btnClearLocal;
    }
}

