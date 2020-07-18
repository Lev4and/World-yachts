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
    public class AddUserViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private EntityFramework _eF;

        public int? SelectedIdRole { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string FamilyName { get; set; }

        public string RoleName { get; set; }

        public ObservableCollection<Role> Roles { get; set; }

        public AddUserViewModel(PageService pageService)
        {
            _pageService = pageService;
        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _eF = new EntityFramework();

            SelectedIdRole = null;
            Login = "";
            Password = "";
            FirstName = "";
            FamilyName = "";
            RoleName = "Manager";

            Roles = CollectionConverter<Role>.ConvertToObservableCollection(_eF.GetRoles());
        });

        public ICommand Add => new DelegateCommand(() =>
        {
            User user = null;

            if(_eF.AddUser((int)SelectedIdRole, Login, Password, out user))
            {
                if(RoleName == "Manager")
                {
                    if(_eF.AddSalesPerson(user.IdUser, FirstName, FamilyName))
                    {
                        MessageBox.Show("Успешное добавление");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка");
                    }
                }
                else
                {
                    MessageBox.Show("Успешное добавление");
                }
            }
            else
            {
                MessageBox.Show("Ошибка");
            }
        }, () => Login.Length > 0 && Password.Length > 0 && SelectedIdRole != null && (RoleName == "Manager" ? FirstName.Length > 0 && FamilyName.Length > 0 : true));
    }
}
