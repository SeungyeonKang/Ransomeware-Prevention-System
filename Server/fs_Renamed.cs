 static void fs_Renamed(object sender, RenamedEventArgs e)
//������ �ִ� ������ �̸� ������ ���� ���
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