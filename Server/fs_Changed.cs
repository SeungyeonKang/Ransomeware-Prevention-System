static void fs_Changed(object sender, FileSystemEventArgs e)
//���Ͻý��� ���� ��⿡�� ���� ���� ������  �۾�
        {
            if (e.Name == "slimftpd.conf")
            {
                // �޸��� ���μ����� Ȯ�� �մϴ�.
                Process[] processList = Process.GetProcessesByName("SlimFTPd");

                Console.WriteLine("slimftpd ���μ��� ����Ʈ {0}", processList[0]);
                
                // �޸����� ���� �Ǿ� ���� ��� ���� �˴ϴ�.
                if (processList.Length > 0)
                {
                    // ���õ� ���μ��� (�޸���) �� ���� ��ŵ�ϴ�.
                    processList[0].Kill();
                    Console.WriteLine("SlimFTPd �� ���� �Ͽ����ϴ�.");
                }
                else
                {
                    Console.WriteLine("SlimFTPd �� ������� �ʾҽ��ϴ�.");
                }

                Process.Start("C:\\ftproot\\temp\\SlimFTPd.exe");
                Console.WriteLine("SlimFTPd �� ����� �Ͽ����ϴ�.");
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
                    ////////////////////////////////////////////// ���絵�� ��

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
                return;
        }