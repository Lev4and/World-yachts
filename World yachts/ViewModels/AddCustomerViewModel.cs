using DevExpress.Mvvm;
using System;
using System.Windows;
using System.Windows.Input;
using World_yachts.Model.Database.Interactions;
using World_yachts.Services;

namespace World_yachts.ViewModels
{
    public class AddCustomerViewModel : BindableBase
    {
        private DateTime _now;
        private readonly PageService _pageService;
        private EntityFramework _eF;

        public string FirstName { get; set; }

        public string FamilyName { get; set; }

        public string OrganisationName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string IdNumber { get; set; }

        public string IdDocumentName { get; set; }

        public DateTime MinDateOfBirth { get; set; }

        public DateTime MaxDateOfBirth { get; set; }

        public DateTime DateOfBirth { get; set; }

        public AddCustomerViewModel(PageService pageService)
        {
            _pageService = pageService;
        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _now = DateTime.Now;
            _eF = new EntityFramework();

            FirstName = "";
            FamilyName = "";
            OrganisationName = "";
            Address = "";
            City = "";
            Email = "";
            Phone = "";
            IdNumber = "";
            IdDocumentName = "";

            MinDateOfBirth = DateTime.MinValue;
            MaxDateOfBirth = _now;
            DateOfBirth = _now;
        });

        public ICommand Add => new DelegateCommand(() =>
        {
            _eF.AddCustomer(FirstName, FamilyName, DateOfBirth, OrganisationName, Address, City, Email, Phone, IdNumber, IdDocumentName);

            MessageBox.Show("Успешное добавление");
        }, () => (FirstName != null ? FirstName.Length > 0 : false) && (FamilyName != null ? FamilyName.Length > 0 : false) && (Address != null ? Address.Length > 0 : false) && (City != null ? City.Length > 0 : false) && (Email != null ? Email.Length > 0 : false) && (Phone != null ? Phone.Length > 0 : false) && (IdNumber != null ? IdNumber.Length > 0 : false) && (IdDocumentName != null ? IdDocumentName.Length > 0 : false));
    }
}
