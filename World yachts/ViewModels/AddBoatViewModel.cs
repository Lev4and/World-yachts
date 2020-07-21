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
    public class AddBoatViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private EntityFramework _eF;

        public bool ThereIsMast { get; set; }

        public bool WhetherOwnColor { get; set; }

        public int NumberOfRowers { get; set; }

        public int BasePrice { get; set; }

        public int? SelectedIdBoatType { get; set; }

        public int? SelectedIdColour { get; set; }

        public int? SelectedIdWood { get; set; }

        public double VAT { get; set; }

        public string Model { get; set; }

        public string OwnColor { get; set; }

        public ObservableCollection<BoatType> BoatTypes { get; set; }

        public ObservableCollection<Colour> Colours { get; set; }

        public ObservableCollection<Wood> Woods { get; set; }

        public AddBoatViewModel(PageService pageService)
        {
            _pageService = pageService;
        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _eF = new EntityFramework();

            ThereIsMast = false;
            WhetherOwnColor = false;
            NumberOfRowers = 0;
            BasePrice = 0;
            SelectedIdBoatType = null;
            SelectedIdColour = null;
            SelectedIdWood = null;
            VAT = 0;
            Model = "";
            OwnColor = "";
            BoatTypes = CollectionConverter<BoatType>.ConvertToObservableCollection(_eF.GetBoatTypes());
            Colours = CollectionConverter<Colour>.ConvertToObservableCollection(_eF.GetColours());
            Woods = CollectionConverter<Wood>.ConvertToObservableCollection(_eF.GetWoods());
        });

        public ICommand Add => new DelegateCommand(() =>
        {
            Colour colour = GetColour();

            CalculateBasePrice(colour);

            if(_eF.AddBoat(Model, (int)SelectedIdBoatType, NumberOfRowers, ThereIsMast, colour.IdColour, (int)SelectedIdWood, BasePrice, PercentConverter.ConvertToFractionalFormat(VAT)))
            {
                MessageBox.Show("Успешное добавление");
            }
            else
            {
                MessageBox.Show("Ошибка");
            }
        }, () => NumberOfRowers >= 0 && BasePrice >= 0 && SelectedIdBoatType != null && (SelectedIdColour != null || OwnColor.Length > 0) && SelectedIdWood != null && VAT >= 0 && Model.Length > 0);

        private Colour GetColour()
        {
            Colour colour = null;

            if (WhetherOwnColor)
            {
                _eF.AddColour(OwnColor, out colour);

                if (colour == null)
                {
                    colour = _eF.GetColour(OwnColor);
                }
            }
            else
            {
                colour = _eF.GetColour((int)SelectedIdColour);
            }

            return colour;
        }

        private void CalculateBasePrice(Colour colour)
        {
            BasePrice += colour.Price;
        }
    }
}
