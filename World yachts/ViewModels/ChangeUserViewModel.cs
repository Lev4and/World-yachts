using Converters;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using World_yachts.Model.Configurations;
using World_yachts.Model.Database.Interactions;
using World_yachts.Model.Database.Models;
using World_yachts.Services;

namespace World_yachts.ViewModels
{
    public class ChangeUserViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private EntityFramework _eF;
        private ConfigurationUser _config;
        private User _user;
        private SalesPerson _salesPerson;

        public static int IdUser { get; set; }

        public int? SelectedIdRole { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string FamilyName { get; set; }

        public string RoleName { get; set; }

        public ObservableCollection<Role> Roles { get; set; }

        public ChangeUserViewModel(PageService pageService)
        {
            _pageService = pageService;
        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _eF = new EntityFramework();
            _config = ConfigurationUser.GetConfiguration();
            _user = _eF.GetUser(IdUser);
            _salesPerson = _eF.GetSalesPerson(IdUser);

            Login = _user.Login;
            Password = _user.Password;
            FirstName = _salesPerson != null ? _salesPerson.FirstName : "";
            FamilyName = _salesPerson != null ? _salesPerson.FamilyName : "";

            Roles = CollectionConverter<Role>.ConvertToObservableCollection(_eF.GetRoles());

            SelectedIdRole = _user.IdRole;
        });

        public ICommand Change => new DelegateCommand(() =>
        {
            _eF.UpdateUser(IdUser, (int)SelectedIdRole, Password);

            if (RoleName == "Manager")
                _eF.AddSalesPerson(IdUser, FirstName, FamilyName);

            UpdateConfigurationUser();

            MessageBox.Show("Успешное изменение");
        }, () => (Login != null ? Login.Length > 0 : false) && (Password != null ? Password.Length > 0 : false) && (RoleName == "Manager" ? FirstName.Length > 0 && FamilyName.Length > 0 : true));

        private void UpdateConfigurationUser()
        {
            if (_config.IdUser == IdUser)
            {
                _config.TypeUser = RoleName;
                _config.Save();
            }
        }
    }
}
