
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
            // 기본 스캔 실행 (비동기 처리 고려)
            ClassMScan();
            NetSupportScan();
        }


        ////// 빠른 스캔 함수들 //////
        public void ClassMScan()
        {
            classMPath = null;
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
            netSupportPath = null;
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

        ////// 전체 스캔 관련 함수들 //////
        public async Task AllClassMScan()
        {
            classMPath = null;
            string result = await DFSFolderFind("ClassM", progressBar1);
            if (result == null)
                classMinfo.Text = "발견되지않음";
            else
                SetClassMPath(Path.Combine(result, "hscagent.exe"));
        }

        public async Task AllNetSupportScanAsync()
        {
            netSupportPath = null;
            string result = await DFSFolderFind("NetSupport School", progressBar1);
            if (result == null)
                netSupportInfo.Text = "발견되지않음";
            else
                SetNetSupportPath(Path.Combine(result, "client32.exe"));
        }

        // 너비 우선 탐색으로 파일 찾기
        static async Task<string> DFSFolderFind(string targetFolderName, ProgressBar progressBar)
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


                try
                {
                    // 비동기적으로 하위 디렉토리 탐색
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


        ////// 처리 함수들 //////
        public async Task isolation() //격리
        {
            progressBar1.Value = 0;

            //ClassM 부분
            if (!string.IsNullOrEmpty(classMPath))
            {
                ProcessKill("ClassM_Client");
                ProcessKill("ClassM_Client_Service");
                ProcessKill("mvnc");
                ProcessKill("SysCtrl");
                ProcessKill("hscagent");
                await XCopy(classMPath, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClassM"));
                File.WriteAllText(Path.Combine(classMPath, "ClassMIsolation.txt"), "이것은 ClassM이 제거됬다는 것을 증명합니다. 삭제하지 말아주세요.");
            }
            else if (File.Exists(Path.Combine(classMPath, "ClassMIsolation.txt")))
                MessageBox.Show("ClassM이 격리된 흔적이 발견되어 격리가 취소되었습니다.");

            progressBar1.Value = 50;

            //NetSupport부분
            if (!string.IsNullOrEmpty(netSupportPath))
            {
                ProcessKill("client32");
                ProcessKill("StudentUI");
                ProcessKill("NSToast");
                ProcessKill("ClassicStartMenu");
                ProcessKill("nspowershell");
                ProcessKill("NSClientTB");
                await XCopy(classMPath, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NetSupport"));
                File.WriteAllText(Path.Combine(netSupportPath, "NetSupportPathIsolation.txt"), "이것은 NetSupportPath이 제거됬다는 것을 증명합니다. 삭제하지 말아주세요.");
            }
            else if (File.Exists(Path.Combine(netSupportPath, "NetSupportPathIsolation.txt")))
                MessageBox.Show("NetSupport이 격리된 흔적이 발견되어 격리가 취소되었습니다.");

            progressBar1.Value = 100;

            MessageBox.Show("격리가 완료되었습니다.");
        }
        public async Task Restoration()
        {
            progressBar1.Value = 0;
            if (!File.Exists(Path.Combine(classMPath, "ClassMIsolation.txt")))
                MessageBox.Show("ClassM이 격리된 흔적이 발견되지않아 복구가 취소되었습니다.");
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

            //NetSupport부분
            if (!File.Exists(Path.Combine(netSupportPath, "NetSupportPathIsolation.txt")))
                MessageBox.Show("NetSupport이 격리된 흔적이 발견되지않아 복구가 취소되었습니다.");
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

            MessageBox.Show("복구가 완료되었습니다.");

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
                // ProcessStartInfo 객체를 생성하여 실행할 파일과 관련된 정보를 설정합니다.
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = name,
                    UseShellExecute = true // 셸을 통해 파일을 실행합니다.
                };
            }

        }


        /*
        public static async Task CopyDirectoryAsync(string sourceFolder, string destFolder, IProgress<int> progress = null)
        {

            // 대상 폴더가 존재하지 않으면 생성
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            // 소스 폴더에서 파일과 디렉터리 목록을 가져옴
            string[] files = Directory.GetFiles(sourceFolder);
            string[] folders = Directory.GetDirectories(sourceFolder);

            int totalItems = files.Length + folders.Length;
            int completedItems = 0;

            // 파일을 비동기적으로 복사
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

            // 폴더를 재귀적으로 복사 (비동기)
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

            // 비동기 작업을 위한 Task 배열
            Task[] tasks = new Task[1];

            tasks[0] = Task.Run(async () =>
            {
                // xcopy 명령어를 구성합니다.
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

                // 로그 파일 이름 생성
                string logFileName = $"{Environment.MachineName}_{DateTime.Now:yyyyMMdd_HHmmss}_copy.log";

                // Process 생성 및 실행
                using (var process = Process.Start(processInfo))
                using (var reader = process.StandardOutput)
                using (var errorReader = process.StandardError)
                {
                    // 명령어의 출력을 비동기적으로 읽습니다.
                    var output = await reader.ReadToEndAsync();
                    var errorOutput = await errorReader.ReadToEndAsync();
                }
            });

            // 모든 Task 완료 대기
            await Task.WhenAll(tasks);
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
        private void FastSc_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            ClassMScan();
            NetSupportScan();
        }

        private async void AllSc_Click(object sender, EventArgs e)
        {
            netSupportInfo.Text = "성능과 시간이 소모됩니다.";
            classMinfo.Text = "탐색중...";
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
