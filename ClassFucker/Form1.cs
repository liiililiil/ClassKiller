
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Xml.Linq;


namespace ClassFucker
{
    public partial class Form1 : Form
    {
        private static bool callstop = false;
        private string classMPath;
        private string netSupportPath;
        private bool tryrestore;
        private CancellationTokenSource cts;

        public Form1()
        {

            InitializeComponent();
            loglabel.ReadOnly = true;
            FastScan();
        }


        ////// 빠른 스캔 함수들 //////
        public void FastScan()
        {
            cts?.Cancel();
            classMPath = null;
            loglabel.Text = $"ClassM 빠른 탐색중.. " + Environment.NewLine;
            if (Directory.Exists(@"C:\Program Files (x86)\Innosoft\ClassM Client")) // 알려진 주소 확인
                SetClassMPath(@"C:\Program Files (x86)\Innosoft\ClassM Client\classM_Client.exe");

            else if (Directory.Exists(@"C:\Program Files (x86)\ClassM")) // 알려진 주소 확인2
                SetClassMPath(@"C:\Program Files (x86)\ClassM\hscagent.exe");
            else
            {
                // 프로세스를 이용해 탐색
                Process[] process = Process.GetProcessesByName("hscagent");

                if (process.Length == 0)
                {
                    // 다른 프로세스 이름을 이용해 탐색
                    process = Process.GetProcessesByName("ClassM_Client");

                    if (process.Length == 0)
                        classMinfo.Text = "발견되지않음";
                    else
                        SetClassMPath(process[0].MainModule.FileName);
                }
                else
                    SetClassMPath(process[0].MainModule.FileName);


            }

            progressBar1.Value += 50;

            netSupportPath = null;
            loglabel.Text += $"NetSupport 빠른 탐색중.. " + Environment.NewLine;

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
            loglabel.Text += $"빠른 탐색 완료됨" + Environment.NewLine;
        }

        ////// 전체 스캔 관련 함수들 //////
        public async Task AllScan()
        {
            {
                cts?.Cancel(); // 기존에 존재하는 경우 취소
                cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;

                // 클래스 M 폴더 탐색
                classMPath = null;
                string result = await DFSFolderFind(new[] { "ClassM", "ClassM Client" }, progressBar1, loglabel, token);
                if (result == null)
                    classMinfo.Text = "발견되지않음";
                else
                    SetClassMPath(Path.Combine(result, "hscagent.exe"));

                // NetSupport School 폴더 탐색
                netSupportPath = null;
                result = await DFSFolderFind(new[] { "NetSupport School" }, progressBar1, loglabel, token);
                if (result == null)
                    netSupportInfo.Text = "발견되지않음";
                else
                    SetNetSupportPath(Path.Combine(result, "client32.exe"));

                loglabel.Text += "전체 탐색 완료됨" + Environment.NewLine;
            }
        }

        // 너비 우선 탐색으로 파일 찾기
        static async Task<string> DFSFolderFind(string[] targetFolderName, ProgressBar progressBar, TextBox loglabel, CancellationToken cancellationToken)
        {
            Queue<string> directoriesToSearch = new Queue<string>();
            int totalDirectories = 0;
            int processedDirectories = 0;

            // 모든 드라이브의 루트 디렉토리를 큐에 추가
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
                loglabel.Text = $"{targetFolderName[0]}를 찾는중 : {currentDirectory}" + Environment.NewLine;

                // 취소 요청 확인
                if (cancellationToken.IsCancellationRequested)
                {
                    loglabel.Text += "전체 탐색이 중단됨" + Environment.NewLine;
                    return null;
                }

                try
                {
                    // 비동기적으로 하위 디렉토리 탐색
                    string[] directories = await Task.Run(() => Directory.GetDirectories(currentDirectory), cancellationToken);

                    foreach (string directory in directories)
                    {
                        foreach (string target in targetFolderName)
                        {
                            if (Path.GetFileName(directory).Equals(target, StringComparison.OrdinalIgnoreCase))
                            {
                                loglabel.Text += $"{targetFolderName[0]} 가 발견됨! {directory}";
                                return directory;
                            }
                        }

                        directoriesToSearch.Enqueue(directory);
                        totalDirectories++;
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    // 권한이 없는 경우 계속 진행
                    continue;
                }
                catch (OperationCanceledException)
                {
                    // 취소된 경우, 로그를 업데이트하고 null 반환
                    loglabel.Text += "작업이 취소되었습니다." + Environment.NewLine;
                    return null;
                }
            }

            return null;
        }



