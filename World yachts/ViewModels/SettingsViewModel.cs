using DevExpress.Mvvm;
using System.Windows;
using System.Windows.Input;
using World_yachts.Model.Configurations;
using World_yachts.Services;
using World_yachts.Views.Pages;

namespace World_yachts.ViewModels
{
    public class SettingsViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private ConfigurationDatabase _config;

        public int CommandTimeout { get; set; }

        public string ServerAddress { get; set; }

        public string DatabaseName { get; set; }

        public SettingsViewModel(PageService pageService)
        {
            _pageService = pageService;
        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _config = ConfigurationDatabase.GetConfiguration();

            CommandTimeout = _config.CommandTimeout;
            ServerAddress = _config.ServerAddress;
            DatabaseName = _config.DatabaseName;
        });

        public ICommand SaveChanges => new DelegateCommand(() =>
        {
            RewriteConfiguration();

            MessageBox.Show("Успешное сохранение");
        }, () => CommandTimeout >= 0 && (ServerAddress != null ? ServerAddress.Length > 0 : false) && (DatabaseName != null ? DatabaseName.Length > 0 : false));

        public ICommand Back => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new Main());
        });

        private void RewriteConfiguration()
        {
            var config = new ConfigurationDatabase(CommandTimeout, ServerAddress, DatabaseName);
            config.Save();
        }
    }
}
