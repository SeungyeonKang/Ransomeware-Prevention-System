//Ŭ���̾�Ʈ ������ ���� ������ �۵�
static void fs_Created(object sender, FileSystemEventArgs e)
        {

	   //���� ��� ����

            FileAttributes attr = File.GetAttributes(tempDir + "\\" + e.Name);


           //������ ���丮�� ������ �ش� ���丮�� ��������

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

	    //���� ���� ����� �������� ������ ������
            if(words2[1] == "backup" && words2[2] == "recover" && words2[3] == "create" && words2[4] == "cloud")
            {
                user = user_name[0];
                Console.WriteLine(user);
                FileName_up = backupDir + '\\' + user;
                Console.WriteLine("���� ���� ���");

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

	   //���Ӱ� ������ �����̸� ������ ����
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

	    //������ ������ ������ �����̸� ���絵 �� �� ������ ����
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

                    ////////////////////////////////////////////// ���絵�� ��
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

		//�����Ǳ� ������ ������ �̹� ���� ��� ����ó��
                catch (FileNotFoundException t)
                {
                    if (File.Exists(sourceFile) == false)
                    {
                        Console.WriteLine("������ ������ ���� Skip");
                        return;
                    }
                    if (failcounter <= 1)
                    {
                        Thread.Sleep(100);
                        Console.WriteLine("���� ��õ� : {0}", failcounter);
                        failcounter++;
                        failcounter_total++;
                        goto again;
                    }
                    else
                    {
                        Console.WriteLine("���� ����");
                        return;
                    }
                }
            }
            else
            {
                return;
            }
        }