        ////// 처리 함수들 //////
        public async Task isolation() //격리
        {
            loglabel.Text = "";
            progressBar1.Value = 0;

            //ClassM 부분
            if (!string.IsNullOrEmpty(classMPath) && !File.Exists(Path.Combine(classMPath, "ClassMIsolation.txt")))
            {
                await allkill();
                await XCopy(classMPath, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClassM"), loglabel);
                DeleteDirectoryAsync(classMPath, loglabel);
                File.WriteAllText(Path.Combine(classMPath, "ClassMIsolation.txt"), "이것은 ClassM이 제거됬다는 것을 증명합니다. 삭제하지 말아주세요.");
                loglabel.Text += $"ClassM 작업 완료 " + Environment.NewLine;
            }
            else if (!string.IsNullOrEmpty(classMPath) && File.Exists(Path.Combine(classMPath, "ClassMIsolation.txt")))
            {
                loglabel.Text += $"ClassM이 격리된 흔적이 발견되 건너뜀 " + Environment.NewLine;
            }

            else
            {
                loglabel.Text += $"ClassM이 없어 건너뜀 " + Environment.NewLine;
            }


            progressBar1.Value = 50;

            //NetSupport부분
            if (!string.IsNullOrEmpty(netSupportPath) && !File.Exists(Path.Combine(netSupportPath, "NetSupportPathIsolation.txt")))
            {
                await allkill();
                await XCopy(netSupportPath, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NetSupport"), loglabel);
                DeleteDirectoryAsync(netSupportPath, loglabel);
                File.WriteAllText(Path.Combine(netSupportPath, "NetSupportPathIsolation.txt"), "이것은 NetSupportPath이 제거됬다는 것을 증명합니다. 삭제하지 말아주세요.");
                loglabel.Text += $"NetSupport 작업 완료 " + Environment.NewLine;
            }
            else if (!string.IsNullOrEmpty(netSupportPath) && File.Exists(Path.Combine(netSupportPath, "NetSupportPathIsolation.txt")))
                loglabel.Text += $"NetSupport가 격리된 흔적이 발견되 건너뜀 " + Environment.NewLine;
            else
                loglabel.Text += $"NetSupport가 없어 건너뜀 " + Environment.NewLine;

            progressBar1.Value = 100;

            loglabel.Text += "작업이 완료됨\n";
        }
        public async Task Restoration()
        {
            loglabel.Text = "";
            progressBar1.Value = 0;
            if (string.IsNullOrEmpty(classMPath))
                loglabel.Text += "ClassM이 없어 건너뜀 " + Environment.NewLine;
            else if (!File.Exists(Path.Combine(classMPath, "ClassMIsolation.txt")))
                loglabel.Text += "ClassM이 격리된 흔적이 발견되지 않아 건너뜀 " + Environment.NewLine;
            else
            {
                XCopy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClassM"), classMPath, loglabel);
                loglabel.Text += "7초뒤 일괄 실행됩니다." + Environment.NewLine;
                Task.Delay(7000).Wait();
                ProcessStart(Path.Combine(classMPath, "ClassM_Client.exe"));
                ProcessStart(Path.Combine(classMPath, "ClassM_Client_Service.exe"));
                ProcessStart(Path.Combine(classMPath, "mvnc.exe"));
                ProcessStart(Path.Combine(classMPath, "SysCtrl.exe"));
                ProcessStart(Path.Combine(classMPath, "hscagent.exe"));
                File.Delete(Path.Combine(classMPath, "ClassMIsolation.txt"));
                loglabel.Text += $"ClassM 작업 완료 " + Environment.NewLine;
            }

            progressBar1.Value = 50;

            //NetSupport부분
            if (string.IsNullOrEmpty(netSupportPath))
                loglabel.Text += $"NetSupport가 없어 건너뜀 " + Environment.NewLine;
            else if (!File.Exists(Path.Combine(netSupportPath, "NetSupportPathIsolation.txt")))
                loglabel.Text += $"NetSupport가 격리된 흔적이 발견되지 않아 건너뜀 " + Environment.NewLine;
            else
            {
                XCopy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NetSupport"), netSupportPath, loglabel);
                loglabel.Text += "7초뒤 일괄 실행됩니다." + Environment.NewLine;
                Task.Delay(7000).Wait();
                ProcessStart(Path.Combine(netSupportPath, "client32.exe"));
                ProcessStart(Path.Combine(netSupportPath, "StudentUI.exe"));
                ProcessStart(Path.Combine(netSupportPath, "NSToast.exe"));
                ProcessStart(Path.Combine(netSupportPath, "ClassicStartMenu.exe"));
                ProcessStart(Path.Combine(netSupportPath, "nspowershell.exe"));
                ProcessStart(Path.Combine(netSupportPath, "NSClientTB.exe"));
                ProcessStart(Path.Combine(netSupportPath, "Runplugin64.exe"));
                ProcessStart(Path.Combine(netSupportPath, "runplugin.exe"));
                File.Delete(Path.Combine(netSupportPath, "NetSupportPathIsolation.txt"));
                loglabel.Text += $"NetSupport 작업 완료 " + Environment.NewLine;
            }
            progressBar1.Value = 100;

            loglabel.Text += "작업 완료됨" + Environment.NewLine;

        }


        public void ProcessKill(string name)
        {
            try
            {
                Process[] processes = Process.GetProcessesByName(name);

                foreach (Process process in processes)
                {
                    loglabel.Text += $"{name} (이)가 종료됨 " + Environment.NewLine;
                    process.Kill();

                }

                if (processes.Length == 0)
                {
                    loglabel.Text += $"{name} (이)가 종료되지않음 " + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                loglabel.Text += "에러발생{ex}" + Environment.NewLine;
            }

        }
        private async Task allkill()
        {
            SilenceProcessKill("ClassM_Client");
            SilenceProcessKill("ClassM_Client_Service");
            SilenceProcessKill("client32");
            SilenceProcessKill("Runplugin64");
            SilenceProcessKill("runplugin");
            SilenceProcessKill("SysCtrl");
            SilenceProcessKill("mvnc");
            SilenceProcessKill("hscagent");
            SilenceProcessKill("CertTool");
            SilenceProcessKill("hscdm");
            SilenceProcessKill("hscfm");
            SilenceProcessKill("hscrelay");
            SilenceProcessKill("ClassicStartMenu");
            SilenceProcessKill("P2PSyncService");
            SilenceProcessKill("BarMonitor");
            SilenceProcessKill("StartMenuExperienceHost");
            SilenceProcessKill("BarClientView");
            SilenceProcessKill("Launcher Start");
            loglabel.Text += $"일괄종료 완료됨" + Environment.NewLine;

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
                loglabel.Text += "에러발생{ex}" + Environment.NewLine;
            }

        }
        public void ProcessStart(string name)
        {
            try
            {
                Process.Start(name);
                loglabel.Text += $"{name}(을)를 실행함 " + Environment.NewLine;
            }
            catch (Exception ex)
            {
                loglabel.Text += $"{name} (이)가 실행되지않음 " + Environment.NewLine;
            }

        }


        static async Task XCopy(string sourcePath, string destinationPath, TextBox loglabel)
        {
            // 소스와 목적지 디렉토리가 존재하지 않는 경우 예외 발생
            if (!Directory.Exists(sourcePath))
            {
                loglabel.Invoke((Action)(() => loglabel.Text += $"Fatal Error: {sourcePath} 폴더가 없습니다. 복사 작업이 취소되었습니다!" + Environment.NewLine));
                return;
            }

            // 목적지 디렉토리 생성
            if (!Directory.Exists(destinationPath))
            {
                loglabel.Invoke((Action)(() => loglabel.Text += $"복사할 위치 {destinationPath}가 없어 디렉토리가 생성되었습니다." + Environment.NewLine));
                Directory.CreateDirectory(destinationPath);
            }

            // 파일 복사
            var fileTasks = Directory.GetFiles(sourcePath).Select(async filePath =>
            {
                string destFilePath = Path.Combine(destinationPath, Path.GetFileName(filePath));
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
                    loglabel.Invoke((Action)(() => loglabel.Text += $"파일 복사 오류: {ex.Message}" + Environment.NewLine));
                }
            });

            await Task.WhenAll(fileTasks);

            // 하위 디렉토리 복사
            var directoryTasks = Directory.GetDirectories(sourcePath).Select(async dirPath =>
            {
                string destDirPath = Path.Combine(destinationPath, Path.GetFileName(dirPath));
                await XCopy(dirPath, destDirPath, loglabel);
            });

            await Task.WhenAll(directoryTasks);
        }

        static async Task DeleteDirectoryAsync(string directoryPath, TextBox loglabel)
        {
            if (!Directory.Exists(directoryPath))
            {
                loglabel.Text += "디렉토리가 존재하지 않습니다. " + Environment.NewLine;
                return;
            }

            // 파일 삭제 작업을 비동기적으로 수행
            foreach (string file in Directory.GetFiles(directoryPath))
            {
                await Task.Run(() =>
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        loglabel.Invoke((Action)(() => loglabel.Text += $"파일 접근 거부: {file} " + Environment.NewLine));
                    }
                    catch (IOException ex)
                    {
                        loglabel.Invoke((Action)(() => loglabel.Text += $"파일 삭제 오류 {file}: {ex.Message} " + Environment.NewLine));
                    }
                });
            }

