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
    public class BoatsViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private EntityFramework _eF;
        private ConfigurationUser _config;

        public bool? ThereIsMast { get; set; }

        public int? SelectedIdBoat { get; set; }

        public int MinValueBasePrice { get; set; }

        public int MinValueNumberOfRowers { get; set; }

        public int MaxValueBasePrice { get; set; }

        public int MaxValueNumberOfRowers { get; set; }

        public int BeginValueBasePrice { get; set; }

        public int BeginValueNumberOfRowers { get; set; }

        public int EndValueBasePrice { get; set; }

        public int EndValueNumberOfRowers { get; set; }

        public double MinValueVATBoat { get; set; }

        public double MaxValueVATBoat { get; set; }

        public double BeginValueVATBoat { get; set; }

        public double EndValueVATBoat { get; set; }

        public string FilterText { get; set; }

        public DateTime MinValueProductionStartDate { get; set; }

        public DateTime MaxValueProductionStartDate { get; set; }

        public DateTime BeginValueProductionStartDate { get; set; }

        public DateTime EndValueProductionStartDate { get; set; }

        public ObservableCollection<string> BoatTypes { get; set; }

        public ObservableCollection<string> Colours { get; set; }

        public ObservableCollection<string> TypesModel { get; set; }

        public ObservableCollection<string> Woods { get; set; }

        public ObservableCollection<string> ListSelectedBoatTypes { get; set; }

        public ObservableCollection<string> ListSelectedColours { get; set; }

        public ObservableCollection<string> ListSelectedTypesModel { get; set; }

        public ObservableCollection<string> ListSelectedWoods { get; set; }

        public ObservableCollection<v_boat> Boats { get; set; }

        public BoatsViewModel(PageService pageService)
        {
            _pageService = pageService;
        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _eF = new EntityFramework();
            _config = ConfigurationUser.GetConfiguration();

            ListSelectedBoatTypes = new ObservableCollection<string>();
            ListSelectedColours = new ObservableCollection<string>();
            ListSelectedTypesModel = new ObservableCollection<string>();
            ListSelectedWoods = new ObservableCollection<string>();

            ResetFilters();
        });

        private void ResetFilters()
        {
            ThereIsMast = null;
            SelectedIdBoat = null;

            MinValueBasePrice = _eF.GetMinBasePrice();
            MinValueNumberOfRowers = _eF.GetMinNumberOfRowers();
            MinValueVATBoat = _eF.GetMinVATBoat();
            MinValueProductionStartDate = _eF.GetMinProductionStartDate();
            MaxValueBasePrice = _eF.GetMaxBasePrice();
            MaxValueNumberOfRowers = _eF.GetMaxNumberOfRowers();
            MaxValueVATBoat = _eF.GetMaxVATBoat();
            MaxValueProductionStartDate = _eF.GetMaxProductionStartDate();

            BeginValueBasePrice = MinValueBasePrice;
            BeginValueNumberOfRowers = MinValueNumberOfRowers;
            BeginValueVATBoat = MinValueVATBoat;
            BeginValueProductionStartDate = MinValueProductionStartDate;

            EndValueBasePrice = MaxValueBasePrice;
            EndValueNumberOfRowers = MaxValueNumberOfRowers;
            EndValueVATBoat = MaxValueVATBoat;
            EndValueProductionStartDate = MaxValueProductionStartDate;

            FilterText = "";

            BoatTypes = CollectionConverter<string>.ConvertToObservableCollection(_eF.GetStringListBoatTypes());
            Colours = CollectionConverter<string>.ConvertToObservableCollection(_eF.GetStringListColours());
            TypesModel = CollectionConverter<string>.ConvertToObservableCollection(_eF.GetTypesModel());
            Woods = CollectionConverter<string>.ConvertToObservableCollection(_eF.GetStringListWoods());

            ListSelectedBoatTypes.Clear();
            ListSelectedColours.Clear();
            ListSelectedTypesModel.Clear();
            ListSelectedWoods.Clear();

            Searching();
        }

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
            WindowService.ShowWindow(new AddBoat());
        });

        public ICommand Change => new DelegateCommand(() =>
        {
            ChangeBoatViewModel.IdBoat = (int)SelectedIdBoat;

            WindowService.ShowWindow(new ChangeBoat());
        }, () => SelectedIdBoat != null);

        public ICommand Remove => new DelegateCommand(() =>
        {
            if (MessageBox.Show("Вы действительно хотите удалить эту запись из базы данных?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _eF.RemoveBoat((int)SelectedIdBoat);

                MessageBox.Show("Успешное удаление");

                Searching();
            }
        }, () => SelectedIdBoat != null);

        private void Searching()
        {
            Boats = CollectionConverter<v_boat>.ConvertToObservableCollection(_eF.GetBoats(FilterText,
                                                                                           ListSelectedBoatTypes.ToList(),
                                                                                           ListSelectedTypesModel.ToList(),
                                                                                           new Range<int>(BeginValueNumberOfRowers, EndValueNumberOfRowers),
                                                                                           ThereIsMast,
                                                                                           ListSelectedColours.ToList(),
                                                                                           ListSelectedWoods.ToList(),
                                                                                           new Range<int>(BeginValueBasePrice, EndValueBasePrice),
                                                                                           new Range<double>(BeginValueVATBoat, EndValueVATBoat),
                                                                                           new Range<DateTime>(BeginValueProductionStartDate, EndValueProductionStartDate)));
        }
    }
}
