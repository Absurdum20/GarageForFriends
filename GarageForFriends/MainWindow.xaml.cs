using GarageForFriends.Data;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Win32;
using GarageForFriends.Abstract;
using GarageForFriends.FtpClient;
using System.Xml.Serialization;
using System.Windows.Data;
using GarageForFriends.Properties;
using ManageAccountsRoles;
using System.Linq;
using System.Threading.Tasks;
using Ionic.Zip;
using System.Diagnostics;

namespace GarageForFriends
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
                  
        private string mainPath = Path.Combine(Environment.CurrentDirectory, "Sync");
        private bool isDownloadedFromFtp = false;
        private string jsonPath = Path.Combine("http", "Data", "Json"); //"http", "Data", 
        private string imgPath = Path.Combine("http", "Data", "Img"); //"http", "Data", 
        private const string pathToFileTree = "tree.json";


        private FtpSync sync;
        private List<byte> _bufferToDownload;

        private bool CheckForUpdates()
        {
           
            FilesInDrive fid = new FilesInDrive("", "");
            var file = fid.Files.OrderByDescending(x => x.VersionUpdate).FirstOrDefault(f => f.ProjectName == "GarageForFriends");
            if (file != null)
            {          
                if (Convert.ToInt64(file.VersionUpdate) > Convert.ToInt64(Settings.Default.Version))
                {
                    var answer = MessageBox.Show(
                        string.Format("Доступно новое обновление!\n{0}\nОбновить?", file.Description != null ? String.Join("\n", file.Description) : ""),
                        "Доступно новое обновление", MessageBoxButton.YesNo);
                    if (answer == MessageBoxResult.Yes)
                    {
                        List<byte> bufferToDownload = new List<byte>();

                        Action<dynamic, Action<string>, Action<bool, dynamic>> action = (buffer, report, save) =>
                        {
                            byte[] b = file.DownloadFileWithProgress(report);
                            save(true, b.ToList());
                        };

                        Progress prog = new Progress(bufferToDownload, action, Utils.GetDownloadedFile);
                        prog.ShowDialog(); 
                        
                        using(MemoryStream ms = new MemoryStream(Utils.downloadedFile))
                        {
                            using (ZipFile zip = ZipFile.Read(ms))
                            {
                                string currentProcess = Application.ResourceAssembly.Location;
                                string bakFile = Path.ChangeExtension(currentProcess, "bak");
                                
                                File.Delete(bakFile);

                                File.Move(currentProcess, bakFile);
                                //MessageBox.Show(currentProcess);
                                string dir = Path.GetDirectoryName(currentProcess);

                                zip.ExtractAll(dir, ExtractExistingFileAction.OverwriteSilently);

                                return true;
                                // Process.Start(Application.ResourceAssembly.Location);
                                
                                //Application.Current.Shutdown();
                            }
                        }
                        

                    }
                }
            }
            return false;
        }


        private void LoadSync()
        {
            if (sync == null)
            {
                sync = new FtpSync(new FluentFTP.FtpClient
                {
                    Host = ftpServerIp.Text,
                    Credentials = new System.Net.NetworkCredential(ftpUsername.Text, ftpPassword.Password)
                },
                mainPath);
            }
        }

        private void LoadFileTree()
        {
            if (Directory.Exists(mainPath) && File.Exists(pathToFileTree))
            {
                string jsonTree = File.ReadAllText(pathToFileTree);

                sync.ServerFiles = JsonConvert.DeserializeObject<FileTree>(jsonTree);
                isDownloadedFromFtp = true;
            }
        }

        private void StartApp ()
        {
            InitializeComponent();

            this.Title = "Garage For Friends. Версия - " + Settings.Default.Version;


            ftpServerIp.Text = Settings.Default.IpAdress;
            ftpServerIp.TextChanged += (e, o) =>
            {
                Settings.Default.IpAdress = ftpServerIp.Text;
                Settings.Default.Save();
            };

            ftpUsername.Text = Settings.Default.UserName;
            ftpUsername.TextChanged += (e, o) =>
            {
                Settings.Default.UserName = ftpUsername.Text;
                Settings.Default.Save();
            };

            ftpPassword.Password = Settings.Default.Password;
            ftpPassword.PasswordChanged += (e, o) =>
            {
                Settings.Default.Password = ftpPassword.Password;
                Settings.Default.Save();
            };

            LoadSync();
            LoadFileTree();

            Init();
        }

        public MainWindow()
        {
            if (!CheckForUpdates() )
            {
                StartApp();
            } else
            {
                Process.Start(Path.ChangeExtension(Application.ResourceAssembly.Location, "exe"));
                Application.Current.Shutdown();
            }


        }

        private void Init()
        {
            if (isDownloadedFromFtp)
            {
                Utils.Promo = new PromoContainer { IdElement = "promo" };
                LoadPanel<PromoContainer, PromoContainer.Box>("promo", "promo.json", mainPromoPanel, listPromoPanel, Utils.Promo);

                Utils.MainPromo = new MainPromoContainer { IdElement = "mainpromo" };
                LoadPanel<MainPromoContainer, object>("mainpromo", "mainpromo.json", mainSuperPanel, null, Utils.MainPromo);
                
                Utils.Contact = new HeaderContainer { IdElement = "header" };
                LoadPanel<HeaderContainer, object>("header", "header.json", mainContactPanel, null, Utils.Contact);

                Utils.Services = new ServicesContainer { IdElement = "service" };
                LoadPanel<ServicesContainer, ServicesContainer.Service>("service", "service.json", mainServicePanel, listServicePanel, Utils.Services);

                Utils.Comments = new CommentContainer { IdElement = "comment" };
                LoadPanel<CommentContainer, CommentContainer.Comment>("comment", "comment.json", mainCommentPanel, listCommentPanel, Utils.Comments);

                Utils.Slider = new SliderContainer { IdElement = "slider" };
                LoadPanel<SliderContainer, SliderContainer.Slide>("slider", "slider.json", mainSliderPanel, listSliderPanel, Utils.Slider);

                Utils.Youtube = new YoutubeContainer { IdElement = "youtube" };
                LoadPanel<YoutubeContainer, YoutubeContainer.Youtube>("youtube", "youtube.json", mainYoutubePanel, listYoutubePanel, Utils.Youtube);

                Utils.News = new NewsContainer { IdElement = "news" };
                LoadPanel<NewsContainer, NewsContainer.News>("news", "news.json", mainNewsPanel, listNewsPanel, Utils.News);
            }
        }

        private Thickness side = new Thickness(150, -25, 0, 0);

        private void SavePanel(string FileName, object container)
        {
            string promoPath = Path.Combine(mainPath, jsonPath, FileName);
            string setJson = JsonConvert.SerializeObject(container);
            File.WriteAllText(promoPath, setJson);
        }

        private void ClearPanel(StackPanel PanelElement)
        {
            while (PanelElement.Children.Count > 0)
            {
                PanelElement.Children.RemoveAt(PanelElement.Children.Count - 1);
            }
        }

        private void PixelsValidator(object sender, EventArgs args)
        {
            TextBox tb = (TextBox)sender;
            int result = 0;
            bool isParsed = Int32.TryParse(tb.Text, out result);
            if (!isParsed || result < 0)
            {
                tb.Text = "0";
                MessageBox.Show("Введите положительное число в пикселях");
            }
        }

        private Tuple<UIElement, UIElement> GetUIFromContainer<T>(string caption, string filename, object container, Func<T, string> toUI, Action<T, string> saveToContainer)
        {
            Label lHeaderText = new Label { Content = caption };
            TextBox tbHeaderText = new TextBox { Margin = side, Text = toUI(((T)container)) };

            tbHeaderText.LostFocus += (s, e) => {
                saveToContainer(((T)container), tbHeaderText.Text);
                SavePanel(filename, container);
            };
            return new Tuple<UIElement, UIElement>(lHeaderText, tbHeaderText);
        }

        private void SetTupleToUI(StackPanel uiPanel,Tuple<UIElement, UIElement> uiTuple)
        {
            uiPanel.Children.Add(uiTuple.Item1);
            uiPanel.Children.Add(uiTuple.Item2);
        }

        private void LoadPanel<T, Y>(string idelement, string filename, StackPanel mainPanelElement, StackPanel listPanelElement, T container) where Y : new() where T : IContainer, new()
        {
            string promoPath = Path.Combine(mainPath, jsonPath, filename);
            try
            {
                

                string getedJson = File.ReadAllText(promoPath);
                container = JsonConvert.DeserializeObject<T>(getedJson);

                    ClearPanel(mainPanelElement);

                var listOfSlides = container as IListOfSlides<Y>;
                if (listOfSlides != null)
                {
                    ClearPanel(mainPanelElement);
                    ClearPanel(listPanelElement);

                    Button bAddSlide = new Button { Content = "Добавить слайд" };
                    bAddSlide.Click += (o,e) => {

                    if (listOfSlides.Slides == null)
                        listOfSlides.Slides = new List<Y>();

                    listOfSlides.Slides.Add(new Y());
                    
                    SavePanel(filename, listOfSlides);
                    LoadPanel<T, Y>(idelement, filename, mainPanelElement, listPanelElement, (T)listOfSlides);
                    
                    // MessageBox.Show(refToContainer.Slides.Count.ToString());
                };

                    mainPanelElement.Children.Add(bAddSlide);
                    }

                if (container is IContainer)
                {
                  /*  var uiContainer = GetUIFromContainer<IContainer>("Название блока:", filename, container, (c) => c.IdElement, (c, s) => c.IdElement = s);                  
                    SetTupleToUI(mainPanelElement, uiContainer);*/
                }

                if (container is IImage)
                {

                    var im = container as IImage;
                    Image picture = new Image { Width = 200, Height = 150 };
                    if (!string.IsNullOrWhiteSpace(im.ImgSource))
                    {
                        picture.Source = Utils.BitmapFromUri(new Uri(Path.Combine(mainPath, imgPath, im.ImgSource), UriKind.Absolute));
                    }
                    Guid fileindex = Guid.NewGuid();
                    Button bChangePicture = new Button { Content = "Сменить изображение" };
                    bChangePicture.Click += (s, e) =>
                    {
                        OpenFileDialog openFile = new OpenFileDialog { Filter = "Файлы картинок (*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*" };

                        if (true == openFile.ShowDialog())
                        {

                            string fileFullName = openFile.FileName;
                            string fileExt = fileFullName.Substring(fileFullName.Length - 3);
                            if (!Directory.Exists(Path.Combine(mainPath, imgPath, idelement)))
                                Directory.CreateDirectory(Path.Combine(mainPath, imgPath, idelement));

                            string fileToCombine = Path.Combine(mainPath, imgPath, idelement, string.Format("{0}{1}.{2}", idelement, fileindex, fileExt));
                            if (File.Exists(fileToCombine))
                                File.Delete(fileToCombine);

                            File.Copy(fileFullName, fileToCombine);
                            ((IImage)container).ImgSource = Path.Combine(idelement, string.Format("{0}{1}.{2}", idelement, fileindex, fileExt));
                            SavePanel(filename, container);
                            LoadPanel<T, Y>(idelement, filename, mainPanelElement, listPanelElement, container);
                        }
                    };


                    Label lAlt = new Label { Content = "Описание картинки:" };
                    TextBox tbAlt = new TextBox { Margin= side, Height = 30, Text = ((IImage)container).AltText.ToString() };
                    tbAlt.LostFocus += (s, e) => {
                        ((IImage)container).AltText = tbAlt.Text;
                        SavePanel(filename, container);
                    };

                    mainPanelElement.Children.Add(picture);
                    mainPanelElement.Children.Add(bChangePicture);
                    mainPanelElement.Children.Add(lAlt);
                    mainPanelElement.Children.Add(tbAlt);

                }

                if (container is ISize)
                {
                    Label lHeight = new Label { Content = "Высота:" };

                    TextBox tbHeight = new TextBox { Margin = side, Text = ((ISize)container).Height.ToString() };
                    tbHeight.LostFocus += PixelsValidator;

                    tbHeight.LostFocus += (s, e) => {
                        ((ISize)container).Height = Int32.Parse(((TextBox)s).Text);
                        SavePanel(filename, container);
                    };

                    Label lWidth = new Label { Content = "Ширина:" };
                    TextBox tbWidth = new TextBox { Margin = side, Text = ((ISize)container).Width.ToString() };
                    tbWidth.LostFocus += PixelsValidator;
                    tbWidth.LostFocus += (s, e) => {
                        ((ISize)container).Width = Int32.Parse(tbWidth.Text);
                        SavePanel(filename, container);
                    };

                    mainPanelElement.Children.Add(lHeight);
                    mainPanelElement.Children.Add(tbHeight);

                    mainPanelElement.Children.Add(lWidth);
                    mainPanelElement.Children.Add(tbWidth);
                }

                if (container is IContacts)
                {
                    var uiContainer = GetUIFromContainer<IContacts>("Телефон:", filename, container, (c) => c.Phone, (c, s) => c.Phone = s);
                    SetTupleToUI(mainPanelElement, uiContainer);

                    var uiAltPhone = GetUIFromContainer<IContacts>("Телефон второй:", filename, container, (c) => c.AltPhone, (c, s) => c.AltPhone = s);
                    SetTupleToUI(mainPanelElement, uiAltPhone);

                    var uiAdress = GetUIFromContainer<IContacts>("Адрес:", filename, container, (c) => c.Adress, (c, s) => c.Adress = s);
                    SetTupleToUI(mainPanelElement, uiAdress);

                    var uiEmail = GetUIFromContainer<IContacts>("Email:", filename, container, (c) => c.Email, (c, s) => c.Email = s);
                    SetTupleToUI(mainPanelElement, uiEmail);

                }

                if (container is ISocial)
                {
                    var uiContainer = GetUIFromContainer<ISocial>("Ссылка на Вконтакте:", filename, container, (c) => c.VKHref, (c, s) => c.VKHref = s);
                    SetTupleToUI(mainPanelElement, uiContainer);

                    var uiYouTubeHref = GetUIFromContainer<ISocial>("Ссылка на Ютуб:", filename, container, (c) => c.YouTubeHref, (c, s) => c.YouTubeHref = s);
                    SetTupleToUI(mainPanelElement, uiYouTubeHref);

                    var uiMapHref = GetUIFromContainer<ISocial>("Ссылка на Карту:", filename, container, (c) => c.MapHref, (c, s) => c.MapHref = s);
                    SetTupleToUI(mainPanelElement, uiMapHref);
                }
            
                if (container is IHeader)
                {

                    StackPanel spHeader = new StackPanel { Orientation = Orientation.Horizontal };

                    Label lHeaderText = new Label { Content = "Заголовок+навигация:" };
                    TextBox tbHeaderText = new TextBox { Margin = new Thickness(15,0,0,0), Text = ((IHeader)container).Header.ToString() };

                    tbHeaderText.LostFocus += (s, e) => {
                        ((IHeader)container).Header = tbHeaderText.Text;
                        SavePanel(filename, container);
                    };
                    
                    spHeader.Children.Add(lHeaderText);
                    spHeader.Children.Add(tbHeaderText);
                    mainPanelElement.Children.Add(spHeader);

                }

                if (container is ITextBlock)
                {
                    var uiContainer = GetUIFromContainer<ITextBlock>("Заголовок:", filename, container, (c) => c.HeaderText, (c, s) => c.HeaderText = s);
                    SetTupleToUI(mainPanelElement, uiContainer);

                    var uiRegular = GetUIFromContainer<ITextBlock>("Текст:", filename, container, (c) => c.RegularText, (c, s) => c.RegularText = s);
                    SetTupleToUI(mainPanelElement, uiRegular);
                }

                int i = 0;
                if (container is IListOfSlides<Y>)
                {
                    var sliders = ((IListOfSlides<Y>)container);
                    if (sliders.Slides != null)
                    {                       
                        foreach (var slide in sliders.Slides)
                        {
                            StackPanel sp = new StackPanel { Width = 150 };
                            Image picture = new Image { Width = 100, Height = 100 };
                            int index = i;
                            var image = slide as IImage;
                            if (image != null)
                            {
                                
                                if (!string.IsNullOrWhiteSpace(image.ImgSource))
                                {
                                    picture.Source = Utils.BitmapFromUri(new Uri(Path.Combine(mainPath, imgPath, image.ImgSource), UriKind.Absolute));
                                }
                                
                                Guid fileindex = Guid.NewGuid();
                                Button bChangePicture = new Button { Content = "Сменить изображение" };
                                bChangePicture.Click += (s, e) =>
                                {
                                    OpenFileDialog openFile = new OpenFileDialog { Filter = "Файлы картинок (*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*" };

                                    if (true == openFile.ShowDialog())
                                    {

                                        string fileFullName = openFile.FileName;
                                        string fileExt = fileFullName.Substring(fileFullName.Length - 3);
                                        if (!Directory.Exists(Path.Combine(mainPath, imgPath, idelement)))
                                            Directory.CreateDirectory(Path.Combine(mainPath, imgPath, idelement));

                                        string fileToCombine = Path.Combine(mainPath, imgPath, idelement, string.Format("{0}{1}.{2}", idelement, fileindex, fileExt));
                                        if (File.Exists(fileToCombine))
                                            File.Delete(fileToCombine);

                                        File.Copy(fileFullName, fileToCombine);
                                        ((IImage)sliders.Slides[index]).ImgSource = Path.Combine(idelement, string.Format("{0}{1}.{2}", idelement, fileindex, fileExt));
                                        SavePanel(filename, container);
                                        LoadPanel<T, Y>(idelement, filename, mainPanelElement, listPanelElement, container);
                                    }
                                };
                                


                                Label lAlt = new Label { Content = "Содержимое картинки:", Margin = new Thickness(0, -7, 0, 0) };
                                TextBox tbAlt = new TextBox { Height = 30, Text = image.AltText.ToString(), Margin = new Thickness(0, -7, 0, 0) };
                                tbAlt.LostFocus += (s, e) =>
                                {   image.AltText = tbAlt.Text;
                                    SavePanel(filename, container);
                                };

                                sp.Children.Add(picture);
                                sp.Children.Add(bChangePicture);
                                sp.Children.Add(lAlt);
                                sp.Children.Add(tbAlt);
                                
                            }

                            if (slide is IGoogleVideo)
                            {
                                Label lGoogleVideo = new Label { Content = "Ссылка на Гугл Видео:", Margin = new Thickness(0, -7, 0, 0) };
                                TextBox tbGoogleVideo = new TextBox { Height = 30, Text = ((IGoogleVideo)slide).HrefToGoogle.ToString(), Margin = new Thickness(0, -7, 0, 0) };
                                tbGoogleVideo.LostFocus += (s, e) => {
                                    tbGoogleVideo.Text = tbGoogleVideo.Text.Replace("https://www.youtube.com/watch?v=", "").Replace("https://youtu.be/", "");
                                    ((IGoogleVideo)slide).HrefToGoogle = tbGoogleVideo.Text;
                                                                  SavePanel(filename, container); };
                                sp.Children.Add(lGoogleVideo);
                                sp.Children.Add(tbGoogleVideo);
                            }

                            var sizeble = slide as ISize;
                            if (sizeble != null)
                            {
                                Label lWidth = new Label { Content = "Ширина:", Margin = new Thickness(0, -7, 0, 0) };
                                TextBox tbWidth = new TextBox { Text = sizeble.Width.ToString(), Margin = new Thickness(0, -7, 0, 0) };
                                tbWidth.LostFocus += PixelsValidator;
                                tbWidth.LostFocus += (s, e) => { sizeble.Width = Int32.Parse(tbWidth.Text);
                                                                 SavePanel(filename, container);
                                };

                                Label lHeight = new Label { Content = "Высота:", Margin = new Thickness(0, -7, 0, 0) };
                                TextBox tbHeight = new TextBox { Text = sizeble.Height.ToString(), Margin = new Thickness(0, -7, 0, 0) };
                                tbHeight.LostFocus += PixelsValidator;
                                tbHeight.LostFocus += (s, e) => { sizeble.Height = Int32.Parse(tbHeight.Text);
                                                                  SavePanel(filename, container);
                                };

                                sp.Children.Add(lWidth);
                                sp.Children.Add(tbWidth);
                                sp.Children.Add(lHeight);
                                sp.Children.Add(tbHeight);
                            }


                      




                            var textBlock = slide as ITextBlock;
                            if (textBlock != null)
                            {
                                Label lHeader = new Label { Content = "Заголовок:", Margin = new Thickness(0, -7, 0, 0) };
                                TextBox tbHeader = new TextBox {
                                    TextWrapping = TextWrapping.Wrap,
                                    AcceptsReturn = true,
                                    HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                                    Height = 100, Text = textBlock.HeaderText.ToString(), Margin = new Thickness(0, -7, 0, 0) };
                                tbHeader.LostFocus += (s, e) =>
                                {
                                    textBlock.HeaderText = tbHeader.Text;
                                    SavePanel(filename, container);
                                };

                                Label lRegular = new Label { Content = "Текст:", Margin = new Thickness(0, -7, 0, 0) };
                                TextBox tbRegular = new TextBox {
                                    TextWrapping = TextWrapping.Wrap,
                                    AcceptsReturn = true,
                                    HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                                    Height = 100,
                                    Text = textBlock.RegularText.ToString(), Margin = new Thickness(0, -7, 0, 0) };
                                tbRegular.LostFocus += (s, e) =>
                                {
                                    textBlock.RegularText = tbRegular.Text;
                                    SavePanel(filename, container);
                                };




                                sp.Children.Add(lHeader);
                                sp.Children.Add(tbHeader);

                                sp.Children.Add(lRegular);
                                sp.Children.Add(tbRegular);

                                
                            }

                            var priceBlock = slide as IPrice;
                            if (priceBlock != null)
                            {
                                Label lPrice = new Label { Content = "Цена от:", Margin = new Thickness(0, -7, 0, 0) };
                                TextBox tbPrice = new TextBox { Text = priceBlock.Price.ToString(), Margin = new Thickness(0, -7, 0, 0) };
                                tbPrice.LostFocus += PixelsValidator;
                                tbPrice.LostFocus += (s, e) => {
                                    priceBlock.Price = Int32.Parse(tbPrice.Text);
                                    SavePanel(filename, container);
                                };

                                sp.Children.Add(lPrice);
                                sp.Children.Add(tbPrice);
                            }

                            Button bDeleteThatPromo = new Button { Content = "Удалить слайд" };
                            bDeleteThatPromo.Click += (s, e) =>
                            {
                                if (slide is IImage)
                                {
                                    picture.Source = null;
                                    if (!string.IsNullOrWhiteSpace(((IImage)sliders.Slides[index]).ImgSource))
                                        File.Delete(Path.Combine(mainPath, imgPath, ((IImage)sliders.Slides[index]).ImgSource));
                                }

                                sliders.Slides.Remove(sliders.Slides[index]);

                                SavePanel(filename, container);
                                LoadPanel<T, Y>(idelement, filename, mainPanelElement, listPanelElement, container);
                            };
                            sp.Children.Add(bDeleteThatPromo);

                            listPanelElement.Children.Add(sp);
                            i++;
                        }
                    }
                    
                }


            } catch (FileNotFoundException e)
            {
                container = new T { IdElement = idelement };
                string setJson = JsonConvert.SerializeObject(container);
                File.WriteAllText(promoPath, setJson);
            }
        }
    
        private void ftpTest_Click(object sender, RoutedEventArgs e)
        {
            
            if (Directory.Exists(mainPath)) {
                Directory.Delete(mainPath, true);
            }
            

            sync = new FtpSync(new FluentFTP.FtpClient
            {   Host = ftpServerIp.Text,
                Credentials = new System.Net.NetworkCredential(ftpUsername.Text, ftpPassword.Password) },
                mainPath);
          
            try
            {


                Action<dynamic, Action<string>, Action<bool, dynamic>> action = (ftpsync, report, save) =>
                {
                    ftpsync.GetFileStructureFromServer(ftpsync.ServerFiles.RootFile, "", report);
                    ftpsync.DownloadFiles(report);
                    save(true, ftpsync);
                };
                Action<bool, dynamic> isOk = (isSaved, objToSave) => {
                    isDownloadedFromFtp = isSaved;
                    sync = objToSave;
                };


                Progress prog = new Progress(sync, action, isOk);
                prog.ShowDialog();


                Init();

          
                JsonIt(pathToFileTree, sync.ServerFiles);
                XmlIt("tree.xml", sync.ServerFiles);


            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void ftpTestSync_Click(object sender, RoutedEventArgs e)
        {
            Action<bool, dynamic> isOk = (isSaved, objToSave) => {
                isDownloadedFromFtp = isSaved;
                sync = objToSave;
            };

            Action<dynamic, Action<string>, Action<bool, dynamic>> action = (ftpsync, report, save) =>
            {
                ftpsync.Synchronize(report);
                save(true, ftpsync);
            };
            
            if(isDownloadedFromFtp)
            {
                Progress prog = new Progress(sync, action, isOk);
                prog.ShowDialog();
                Init();

                JsonIt(pathToFileTree, sync.ServerFiles);
                XmlIt("tree.xml", sync.ServerFiles);
            } else
            {
                MessageBox.Show("Вначале необходимо запросить синхронизацию с сервером.");
            }



        }


        private void JsonIt<T>(string path, T fileTree)
        {
            string setJson = JsonConvert.SerializeObject(fileTree);
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, path), setJson);
        }
        private void XmlIt<T>(string path, T fileTree)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(T));
            using (MemoryStream fs = new MemoryStream())
            {
                formatter.Serialize(fs, fileTree);
                File.WriteAllBytes(Path.Combine(Environment.CurrentDirectory, path), fs.ToArray());
            }
        }
    }
}