            // 하위 디렉토리 삭제 작업을 비동기적으로 수행
            foreach (string subdirectory in Directory.GetDirectories(directoryPath))
            {
                await Task.Run(async () =>
                {
                    try
                    {
                        await DeleteDirectoryAsync(subdirectory, loglabel); // 하위 디렉토리 비동기 호출
                        Directory.Delete(subdirectory);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        loglabel.Invoke((Action)(() => loglabel.Text += $"디렉토리 접근 거부: {subdirectory} " + Environment.NewLine));
                    }
                    catch (IOException ex)
                    {
                        loglabel.Invoke((Action)(() => loglabel.Text += $"디렉토리 삭제 오류 {subdirectory}: {ex.Message} " + Environment.NewLine));
                    }
                });
            }

            // 최상위 디렉토리 삭제 작업을 비동기적으로 수행
            await Task.Run(() =>
            {
                try
                {
                    Directory.Delete(directoryPath);
                    loglabel.Invoke((Action)(() => loglabel.Text += $"디렉토리 삭제됨: {directoryPath} " + Environment.NewLine));
                }
                catch (UnauthorizedAccessException)
                {
                    loglabel.Invoke((Action)(() => loglabel.Text += $"디렉토리 접근 거부: {directoryPath} " + Environment.NewLine));
                }
                catch (IOException ex)
                {
                    loglabel.Invoke((Action)(() => loglabel.Text += $"디렉토리 삭제 오류 {directoryPath}: {ex.Message}" + Environment.NewLine));
                }
            });
        }




