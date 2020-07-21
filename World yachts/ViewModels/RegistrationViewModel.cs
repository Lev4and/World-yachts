using Converters;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Input;
using World_yachts.Model.Configurations;
using World_yachts.Model.Database.Interactions;
using World_yachts.Model.Database.Models;
using World_yachts.Services;
using World_yachts.Views.Pages;

namespace World_yachts.ViewModels
{
    public class RegistrationViewModel : BindableBase
    {
        private int? _selectedIdRole;
        private string _login;
        private string _password;
        private string _repeatPassword;
        private string _selectedRoleName;
        private readonly PageService _pageService;
        private EntityFramework _eF;

        public int? SelectedIdRole
        {
            get { return _selectedIdRole; }
            set
            {
                _selectedIdRole = value;
            }
        }

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;

                CorrectLogin = _login.Length > 0 && !_eF.ContainsUser(_login);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;

                CorrectPassword = _password.Length > 0;
            }
        }

        public string RepeatPassword
        {
            get { return _repeatPassword; }
            set
            {
                _repeatPassword = value;

                CorrectRepeatPassword = _repeatPassword == _password && _repeatPassword.Length > 0;
            }
        }

        public string SelectedRoleName
        {
            get { return _selectedRoleName; }
            set
            {
                _selectedRoleName = value;

                CorrectSelectedRoleName = _selectedRoleName.Length > 0;
            }
        }

        public bool CorrectSelectedRoleName { get; set; }

        public bool CorrectLogin { get; set; }

        public bool CorrectPassword { get; set; }

        public bool CorrectRepeatPassword { get; set; }

        public ObservableCollection<Role> Roles { get; set; }

        public RegistrationViewModel(PageService pageService)
        {
            _pageService = pageService;
        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _eF = new EntityFramework();

            SelectedIdRole = null;
            Login = "";
            Password = "";
            RepeatPassword = "";
            SelectedRoleName = "";

            CorrectSelectedRoleName = false;
            CorrectLogin = false;
            CorrectPassword = false;
            CorrectRepeatPassword = false;

            Roles = CollectionConverter<Role>.ConvertToObservableCollection(_eF.GetRoles());
        });

        public ICommand Back => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new Main());
        });

        public ICommand Registration => new DelegateCommand(() =>
        {
            User user = null;

            _eF.AddUser((int)SelectedIdRole, Login, Password, out user);

            if (SelectedRoleName != "Manager")
            {
                ConfigurationUser config = new ConfigurationUser(user.IdUser, SelectedRoleName);
                config.Save();

                _pageService.ChangePage(new Menu());
            }
            else
            {
                UserInformationSupplementViewModel.IdUser = user.IdUser;

                _pageService.ChangePage(new UserInformationSupplement());
            }

        }, () => CorrectSelectedRoleName == true && CorrectLogin == true && CorrectPassword == true && CorrectRepeatPassword == true);

        private ObservableCollection<Role> GetRoles()
        {
            var oc = new ObservableCollection<Role>();

            _eF.GetRoles().ForEach(r => oc.Add(r));

            return oc;
        }
    }
}
