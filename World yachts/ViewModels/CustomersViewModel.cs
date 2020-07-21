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
    public class CustomersViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private EntityFramework _eF;
        private ConfigurationUser _config;

        public int? SelectedIdCustomer { get; set; }

        public string FilterText { get; set; }

        public string OrganisationName { get; set; }

        public string City { get; set; }

        public ObservableCollection<v_cityCustomer> CitiesCustomers { get; set; }

        public ObservableCollection<v_organisationNameCustomer> OrganisationsNameCustomers { get; set; }

        public ObservableCollection<v_customer> Customers { get; set; }

        public CustomersViewModel(PageService pageService)
        {
            _pageService = pageService;
            _config = ConfigurationUser.GetConfiguration();
        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _eF = new EntityFramework();

            ResetFilters();
        });

        public ICommand Reset => new DelegateCommand(() =>
        {
            ResetFilters();
        });

        public ICommand Search => new DelegateCommand(() =>
        {
            Searching();
        });

        public ICommand Add => new DelegateCommand(() =>
        {
            WindowService.ShowWindow(new AddCustomer());
        });

        public ICommand Change => new DelegateCommand(() =>
        {
            ChangeCustomerViewModel.IdCustomer = (int)SelectedIdCustomer;

            WindowService.ShowWindow(new ChangeCustomer());
        }, () => SelectedIdCustomer != null && _config.TypeUser == "Administrator");

        public ICommand Remove => new DelegateCommand(() =>
        {
            if (MessageBox.Show("Вы действительно хотите удалить эту запись из базы данных?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _eF.RemoveCustomer((int)SelectedIdCustomer);

                MessageBox.Show("Успешное удаление");

                Searching();
            }

        }, () => SelectedIdCustomer != null && _config.TypeUser == "Administrator");

        public ICommand Back => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new Menu());
        });

        private void ResetFilters()
        {
            SelectedIdCustomer = null;

            FilterText = "";
            OrganisationName = "";
            City = "";

            CitiesCustomers = CollectionConverter<v_cityCustomer>.ConvertToObservableCollection(_eF.GetCitiesCustomers());
            OrganisationsNameCustomers = CollectionConverter<v_organisationNameCustomer>.ConvertToObservableCollection(_eF.GetOrganisationsNamesCustomers());

            Searching();
        }

        private void Searching()
        {
            Customers = CollectionConverter<v_customer>.ConvertToObservableCollection(_eF.GetCustomers(FilterText, 
                                                                                                       OrganisationName, 
                                                                                                       City));
        }
    }
}
