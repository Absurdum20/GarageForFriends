using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace ManageAccountsRoles
{
    [Serializable]
    public class FilesInDrive
    {
        [XmlIgnore]
        [XmlElement(ElementName = "isConnected")]
        public bool isConnected { get; private set; }

        private const string IdMain = "0B5XcsexqtTPmakZ2RFRyTnRNcTQ";

        [XmlArray(ElementName = "files")]
        public List<FileGD> Files { get; set; } = new List<FileGD>();

        public FilesInDrive() {}

        public FilesInDrive(string _clientId = "", string _clientSecret = "")
        {
            MemoryStream listXml = new MemoryStream((new FileGD() { FullName = "fileList.xml", IdFile = IdMain }).DownloadFile());
            if (listXml.Length == 0)
            {
                isConnected = false;
            }
            else
            {
                isConnected = true;
                XmlSerializer xmlSer = new XmlSerializer(typeof(FilesInDrive), "OptimusGTD");
                FilesInDrive fromXmlObj;
                listXml.Flush();
                listXml.Position = 0;
                fromXmlObj = (FilesInDrive)xmlSer.Deserialize(listXml);
                Files = fromXmlObj.Files;
            }
        }

        public class FileGD
        {
            [XmlElement(ElementName = "idFile")]
            public string IdFile { get; set; }

            [XmlElement(ElementName = "fullName")]
            public string FullName { get; set; }

            [XmlElement(ElementName = "sha512")]
            public string Sha512 { get; set; }

            [XmlElement(ElementName = "isUpdate")]
            public bool IsUpdate { get; set; }

            [XmlElement(ElementName = "versionUpdate")]
            public string VersionUpdate { get; set; }

            [XmlElement(ElementName = "projectName")]
            public string ProjectName { get; set; }

            [XmlElement(ElementName = "description")]
            public string[] Description { get; set; }

            [XmlElement(ElementName = "length")]
            public long Length { get; set; }

            public FileGD()
            {
                IdFile = "";
                FullName = "";
                Sha512 = "";
                IsUpdate = false;
                VersionUpdate = "";
                Description = new string[] { };
                Length = 0;
                ProjectName = "";
            }

            public byte[] DownloadFile()
            {
                byte[] buffer;
                using (WebClient httpclient = new WebClient())
                {
                    buffer = httpclient.DownloadData(
                    string.Format("https://drive.google.com/uc?export=download&id={0}", this.IdFile));
                }
                return buffer;
            }

            public byte[] DownloadFileWithProgress(Action<string> reportProgress)
            {
                Uri url = new Uri(string.Format("https://drive.google.com/uc?export=download&id={0}", this.IdFile));
                WebClient wc = new ExtendedWebClient(url, 15000);
                var handler = new DownloadProgressChangedEventHandler((o,s) => {
                    reportProgress("Загрузка... " + s.BytesReceived.ToString() + "/" + this.Length);
                });
                byte[] getFile = null;
                wc.DownloadProgressChanged += handler;
                wc.DownloadDataCompleted += (o,e) => {
                    lock (e.UserState)
                    {
                        Monitor.Pulse(e.UserState);
                        getFile = e.Result;
                    }
                };
                SHA512 hash = new SHA512Managed();
                
                var syncObject = new Object();
                lock (syncObject)
                {
                    wc.DownloadDataAsync(url, syncObject);
                    Monitor.Wait(syncObject);
                }
                try
                {

                    if (Convert.ToBase64String(hash.ComputeHash(getFile)) == this.Sha512)
                    {
                        //MessageBox.Show(getFile.Length.ToString());
                        return getFile;
                    }
                    else
                    {
                        MessageBox.Show("Ошибка целостности файла, попробуйте снова.");
                        return null;
                    }

                }
                catch
                {
                    MessageBox.Show("Недоступно соединение с интернетом.");
                    return null;
                }
            }

        }
    }

    class ExtendedWebClient : WebClient
    {
        public int Timeout { get; set; }
        public ExtendedWebClient(Uri address, int timeout)
        {
            Timeout = timeout;
            var objWebClient = GetWebRequest(address);
        }
        protected override WebRequest GetWebRequest(Uri address)
        {
            var objWebRequest = base.GetWebRequest(address);
            objWebRequest.Timeout = Timeout;
            return objWebRequest;
        }
    }
}

