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

        // ���� ��ĵ �Լ�
        public void ClassMScan()
        {
            if (Directory.Exists(@"C:\Program Files (x86)\ClassM")) // �˷��� �ּ� Ȯ��
                SetClassMPath(@"C:\Program Files (x86)\ClassM\hscagent.exe");
            else
            {
                // ���μ����� �̿��� Ž��
                Process[] process = Process.GetProcessesByName("hscagent");

                if (process.Length == 0)
                    classMinfo.Text = "�߰ߵ�������";
                else
                    SetClassMPath(process[0].MainModule.FileName);
            }

            progressBar1.Value += 50;
        }

        public void NetSupportScan()
        {
            if (Directory.Exists(@"C:\Program Files (x86)\NetSupport\NetSupport School")) // �˷��� �ּ� Ȯ��
                SetNetSupportPath(@"C:\Program Files (x86)\NetSupport\NetSupport School\client32.exe");
            else
            {
                // ���μ����� �̿��� Ž��
                Process[] process = Process.GetProcessesByName("client32");

                if (process.Length == 0)
                    netSupportInfo.Text = "�߰ߵ�������";
                else
                    SetNetSupportPath(process[0].MainModule.FileName);
            }

            progressBar1.Value += 50;
        }


        // ���, �� ���� �Լ� //
        public void SetNetSupportPath(string path)
        {
            netSupportPath = Path.GetDirectoryName(path);
            netSupportInfo.Text = "�߰ߵ�! �ּ�: " + netSupportPath;
        }

        public void SetClassMPath(string path)
        {
            classMPath = Path.GetDirectoryName(path);
            classMinfo.Text = "�߰ߵ�! �ּ�: " + classMPath;
        }

        private void FastSc_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            ClassMScan();
            NetSupportScan();
        }
    }
}
