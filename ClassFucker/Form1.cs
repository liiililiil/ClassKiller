
using Microsoft.VisualBasic;
using System.Diagnostics;


namespace ClassFucker
{
    public partial class Form1 : Form
    {
        private static bool callstop = false;
        private string classMPath;
        private string netSupportPath;
        private bool tryrestore;

        public Form1()
        {

            InitializeComponent();
            loglabel.ReadOnly = true;
            FastScan();
        }


        ////// ���� ��ĵ �Լ��� //////
        public void FastScan()
        {
            callstop = true;
            classMPath = null;
            loglabel.Text = $"ClassM ���� Ž����.. " + Environment.NewLine;
            if (Directory.Exists(@"C:\Program Files (x86)\Innosoft\ClassM Client")) // �˷��� �ּ� Ȯ��
                SetClassMPath(@"C:\Program Files (x86)\Innosoft\ClassM Client\classM_Client.exe");

            else if (Directory.Exists(@"C:\Program Files (x86)\ClassM")) // �˷��� �ּ� Ȯ��2
                SetClassMPath(@"C:\Program Files (x86)\ClassM\hscagent.exe");
            else
            {
                // ���μ����� �̿��� Ž��
                Process[] process = Process.GetProcessesByName("hscagent");

                if (process.Length == 0)
                {
                    // �ٸ� ���μ��� �̸��� �̿��� Ž��
                    process = Process.GetProcessesByName("ClassM_Client");

                    if (process.Length == 0)
                        classMinfo.Text = "�߰ߵ�������";
                    else
                        SetClassMPath(process[0].MainModule.FileName);
                }
                else
                    SetClassMPath(process[0].MainModule.FileName);


            }

            progressBar1.Value += 50;

            netSupportPath = null;
            loglabel.Text += $"NetSupport ���� Ž����.. " + Environment.NewLine;

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
            loglabel.Text += $"���� Ž�� �Ϸ��" + Environment.NewLine;
        }

        ////// ��ü ��ĵ ���� �Լ��� //////
        public async Task AllScan()
        {
            classMPath = null;
            string result = await DFSFolderFind(["ClassM", "ClassM Client"], progressBar1, loglabel);
            if (result == null)
                classMinfo.Text = "�߰ߵ�������";
            else
                SetClassMPath(Path.Combine(result, "hscagent.exe"));

            netSupportPath = null;
            result = await DFSFolderFind(["NetSupport School"], progressBar1, loglabel);
            if (result == null)
                netSupportInfo.Text = "�߰ߵ�������";
            else
                SetNetSupportPath(Path.Combine(result, "client32.exe"));
            loglabel.Text += "��ü Ž�� �Ϸ��" + Environment.NewLine;
        }

        // �ʺ� �켱 Ž������ ���� ã��
        static async Task<string> DFSFolderFind(string[] targetFolderName, ProgressBar progressBar, TextBox loglabel)
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
                loglabel.Text = $"{targetFolderName[0]}�� ã���� : {currentDirectory}";

                if (callstop)
                {
                    callstop = false;
                    loglabel.Text += "����Ž�� �������� ��ü Ž���� �ߴܵ�" + Environment.NewLine;
                    return null;
                }


