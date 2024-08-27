
using System.Diagnostics;
using System.IO;
using System.Security.Claims;


namespace ClassFucker
{
    public partial class Form1 : Form
    {

        private static bool callstop = false;
        private string classMPath;
        private string netSupportPath;

        public Form1()
        {

            InitializeComponent();
            FastScan();
        }


        ////// ���� ��ĵ �Լ��� //////
        public void FastScan()
        {
            callstop = true;
            classMPath = null;
            loglabel.Text = "ClassM ���� Ž����..";
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

            netSupportPath = null;
            loglabel.Text = "NetSupport ���� Ž����..";
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
            loglabel.Text = "���� Ž�� �Ϸ��\n";
        }

        ////// ��ü ��ĵ ���� �Լ��� //////
        public async Task AllScan()
        {
            classMPath = null;
            string result = await DFSFolderFind("ClassM", progressBar1, loglabel);
            if (result == null)
                classMinfo.Text = "�߰ߵ�������";
            else
                SetClassMPath(Path.Combine(result, "hscagent.exe"));

            netSupportPath = null;
            result = await DFSFolderFind("NetSupport School", progressBar1, loglabel);
            if (result == null)
                netSupportInfo.Text = "�߰ߵ�������";
            else
                SetNetSupportPath(Path.Combine(result, "client32.exe"));
            loglabel.Text = "��ü Ž�� �Ϸ��\n";
        }

