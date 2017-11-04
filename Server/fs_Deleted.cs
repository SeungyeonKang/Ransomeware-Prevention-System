static void fs_Deleted(object sender, FileSystemEventArgs e)
//파일 삭제 기능을 위한 서버측 모듈
        {
            Console.WriteLine(DateTime.Now.ToString("T") + "] Deleted data  " + e.Name);

            FileName_del = e.Name;

            Thread[] aThread =
           {
                new Thread(new ThreadStart(Delete_goorum))
            };
            foreach (Thread t in aThread)
            {
                t.Start();
                t.Join();
            }
        }