                try
                {
                    // �񵿱������� ���� ���丮 Ž��
                    string[] directories = await Task.Run(() => Directory.GetDirectories(currentDirectory));

                    foreach (string directory in directories)
                    {
                        foreach (string target in targetFolderName)
                        {
                            if (Path.GetFileName(directory).Equals(target, StringComparison.OrdinalIgnoreCase))
                            {
                                loglabel.Text += $"{targetFolderName[0]} �� �߰ߵ�! {directory}";
                                return directory;
                            }
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
                loglabel.Text += $"ClassM �۾� �Ϸ� " + Environment.NewLine;
            }
            else if (!string.IsNullOrEmpty(classMPath) && File.Exists(Path.Combine(classMPath, "ClassMIsolation.txt")))
            {
                loglabel.Text += $"ClassM�� �ݸ��� ������ �߰ߵ� �ǳʶ� " + Environment.NewLine;
            }

            else
            {
                loglabel.Text += $"ClassM�� ���� �ǳʶ� " + Environment.NewLine;
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
                loglabel.Text += $"NetSupport �۾� �Ϸ� " + Environment.NewLine;
            }
            else if (!string.IsNullOrEmpty(netSupportPath) && File.Exists(Path.Combine(netSupportPath, "NetSupportPathIsolation.txt")))
                loglabel.Text += $"NetSupport�� �ݸ��� ������ �߰ߵ� �ǳʶ� " + Environment.NewLine;
            else
                loglabel.Text += $"NetSupport�� ���� �ǳʶ� " + Environment.NewLine;

            progressBar1.Value = 100;

            loglabel.Text += "�۾��� �Ϸ��\n";
        }
        public async Task Restoration()
        {
            loglabel.Text = "";
            progressBar1.Value = 0;
            if (string.IsNullOrEmpty(classMPath))
                loglabel.Text += "ClassM�� ���� �ǳʶ� " + Environment.NewLine;
            else if (!File.Exists(Path.Combine(classMPath, "ClassMIsolation.txt")))
                loglabel.Text += "ClassM�� �ݸ��� ������ �߰ߵ��� �ʾ� �ǳʶ� " + Environment.NewLine;
            else
            {
                await XCopy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClassM"), classMPath, loglabel);
                ProcessStart(Path.Combine(classMPath, "ClassM_Client.exe"));
                ProcessStart(Path.Combine(classMPath, "ClassM_Client_Service.exe"));
                ProcessStart(Path.Combine(classMPath, "mvnc.exe"));
                ProcessStart(Path.Combine(classMPath, "SysCtrl.exe"));
                ProcessStart(Path.Combine(classMPath, "hscagent.exe"));
                File.Delete(Path.Combine(classMPath, "ClassMIsolation.txt"));
                loglabel.Text += $"ClassM �۾� �Ϸ� " + Environment.NewLine;
            }

            progressBar1.Value = 50;

            //NetSupport�κ�
            if (string.IsNullOrEmpty(netSupportPath))
                loglabel.Text += $"NetSupport�� ���� �ǳʶ� " + Environment.NewLine;
            else if (!File.Exists(Path.Combine(netSupportPath, "NetSupportPathIsolation.txt")))
                loglabel.Text += $"NetSupport�� �ݸ��� ������ �߰ߵ��� �ʾ� �ǳʶ� " + Environment.NewLine;
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
        public void SilenceProcessKill(string name)
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
                Process.Start(name);
                loglabel.Text += $"{name}(��)�� ������ " + Environment.NewLine;
            }
            else
            {
                loglabel.Text += $"{name} (��)�� ����������� " + Environment.NewLine;
            }

        }


