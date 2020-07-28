using DevExpress.Mvvm;
using System.Data.Entity.Core;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using World_yachts.Model.Database.Interactions;
using World_yachts.Services;
using World_yachts.Views.Pages;

namespace World_yachts.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private EntityFramework _eF;

        public bool IsBackgroundTaskRunning { get; set; }

        public MainViewModel(PageService pageService)
        {
            _pageService = pageService;
        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _eF = new EntityFramework();

            IsBackgroundTaskRunning = true;

            Loading();
        });

        public async void Loading()
        {
            await Task.Run(() => 
            {
                Thread.Sleep(2000);

                try
                {
                    _eF.BlockingUsers();
                }
                catch(EntityException entityEx)
                {
                    MessageBox.Show("Сервер базы данных отключен или указан неверный адрес сервера", "Ошибка");
                }

                IsBackgroundTaskRunning = false;
            });
        }

        public ICommand Authorization => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new Authorization());
        }, () => IsBackgroundTaskRunning == false);

        public ICommand Registration => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new Registration());
        }, () => IsBackgroundTaskRunning == false);

        public ICommand Settings => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new Settings());
        }, () => IsBackgroundTaskRunning == false);

        public ICommand Exit => new DelegateCommand(() =>
        {
            Application.Current.Shutdown();
        }, () => IsBackgroundTaskRunning == false);
    }
}
