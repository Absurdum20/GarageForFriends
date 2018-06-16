using GarageForFriends.FtpClient;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;


namespace GarageForFriends
{
    /// <summary>
    /// Логика взаимодействия для Progress.xaml
    /// </summary>
    public partial class Progress : Window
    {

        /// <summary>
        /// Отображение прогресса с определенным объектом
        /// </summary>
        /// <param name="sync">Объект с которым работаем</param>
        /// <param name="actionToDo">Что необходимо сделать с объектом: 1-вая переменная сам объект, 2-я - прогресс(строчный), 3-я - что сделать с объектом по окончанию.</param>
        /// <param name="ok">Что сделать с объектом по окончанию</param>
        public Progress(dynamic sync, Action<dynamic, Action<string>, Action<bool, dynamic>> actionToDo, Action<bool, dynamic> ok)
        {
            InitializeComponent();
            Make(sync, actionToDo, ok);
        }

        private void Make(dynamic sync, Action<dynamic, Action<string>, Action<bool, dynamic>> actionToDo, Action<bool, dynamic> ok)
        {
            var task = new Task(() => MakeIt(sync, actionToDo, ok));
            task.RunSynchronously();
        }

        private async Task MakeIt(dynamic ftpSync, Action<dynamic, Action<string>, Action<bool, dynamic>> actionToDo, Action<bool, dynamic> ok)
        {
            var task = new Task(() =>
            {
                Action<string> ProgChanged = (s) =>
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        this.lProgress.Content = s;
                    }));
                };

                Action CloseWindow = () =>
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        Close();
                    }));
                };

                actionToDo(ftpSync, ProgChanged, ok);
                CloseWindow();

            });

            task.Start();
            await task;

        }
    }
}
