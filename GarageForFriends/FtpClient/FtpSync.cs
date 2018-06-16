using System;
using System.Collections.Generic;
using System.Linq;
using FluentFTP;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;
using Newtonsoft.Json;


namespace GarageForFriends.FtpClient
{
    public class FtpSync
    {
        public FileTree ServerFiles { get; set; }

        private MD5 mD5 = new MD5CryptoServiceProvider();

        protected byte[] CalcMd5HashLocal(Node file)
        {
            return mD5.ComputeHash(File.ReadAllBytes(file.EnviromentPath));  
        }

        public string LocalRootDirectory { get; protected set; }

        public IFtpClient Client { get; private set; }

        private FtpSync() { }
        public FtpSync(IFtpClient client, string localDirectory)
        {
            ServerFiles = new FileTree();
            Client = client;
            LocalRootDirectory = localDirectory;           
        }

        public void Synchronize(Action<string> syncText, string path = "")
        {
            SyncChanges(syncText, path);
            SyncDeletedFiles(syncText, path);

            SyncFoldersStructure(syncText, path);
            SyncFilesStructure(syncText, path);

            syncText("Синхронизация завершена!");
        }

        private void SyncFoldersStructure(Action<string> syncText, string path = "")
        {
            syncText("Синхронизация папок...");
            var folders = ServerFiles.GetDirectories(x => x.IsSync);
            if (folders != null)
            {
                foreach (var folder in folders)
                {
                    if(folder.IsDeleted)
                    {
                        if (Client.DirectoryExists(folder.ServerPath))
                        {
                            try
                            {
                                Client.DeleteDirectory(folder.ServerPath);
                            }
                            catch { }
                        }                       
                        folder.IsSync = false;
                    } else
                    {
                        try
                        {
                            Client.CreateDirectory(folder.ServerPath);
                        }
                        catch { }
                        folder.IsSync = false;
                    }                 
                }
            }
            syncText("Синхронизация папок завершена...");
        }

