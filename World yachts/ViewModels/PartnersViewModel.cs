using Converters;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using World_yachts.Model.Configurations;
using World_yachts.Model.Database.Interactions;
using World_yachts.Model.Database.Models;
using World_yachts.Services;
using World_yachts.Views.Pages;
using World_yachts.Views.Windows;

namespace World_yachts.ViewModels
{
    public class PartnersViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private EntityFramework _eF;
        private ConfigurationUser _config;

        public int? SelectedIdPartner { get; set; }

        public string FilterText { get; set; }

        public string City { get; set; }

        public ObservableCollection<v_cityPartner> CitiesPartners { get; set; }

        public ObservableCollection<v_partner> Partners { get; set; }

        public PartnersViewModel(PageService pageService)
        {
            _pageService = pageService;
            _config = ConfigurationUser.GetConfiguration();
        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _eF = new EntityFramework();

            ResetFilters();
        });

        public ICommand Back => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new Menu());
        });

        public ICommand Add => new DelegateCommand(() =>
        {
            WindowService.ShowWindow(new AddPartner());
        }, () => _config.TypeUser == "Administrator");

        public ICommand Change => new DelegateCommand(() =>
        {
            ChangePartnerViewModel.IdPartner = (int)SelectedIdPartner;

            WindowService.ShowWindow(new ChangePartner());
        }, () => SelectedIdPartner != null && _config.TypeUser == "Administrator");

        public ICommand Remove => new DelegateCommand(() =>
        {
            if (MessageBox.Show("Вы действительно хотите удалить эту запись из базы данных?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _eF.RemovePartner((int)SelectedIdPartner);

                MessageBox.Show("Успешное удаление");

                Searching();
            }
        }, () => SelectedIdPartner != null && _config.TypeUser == "Administrator");

        public ICommand Reset => new DelegateCommand(() =>
        {
            ResetFilters();
        });

        public ICommand Search => new DelegateCommand(() =>
        {
            Searching();
        });

        private void ResetFilters()
        {
            SelectedIdPartner = null;

            FilterText = "";
            City = "";

            CitiesPartners = CollectionConverter<v_cityPartner>.ConvertToObservableCollection(_eF.GetCitiesPartners());

            Searching();
        }

        private void Searching()
        {
            Partners = CollectionConverter<v_partner>.ConvertToObservableCollection(_eF.GetPartners(FilterText,
                                                                                                    City));
        }
    }
}
