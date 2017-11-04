 static void fs_Renamed(object sender, RenamedEventArgs e)
//기존에 있던 파일의 이름 변경을 위한 모듈
        {
            Console.WriteLine(DateTime.Now.ToString("T") + "] Renamed data  " + e.OldName + " -> " + e.Name);

            FileName_del = e.OldName;
            FileName_up = e.Name;
            FullPath = e.FullPath;

            Thread[] aThread =
            {
                new Thread(new ThreadStart(Rename_goorum))
            };
            foreach (Thread t in aThread)
            {
                t.Start();
                t.Join();
            }
        }