using System;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassFucker
{
    public partial class Form1 : Form
    {
        private string classMPath;
        private string netSupportPath;

        public Form1()
        {
            InitializeComponent();
            // Fast scan to find ClassM and NetSupport
            ClassMScan();
            NetSupportScan();
        }

        // Fast scan functions
        public void ClassMScan()
        {
            if (Directory.Exists(@"C:\Program Files (x86)\ClassM")) // Check known path
                SetClassMPath(@"C:\Program Files (x86)\ClassM\hscagent.exe");
            else
            {
                // Search for the process if not found in the known path
                Process[] process = Process.GetProcessesByName("hscagent");

                if (process.Length == 0)
                    classMinfo.Text = "발견되지않음";
                else
                    SetClassMPath(process[0].MainModule.FileName);
            }

            // Update progress
            progressBar1.Value += 50;
        }

        public void NetSupportScan()
        {
            if (Directory.Exists(@"C:\Program Files (x86)\NetSupport\NetSupport School")) // Check known path
                SetNetSupportPath(@"C:\Program Files (x86)\NetSupport\NetSupport School\client32.exe");
            else
            {
                // Search for the process if not found in the known path
                Process[] process = Process.GetProcessesByName("client32");

                if (process.Length == 0)
                    netSupportInfo.Text = "발견되지않음";
                else
                    SetNetSupportPath(process[0].MainModule.FileName);
            }

            // Update progress
            progressBar1.Value += 50;
        }



        // Update path and display in the label
        public void SetNetSupportPath(string path)
        {
            netSupportPath = Path.GetDirectoryName(path);
            netSupportInfo.Text = "발견됨! 주소: " + netSupportPath;
        }

        public void SetClassMPath(string path)
        {
            classMPath = Path.GetDirectoryName(path);
            classMinfo.Text = "발견됨! 주소: " + classMPath;
        }

        private void FastSc_Click(object sender, EventArgs e)
        {
            // Start fast scan
            progressBar1.Value = 0;
            ClassMScan();
            NetSupportScan();
        }


        private void AllSc_Click(object sender, EventArgs e)
        {
            AllScanClassM();
            AllScanNetSupportSchool();
        }
    }
}
