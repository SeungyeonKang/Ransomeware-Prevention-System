static void fs_creation()
//Ŭ���̾�Ʈ�� ���Ͻý��� ���� ��� ������ ���� �޼ҵ�
        {
            FileSystemWatcher fw = new FileSystemWatcher();
            fw.Path = ClientPath; //Test ���� ���� 
            fw.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size; //���� �̸��� ���� �̸� ���� 
            fw.Filter = ""; //Ư�� ���� ���� ex)*.exe,(��� ����"", *.*) 

            fw.Created += new FileSystemEventHandler(fs_Created); //���ǿ� �ش��ϴ� ���� �� ������ ���� �̺�Ʈ ��� 
            fw.Deleted += new FileSystemEventHandler(fs_Deleted); //���ǿ� �ش��ϴ� ���� �� ������ ���� �̺�Ʈ ��� 
            fw.Renamed += new RenamedEventHandler(fs_Renamed);   //���ǿ� �ش��ϴ� ���� �� ������ �̸� ���� �̺�Ʈ ��� 
            fw.Changed += new FileSystemEventHandler(fs_Changed);
            fw.EnableRaisingEvents = true; //�̺�Ʈ Ȱ��ȭ 
            fw.IncludeSubdirectories = true;

            while (true)    //���Ḧ �����ϱ� ���� ��� �ڵ�
            {
                fw.WaitForChanged(WatcherChangeTypes.All);
            }
        }