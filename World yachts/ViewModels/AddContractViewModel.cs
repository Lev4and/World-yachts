using Converters;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using World_yachts.Model.Configurations;
using World_yachts.Model.Database.Interactions;
using World_yachts.Model.Database.Models;

namespace World_yachts.ViewModels
{
    public class AddContractViewModel : BindableBase
    {
        private int _idSalesPerson;
        private int? _selectedIdBoat;
        private ConfigurationUser _config;
        private EntityFramework _eF;
        private v_boat _boat;
        private List<v_accessory> _listSelectedAccessories;
        private ObservableCollection<v_accessory> _accessories;

        public int DepositPayed { get; set; }

        public int ContractTotalPrice { get; set; }

        public int ContractTotalPriceInclVAT { get; set; }

        public int? SelectedIdCustomer { get; set; }

        public int? SelectedIdBoat
        {
            get { return _selectedIdBoat; }
            set
            {
                _selectedIdBoat = value;

                _boat = _eF != null ? _eF.GetBoat((int)SelectedIdBoat) : null;

                Accessories.Clear();
                Accessories = _boat != null ? CollectionConverter<v_accessory>.ConvertToObservableCollection(_eF.GetAccessories(_boat.Model)) : null;

                Calculating();
            }
        }

        public string DeliveryAddress { get; set; }

        public string City { get; set; }

        public ObservableCollection<string> SelectedAccessories { get; set; }

        public ObservableCollection<string> StringListAccessories { get; set; }

        public ObservableCollection<v_accessory> Accessories
        {
            get { return _accessories; }
            set
            {
                _accessories = value;

                UpdateStringListAccessories();
            }
        }

        public ObservableCollection<v_customer> Customers { get; set; }

        public ObservableCollection<v_boatSimplifiedInformation> Boats { get; set; }

        public AddContractViewModel()
        {

        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _eF = new EntityFramework();
            _config = ConfigurationUser.GetConfiguration();
            _listSelectedAccessories = new List<v_accessory>();
            _idSalesPerson = _config.IdUser;

            SelectedIdBoat = null;
            SelectedIdCustomer = null;

            DepositPayed = 0;
            ContractTotalPrice = 0;
            ContractTotalPriceInclVAT = 0;

            DeliveryAddress = "";
            City = "";

            SelectedAccessories = new ObservableCollection<string>();

            Accessories = new ObservableCollection<v_accessory>();
            Customers = CollectionConverter<v_customer>.ConvertToObservableCollection(_eF.GetCustomers());
            Boats = CollectionConverter<v_boatSimplifiedInformation>.ConvertToObservableCollection(_eF.GetBoats());
        });

        public ICommand Add => new DelegateCommand(() =>
        {
            UpdateListSelectedAccessories();

            if (MessageBox.Show($"Вы действительно хотите продолжить?\n\nDepositPayed: {DepositPayed}\nContractTotalPrice: {ContractTotalPrice}\nContractTotalPriceInclVAT: {ContractTotalPriceInclVAT}", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var list = new List<int>();

                _listSelectedAccessories.ForEach(l => list.Add(l.IdAccessory));

                _eF.EnterIntoContract(_idSalesPerson, (int)SelectedIdCustomer, (int)SelectedIdBoat, DeliveryAddress, City, list, DepositPayed, ContractTotalPrice, ContractTotalPriceInclVAT);

                MessageBox.Show("Успешное заключение договора");
            }
        }, () => SelectedIdBoat != null && SelectedIdCustomer != null && DeliveryAddress.Length > 0 && City.Length > 0 && DepositPayed >= 0 && ContractTotalPrice >= 0 && ContractTotalPriceInclVAT >= 0);

        private void Calculating()
        {
            if(SelectedIdBoat != null)
            {
                DepositPayed = 0;
                ContractTotalPrice = 0;
                ContractTotalPriceInclVAT = 0;

                _boat = _eF.GetBoat((int)SelectedIdBoat);

                ContractTotalPrice = _boat.BasePrice;
                ContractTotalPriceInclVAT = (int)((double)_boat.BasePrice + ((double)_boat.BasePrice * (double)_boat.VAT));

                _listSelectedAccessories.ForEach(lsa =>
                {
                    ContractTotalPrice += lsa.Price;
                    ContractTotalPriceInclVAT += (int)((double)lsa.Price + ((double)lsa.Price * (double)lsa.VAT));
                });

                DepositPayed = (int)((double)ContractTotalPrice / (double)3);
            }
        }

        private void UpdateStringListAccessories()
        {
            var stringListAccessories = new List<string>();

            foreach (var accessory in Accessories)
                stringListAccessories.Add(accessory.AccName);

            StringListAccessories = CollectionConverter<string>.ConvertToObservableCollection(stringListAccessories);
        }

        private bool UpdateListSelectedAccessories()
        {
            _listSelectedAccessories = new List<v_accessory>();

            SelectedAccessories.ForEach(sa => _listSelectedAccessories.Add(Accessories.Single(a => a.AccName == sa)));

            Calculating();

            return true;
        }
    }
}
