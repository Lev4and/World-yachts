using DevExpress.Mvvm;
using System.Windows;
using System.Windows.Input;
using World_yachts.Model.Database.Interactions;

namespace World_yachts.ViewModels
{
    public class AddPartnerViewModel : BindableBase
    {
        private EntityFramework _eF;

        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public AddPartnerViewModel()
        {

        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _eF = new EntityFramework();

            Name = "";
            Address = "";
            City = "";
        });

        public ICommand Add => new DelegateCommand(() =>
        {
            if(_eF.AddPartner(Name, Address, City))
            {
                MessageBox.Show("Успешное добавление");
            }
            else
            {
                MessageBox.Show("Ошибка");
            }
        }, () => (Name != null ? Name.Length > 0 : false) && (Address != null ? Address.Length > 0 : false) && (City != null ? City.Length > 0 : false));
    }
}
