using Converters;
using DevExpress.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using World_yachts.Model.Database.Interactions;
using World_yachts.Model.Database.Models;
using World_yachts.Model.Logic;
using World_yachts.Services;
using World_yachts.Views.Pages;
using World_yachts.Views.Windows;

namespace World_yachts.ViewModels
{
    public class UsersViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private EntityFramework _eF;

        public int? SelectedIdUser { get; set; }

        public string FilterText { get; set; }

        public string RoleName { get; set; }

        public DateTime MinValueDateOfRegistration { get; set; }

        public DateTime BeginValueDateOfRegistration { get; set; }

        public DateTime EndValueDateOfRegistration { get; set; }

        public DateTime MaxValueDateOfRegistration { get; set; }

        public DateTime MinValueDateOfLastChangePassword { get; set; }

        public DateTime BeginValueDateOfLastChangePassword { get; set; }

        public DateTime EndValueDateOfLastChangePassword { get; set; }

        public DateTime MaxValueDateOfLastChangePassword { get; set; }

        public DateTime MinValueWasOnline { get; set; }

        public DateTime BeginValueWasOnline { get; set; }

        public DateTime EndValueWasOnline { get; set; }

        public DateTime MaxValueWasOnline { get; set; }

        public ObservableCollection<v_user> Users { get; set; }

        public ObservableCollection<Role> Roles { get; set; }

        public UsersViewModel(PageService pageService)
        {
            _pageService = pageService;
        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _eF = new EntityFramework();

            ResetFilters();
        });

        public ICommand Back => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new Menu());
        });

        public ICommand Reset => new DelegateCommand(() =>
        {
            ResetFilters();
        });

        public ICommand Search => new DelegateCommand(() =>
        {
            Searching();
        });

        public ICommand Add => new DelegateCommand(() =>
        {
            WindowService.ShowWindow(new AddUser());
        });

        public ICommand Change => new DelegateCommand(() =>
        {
            ChangeUserViewModel.IdUser = (int)SelectedIdUser;

            WindowService.ShowWindow(new ChangeUser());
        }, () => SelectedIdUser != null);

        public ICommand Remove => new DelegateCommand(() =>
        {
            if (MessageBox.Show("Вы действительно хотите удалить эту запись из базы данных?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _eF.RemoveUser((int)SelectedIdUser);

                MessageBox.Show("Успешное удаление");

                Searching();
            }
        }, () => SelectedIdUser != null);

        private void ResetFilters()
        {
            SelectedIdUser = null;
            FilterText = "";
            RoleName = "";

            MinValueDateOfLastChangePassword = _eF.GetMinDateOfLastChangePassword();
            MinValueDateOfRegistration = _eF.GetMinDateOfRegistration();
            MinValueWasOnline = _eF.GetMinWasOnline();
            MaxValueDateOfLastChangePassword = _eF.GetMaxDateOfLastChangePassword();
            MaxValueDateOfRegistration = _eF.GetMaxDateOfRegistration();
            MaxValueWasOnline = _eF.GetMaxWasOnline();

            BeginValueDateOfLastChangePassword = MinValueDateOfLastChangePassword;
            BeginValueDateOfRegistration = MinValueDateOfRegistration;
            BeginValueWasOnline = MinValueWasOnline;

            EndValueDateOfLastChangePassword = MaxValueDateOfLastChangePassword;
            EndValueDateOfRegistration = MaxValueDateOfRegistration;
            EndValueWasOnline = MaxValueWasOnline;

            Roles = CollectionConverter<Role>.ConvertToObservableCollection(_eF.GetRoles());

            Searching();
        }

        private void Searching()
        {
            Users = CollectionConverter<v_user>.ConvertToObservableCollection(_eF.GetUsers(FilterText,
                                                                                           RoleName,
                                                                                           new Range<DateTime>(BeginValueDateOfRegistration.Date, EndValueDateOfRegistration.AddHours(23).AddMinutes(59).AddSeconds(59)),
                                                                                           new Range<DateTime>(BeginValueDateOfLastChangePassword.Date, EndValueDateOfLastChangePassword.AddHours(23).AddMinutes(59).AddSeconds(59)),
                                                                                           new Range<DateTime>(BeginValueWasOnline.Date, EndValueWasOnline.Date.AddHours(23).AddMinutes(59).AddSeconds(59))));
        }
    }
}
