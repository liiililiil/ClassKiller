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
            ClassMScan();
            NetSupportScan();
        }

        // 빠른 스캔 함수
        public void ClassMScan()
        {
            if (Directory.Exists(@"C:\Program Files (x86)\ClassM")) // 알려진 주소 확인
                SetClassMPath(@"C:\Program Files (x86)\ClassM\hscagent.exe");
            else
            {
                // 프로세스를 이용해 탐색
                Process[] process = Process.GetProcessesByName("hscagent");

                if (process.Length == 0)
                    classMinfo.Text = "발견되지않음";
                else
                    SetClassMPath(process[0].MainModule.FileName);
            }

            progressBar1.Value += 50;
        }

        public void NetSupportScan()
        {
            if (Directory.Exists(@"C:\Program Files (x86)\NetSupport\NetSupport School")) // 알려진 주소 확인
                SetNetSupportPath(@"C:\Program Files (x86)\NetSupport\NetSupport School\client32.exe");
            else
            {
                // 프로세스를 이용해 탐색
                Process[] process = Process.GetProcessesByName("client32");

                if (process.Length == 0)
                    netSupportInfo.Text = "발견되지않음";
                else
                    SetNetSupportPath(process[0].MainModule.FileName);
            }

            progressBar1.Value += 50;
        }


        // 경로, 라벨 수정 함수 //
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
            progressBar1.Value = 0;
            ClassMScan();
            NetSupportScan();
        }
    }
}
