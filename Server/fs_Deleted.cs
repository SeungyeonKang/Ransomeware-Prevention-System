static void fs_Deleted(object sender, FileSystemEventArgs e)
//���� ���� ����� ���� ������ ���
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