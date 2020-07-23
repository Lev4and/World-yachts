using DevExpress.Mvvm;
using System.Windows.Controls;
using System.Windows.Input;
using World_yachts.Model.Configurations;
using World_yachts.Model.Database.Interactions;
using World_yachts.Services;
using World_yachts.Views.Pages;

namespace World_yachts.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private EntityFramework _eF;
        private ConfigurationUser _config;

        public Page PageSource { get; set; }

        public MainWindowViewModel(PageService pageService)
        {
            _pageService = pageService;
            _eF = new EntityFramework();

            _pageService.OnPageChanged += (page) => PageSource = page;
            _pageService.ChangePage(new Main());
        }

        public ICommand Closed => new DelegateCommand(() =>
        {
            _config = ConfigurationUser.GetConfiguration();

            if (_config.IsAuthorized)
            {
                _config.IsAuthorized = false;
                _config.Save();

                _eF.UpdateValueWasOnline(_config.IdUser);
            }
        });
    }
}
