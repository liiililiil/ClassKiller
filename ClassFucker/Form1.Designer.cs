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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            tableLayoutPanel1 = new TableLayoutPanel();
            progressBar1 = new ProgressBar();
            label1 = new Label();
            label2 = new Label();
            netSupportInfo = new Label();
            classMinfo = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            trackBar1 = new TrackBar();
            Sc = new Button();
            TempTurnOn = new Button();
            _Restoration = new Button();
            _isolation = new Button();
            start = new Button();
            stop = new Button();
            label5 = new Label();
            loglabel = new TextBox();
            timer1 = new System.Windows.Forms.Timer(components);
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoScroll = true;
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
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel1.Size = new Size(365, 309);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // progressBar1
            // 
            progressBar1.Dock = DockStyle.Fill;
            progressBar1.Location = new Point(3, 282);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(359, 24);
            progressBar1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 219);
            label1.Name = "label1";
            label1.Size = new Size(359, 25);
            label1.TabIndex = 1;
            label1.Text = "NetSupport";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(3, 159);
            label2.Name = "label2";
            label2.Size = new Size(359, 25);
            label2.TabIndex = 2;
            label2.Text = "ClassM";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // netSupportInfo
            // 
            netSupportInfo.AutoSize = true;
            netSupportInfo.Dock = DockStyle.Fill;
            netSupportInfo.ImageAlign = ContentAlignment.MiddleLeft;
            netSupportInfo.Location = new Point(3, 244);
            netSupportInfo.Name = "netSupportInfo";
            netSupportInfo.Size = new Size(359, 25);
            netSupportInfo.TabIndex = 3;
            netSupportInfo.Text = "알수없음";
            // 
            // classMinfo
            // 
            classMinfo.AutoSize = true;
            classMinfo.Dock = DockStyle.Fill;
            classMinfo.Location = new Point(3, 184);
            classMinfo.Name = "classMinfo";
            classMinfo.Size = new Size(359, 25);
            classMinfo.TabIndex = 4;
            classMinfo.Text = "알수없음";
            classMinfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.AutoSize = true;
            tableLayoutPanel2.ColumnCount = 5;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Controls.Add(trackBar1, 0, 2);
            tableLayoutPanel2.Controls.Add(Sc, 2, 0);
            tableLayoutPanel2.Controls.Add(TempTurnOn, 2, 2);
            tableLayoutPanel2.Controls.Add(_Restoration, 3, 2);
            tableLayoutPanel2.Controls.Add(_isolation, 3, 0);
            tableLayoutPanel2.Controls.Add(start, 4, 0);
            tableLayoutPanel2.Controls.Add(stop, 4, 2);
            tableLayoutPanel2.Controls.Add(label5, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tableLayoutPanel2.Location = new Point(3, 72);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel2.Size = new Size(359, 74);
            tableLayoutPanel2.TabIndex = 5;
            // 
            // trackBar1
            // 
            trackBar1.Location = new Point(3, 47);
            trackBar1.Maximum = 300;
            trackBar1.Minimum = 10;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(84, 24);
            trackBar1.TabIndex = 1;
            trackBar1.Value = 10;
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // Sc
            // 
            Sc.AutoSize = true;
            Sc.Dock = DockStyle.Fill;
            Sc.Location = new Point(92, 3);
            Sc.Name = "Sc";
            Sc.Size = new Size(84, 24);
            Sc.TabIndex = 0;
            Sc.Text = "탐색";
            Sc.UseVisualStyleBackColor = true;
            Sc.Click += Sc_Click;
            // 
            // TempTurnOn
            // 
            TempTurnOn.AutoSize = true;
            TempTurnOn.Dock = DockStyle.Fill;
            TempTurnOn.Location = new Point(92, 47);
            TempTurnOn.Name = "TempTurnOn";
            TempTurnOn.Size = new Size(84, 24);
            TempTurnOn.TabIndex = 1;
            TempTurnOn.Text = "잠시 실행";
            TempTurnOn.UseVisualStyleBackColor = true;
            TempTurnOn.Click += TempTurnOn_Click;
            // 
            // _Restoration
            // 
            _Restoration.Dock = DockStyle.Fill;
            _Restoration.Location = new Point(182, 47);
            _Restoration.Name = "_Restoration";
            _Restoration.Size = new Size(84, 24);
            _Restoration.TabIndex = 3;
            _Restoration.Text = "복구";
            _Restoration.UseVisualStyleBackColor = true;
            _Restoration.Click += _Restoration_Click;
            // 
            // _isolation
            // 
            _isolation.Dock = DockStyle.Fill;
            _isolation.Location = new Point(182, 3);
            _isolation.Name = "_isolation";
            _isolation.Size = new Size(84, 24);
            _isolation.TabIndex = 4;
            _isolation.Text = "격리";
            _isolation.UseVisualStyleBackColor = true;
            _isolation.Click += _isolation_Click;
            // 
            // start
            // 
            start.Dock = DockStyle.Fill;
            start.Location = new Point(272, 3);
            start.Name = "start";
            start.Size = new Size(84, 24);
            start.TabIndex = 7;
            start.Text = "켜기";
            start.UseVisualStyleBackColor = true;
            start.Click += start_Click;
            // 
            // stop
            // 
            stop.Dock = DockStyle.Fill;
            stop.Location = new Point(272, 47);
            stop.Name = "stop";
            stop.Size = new Size(84, 24);
            stop.TabIndex = 8;
            stop.Text = "끄기";
            stop.UseVisualStyleBackColor = true;
            stop.Click += stop_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Fill;
            label5.ImageAlign = ContentAlignment.BottomCenter;
            label5.Location = new Point(3, 0);
            label5.Name = "label5";
            label5.Size = new Size(84, 30);
            label5.TabIndex = 9;
            label5.Text = "label5";
            label5.TextAlign = ContentAlignment.BottomCenter;
            // 
            // loglabel
            // 
            loglabel.Dock = DockStyle.Fill;
            loglabel.Location = new Point(3, 3);
            loglabel.Multiline = true;
            loglabel.Name = "loglabel";
            loglabel.ReadOnly = true;
            loglabel.ScrollBars = ScrollBars.Both;
            loglabel.Size = new Size(359, 63);
            loglabel.TabIndex = 6;
            loglabel.TabStop = false;
            loglabel.Text = "로그창";
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 800;
            timer1.Tick += timer1_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(365, 309);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(381, 348);
            Name = "Form1";
            Text = "abiectio";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
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
        private Label label1;
        private Label label2;
        private Label netSupportInfo;
        private TableLayoutPanel tableLayoutPanel2;
        private Button Sc;
        private Button _isolation;
        private ProgressBar progressBar1;
        private Label classMinfo;
        private System.Windows.Forms.Timer timer1;
        private TextBox loglabel;
        private Button start;
        private Button stop;
        private Button TempTurnOn;
        private TrackBar trackBar1;
        private Label label5;
    }
}
