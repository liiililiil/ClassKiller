namespace ClassFucker
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            progressBar1 = new ProgressBar();
            label1 = new Label();
            label2 = new Label();
            netSupportInfo = new Label();
            classMinfo = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            FastSc = new Button();
            AllSc = new Button();
            _Restoration = new Button();
            _isolation = new Button();
            loglabel = new Label();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(progressBar1, 0, 9);
            tableLayoutPanel1.Controls.Add(label1, 0, 6);
            tableLayoutPanel1.Controls.Add(label2, 0, 3);
            tableLayoutPanel1.Controls.Add(netSupportInfo, 0, 7);
            tableLayoutPanel1.Controls.Add(classMinfo, 0, 4);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Controls.Add(loglabel, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 10;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 47.3684158F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 31.5789547F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 21.0526314F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel1.Size = new Size(283, 372);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // progressBar1
            // 
            progressBar1.Dock = DockStyle.Fill;
            progressBar1.Location = new Point(3, 343);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(277, 26);
            progressBar1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 298);
            label1.Name = "label1";
            label1.Size = new Size(277, 20);
            label1.TabIndex = 1;
            label1.Text = "NetSupport";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(3, 255);
            label2.Name = "label2";
            label2.Size = new Size(277, 20);
            label2.TabIndex = 2;
            label2.Text = "ClassM";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // netSupportInfo
            // 
            netSupportInfo.AutoSize = true;
            netSupportInfo.Dock = DockStyle.Fill;
            netSupportInfo.ImageAlign = ContentAlignment.MiddleLeft;
            netSupportInfo.Location = new Point(3, 318);
            netSupportInfo.Name = "netSupportInfo";
            netSupportInfo.Size = new Size(277, 20);
            netSupportInfo.TabIndex = 3;
            netSupportInfo.Text = "알수없음";
            // 
            // classMinfo
            // 
            classMinfo.AutoSize = true;
            classMinfo.Dock = DockStyle.Fill;
            classMinfo.Location = new Point(3, 275);
            classMinfo.Name = "classMinfo";
            classMinfo.Size = new Size(277, 20);
            classMinfo.TabIndex = 4;
            classMinfo.Text = "알수없음";
            classMinfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.AutoSize = true;
            tableLayoutPanel2.ColumnCount = 5;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333244F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333435F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel2.Controls.Add(FastSc, 1, 0);
            tableLayoutPanel2.Controls.Add(AllSc, 1, 2);
            tableLayoutPanel2.Controls.Add(_Restoration, 3, 2);
            tableLayoutPanel2.Controls.Add(_isolation, 3, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tableLayoutPanel2.Location = new Point(3, 153);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 42.6303864F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 14.7392311F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 42.6303825F));
            tableLayoutPanel2.Size = new Size(277, 94);
            tableLayoutPanel2.TabIndex = 5;
            // 
            // FastSc
            // 
            FastSc.AutoSize = true;
            FastSc.Dock = DockStyle.Fill;
            FastSc.Location = new Point(41, 3);
            FastSc.Name = "FastSc";
            FastSc.Size = new Size(74, 34);
            FastSc.TabIndex = 0;
            FastSc.Text = "빠른 탐색";
            FastSc.UseVisualStyleBackColor = true;
            FastSc.Click += FastSc_Click_1;
            // 
            // AllSc
            // 
            AllSc.AutoSize = true;
            AllSc.Dock = DockStyle.Fill;
            AllSc.Location = new Point(41, 56);
            AllSc.Name = "AllSc";
            AllSc.Size = new Size(74, 35);
            AllSc.TabIndex = 1;
            AllSc.Text = "전체 탐색";
            AllSc.UseVisualStyleBackColor = true;
            AllSc.Click += AllSc_Click;
            // 
            // _Restoration
            // 
            _Restoration.Dock = DockStyle.Fill;
            _Restoration.Location = new Point(160, 56);
            _Restoration.Name = "_Restoration";
            _Restoration.Size = new Size(74, 35);
            _Restoration.TabIndex = 3;
            _Restoration.Text = "복구";
            _Restoration.UseVisualStyleBackColor = true;
            _Restoration.Click += _Restoration_Click;
            // 
            // _isolation
            // 
            _isolation.Dock = DockStyle.Fill;
            _isolation.Location = new Point(160, 3);
            _isolation.Name = "_isolation";
            _isolation.Size = new Size(74, 34);
            _isolation.TabIndex = 4;
            _isolation.Text = "격리";
            _isolation.UseVisualStyleBackColor = true;
            _isolation.Click += _isolation_Click;
            // 
            // loglabel
            // 
            loglabel.AutoSize = true;
            loglabel.Dock = DockStyle.Fill;
            loglabel.Location = new Point(3, 0);
            loglabel.Name = "loglabel";
            loglabel.Size = new Size(277, 150);
            loglabel.TabIndex = 6;
            loglabel.Text = "로그";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(283, 372);
            Controls.Add(tableLayoutPanel1);
            Name = "Form1";
            Text = "ClassFucker";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label label3;
        private Label label4;
        private Button Isolation;
        private Button _Restoration;
        private Button button4;
        private ProgressBar progressBar1;
        private Label label1;
        private Label label2;
        private Label netSupportInfo;
        private Label classMinfo;
        private TableLayoutPanel tableLayoutPanel2;
        private Button FastSc;
        private Button AllSc;
        private Button _isolation;
        private Label loglabel;
    }
}