        ////// 경로, 라벨 수정 함수 //////
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




        ////// 이벤트 함수들 //////

        private async void AllSc_Click(object sender, EventArgs e)
        {
            netSupportInfo.Text = "성능과 시간이 소모됩니다.";
            classMinfo.Text = "탐색중...";
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
                MessageBox.Show("복구 시도를 완료하였습니다.");
                tryrestore = false;

            }

            if (checkBox2.Checked == true)
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


        private void start_Click(object sender, EventArgs e) //켜기
        {
            loglabel.Text = "";
            if(classMPath != null)
            {
                ProcessStart(Path.Combine(classMPath, "ClassM_Client.exe"));
                ProcessStart(Path.Combine(classMPath, "ClassM_Client_Service.exe"));
                ProcessStart(Path.Combine(classMPath, "mvnc.exe"));
                ProcessStart(Path.Combine(classMPath, "SysCtrl.exe"));
                ProcessStart(Path.Combine(classMPath, "hscagent.exe"));
            }

            if (netSupportPath != null)
            {
                ProcessStart(Path.Combine(netSupportPath, "client32.exe"));
                ProcessStart(Path.Combine(netSupportPath, "StudentUI.exe"));
                ProcessStart(Path.Combine(netSupportPath, "NSToast.exe"));
                ProcessStart(Path.Combine(netSupportPath, "ClassicStartMenu.exe"));
                ProcessStart(Path.Combine(netSupportPath, "nspowershell.exe"));
                ProcessStart(Path.Combine(netSupportPath, "NSClientTB.exe"));
                ProcessStart(Path.Combine(netSupportPath, "Runplugin64.exe"));
                ProcessStart(Path.Combine(netSupportPath, "runplugin.exe"));
            }

        }

        private void stop_Click(object sender, EventArgs e) //끄기
        {
            loglabel.Text = "";
            ProcessKill("ClassM_Client");
            ProcessKill("ClassM_Client_Service");
            ProcessKill("client32");
            ProcessKill("Runplugin64");
            ProcessKill("runplugin");
            ProcessKill("SysCtrl");
            ProcessKill("mvnc");
            ProcessKill("hscagent");
            ProcessKill("CertTool");
            ProcessKill("hscdm");
            ProcessKill("hscfm");
            ProcessKill("hscrelay");
            ProcessKill("ClassicStartMenu");
            ProcessKill("P2PSyncService");
            ProcessKill("BarMonitor");
            ProcessKill("StartMenuExperienceHost");
            ProcessKill("BarClientView");
            ProcessKill("Launcher Start");
        }
    }
}
