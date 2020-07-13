using DevExpress.Mvvm;
using System.Windows;
using System.Windows.Input;
using World_yachts.Services;
using World_yachts.Views.Pages;

namespace World_yachts.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private readonly PageService _pageService;

        public MainViewModel(PageService pageService)
        {
            _pageService = pageService;
        }

        public ICommand Authorization => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new Authorization());
        });

        public ICommand Exit => new DelegateCommand(() =>
        {
            Application.Current.Shutdown();
        });
    }
}
