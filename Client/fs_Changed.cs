//변경을 감지하고 서버로 보냄
static void fs_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.Name.Contains("BAK") || e.Name.Contains("$$$")  )
            {  
                return;
            }
          
            Console.WriteLine(DateTime.Now.ToString("T") + "] " + e.ChangeType + " " + e.Name);

            FileName_del = e.Name;
            FileName_up = e.Name;

            status = ".change";

            Thread[] aThread =
            {
                //new Thread( new ThreadStart(FtpDelete) ),
                new Thread( new ThreadStart(FtpUpload) ),
                //new Thread( new ThreadStart(FtpControler) )
            };

            foreach (Thread t in aThread)
            {
                t.Start();
                t.Join();
            }
            initialize();
        }