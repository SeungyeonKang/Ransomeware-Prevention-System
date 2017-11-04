//클라이언트 측에서 파일 생성시 작동
static void fs_Created(object sender, FileSystemEventArgs e)
        {

	   //파일 경로 설정

            FileAttributes attr = File.GetAttributes(tempDir + "\\" + e.Name);


           //목적지 디렉토리가 없으면 해당 디렉토리를 생성해줌

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                
                string pathString_backup = Path.Combine(backupDir, e.Name);
                Directory.CreateDirectory(goorumDir + "\\" + e.Name);
                Directory.CreateDirectory(pathString_backup);
                

                return;
            }

            

            string name = e.Name;

            string[] words = name.Split('^');


            if(words.Length < 2)
            {
                return;
            }

        
            string[] words2 = words[1].Split('.');
            string[] user_name = words[0].Split('\\');
            Console.WriteLine(name);


            if (words2.Length < 5)
            {
                return;
            }

	    //파일 복구 명령이 내려지만 파일을 복구함
            if(words2[1] == "backup" && words2[2] == "recover" && words2[3] == "create" && words2[4] == "cloud")
            {
                user = user_name[0];
                Console.WriteLine(user);
                FileName_up = backupDir + '\\' + user;
                Console.WriteLine("파일 복구 명령");

                Thread[] aThread =
                {
                    new Thread (new ThreadStart(recover_goorum))
                };
                foreach(Thread t in aThread)
                {
                    t.Start();
                    t.Join();
                }

                if (e.Name.Contains("backup.recover"))
                {
                    File.Delete(tempDir + "\\" + user + "\\" + "backup.recover");
                    File.Delete(tempDir + "\\" + user + "\\" + "^_.backup.recover.create.cloud");


                }
                

                return;

            }

	   //새롭게 생성된 파일이면 서버에 저장
            else if (words2[4] == "cloud" && words2[3] == "create")
            {
                
                FileName_up = words[0] + words2[1] + "." + words2[2];

                Console.WriteLine(DateTime.Now.ToString("T") + "] Created data  " + FileName_up);


                Thread[] aThread =
            {
                new Thread(new ThreadStart(Create_goorum))
            };
                foreach (Thread t in aThread)
                {
                    t.Start();
                    t.Join();
                  
                    if (name.Contains("^_"))
                    {
                    again:
                        try
                        {
                            File.Delete(tempDir + "\\" + name);
                        }
                        catch
                        {
                            goto again;
                        }
                    } 
                }
              
            }

	    //기존의 파일이 수정된 파일이면 유사도 비교 후 서버에 저장
            else if (words2[4] == "cloud" && words2[3] == "change")
            {
                string date = DateTime.Now.ToString("T");
                FileName_del = words[0] + words2[1] + "." + words2[2];
                FileName_up = words[0] + words2[1] + "." + words2[2];

                
                Console.WriteLine(DateTime.Now.ToString("T") + "] Change data  " + FileName_up);


                int failcounter = 1;
                again:
                string sourceFile = Path.Combine(tempDir, FileName_up);
                string destFile = Path.Combine(goorumDir, FileName_del);

                try
                {
                    string extension;
                    string fullName = String.Empty;
                    string name1 = "";
                    fullName = sourceFile.Substring(sourceFile.LastIndexOf('\\') + 1);
                    name1 = fullName.Substring(0, fullName.LastIndexOf('.'));
                    extension = Path.GetExtension(sourceFile);
                    
                    string ppt_temp = "~$";
                    if (name1.Contains(ppt_temp))
                    {
                        return;
                    }
                   

                    usado(sourceFile, destFile);

                    ////////////////////////////////////////////// 유사도비교 끝
                    if (name.Contains("^_"))
                    {
                        again2:
                        try
                        {
                            File.Delete(tempDir + "\\" + name);
                        }
                        catch
                        {
                            goto again2;
                        }
                    } 
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

		//수정되기 이전의 파일이 이미 없는 경우 예외처리
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
            {
                return;
            }
        }