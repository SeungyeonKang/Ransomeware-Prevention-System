//Ŭ���̾�Ʈ ���� ���� ������ �����Ͽ� ������ �Ѱ���
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
                Console.WriteLine("������û�� �ϼ̽��ϴ�.\n���������� ���� �� �Դϴ�.");
                Console.WriteLine("Goorum ���� ������ ��� �۾��� ���� ���� �ּ���...");
                initialize();
            }


            //File.Delete("C:\\Temp" + "\\_." + FileName_up + status + ".cloud");
        }