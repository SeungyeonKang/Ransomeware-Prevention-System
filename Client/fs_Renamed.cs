//클라이언트 측 파일의 리네임을 감지하고 서버에 전송해줌 
public static void fs_Renamed(object sender, RenamedEventArgs e)
        {
            if (e.Name.Contains("BAK") || e.Name.Contains("$$$") || e.Name.Contains("tmp") || e.OldName.Contains("$$$")|| e.OldName.Contains("tmp"))
            {
                return;
            }

            Console.WriteLine(DateTime.Now.ToString("T") + "] Renamed data  " + e.OldName + " -> " + e.Name);
            
            FileName_rename_newname = e.Name;
            FileName_rename_oldname = e.OldName;
            status = ".rename";
            //FtpDownload();
            //DownloadFileFTP();
            Thread[] aThread =
            {
                //new Thread( new ThreadStart(FtpDelete) ),
                //new Thread( new ThreadStart(FtpUpload) )
                new Thread( new ThreadStart(FtpRename) )
            };

            foreach (Thread t in aThread)
            {
                t.Start();
                t.Join();
            }

            if (e.Name == "backup.recover")
            {
                status = ".create";

                Thread[] aThread2 =
                {
                //new Thread( new ThreadStart(FtpUpload) ),
                new Thread( new ThreadStart(FtpControler) )
                };

                foreach (Thread t in aThread2)
                {
                    t.Start();
                    t.Join();
                }

                Console.WriteLine("복구요청을 하셨습니다.\n복구파일을 생성 중 입니다.");
                Console.WriteLine("Goorum 폴더 내에서 어떠한 작업도 하지 말아 주세요...");
                initialize();

                return;
            }

        }