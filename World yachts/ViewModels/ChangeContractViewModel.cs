using Converters;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using World_yachts.Model.Database.Interactions;
using World_yachts.Model.Database.Models;
using World_yachts.Services;

namespace World_yachts.ViewModels
{
    public class ChangeContractViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private EntityFramework _eF;
        private Customer _customer;
        private v_contract2 _contract;
        private Order _order;
        private v_boat _boat;

        public static int IdContract { get; set; }

        public int DepositPayed { get; set; }

        public int ContractTotalPrice { get; set; }

        public int ContractTotalPriceInclVAT { get; set; }

        public string FullNameCustomer { get; set; }

        public string ModelBoat { get; set; }

        public string SelectedAccessoriesAtOrder { get; set; }

        public string DeliveryAddress { get; set; }

        public string City { get; set; }

        public string SelectedProductionProcess { get; set; }

        public ObservableCollection<string> ProductionProcess { get; set; }

        public ChangeContractViewModel(PageService pageService)
        {
            _pageService = pageService;
        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _eF = new EntityFramework();
            _contract = _eF.GetContractExtendedVersion(IdContract);
            _order = _eF.GetOrder(_contract.IdOrder);
            _customer = _eF.GetCustomer(_order.IdCustomer);
            _boat = _eF.GetBoat(_order.IdBoat);

            FullNameCustomer = $"{_customer.FirstName + " " + _customer.FamilyName}";
            ModelBoat = _boat.Model;
            SelectedAccessoriesAtOrder = _contract.SelectedAccessoriesAtOrder;
            DeliveryAddress = _order.DeliveryAddress;
            City = _order.City;
            DepositPayed = _contract.DepositPayed;
            ContractTotalPrice = _contract.ContractTotalPrice;
            ContractTotalPriceInclVAT = _contract.ContractTotalPriceInclVAT;
            SelectedProductionProcess = _contract.ProductionProcess;

            ProductionProcess = CollectionConverter<string>.ConvertToObservableCollection(_eF.GetProductionProcess());
        });

        public ICommand Change => new DelegateCommand(() =>
        {
            _eF.UpdateContract(IdContract, DepositPayed, SelectedProductionProcess);

            MessageBox.Show("Успешное изменение");
        }, () => DepositPayed >= 0 && (SelectedProductionProcess != null ? SelectedProductionProcess.Length > 0 : false));
    }
}