        private void SyncFilesStructure(Action<string> syncText, string path = "")
        {
            syncText("Синхронизация файлов...");
            var files = ServerFiles.GetFiles(x => x.IsSync);
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file.IsDeleted)
                    {
                        if (Client.FileExists(file.ServerPath))
                        {
                            try
                            {
                                Client.DeleteFile(file.ServerPath);
                            }
                            catch {  }
                            
                        }
                        file.IsSync = false;
                    } else
                    {   
                        if(Client.FileExists(file.ServerPath))
                        {
                            try
                            {
                                Client.DeleteFile(file.ServerPath);
                            }
                            catch { }
                        }
                        try
                        {
                            syncText("Загружается файл...\n" + file.Path);
                            Client.UploadFile(file.EnviromentPath, file.ServerPath);
                            file.Md5Hash = CalcMd5HashLocal(file);
                            file.IsSync = false;
                        }
                        catch {  }
                       
                    }
                }
            }
            syncText("Синхронизация файлов завершена...");
        }

        private void SyncDeletedFiles(Action<string> syncText, string path = "")
        {
            syncText("Удаление лишних папок...");
            var folders = ServerFiles.GetDirectories(x => true);
            if (folders != null)
            {
                foreach (var folder in folders)
                {
                    folder.IsDeleted = !Directory.Exists(folder.EnviromentPath);
                    folder.IsSync = folder.IsSync || folder.IsDeleted;
                }
            }
            syncText("Удаление лишних файлов...");
            var files = ServerFiles.GetFiles(x => true);
            if (files != null)
            {
                foreach(var file in files)
                {
                    file.IsDeleted = !File.Exists(file.EnviromentPath);
                    file.IsSync = file.IsSync || file.IsDeleted;
                }
            }
            syncText("Удаление файлов и папок завершено...");
        }

        private void SyncChanges(Action<string> syncText, string path = "")
        {
            syncText("Получаем список изменений...");
            string[] fileEntries = Directory.GetFiles(Path.Combine(LocalRootDirectory, path));
            foreach (string fileName in fileEntries)
            {
                SyncFile(fileName.Replace(LocalRootDirectory, ""));
            }
               

            string[] subdirectoryEntries = Directory.GetDirectories(Path.Combine(LocalRootDirectory, path));
            foreach (string subdirectory in subdirectoryEntries)
            {
                Console.WriteLine("Subdir - " + subdirectory);
                SyncChanges(syncText, subdirectory);
            }              
        }

        private void SyncFile(string fileName)
        {

            //MessageBox.Show("Sync - " + fileName);
            fileName = fileName.Replace(@"\\", @"\");

            string[] paths = fileName.Split('\\');
            string parentPath = "";

            foreach (var s in paths)
            {                
                if (s != "")// Рут директорию не нужно обрабатывать
                {
                    string fileFullPath = (parentPath + @"\" + s).Replace(@"\\", @"\");
                    if (s.Contains('.'))
                    {
                        var n = ServerFiles.GetFiles(x => x.LocalPath == fileFullPath).FirstOrDefault();
                        if (n!=null)
                        {
                            Console.WriteLine("Found File - " + fileFullPath);
                            if (n.Md5Hash == null)
                            {
                                Console.WriteLine("Md5Hash is null");
                                n.IsSync = true;

                            } else {

                                n.IsSync = !(Convert.ToBase64String(CalcMd5HashLocal(n)) == Convert.ToBase64String(n.Md5Hash));
                            }                                           
                            Console.WriteLine(n.IsSync ? "Something Changed On - " + fileFullPath : "Nothing Changed On - " + fileFullPath);
                        } else
                        {
                            var d = ServerFiles.GetDirectories(x => x.LocalPath == parentPath).FirstOrDefault();
                            if(d != null)
                            {
                                Console.WriteLine("Found Dir Of File and Add - " + fileFullPath);
                                Node sync = new Node(true, fileFullPath, LocalRootDirectory);
                                sync.IsSync = true;
                                d.AddNode(sync);
                            } else
                            {
                                Console.WriteLine("Dont Found DiR!!!");
                            }                    
                        }
                    } else
                    {                        
                        Console.WriteLine("Try Find Directory - '" + fileFullPath + "'");
                      
                        var dir = ServerFiles.GetDirectories(x => x.LocalPath == fileFullPath).FirstOrDefault();
                        if (dir != null)
                        {
                            Console.WriteLine("Found Dir - Ok - " + fileFullPath);
                        }
                        else //Если не найдена директория нужно добавить к предку этой директории 
                        {
                            Console.WriteLine("Not Found Dir Go Add To Parent - " + fileFullPath);
                            var parentDir = ServerFiles.GetDirectories(x => x.LocalPath == parentPath).FirstOrDefault();
                            if (parentDir != null)
                            {
                                Node sync = new Node(false, fileFullPath, LocalRootDirectory);
                                sync.IsSync = true;
                                parentDir.AddNode(sync);
                            }
                        }
                    }                   
                }
                parentPath = (parentPath + @"\" + s).Replace(@"\\", @"\");
            }
        }

        public void GetFileStructureFromServer(Node startNode, string path, Action<string> syncText)
        {
            syncText("Получение структуры файлов...");
            Node n = startNode;         
            foreach (FtpListItem item in Client.GetListing(path))
            {
                Console.WriteLine(item.FullName);

                if (item.Type == FtpFileSystemObjectType.File)
                {
                    Node newNode = new Node(true, item.FullName, LocalRootDirectory);                                    
                    n.AddNode(newNode);

                }
                if (item.Type == FtpFileSystemObjectType.Directory)
                {
                    Node newNode = new Node(false, item.FullName, LocalRootDirectory);
                    n.AddNode(newNode);

                   if(!Directory.Exists(newNode.EnviromentPath))
                   {
                        Directory.CreateDirectory(newNode.EnviromentPath);
                   }

                    GetFileStructureFromServer(newNode, item.FullName, syncText);
                }
            }
            //syncText("Cтруктура файлов получена...");
        }

        public void DownloadFiles(Action<string> syncText)
        {
            syncText("Скачивание файлов...");
            var files = ServerFiles.GetFiles(x => true).ToList();            
            int index = 1;
            foreach(var file in files)
            {
                try
                {
                    syncText("Скачивается файл...\n" + file.Path);
                    Client.DownloadFile(file.EnviromentPath, file.ServerPath, overwrite: true);
                    byte[] f = File.ReadAllBytes(file.EnviromentPath);
                    file.Md5Hash = mD5.ComputeHash(f);
                    file.FileSize = f.LongLength;
                }
                catch { }

                index++;
            }
            syncText("Файлы загружены...");
        }
    }
    public class FileTree
    {
       public FileTree()
       {
            RootFile = new Node();
            RootFile.Path = "/";
       }
       public Node RootFile { get; set; }

       public IEnumerable<Node> GetFiles(Predicate<Node> findByField)
       {
            return RootFile.GetFilesByPredicate(RootFile, findByField);
       }

       public IEnumerable<Node> GetDirectories(Predicate<Node> findByField)
        {
            return RootFile.GetDirectoriesByPredicate(RootFile, findByField);
        }



    }
    public class Node
    {
        public Node() {
            ChildNodes = new List<Node>();
        }
        public Node(bool isFile, string path, string syncPath)
        {
            IsFile = isFile;
            Path = path.Replace(@"\", @"/");
            ChildNodes = new List<Node>();
            SyncPath = syncPath;
        }

        public string SyncPath { get; set; }

        public bool IsFile { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsSync { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public string EnviromentPath => Utils.EnviromentLocation + LocalPath;

        [JsonIgnore]
        [XmlIgnore]
        public string LocalPath => Path.Replace(@"/", @"\");

        [JsonIgnore]
        [XmlIgnore]
        public string ServerPath => Path.Replace(@"\", @"/");

        public string Path { get; set; }

        public List<Node> ChildNodes { get; set; }

        public byte[] Md5Hash { get; set; }
        public long FileSize { get; set; }

        public void AddNode(Node file)
        {
            ChildNodes.Add(file);
        }

        public IEnumerable<Node> GetFilesByPredicate(Node startNode, Predicate<Node> findByField)
        {
            foreach (var n in startNode.ChildNodes)
            {
                if (n.IsFile)
                {                    
                    if (findByField(n))
                    {
                        yield return n;
                    }                        
                } else
                {
                    foreach (var x in GetFilesByPredicate(n, findByField))
                    {
                        yield return x;
                    }                 
                }

            }
        }

        public IEnumerable<Node> GetDirectoriesByPredicate(Node startNode, Predicate<Node> findByField)
        {

            if (findByField(startNode))
            {             
                if(!startNode.IsFile)
                {
                    yield return startNode;
                }               
            }

            foreach (var n in startNode.ChildNodes)
            {
                    foreach (var x in GetDirectoriesByPredicate(n, findByField))
                    {
                        yield return x;
                    }
            }
            //yield break;
        }
    }
}




