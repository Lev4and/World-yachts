using DevExpress.Mvvm;
using System.Windows;
using System.Windows.Input;
using World_yachts.Model.Configurations;
using World_yachts.Model.Database.Interactions;
using World_yachts.Services;

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

        public ICommand Boats => new DelegateCommand(() =>
        {

        }, () => _config.TypeUser == "Administrator" || _config.TypeUser == "Manager");

        public ICommand Users => new DelegateCommand(() =>
        {

        }, () => _config.TypeUser == "Administrator" || _config.TypeUser == "Owner");

        public ICommand Accessories => new DelegateCommand(() =>
        {

        }, () => _config.TypeUser == "Administrator" || _config.TypeUser == "Manager");

        public ICommand Customers => new DelegateCommand(() =>
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
            new EntityFramework().UpdateValueWasOnline(_config.IdUser);

            Application.Current.Shutdown();
        });
    }
}