        static async Task XCopy(string sourcePath, string destinationPath, TextBox loglabel)
        {
            Task task = Task.Run(() => CopyDirectory(sourcePath, destinationPath, loglabel));

            // ��� Task �Ϸ� ���
            await Task.WhenAll(task);
        }
        static async Task DeleteDirectoryAsync(string directoryPath, TextBox loglabel)
        {
            if (!Directory.Exists(directoryPath))
            {
                loglabel.Text += "���丮�� �������� �ʽ��ϴ�. " + Environment.NewLine;
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
                        loglabel.Invoke((Action)(() => loglabel.Text += $"���� ������: {file} " + Environment.NewLine));
                    }
                    catch (UnauthorizedAccessException)
                    {
                        loglabel.Invoke((Action)(() => loglabel.Text += $"���� ���� �ź�: {file} " + Environment.NewLine));
                    }
                    catch (IOException ex)
                    {
                        loglabel.Invoke((Action)(() => loglabel.Text += $"���� ���� ���� {file}: {ex.Message} " + Environment.NewLine));
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
                        loglabel.Invoke((Action)(() => loglabel.Text += $"���丮 ������: {subdirectory} " + Environment.NewLine));
                    }
                    catch (UnauthorizedAccessException)
                    {
                        loglabel.Invoke((Action)(() => loglabel.Text += $"���丮 ���� �ź�: {subdirectory} " + Environment.NewLine));
                    }
                    catch (IOException ex)
                    {
                        loglabel.Invoke((Action)(() => loglabel.Text += $"���丮 ���� ���� {subdirectory}: {ex.Message} " + Environment.NewLine));
                    }
                });
            }

            // �ֻ��� ���丮 ���� �۾��� �񵿱������� ����
            await Task.Run(() =>
            {
                try
                {
                    Directory.Delete(directoryPath);
                    loglabel.Invoke((Action)(() => loglabel.Text += $"���丮 ������: {directoryPath} " + Environment.NewLine));
                }
                catch (UnauthorizedAccessException)
                {
                    loglabel.Invoke((Action)(() => loglabel.Text += $"���丮 ���� �ź�: {directoryPath} " + Environment.NewLine));
                }
                catch (IOException ex)
                {
                    loglabel.Invoke((Action)(() => loglabel.Text += $"���丮 ���� ���� {directoryPath}: {ex.Message}" + Environment.NewLine));
                }
            });
        }

        static async Task CopyDirectory(string sourceDir, string destDir, TextBox loglabel)
        {
            // �ҽ��� ������ ���丮�� �������� �ʴ� ��� ���� �߻�
            if (!Directory.Exists(sourceDir))
            {
                loglabel.Invoke((Action)(() => loglabel.Text += $"Fatal Error : {sourceDir} ������ �����ϴ�. �����۾��� ��ҵǾ����ϴ�! "+ Environment.NewLine));
                return;
            }

            // ������ ���丮 ����
            if (!Directory.Exists(destDir))
            {
                loglabel.Invoke((Action)(() => loglabel.Text += $"������ ��ġ {destDir}�� ���� ���丮�� �����Ǿ����ϴ�. " + Environment.NewLine));
                Directory.CreateDirectory(destDir);
            }

            // ���� ����
            var fileTasks = Directory.GetFiles(sourceDir).Select(async filePath =>
            {
                string destFilePath = Path.Combine(destDir, Path.GetFileName(filePath));
                try
                {
                    using (var sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (var destStream = new FileStream(destFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await sourceStream.CopyToAsync(destStream);
                    }
                }
                catch (Exception ex)
                {
                    loglabel.Invoke((Action)(() => loglabel.Text += $"���� ���� ����: {ex.Message} " + Environment.NewLine));
                }
            });

            await Task.WhenAll(fileTasks);

            // ���� ���丮 ����
            var directoryTasks = Directory.GetDirectories(sourceDir).Select(async dirPath =>
            {
                string destDirPath = Path.Combine(destDir, Path.GetFileName(dirPath));
                await CopyDirectory(dirPath, destDirPath, loglabel);
            });

            await Task.WhenAll(directoryTasks);
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

        private async void timer1_Tick(object sender, EventArgs e)
        {
            if (tryrestore == false && checkBox1.Checked == true && !Process.GetProcessesByName("explorer").Any())
            {
                tryrestore = true;
                await AllScan();
                await isolation();
                ProcessStart("Explorer");
                MessageBox.Show("���� �õ��� �Ϸ��Ͽ����ϴ�.");
                tryrestore = false;

            }

            if(checkBox2.Checked == true)
            {
                try
                {
                    SilenceProcessKill("ClassM_Client");
                    SilenceProcessKill("ClassM_Client_Service");
                    SilenceProcessKill("mvnc");
                    SilenceProcessKill("SysCtrl");
                    SilenceProcessKill("hscagent");
                    SilenceProcessKill("client32");
                    SilenceProcessKill("StudentUI");
                    SilenceProcessKill("NSToast");
                    SilenceProcessKill("ClassicStartMenu");
                    SilenceProcessKill("nspowershell");
                    SilenceProcessKill("NSClientTB");
                }
                catch
                {
                    
                }
            }
        }

    }
}
