static void fs_Changed(object sender, FileSystemEventArgs e)
//파일시스템 감시 모듈에서 파일 변경 감지시  작업
        {
            if (e.Name == "slimftpd.conf")
            {
                // 메모장 프로세스를 확인 합니다.
                Process[] processList = Process.GetProcessesByName("SlimFTPd");

                Console.WriteLine("slimftpd 프로세스 리스트 {0}", processList[0]);
                
                // 메모장이 실행 되어 있을 경우 실행 됩니다.
                if (processList.Length > 0)
                {
                    // 선택된 프로세스 (메모장) 을 종료 시킵니다.
                    processList[0].Kill();
                    Console.WriteLine("SlimFTPd 를 종료 하였습니다.");
                }
                else
                {
                    Console.WriteLine("SlimFTPd 가 실행되지 않았습니다.");
                }

                Process.Start("C:\\ftproot\\temp\\SlimFTPd.exe");
                Console.WriteLine("SlimFTPd 를 재시작 하였습니다.");
                return;
            }
            return;


            string name1 = e.Name;
            string[] words = name1.Split('.');
            
            if (words[0] == "_" && words[4] == "cloud" && words[3] == "change")
            {
                string date = DateTime.Now.ToString("T");
                FileName_del = words[1] + "." + words[2];
                FileName_up = words[1] + "." + words[2];

                Console.WriteLine(DateTime.Now.ToString("T") + "] " + e.ChangeType + " " + FileName_up);

                

                int failcounter = 1;
                again:
                string sourceFile = Path.Combine(tempDir, FileName_up);
                string destFile = Path.Combine(goorumDir, FileName_del);

                try
                {
                    string extension;
                    string fullName = String.Empty;
                    string name = "";
                    fullName = sourceFile.Substring(sourceFile.LastIndexOf('\\') + 1);
                    name = fullName.Substring(0, fullName.LastIndexOf('.'));
                    extension = Path.GetExtension(sourceFile);
                    // Console.WriteLine(name);
                    string ppt_temp = "~$";
                    if (name.Contains(ppt_temp))
                    {
                        return;

                    }
                    

                    usado(sourceFile, destFile);
                    ////////////////////////////////////////////// 유사도비교 끝

                    FileAttributes attr2 = File.GetAttributes(destFile);
                    if ((attr2 & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        if (Directory.Exists(destFile))
                        {
                            try
                            {
                                Directory.Delete(destFile, true);
                            }
                            catch (IOException u)
                            {
                                // Console.WriteLine(u.Message);
                            }
                        }
                    }
                    else
                    {
                        if (File.Exists(destFile))
                        {
                            try
                            {
                                File.Delete(destFile);
                            }
                            catch (IOException u)
                            {
                                // Console.WriteLine(u.Message);
                                return;
                            }
                        }
                        if (!Directory.Exists(goorumDir))
                        {
                            Directory.CreateDirectory(goorumDir);
                        }

                        File.Copy(sourceFile, destFile, true);

                        if (Directory.Exists(tempDir))
                        {
                            string[] files = Directory.GetFiles(tempDir);
                            foreach (string s in files)
                            {
                                FileName_up = Path.GetFileName(s);
                                destFile = Path.Combine(goorumDir, FileName_up);
                                File.Copy(s, destFile, true);
                            }
                        }
                    }
                }
                catch (FileNotFoundException t)
                {
                    if (File.Exists(sourceFile) == false)
                    {
                        Console.WriteLine("파일이 없어져 수정 Skip");
                        return;
                    }
                    if (failcounter <= 1)
                    {
                        Thread.Sleep(100);
                        Console.WriteLine("수정 재시도 : {0}", failcounter);
                        failcounter++;
                        failcounter_total++;
                        goto again;
                    }
                    else
                    {
                        Console.WriteLine("수정 실패");
                        return;
                    }
                }

            }
            else
                return;
        }