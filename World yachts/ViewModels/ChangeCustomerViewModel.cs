using DevExpress.Mvvm;
using System;
using System.Windows;
using System.Windows.Input;
using World_yachts.Model.Database.Interactions;
using World_yachts.Model.Database.Models;

namespace World_yachts.ViewModels
{
    public class ChangeCustomerViewModel : BindableBase
    {
        private EntityFramework _eF;
        private Customer _customer;

        public static int IdCustomer { get; set; }

        public string FirstName { get; set; }

        public string FamilyName { get; set; }

        public string OrganisationName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string IdNumber { get; set; }

        public string IdDocumentName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public ChangeCustomerViewModel()
        {

        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _eF = new EntityFramework();
            _customer = _eF.GetCustomer(IdCustomer);

            FirstName = _customer.FirstName;
            FamilyName = _customer.FamilyName;
            DateOfBirth = _customer.DateOfBirth;
            OrganisationName = _customer.OrganisationName;
            Address = _customer.Address;
            City = _customer.City;
            Email = _customer.Email;
            Phone = _customer.Phone;
            IdNumber = _customer.IdNumber;
            IdDocumentName = _customer.IdDocumentName;
        });

        public ICommand Change => new DelegateCommand(() =>
        {
            _eF.UpdateCustomer(IdCustomer, OrganisationName, Address, City, Email, Phone, IdNumber, IdDocumentName);

            MessageBox.Show("Успешное изменение");
        }, () => (Address != null ? Address.Length > 0 : false) && (City != null ? City.Length > 0 : false) && (Email != null ? Email.Length > 0 : false) && (Phone != null ? Phone.Length > 0 : false) && (IdNumber != null ? IdNumber.Length > 0 : false) && (IdDocumentName != null ? IdDocumentName.Length > 0 : false));
    }
}
