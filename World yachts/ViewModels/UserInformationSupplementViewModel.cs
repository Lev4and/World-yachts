using DevExpress.Mvvm;
using System.Windows.Input;
using World_yachts.Model.Configurations;
using World_yachts.Model.Database.Interactions;
using World_yachts.Services;
using World_yachts.Views.Pages;

namespace World_yachts.ViewModels
{
    public class UserInformationSupplementViewModel : BindableBase
    {
        private string _firstName;
        private string _familyName;
        private readonly PageService _pageService;
        private EntityFramework _eF;

        public static int? IdUser { get; set; }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;

                CorrectFirstName = _firstName.Length > 0;
            }
        }

        public string FamilyName
        {
            get { return _familyName; }
            set
            {
                _familyName = value;

                CorrectFamilyName = _familyName.Length > 0;
            }
        }

        public bool CorrectFirstName { get; set; }

        public bool CorrectFamilyName { get; set; }

        public UserInformationSupplementViewModel(PageService pageService)
        {
            _pageService = pageService;
        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _eF = new EntityFramework();

            FirstName = "";
            FamilyName = "";

            CorrectFirstName = false;
            CorrectFamilyName = false;
        });

        public ICommand Supplement => new DelegateCommand(() =>
        {
            _eF.AddSalesPerson((int)IdUser, FirstName, FamilyName);

            ConfigurationUser config = new ConfigurationUser((int)IdUser, "Manager");
            config.Save();

            _pageService.ChangePage(new Menu());
        }, () => IdUser != null && CorrectFirstName == true && CorrectFamilyName == true);
    }
}
