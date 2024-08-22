
using System.Diagnostics;
using System.IO;
using System.Security.Claims;


namespace ClassFucker
{
    public partial class Form1 : Form
    {


        private string classMPath;
        private string netSupportPath;

        public Form1()
        {

            InitializeComponent();
            // �⺻ ��ĵ ���� (�񵿱� ó�� ���)
            ClassMScan();
            NetSupportScan();
        }


        ////// ���� ��ĵ �Լ��� //////
        public void ClassMScan()
        {
            classMPath = null;
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
            netSupportPath = null;
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

        ////// ��ü ��ĵ ���� �Լ��� //////
        public async Task AllClassMScan()
        {
            classMPath = null;
            string result = await DFSFolderFind("ClassM", progressBar1);
            if (result == null)
                classMinfo.Text = "�߰ߵ�������";
            else
                SetClassMPath(Path.Combine(result, "hscagent.exe"));
        }

        public async Task AllNetSupportScanAsync()
        {
            netSupportPath = null;
            string result = await DFSFolderFind("NetSupport School", progressBar1);
            if (result == null)
                netSupportInfo.Text = "�߰ߵ�������";
            else
                SetNetSupportPath(Path.Combine(result, "client32.exe"));
        }

        // �ʺ� �켱 Ž������ ���� ã��
        static async Task<string> DFSFolderFind(string targetFolderName, ProgressBar progressBar)
        {
            Queue<string> directoriesToSearch = new Queue<string>();
            int totalDirectories = 0;
            int processedDirectories = 0;

            // ��� ����̺��� ��Ʈ ���丮�� ť�� �߰�
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    directoriesToSearch.Enqueue(drive.RootDirectory.FullName);
                    totalDirectories++;
                }
            }

            while (directoriesToSearch.Count > 0)
            {
                string currentDirectory = directoriesToSearch.Dequeue();
                processedDirectories++;
                progressBar.Value = (int)((double)processedDirectories / totalDirectories * 100);


                try
                {
                    // �񵿱������� ���� ���丮 Ž��
                    string[] directories = await Task.Run(() => Directory.GetDirectories(currentDirectory));

                    foreach (string directory in directories)
                    {
                        if (Path.GetFileName(directory).Equals(targetFolderName, StringComparison.OrdinalIgnoreCase))
                        {
                            return directory;
                        }
                        directoriesToSearch.Enqueue(directory);
                        totalDirectories++;
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    continue;
                }
            }

            return null;
        }


