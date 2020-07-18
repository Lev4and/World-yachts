using DevExpress.Mvvm;
using System.Windows;
using System.Windows.Input;
using World_yachts.Model.Database.Interactions;
using World_yachts.Model.Database.Models;
using World_yachts.Services;

namespace World_yachts.ViewModels
{
    public class ChangePartnerViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private EntityFramework _eF;
        private Partner _partner;

        public static int IdPartner { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public ChangePartnerViewModel(PageService pageService)
        {
            _pageService = pageService;
        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _eF = new EntityFramework();
            _partner = _eF.GetPartner(IdPartner);

            Name = _partner.Name;
            Address = _partner.Address;
            City = _partner.City;
        });

        public ICommand Change => new DelegateCommand(() =>
        {
            if (_eF.UpdatePartner(IdPartner, Name, Address, City))
            {
                MessageBox.Show("Успешное изменение");
            }
            else
            {
                MessageBox.Show("Ошибка");
            }
        }, () => (Name != null ? Name.Length > 0 : false) && (Address != null ? Address.Length > 0 : false) && (City != null ? City.Length > 0 : false));
    }
}
