using DevExpress.Mvvm;
using System.Windows;
using System.Windows.Input;
using World_yachts.Model.Configurations;
using World_yachts.Services;
using World_yachts.Views.Pages;

namespace World_yachts.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ConfigurationUser _config;

        public MenuViewModel(PageService pageService)
        {
            _pageService = pageService;
            _config = ConfigurationUser.GetConfiguration();
        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _config.IsAuthorized = true;
            _config.Save();
        });

        public ICommand Boats => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new Boats());
        }, () => _config.TypeUser == "Administrator" || _config.TypeUser == "Manager");

        public ICommand Users => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new Users());
        }, () => _config.TypeUser == "Administrator" || _config.TypeUser == "Owner");

        public ICommand Accessories => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new Accessories());
        }, () => _config.TypeUser == "Administrator" || _config.TypeUser == "Manager");

        public ICommand Customers => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new Customers());
        }, () => _config.TypeUser == "Administrator" || _config.TypeUser == "Manager");

        public ICommand Partners => new DelegateCommand(() =>
        {

        }, () => _config.TypeUser == "Administrator" || _config.TypeUser == "Manager");

        public ICommand Orders => new DelegateCommand(() =>
        {

        }, () => _config.TypeUser == "Administrator" || _config.TypeUser == "Manager");

        public ICommand Statistics => new DelegateCommand(() =>
        {

        }, () => _config.TypeUser == "Owner");

        public ICommand Exit => new DelegateCommand(() =>
        {
            Application.Current.Shutdown();
        });
    }
}
