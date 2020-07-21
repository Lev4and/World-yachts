using Converters;
using DevExpress.Mvvm;
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
    public class AccessoriesViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private ConfigurationUser _config;
        private EntityFramework _eF;

        public int MinValuePrice { get; set; }

        public int MinValueInventory { get; set; }

        public int MinValueOrderLevel { get; set; }

        public int MinValueOrderBatch { get; set; }

        public int MaxValuePrice { get; set; }

        public int MaxValueInventory { get; set; }

        public int MaxValueOrderLevel { get; set; }

        public int MaxValueOrderBatch { get; set; }

        public int BeginValuePrice { get; set; }

        public int BeginValueInventory { get; set; }

        public int BeginValueOrderLevel { get; set; }

        public int BeginValueOrderBatch { get; set; }

        public int EndValuePrice { get; set; }

        public int EndValueInventory { get; set; }

        public int EndValueOrderLevel { get; set; }

        public int EndValueOrderBatch { get; set; }

        public int? SelectedIdAccessory { get; set; }

        public double MinValueVATAccessory { get; set; }

        public double MaxValueVATAccessory { get; set; }

        public double BeginValueVATAccessory { get; set; }

        public double EndValueVATAccessory { get; set; }

        public string FilterText { get; set; }

        public ObservableCollection<string> ListSelectedBoats { get; set; }

        public ObservableCollection<string> ListSelectedPartners { get; set; }

        public ObservableCollection<v_accessory> Accessories { get; set; }

        public ObservableCollection<string> Boats { get; set; }

        public ObservableCollection<string> Partners { get; set; }

        public AccessoriesViewModel(PageService pageService)
        {
            _pageService = pageService;
            _config = ConfigurationUser.GetConfiguration();
        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _eF = new EntityFramework();

            ListSelectedBoats = new ObservableCollection<string>();
            ListSelectedPartners = new ObservableCollection<string>();

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

        public ICommand Back => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new Menu());
        });

        public ICommand Add => new DelegateCommand(() =>
        {
            WindowService.ShowWindow(new AddAccessory());
        }, () => _config.TypeUser == "Administrator");

        public ICommand Change => new DelegateCommand(() =>
        {
            ChangeAccessoryViewModel.IdAccessory = (int)SelectedIdAccessory;

            WindowService.ShowWindow(new ChangeAccessory());
        }, () => SelectedIdAccessory != null && _config.TypeUser == "Administrator");

        public ICommand Remove => new DelegateCommand(() =>
        {
            if (MessageBox.Show("Вы действительно хотите удалить эту запись из базы данных?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _eF.RemoveAccessory((int)SelectedIdAccessory);

                MessageBox.Show("Успешное удаление");

                Searching();
            }
        }, () => SelectedIdAccessory != null && _config.TypeUser == "Administrator");

        private void ResetFilters()
        {
            SelectedIdAccessory = null;

            MinValueInventory = _eF.GetMinInventory();
            MinValueOrderBatch = _eF.GetMinOrderBatch();
            MinValueOrderLevel = _eF.GetMinOrderLevel();
            MinValuePrice = _eF.GetMinPriceAccessory();
            MinValueVATAccessory = _eF.GetMinVATAccessory();
            MaxValueInventory = _eF.GetMaxInventory();
            MaxValueOrderBatch = _eF.GetMaxOrderBatch();
            MaxValueOrderLevel = _eF.GetMaxOrderLevel();
            MaxValuePrice = _eF.GetMaxPriceAccessory();
            MaxValueVATAccessory = _eF.GetMaxVATAccessory();

            BeginValueInventory = MinValueInventory;
            BeginValueOrderBatch = MinValueOrderBatch;
            BeginValueOrderLevel = MinValueOrderLevel;
            BeginValuePrice = MinValuePrice;
            BeginValueVATAccessory = MinValueVATAccessory;

            EndValueInventory = MaxValueInventory;
            EndValueOrderBatch = MaxValueOrderBatch;
            EndValueOrderLevel = MaxValueOrderLevel;
            EndValuePrice = MaxValuePrice;
            EndValueVATAccessory = MaxValueVATAccessory;

            FilterText = "";

            Boats = CollectionConverter<string>.ConvertToObservableCollection(_eF.GetStringListBoats());
            Partners = CollectionConverter<string>.ConvertToObservableCollection(_eF.GetStringListPartners());

            ListSelectedBoats.Clear();
            ListSelectedPartners.Clear();

            Searching();
        }

        private void Searching()
        {
            Accessories = CollectionConverter<v_accessory>.ConvertToObservableCollection(_eF.GetAccessories(FilterText,
                                                                                         new Range<int>(BeginValuePrice, EndValuePrice), 
                                                                                         new Range<double>(BeginValueVATAccessory, EndValueVATAccessory), 
                                                                                         new Range<int>(BeginValueInventory, EndValueInventory), 
                                                                                         new Range<int>(BeginValueOrderLevel, EndValueOrderLevel),
                                                                                         new Range<int>(BeginValueOrderBatch, EndValueOrderBatch), 
                                                                                         ListSelectedPartners.ToList(),
                                                                                         ListSelectedBoats.ToList()));
        }
    }
}