        ////// ó�� �Լ��� //////
        public async Task isolation() //�ݸ�
        {
            progressBar1.Value = 0;

            //ClassM �κ�
            if (!string.IsNullOrEmpty(classMPath))
            {
                ProcessKill("ClassM_Client");
                ProcessKill("ClassM_Client_Service");
                ProcessKill("mvnc");
                ProcessKill("SysCtrl");
                ProcessKill("hscagent");
                await XCopy(classMPath, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClassM"));
                File.WriteAllText(Path.Combine(classMPath, "ClassMIsolation.txt"), "�̰��� ClassM�� ���ŉ�ٴ� ���� �����մϴ�. �������� �����ּ���.");
            }
            else if (File.Exists(Path.Combine(classMPath, "ClassMIsolation.txt")))
                MessageBox.Show("ClassM�� �ݸ��� ������ �߰ߵǾ� �ݸ��� ��ҵǾ����ϴ�.");

            progressBar1.Value = 50;

            //NetSupport�κ�
            if (!string.IsNullOrEmpty(netSupportPath))
            {
                ProcessKill("client32");
                ProcessKill("StudentUI");
                ProcessKill("NSToast");
                ProcessKill("ClassicStartMenu");
                ProcessKill("nspowershell");
                ProcessKill("NSClientTB");
                await XCopy(classMPath, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NetSupport"));
                File.WriteAllText(Path.Combine(netSupportPath, "NetSupportPathIsolation.txt"), "�̰��� NetSupportPath�� ���ŉ�ٴ� ���� �����մϴ�. �������� �����ּ���.");
            }
            else if (File.Exists(Path.Combine(netSupportPath, "NetSupportPathIsolation.txt")))
                MessageBox.Show("NetSupport�� �ݸ��� ������ �߰ߵǾ� �ݸ��� ��ҵǾ����ϴ�.");

            progressBar1.Value = 100;

            MessageBox.Show("�ݸ��� �Ϸ�Ǿ����ϴ�.");
        }
        public async Task Restoration()
        {
            progressBar1.Value = 0;
            if (!File.Exists(Path.Combine(classMPath, "ClassMIsolation.txt")))
                MessageBox.Show("ClassM�� �ݸ��� ������ �߰ߵ����ʾ� ������ ��ҵǾ����ϴ�.");
            else
            {
                await XCopy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClassM"), classMPath);
                ProcessStart(Path.Combine(classMPath, "ClassM_Client.exe"));
                ProcessStart(Path.Combine(classMPath, "ClassM_Client_Service.exe"));
                ProcessStart(Path.Combine(classMPath, "mvnc.exe"));
                ProcessStart(Path.Combine(classMPath, "SysCtrl.exe"));
                ProcessStart(Path.Combine(classMPath, "hscagent.exe"));
                File.Delete(Path.Combine(classMPath, "ClassMIsolation.txt"));
            }

            progressBar1.Value = 50;

            //NetSupport�κ�
            if (!File.Exists(Path.Combine(netSupportPath, "NetSupportPathIsolation.txt")))
                MessageBox.Show("NetSupport�� �ݸ��� ������ �߰ߵ����ʾ� ������ ��ҵǾ����ϴ�.");
            else
            {
                await XCopy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NetSupport"), netSupportPath);
                ProcessStart(Path.Combine(netSupportPath, "client32.exe"));
                ProcessStart(Path.Combine(netSupportPath, "StudentUI.exe"));
                ProcessStart(Path.Combine(netSupportPath, "NSToast.exe"));
                ProcessStart(Path.Combine(netSupportPath, "ClassicStartMenu.exe"));
                ProcessStart(Path.Combine(netSupportPath, "nspowershell.exe"));
                ProcessStart(Path.Combine(netSupportPath, "NSClientTB.exe"));
                File.Delete(Path.Combine(netSupportPath, "NetSupportPathIsolation.txt"));
            }
            progressBar1.Value = 100;

            MessageBox.Show("������ �Ϸ�Ǿ����ϴ�.");

        }

        public void ProcessKill(string name)
        {
            try
            {
                Process[] processes = Process.GetProcessesByName(name);

                foreach (Process process in processes)
                {
                    process.Kill();
                }

                if (processes.Length == 0)
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public void ProcessStart(string name)
        {

            if (File.Exists(name))
            {
                // ProcessStartInfo ��ü�� �����Ͽ� ������ ���ϰ� ���õ� ������ �����մϴ�.
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = name,
                    UseShellExecute = true // ���� ���� ������ �����մϴ�.
                };
            }

        }


        /*
        public static async Task CopyDirectoryAsync(string sourceFolder, string destFolder, IProgress<int> progress = null)
        {

            // ��� ������ �������� ������ ����
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            // �ҽ� �������� ���ϰ� ���͸� ����� ������
            string[] files = Directory.GetFiles(sourceFolder);
            string[] folders = Directory.GetDirectories(sourceFolder);

            int totalItems = files.Length + folders.Length;
            int completedItems = 0;

            // ������ �񵿱������� ����
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                using (FileStream sourceStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                using (FileStream destStream = new FileStream(dest, FileMode.Create, FileAccess.Write))
                {
                    await sourceStream.CopyToAsync(destStream);
                }
                completedItems++;
                progress?.Report((int)((double)completedItems / totalItems * 100));
            }

            // ������ ��������� ���� (�񵿱�)
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                await CopyDirectoryAsync(folder, dest, progress);
                completedItems++;
                progress?.Report((int)((double)completedItems / totalItems * 100));
            }
        }
        */
        static async Task XCopy(string sourcePath, string destinationPath)
        {

            // �񵿱� �۾��� ���� Task �迭
            Task[] tasks = new Task[1];

            tasks[0] = Task.Run(async () =>
            {
                // xcopy ��ɾ �����մϴ�.
                string command = $"/c robocopy /mir \"{sourcePath}\" \"{destinationPath}\"";

                var processInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = command,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = false
                };

                // �α� ���� �̸� ����
                string logFileName = $"{Environment.MachineName}_{DateTime.Now:yyyyMMdd_HHmmss}_copy.log";

                // Process ���� �� ����
                using (var process = Process.Start(processInfo))
                using (var reader = process.StandardOutput)
                using (var errorReader = process.StandardError)
                {
                    // ��ɾ��� ����� �񵿱������� �н��ϴ�.
                    var output = await reader.ReadToEndAsync();
                    var errorOutput = await errorReader.ReadToEndAsync();
                }
            });

            // ��� Task �Ϸ� ���
            await Task.WhenAll(tasks);
        }

        ////// ���, �� ���� �Լ� //////
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




        ////// �̺�Ʈ �Լ��� //////
        private void FastSc_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            ClassMScan();
            NetSupportScan();
        }

        private async void AllSc_Click(object sender, EventArgs e)
        {
            netSupportInfo.Text = "���ɰ� �ð��� �Ҹ�˴ϴ�.";
            classMinfo.Text = "Ž����...";
            progressBar1.Value = 0;
            await Task.WhenAll(AllClassMScan(), AllNetSupportScanAsync());
            progressBar1.Value = 100;
        }

        private void _isolation_Click(object sender, EventArgs e)
        {
            isolation();
        }

        private void _Restoration_Click(object sender, EventArgs e)
        {
            Restoration();
        }
    }
}
