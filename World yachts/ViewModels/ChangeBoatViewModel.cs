using Converters;
using DevExpress.Mvvm;
using System.Windows;
using System.Windows.Input;
using World_yachts.Model.Database.Interactions;
using World_yachts.Model.Database.Models;
using World_yachts.Services;

namespace World_yachts.ViewModels
{
    public class ChangeBoatViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private EntityFramework _eF;
        private v_boat _boat;

        public static int IdBoat { get; set; } 

        public bool ThereIsMast { get; set; }

        public int BasePrice { get; set; }

        public int NumberOfRowers { get; set; }

        public double VAT { get; set; }

        public string Model { get; set; }

        public string NameBoatType { get; set; }

        public string NameColour { get; set; }

        public string NameWood { get; set; }

        public ChangeBoatViewModel(PageService pageService)
        {
            _pageService = pageService;
        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _eF = new EntityFramework();
            _boat = _eF.GetBoat(IdBoat);

            Model = _boat.Model;
            NameBoatType = _boat.NameBoatType;
            NumberOfRowers = _boat.NumberOfRowers;
            ThereIsMast = _boat.Mast;
            NameColour = _boat.NameColour;
            NameWood = _boat.NameWood;
            BasePrice = _boat.BasePrice;
            VAT = PercentConverter.ConvertToPercentageFormat(_boat.VAT);
        });

        public ICommand Change => new DelegateCommand(() =>
        {
            _eF.UpdateBoat(IdBoat, BasePrice, PercentConverter.ConvertToFractionalFormat(VAT));

            MessageBox.Show("Успешное изменение");
        }, () => BasePrice >= 0 && VAT >= 0);
    }
}
