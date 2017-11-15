namespace CFWDatamigrationApp
{
    partial class Form1
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
            this.btnInsertData = new System.Windows.Forms.Button();
            this.comboDestTables = new System.Windows.Forms.ComboBox();
            this.comboDestcolumns = new System.Windows.Forms.ComboBox();
            this.comboSourcetable = new System.Windows.Forms.ComboBox();
            this.lblSource = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lblDest = new System.Windows.Forms.Label();
            this.btmAddSourceColumn = new System.Windows.Forms.Button();
            this.comboSourceColumns = new System.Windows.Forms.ComboBox();
            this.btnGetData = new System.Windows.Forms.Button();
            this.dataGridSourceColumns = new System.Windows.Forms.DataGridView();
            this.dataGridDestinationColumns = new System.Windows.Forms.DataGridView();
            this.btnAddDestColumn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSourceColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDestinationColumns)).BeginInit();
            this.SuspendLayout();
            // 
            // btnInsertData
            // 
            this.btnInsertData.Location = new System.Drawing.Point(801, 460);
            this.btnInsertData.Name = "btnInsertData";
            this.btnInsertData.Size = new System.Drawing.Size(342, 62);
            this.btnInsertData.TabIndex = 0;
            this.btnInsertData.Text = "Insert to destination";
            this.btnInsertData.UseVisualStyleBackColor = true;
            this.btnInsertData.Click += new System.EventHandler(this.btnInsertData_Click);
            // 
            // comboDestTables
            // 
            this.comboDestTables.FormattingEnabled = true;
            this.comboDestTables.Location = new System.Drawing.Point(516, 98);
            this.comboDestTables.Name = "comboDestTables";
            this.comboDestTables.Size = new System.Drawing.Size(121, 28);
            this.comboDestTables.TabIndex = 1;
            this.comboDestTables.SelectedIndexChanged += new System.EventHandler(this.comboDestTables_SelectedIndexChanged);
            // 
            // comboDestcolumns
            // 
            this.comboDestcolumns.FormattingEnabled = true;
            this.comboDestcolumns.Location = new System.Drawing.Point(668, 98);
            this.comboDestcolumns.Name = "comboDestcolumns";
            this.comboDestcolumns.Size = new System.Drawing.Size(121, 28);
            this.comboDestcolumns.TabIndex = 2;
            // 
            // comboSourcetable
            // 
            this.comboSourcetable.FormattingEnabled = true;
            this.comboSourcetable.Location = new System.Drawing.Point(41, 98);
            this.comboSourcetable.Name = "comboSourcetable";
            this.comboSourcetable.Size = new System.Drawing.Size(121, 28);
            this.comboSourcetable.TabIndex = 4;
            this.comboSourcetable.SelectedIndexChanged += new System.EventHandler(this.comboSourcetable_SelectedIndexChanged);
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(37, 52);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(103, 20);
            this.lblSource.TabIndex = 5;
            this.lblSource.Text = "Source Table";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(41, 559);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(1114, 249);
            this.dataGridView1.TabIndex = 7;
            // 
            // lblDest
            // 
            this.lblDest.AutoSize = true;
            this.lblDest.Location = new System.Drawing.Point(512, 52);
            this.lblDest.Name = "lblDest";
            this.lblDest.Size = new System.Drawing.Size(142, 20);
            this.lblDest.TabIndex = 8;
            this.lblDest.Text = "Destination Tabale";
            // 
            // btmAddSourceColumn
            // 
            this.btmAddSourceColumn.Location = new System.Drawing.Point(317, 98);
            this.btmAddSourceColumn.Name = "btmAddSourceColumn";
            this.btmAddSourceColumn.Size = new System.Drawing.Size(158, 35);
            this.btmAddSourceColumn.TabIndex = 6;
            this.btmAddSourceColumn.Text = "Add column";
            this.btmAddSourceColumn.UseVisualStyleBackColor = true;
            this.btmAddSourceColumn.Click += new System.EventHandler(this.btmAddSourceColumn_Click);
            // 
            // comboSourceColumns
            // 
            this.comboSourceColumns.FormattingEnabled = true;
            this.comboSourceColumns.Location = new System.Drawing.Point(169, 98);
            this.comboSourceColumns.Name = "comboSourceColumns";
            this.comboSourceColumns.Size = new System.Drawing.Size(121, 28);
            this.comboSourceColumns.TabIndex = 9;
            // 
            // btnGetData
            // 
            this.btnGetData.Location = new System.Drawing.Point(159, 460);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(249, 62);
            this.btnGetData.TabIndex = 10;
            this.btnGetData.Text = "Get data from Source";
            this.btnGetData.UseVisualStyleBackColor = true;
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // dataGridSourceColumns
            // 
            this.dataGridSourceColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridSourceColumns.Location = new System.Drawing.Point(41, 193);
            this.dataGridSourceColumns.Name = "dataGridSourceColumns";
            this.dataGridSourceColumns.RowTemplate.Height = 28;
            this.dataGridSourceColumns.Size = new System.Drawing.Size(687, 261);
            this.dataGridSourceColumns.TabIndex = 11;
            // 
            // dataGridDestinationColumns
            // 
            this.dataGridDestinationColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridDestinationColumns.Location = new System.Drawing.Point(783, 193);
            this.dataGridDestinationColumns.Name = "dataGridDestinationColumns";
            this.dataGridDestinationColumns.RowTemplate.Height = 28;
            this.dataGridDestinationColumns.Size = new System.Drawing.Size(372, 261);
            this.dataGridDestinationColumns.TabIndex = 12;
            // 
            // btnAddDestColumn
            // 
            this.btnAddDestColumn.Location = new System.Drawing.Point(964, 94);
            this.btnAddDestColumn.Name = "btnAddDestColumn";
            this.btnAddDestColumn.Size = new System.Drawing.Size(158, 35);
            this.btnAddDestColumn.TabIndex = 13;
            this.btnAddDestColumn.Text = "Add column";
            this.btnAddDestColumn.UseVisualStyleBackColor = true;
            this.btnAddDestColumn.Click += new System.EventHandler(this.btnAddDestColumn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1241, 820);
            this.Controls.Add(this.btnAddDestColumn);
            this.Controls.Add(this.dataGridDestinationColumns);
            this.Controls.Add(this.dataGridSourceColumns);
            this.Controls.Add(this.btnGetData);
            this.Controls.Add(this.comboSourceColumns);
            this.Controls.Add(this.lblDest);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btmAddSourceColumn);
            this.Controls.Add(this.lblSource);
            this.Controls.Add(this.comboSourcetable);
            this.Controls.Add(this.comboDestcolumns);
            this.Controls.Add(this.comboDestTables);
            this.Controls.Add(this.btnInsertData);
            this.Name = "Form1";
            this.Text = "Data migration from Oracle -> SQL";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSourceColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDestinationColumns)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInsertData;
        private System.Windows.Forms.ComboBox comboDestTables;
        private System.Windows.Forms.ComboBox comboDestcolumns;
        private System.Windows.Forms.ComboBox comboSourcetable;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lblDest;
        private System.Windows.Forms.Button btmAddSourceColumn;
        private System.Windows.Forms.ComboBox comboSourceColumns;
        private System.Windows.Forms.Button btnGetData;
        private System.Windows.Forms.DataGridView dataGridSourceColumns;
        private System.Windows.Forms.DataGridView dataGridDestinationColumns;
        private System.Windows.Forms.Button btnAddDestColumn;
    }
}