        // �ʺ� �켱 Ž������ ���� ã��
        static async Task<string> DFSFolderFind(string targetFolderName, ProgressBar progressBar, Label loglabel)
        {
            callstop = false;
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
                loglabel.Text = $"{targetFolderName}�� ã���� : {currentDirectory}";

                if (callstop)
                {
                    callstop = false;
                    loglabel.Text += "����Ž�� �������� ��ü Ž���� �ߴܵ�";
                    return null;
                }


                try
                {
                    // �񵿱������� ���� ���丮 Ž��
                    string[] directories = await Task.Run(() => Directory.GetDirectories(currentDirectory));

                    foreach (string directory in directories)
                    {
                        if (Path.GetFileName(directory).Equals(targetFolderName, StringComparison.OrdinalIgnoreCase))
                        {
                            loglabel.Text += $"{targetFolderName} �� �߰ߵ�! {directory}";
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
            loglabel.Text = "";
            progressBar1.Value = 0;

            //ClassM �κ�
            if (!string.IsNullOrEmpty(classMPath) && !File.Exists(Path.Combine(classMPath, "ClassMIsolation.txt")))
            {
                ProcessKill("ClassM_Client");
                ProcessKill("ClassM_Client_Service");
                ProcessKill("mvnc");
                ProcessKill("SysCtrl");
                ProcessKill("hscagent");
                await XCopy(classMPath, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClassM"), loglabel);
                DeleteDirectoryAsync(classMPath, loglabel);
                File.WriteAllText(Path.Combine(classMPath, "ClassMIsolation.txt"), "�̰��� ClassM�� ���ŉ�ٴ� ���� �����մϴ�. �������� �����ּ���.");
                loglabel.Text += $"ClassM �۾� �Ϸ� \n";
            }
            else if (!string.IsNullOrEmpty(classMPath) && File.Exists(Path.Combine(classMPath, "ClassMIsolation.txt")))
            {
                loglabel.Text += $"ClassM�� �ݸ��� ������ �߰ߵ� �ǳʶ� \n";
            }

            else
            {
                loglabel.Text += $"ClassM�� ���� �ǳʶ� \n";
            }


            progressBar1.Value = 50;

            //NetSupport�κ�
            if (!string.IsNullOrEmpty(netSupportPath) && !File.Exists(Path.Combine(netSupportPath, "NetSupportPathIsolation.txt")))
            {
                ProcessKill("client32");
                ProcessKill("StudentUI");
                ProcessKill("NSToast");
                ProcessKill("ClassicStartMenu");
                ProcessKill("nspowershell");
                ProcessKill("NSClientTB");
                await XCopy(netSupportPath, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NetSupport"), loglabel);
                DeleteDirectoryAsync(netSupportPath, loglabel);
                File.WriteAllText(Path.Combine(netSupportPath, "NetSupportPathIsolation.txt"), "�̰��� NetSupportPath�� ���ŉ�ٴ� ���� �����մϴ�. �������� �����ּ���.");
                loglabel.Text += $"NetSupport �۾� �Ϸ� \n";
            }
            else if (!string.IsNullOrEmpty(netSupportPath) && File.Exists(Path.Combine(netSupportPath, "NetSupportPathIsolation.txt")))
                loglabel.Text += $"NetSupport�� �ݸ��� ������ �߰ߵ� �ǳʶ� \n";
            else
                loglabel.Text += $"NetSupport�� ���� �ǳʶ� \n";

            progressBar1.Value = 100;

            loglabel.Text += "�۾��� �Ϸ��\n";
        }
        public async Task Restoration()
        {
            loglabel.Text = "";
            progressBar1.Value = 0;
            if (string.IsNullOrEmpty(classMPath))
                loglabel.Text += "ClassM�� ���� �ǳʶ� \n";
            else if (!File.Exists(Path.Combine(classMPath, "ClassMIsolation.txt")))
                loglabel.Text += "ClassM�� �ݸ��� ������ �߰ߵ��� �ʾ� �ǳʶ� \n";
            else
            {
                await XCopy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClassM"), classMPath, loglabel);
                ProcessStart(Path.Combine(classMPath, "ClassM_Client.exe"));
                ProcessStart(Path.Combine(classMPath, "ClassM_Client_Service.exe"));
                ProcessStart(Path.Combine(classMPath, "mvnc.exe"));
                ProcessStart(Path.Combine(classMPath, "SysCtrl.exe"));
                ProcessStart(Path.Combine(classMPath, "hscagent.exe"));
                File.Delete(Path.Combine(classMPath, "ClassMIsolation.txt"));
                loglabel.Text += $"ClassM �۾� �Ϸ� \n";
            }

            progressBar1.Value = 50;

            //NetSupport�κ�
            if (string.IsNullOrEmpty(netSupportPath))
                loglabel.Text += $"NetSupport�� ���� �ǳʶ� \n";
            else if (!File.Exists(Path.Combine(netSupportPath, "NetSupportPathIsolation.txt")))
                loglabel.Text += $"NetSupport�� �ݸ��� ������ �߰ߵ��� �ʾ� �ǳʶ� \n";
            else
            {
                await XCopy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NetSupport"), netSupportPath, loglabel);
                ProcessStart(Path.Combine(netSupportPath, "client32.exe"));
                ProcessStart(Path.Combine(netSupportPath, "StudentUI.exe"));
                ProcessStart(Path.Combine(netSupportPath, "NSToast.exe"));
                ProcessStart(Path.Combine(netSupportPath, "ClassicStartMenu.exe"));
                ProcessStart(Path.Combine(netSupportPath, "nspowershell.exe"));
                ProcessStart(Path.Combine(netSupportPath, "NSClientTB.exe"));
                File.Delete(Path.Combine(netSupportPath, "NetSupportPathIsolation.txt"));
                loglabel.Text += $"NetSupport �۾� �Ϸ� \n";
            }
            progressBar1.Value = 100;

            loglabel.Text += "�۾� �Ϸ��\n";

        }

        public void ProcessKill(string name)
        {
            try
            {
                Process[] processes = Process.GetProcessesByName(name);

                foreach (Process process in processes)
                {
                    loglabel.Text += $"{name} (��)�� ����� \n";
                    process.Kill();

                }

                if (processes.Length == 0)
                {
                    loglabel.Text += $"{name} (��)�� ����������� \n";
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
                Process.Start(name);
                loglabel.Text += $"{name}(��)�� ������ \n";
            } else {
                loglabel.Text += $"{name} (��)�� ����������� \n";
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

        /*
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
        */

        static async Task XCopy(string sourcePath, string destinationPath,Label loglabel)
        {
            Task task = Task.Run(() => CopyDirectory(sourcePath, destinationPath, loglabel));

            // ��� Task �Ϸ� ���
            await Task.WhenAll(task);
        }
        static async Task DeleteDirectoryAsync(string directoryPath, Label loglabel)
        {
            if (!Directory.Exists(directoryPath))
            {
                loglabel.Text += "���丮�� �������� �ʽ��ϴ�. \n";
                return;
            }

            // ���� ���� �۾��� �񵿱������� ����
            foreach (string file in Directory.GetFiles(directoryPath))
            {
                await Task.Run(() =>
                {
                    try
                    {
                        File.Delete(file);
                        loglabel.Invoke((Action)(() => loglabel.Text += $"���� ������: {file} \n"));
                    }
                    catch (UnauthorizedAccessException)
                    {
                        loglabel.Invoke((Action)(() => loglabel.Text += $"���� ���� �ź�: {file} \n"));
                    }
                    catch (IOException ex)
                    {
                        loglabel.Invoke((Action)(() => loglabel.Text += $"���� ���� ���� {file}: {ex.Message} \n"));
                    }
                });
            }

            // ���� ���丮 ���� �۾��� �񵿱������� ����
            foreach (string subdirectory in Directory.GetDirectories(directoryPath))
            {
                await Task.Run(async () =>
                {
                    try
                    {
                        await DeleteDirectoryAsync(subdirectory, loglabel); // ���� ���丮 �񵿱� ȣ��
                        Directory.Delete(subdirectory);
                        loglabel.Invoke((Action)(() => loglabel.Text += $"���丮 ������: {subdirectory} \n"));
                    }
                    catch (UnauthorizedAccessException)
                    {
                        loglabel.Invoke((Action)(() => loglabel.Text += $"���丮 ���� �ź�: {subdirectory} \n"));
                    }
                    catch (IOException ex)
                    {
                        loglabel.Invoke((Action)(() => loglabel.Text += $"���丮 ���� ���� {subdirectory}: {ex.Message} \n"));
                    }
                });
            }

            // �ֻ��� ���丮 ���� �۾��� �񵿱������� ����
            await Task.Run(() =>
            {
                try
                {
                    Directory.Delete(directoryPath);
                    loglabel.Invoke((Action)(() => loglabel.Text += $"���丮 ������: {directoryPath} \n"));
                }
                catch (UnauthorizedAccessException)
                {
                    loglabel.Invoke((Action)(() => loglabel.Text += $"���丮 ���� �ź�: {directoryPath} \n"));
                }
                catch (IOException ex)
                {
                    loglabel.Invoke((Action)(() => loglabel.Text += $"���丮 ���� ���� {directoryPath}: {ex.Message} \n"));
                }
            });
        }
        static void CopyDirectory(string sourceDir, string destDir,Label loglabel)
        {
            // �ҽ��� ������ ���丮�� �������� �ʴ� ��� ���� �߻�
            if (!Directory.Exists(sourceDir))
            {
                loglabel.Text += $"Fatal Error : {sourceDir}������ �����ϴ�. �����۾��� ��ҵǾ����ϴ�! \n";
                return;
            }

            // ������ ���丮 ����
            if (!Directory.Exists(destDir))
            {
                loglabel.Text += $"������ ��ġ {destDir}�� ���� ������ �����Ǿ����ϴ�. \n";
                Directory.CreateDirectory(destDir);
            }

            // ���� ����
            foreach (var filePath in Directory.GetFiles(sourceDir))
            {
                string destFilePath = Path.Combine(destDir, Path.GetFileName(filePath));
                try
                {
                    File.Copy(filePath, destFilePath, true);
                }
                catch
                {
                    
                }
            }

            // ���� ���丮 ����
            foreach (var dirPath in Directory.GetDirectories(sourceDir))
            {
                string destDirPath = Path.Combine(destDir, Path.GetFileName(dirPath));
                CopyDirectory(dirPath, destDirPath,loglabel);
            }
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

        private async void AllSc_Click(object sender, EventArgs e)
        {
            netSupportInfo.Text = "���ɰ� �ð��� �Ҹ�˴ϴ�.";
            classMinfo.Text = "Ž����...";
            progressBar1.Value = 0;
            await Task.WhenAll(AllScan());
            progressBar1.Value = 100;
        }

        private async void _isolation_Click(object sender, EventArgs e)
        {
            await isolation();
        }

        private void _Restoration_Click(object sender, EventArgs e)
        {
            Restoration();
        }

        private void FastSc_Click_1(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            FastScan();

        }
    }
}
