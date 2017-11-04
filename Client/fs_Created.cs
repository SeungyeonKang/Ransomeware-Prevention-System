//클라이언트 측의 파일 생성을 감지하여 서버로 넘겨줌
static void fs_Created(object sender, FileSystemEventArgs e)
        {
            if (e.Name.Contains("$$$") || e.Name.Contains("tmp"))
            {
                return;
            }
            Console.WriteLine(DateTime.Now.ToString("T") + "] Created data  " + e.Name);
            
            FileName_up = e.Name;
            status = ".create";

            Thread[] aThread =
            {
                new Thread( new ThreadStart(FtpUpload) ),
                //new Thread( new ThreadStart(FtpControler) )
            };

            foreach (Thread t in aThread)
            {
                t.Start();
                t.Join();
            }

            if (e.Name == "backup.recover")
            {
                Console.WriteLine("복구요청을 하셨습니다.\n복구파일을 생성 중 입니다.");
                Console.WriteLine("Goorum 폴더 내에서 어떠한 작업도 하지 말아 주세요...");
                initialize();
            }


            //File.Delete("C:\\Temp" + "\\_." + FileName_up + status + ".cloud");
        }