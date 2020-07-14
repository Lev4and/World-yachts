using DevExpress.Mvvm;
using System.Windows.Input;
using World_yachts.Model.Configurations;
using World_yachts.Model.Database.Interactions;
using World_yachts.Model.Database.Models;
using World_yachts.Services;
using World_yachts.Views.Pages;

namespace World_yachts.ViewModels
{
    public class ChangePasswordViewModel : BindableBase
    {
        public static v_user User { get; set; }

        private string _oldPassword;
        private string _newPassword;
        private string _repeatNewPassword;
        private readonly PageService _pageService;
        private readonly EntityFramework _eF;

        public bool CorrectOldPassword { get; set; }

        public bool CorrectNewPassword { get; set; }

        public bool CorrectRepeatNewPassword { get; set; }

        public string OldPassword
        {
            get { return _oldPassword; }
            set 
            {
                _oldPassword = value;

                if(User != null)
                    CorrectOldPassword = _oldPassword == User.Password ? true : false;
            }
        }

        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                _newPassword = value;

                if (User != null)
                    CorrectNewPassword = _newPassword.Length > 0 && _newPassword != OldPassword ? true : false;
            }
        }

        public string RepeatNewPassword
        {
            get { return _repeatNewPassword; }
            set
            {
                _repeatNewPassword = value;

                if (User != null)
                    CorrectRepeatNewPassword = _repeatNewPassword.Length > 0 && _repeatNewPassword == NewPassword ? true : false;
            }
        }

        public ChangePasswordViewModel(PageService pageService)
        {
            _pageService = pageService;
            _eF = new EntityFramework();

            CorrectOldPassword = false;
            CorrectNewPassword = false;
            CorrectRepeatNewPassword = false;

            OldPassword = "";
            NewPassword = "";
            RepeatNewPassword = "";
        }

        public ICommand ChangePassword => new DelegateCommand(() => 
        {
            _eF.UpdatePassword(User.IdUser, NewPassword);

            ConfigurationUser config = new ConfigurationUser(User.IdUser, User.RoleName);
            config.Save();

            _pageService.ChangePage(new Menu());

        }, () => OldPassword == User.Password && NewPassword.Length > 0 && NewPassword != OldPassword && NewPassword == RepeatNewPassword);
    }
}
