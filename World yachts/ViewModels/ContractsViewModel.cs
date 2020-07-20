using Converters;
using DevExpress.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using World_yachts.Model.Configurations;
using World_yachts.Model.Database.Interactions;
using World_yachts.Model.Database.Models;
using World_yachts.Model.Logic;
using World_yachts.Services;
using World_yachts.Views.Pages;
using World_yachts.Views.Windows;

namespace World_yachts.ViewModels
{
    public class ContractsViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private ConfigurationUser _config;
        private EntityFramework _eF;

        public int? SelectedIdContract { get; set; }

        public int MinValueDepositPayed { get; set; }

        public int MinValueContractTotalPrice { get; set; }

        public int MinValueContractTotalPriceInclVAT { get; set; }

        public int MaxValueDepositPayed { get; set; }

        public int MaxValueContractTotalPrice { get; set; }

        public int MaxValueContractTotalPriceInclVAT { get; set; }

        public int BeginValueDepositPayed { get; set; }

        public int BeginValueContractTotalPrice { get; set; }

        public int BeginValueContractTotalPriceInclVAT { get; set; }

        public int EndValueDepositPayed { get; set; }

        public int EndValueContractTotalPrice { get; set; }

        public int EndValueContractTotalPriceInclVAT { get; set; }

        public DateTime MinValueDateOfConclusionContract { get; set; }

        public DateTime MaxValueDateOfConclusionContract { get; set; }

        public DateTime BeginValueDateOfConclusionContract { get; set; }

        public DateTime EndValueDateOfConclusionContract { get; set; }

        public ObservableCollection<string> ListSelectedModelsBoats { get; set; }

        public ObservableCollection<string> ListSelectedAccessoriesAtOrder { get; set; }

        public ObservableCollection<string> ListSelectedProductionProcess { get; set; }

        public ObservableCollection<string> ListSelectedDeliveryAddressOrders { get; set; }

        public ObservableCollection<string> ListSelectedCitiesOrders { get; set; }

        public ObservableCollection<string> Boats { get; set; }

        public ObservableCollection<string> Accessories { get; set; }

        public ObservableCollection<string> ProductionProcess { get; set; }

        public ObservableCollection<string> DeliveryAddressOrders { get; set; }

        public ObservableCollection<string> CitiesOrders { get; set; }

        public ObservableCollection<v_contract> Contracts { get; set; }

        public ContractsViewModel(PageService pageService)
        {
            _pageService = pageService;
        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _config = ConfigurationUser.GetConfiguration();
            _eF = new EntityFramework();

            ListSelectedModelsBoats = new ObservableCollection<string>();
            ListSelectedAccessoriesAtOrder = new ObservableCollection<string>();
            ListSelectedProductionProcess = new ObservableCollection<string>();
            ListSelectedDeliveryAddressOrders = new ObservableCollection<string>();
            ListSelectedCitiesOrders = new ObservableCollection<string>();

            ResetFilters();
        });

        public ICommand Back => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new Menu());
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
            WindowService.ShowWindow(new AddContract());
        });

        public ICommand Change => new DelegateCommand(() =>
        {
            ChangeContractViewModel.IdContract = (int)SelectedIdContract;

            WindowService.ShowWindow(new ChangeContract());
        }, () => SelectedIdContract != null);

        public ICommand Remove => new DelegateCommand(() =>
        {
            if (MessageBox.Show("Вы действительно хотите удалить эту запись из базы данных?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _eF.RemoveContract((int)SelectedIdContract);

                MessageBox.Show("Успешное удаление");

                Searching();
            }
        }, () => SelectedIdContract != null);

        private void ResetFilters()
        {
            SelectedIdContract = null;

            MinValueDepositPayed = _eF.GetMinDepositPayed();
            MinValueContractTotalPrice = _eF.GetMinContractTotalPrice();
            MinValueContractTotalPriceInclVAT = _eF.GetMinContractTotalPriceInclVAT();
            MinValueDateOfConclusionContract = _eF.GetMinDateOfConclusionContract();
            MaxValueDepositPayed = _eF.GetMaxDepositPayed();
            MaxValueContractTotalPrice = _eF.GetMaxContractTotalPrice();
            MaxValueContractTotalPriceInclVAT = _eF.GetMaxContractTotalPriceInclVAT();
            MaxValueDateOfConclusionContract = _eF.GetMaxDateOfConclusionContract();

            BeginValueDepositPayed = MinValueDepositPayed;
            BeginValueContractTotalPrice = MinValueContractTotalPrice;
            BeginValueContractTotalPriceInclVAT = MinValueContractTotalPriceInclVAT;
            BeginValueDateOfConclusionContract = MinValueDateOfConclusionContract;
            EndValueDepositPayed = MaxValueDepositPayed;
            EndValueContractTotalPrice = MaxValueContractTotalPrice;
            EndValueContractTotalPriceInclVAT = MaxValueContractTotalPriceInclVAT;
            EndValueDateOfConclusionContract = MaxValueDateOfConclusionContract;

            Boats = CollectionConverter<String>.ConvertToObservableCollection(_eF.GetStringListBoats());
            Accessories = CollectionConverter<String>.ConvertToObservableCollection(_eF.GetStringListAccessories());
            ProductionProcess = CollectionConverter<String>.ConvertToObservableCollection(_eF.GetProductionProcess());
            DeliveryAddressOrders = CollectionConverter<String>.ConvertToObservableCollection(_eF.GetStringListDeliveryAddressOrders());
            CitiesOrders = CollectionConverter<String>.ConvertToObservableCollection(_eF.GetStringListCitiesOrders());

            ListSelectedModelsBoats.Clear();
            ListSelectedAccessoriesAtOrder.Clear();
            ListSelectedProductionProcess.Clear();
            ListSelectedDeliveryAddressOrders.Clear();
            ListSelectedCitiesOrders.Clear();

            Searching();
        }

        private void Searching()
        {
            Contracts = CollectionConverter<v_contract>.ConvertToObservableCollection(_eF.GetContracts(ListSelectedModelsBoats.ToList(),
                                                                                                        ListSelectedAccessoriesAtOrder.ToList(),
                                                                                                        ListSelectedProductionProcess.ToList(),
                                                                                                        ListSelectedDeliveryAddressOrders.ToList(),
                                                                                                        ListSelectedCitiesOrders.ToList(),
                                                                                                        new Range<DateTime>(BeginValueDateOfConclusionContract, EndValueDateOfConclusionContract),
                                                                                                        new Range<int>(BeginValueDepositPayed, EndValueDepositPayed), 
                                                                                                        new Range<int>(BeginValueContractTotalPrice, EndValueContractTotalPrice), 
                                                                                                        new Range<int>(BeginValueContractTotalPriceInclVAT, EndValueContractTotalPriceInclVAT)));
        }
    }
}
