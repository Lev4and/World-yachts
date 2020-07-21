using DevExpress.Mvvm;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using World_yachts.Model.Configurations;
using World_yachts.Model.Database.Interactions;
using World_yachts.Model.Database.Models;
using World_yachts.Services;
using World_yachts.Views.Pages;

namespace World_yachts.ViewModels
{
    public class AuthorizationViewModel : BindableBase
    {
        private int _countIncorrectAttempts;
        private readonly PageService _pageService;
        private EntityFramework _eF;

        public bool IsLocked { get; set; }

        public bool? IsAuthorizationed { get; set; }

        public int CountIncorrectAttempts
        {
            get { return _countIncorrectAttempts; }
            set 
            { 
                _countIncorrectAttempts = value;

                if (_countIncorrectAttempts >= 3)
                    Locking();
            }
        }

        public int CountSecondsLock { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public AuthorizationViewModel(PageService pageService)
        {
            _pageService = pageService;
        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _countIncorrectAttempts = 0;
            _eF = new EntityFramework();

            IsLocked = false;
            IsAuthorizationed = null;
            CountIncorrectAttempts = 0;
            CountSecondsLock = 0;
            Login = "";
            Password = "";
        });

        public ICommand Back => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new Main());
        });

        public ICommand Authorization => new DelegateCommand(() =>
        {
            v_user user = null;

            if(_eF.CorrectDataUser(Login, Password, out user))
            {
                IsAuthorizationed = true;

                if(_eF.NecessaryChagePassword(user))
                {
                    ChangePasswordViewModel.User = user;

                    _pageService.ChangePage(new ChangePassword());
                }
                else
                {
                    ConfigurationUser config = new ConfigurationUser(user.IdUser, user.RoleName);
                    config.Save();

                    _pageService.ChangePage(new Menu());
                }
            }
            else
            {
                IsAuthorizationed = false;

                CountIncorrectAttempts++;
            }
        }, () => (Login != null ? Login.Length > 0 : false) && (Password != null ? Password.Length > 0 : false) && CountSecondsLock == 0);

        private async void Locking()
        {
            if (CountIncorrectAttempts == 3)
                CountSecondsLock = 15 * (CountIncorrectAttempts - 2);
            else
                CountSecondsLock = 20 * (CountIncorrectAttempts - 2);

            await Task.Run(() => 
            {
                IsLocked = true;

                while (CountSecondsLock > 0)
                {
                    Thread.Sleep(1000);

                    CountSecondsLock--;
                }

                IsLocked = false;
            });
        }
    }
}
