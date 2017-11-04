//파일 및 폴더의 삭제를 감지한 후 서버측에 삭제 요청
static void fs_Deleted(object sender, FileSystemEventArgs e)
        {
            if (e.Name.Contains("BAK") || e.Name.Contains("$$$") || e.Name.Contains("tmp") ||e.Name.Contains("backup.recover"))
            {
                return;
            }
            Console.WriteLine(DateTime.Now.ToString("T") + "] Deleted data  " + e.Name);

            FileName_del = e.Name;
            status = ".delete";

            Thread[] aThread =
            {
                new Thread( new ThreadStart(FtpDelete) )
            };

            foreach (Thread t in aThread)
            {
                t.Start();
                t.Join();
            }
        }