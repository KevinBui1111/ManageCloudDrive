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
            this.btnSaveHash = new System.Windows.Forms.Button();
            this.rbOneDrive = new System.Windows.Forms.RadioButton();
            this.rbGDrive = new System.Windows.Forms.RadioButton();
            this.btnCheckDup = new System.Windows.Forms.Button();
            this.lbDir = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.olvFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // olvFiles
            // 
            this.olvFiles.AllColumns.Add(this.olvColumn1);
            this.olvFiles.AllColumns.Add(this.olvColumn2);
            this.olvFiles.AllColumns.Add(this.olvColumn4);
            this.olvFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvFiles.CellEditUseWholeCell = false;
            this.olvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1,
            this.olvColumn2,
            this.olvColumn4});
            this.olvFiles.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvFiles.FullRowSelect = true;
            this.olvFiles.GridLines = true;
            this.olvFiles.Location = new System.Drawing.Point(12, 77);
            this.olvFiles.Name = "olvFiles";
            this.olvFiles.ShowGroups = false;
            this.olvFiles.Size = new System.Drawing.Size(452, 372);
            this.olvFiles.TabIndex = 0;
            this.olvFiles.UseCompatibleStateImageBehavior = false;
            this.olvFiles.View = System.Windows.Forms.View.Details;
            this.olvFiles.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.olvFiles_FormatRow);
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
            // btnSaveHash
            // 
            this.btnSaveHash.Location = new System.Drawing.Point(389, 8);
            this.btnSaveHash.Name = "btnSaveHash";
            this.btnSaveHash.Size = new System.Drawing.Size(75, 27);
            this.btnSaveHash.TabIndex = 1;
            this.btnSaveHash.Text = "Save hash";
            this.btnSaveHash.UseVisualStyleBackColor = true;
            this.btnSaveHash.Click += new System.EventHandler(this.btnSaveHash_Click);
            // 
            // rbOneDrive
            // 
            this.rbOneDrive.AutoSize = true;
            this.rbOneDrive.Location = new System.Drawing.Point(12, 12);
            this.rbOneDrive.Name = "rbOneDrive";
            this.rbOneDrive.Size = new System.Drawing.Size(77, 19);
            this.rbOneDrive.TabIndex = 2;
            this.rbOneDrive.TabStop = true;
            this.rbOneDrive.Text = "One Drive";
            this.rbOneDrive.UseVisualStyleBackColor = true;
            this.rbOneDrive.CheckedChanged += new System.EventHandler(this.rbOneDrive_CheckedChanged);
            // 
            // rbGDrive
            // 
            this.rbGDrive.AutoSize = true;
            this.rbGDrive.Location = new System.Drawing.Point(95, 12);
            this.rbGDrive.Name = "rbGDrive";
            this.rbGDrive.Size = new System.Drawing.Size(93, 19);
            this.rbGDrive.TabIndex = 2;
            this.rbGDrive.TabStop = true;
            this.rbGDrive.Text = "Google Drive";
            this.rbGDrive.UseVisualStyleBackColor = true;
            this.rbGDrive.CheckedChanged += new System.EventHandler(this.rbOneDrive_CheckedChanged);
            // 
            // btnCheckDup
            // 
            this.btnCheckDup.Location = new System.Drawing.Point(308, 8);
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
            this.lbDir.Location = new System.Drawing.Point(12, 43);
            this.lbDir.Name = "lbDir";
            this.lbDir.Size = new System.Drawing.Size(38, 15);
            this.lbDir.TabIndex = 4;
            this.lbDir.Text = "label1";
            // 
            // frmMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 461);
            this.Controls.Add(this.lbDir);
            this.Controls.Add(this.btnCheckDup);
            this.Controls.Add(this.rbGDrive);
            this.Controls.Add(this.rbOneDrive);
            this.Controls.Add(this.btnSaveHash);
            this.Controls.Add(this.olvFiles);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmMain_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.frmMain_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.olvFiles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView olvFiles;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private BrightIdeasSoftware.OLVColumn olvColumn4;
        private System.Windows.Forms.Button btnSaveHash;
        private System.Windows.Forms.RadioButton rbOneDrive;
        private System.Windows.Forms.RadioButton rbGDrive;
        private System.Windows.Forms.Button btnCheckDup;
        private System.Windows.Forms.Label lbDir;
    }
}

