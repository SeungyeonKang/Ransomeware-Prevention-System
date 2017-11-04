static void fs_creation()
//클라이언트측 파일시스템 감시 모듈 실행을 위한 메소드
        {
            FileSystemWatcher fw = new FileSystemWatcher();
            fw.Path = ClientPath; //Test 폴더 감시 
            fw.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size; //파일 이름과 폴더 이름 감시 
            fw.Filter = ""; //특정 파일 감시 ex)*.exe,(모두 감시"", *.*) 

            fw.Created += new FileSystemEventHandler(fs_Created); //조건에 해당하는 파일 및 폴더의 생성 이벤트 등록 
            fw.Deleted += new FileSystemEventHandler(fs_Deleted); //조건에 해당하는 파일 및 폴더의 삭제 이벤트 등록 
            fw.Renamed += new RenamedEventHandler(fs_Renamed);   //조건에 해당하는 파일 및 폴더의 이름 변경 이벤트 등록 
            fw.Changed += new FileSystemEventHandler(fs_Changed);
            fw.EnableRaisingEvents = true; //이벤트 활성화 
            fw.IncludeSubdirectories = true;

            while (true)    //종료를 방지하기 위한 대기 코드
            {
                fw.WaitForChanged(WatcherChangeTypes.All);
            }
        }