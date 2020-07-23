using Converters;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using World_yachts.Model.Database.Interactions;
using World_yachts.Model.Database.Models;
using World_yachts.Model.Logic;

namespace World_yachts.ViewModels
{
    public class ChangeAccessoryViewModel : BindableBase
    {
        private EntityFramework _eF;
        private v_accessory _accessory;

        public static int IdAccessory { get; set; }

        public int Price { get; set; }

        public int Inventory { get; set; }

        public int OrderLevel { get; set; }

        public int OrderBatch { get; set; }

        public double VAT { get; set; }

        public string AccName { get; set; }

        public string DescriptionOfAccessory { get; set; }

        public string PartnerName { get; set; }

        public ObservableCollection<string> ListSelectedModelBoatsFromAnExternalSource { get; set; }

        public ObservableCollection<string> ListSelectedModelBoats { get; set; }

        public ObservableCollection<string> Boats { get; set; }

        public ChangeAccessoryViewModel()
        {

        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _eF = new EntityFramework();
            _accessory = _eF.GetAccessory(IdAccessory);

            Price = _accessory.Price;
            Inventory = _accessory.Inventory;
            OrderLevel = _accessory.OrderLevel;
            OrderBatch = _accessory.OrderBatch;
            VAT = PercentConverter.ConvertToPercentageFormat(_accessory.VAT);
            AccName = _accessory.AccName;
            DescriptionOfAccessory = _accessory.DescriptionOfAccessory;
            PartnerName = _accessory.PartnerName;
            ListSelectedModelBoatsFromAnExternalSource = CollectionConverter<string>.ConvertToObservableCollection(FormattingList.GetList(_accessory.ListCompatibleModelBoats));
            ListSelectedModelBoats = new ObservableCollection<string>();
            Boats = CollectionConverter<string>.ConvertToObservableCollection(_eF.GetStringListBoats());
        });

        public ICommand Change => new DelegateCommand(() =>
        {
            _eF.UpdateAccessory(IdAccessory, DescriptionOfAccessory, Price, PercentConverter.ConvertToFractionalFormat(VAT), Inventory, OrderLevel, OrderBatch, ListSelectedModelBoats.ToList());

            MessageBox.Show("Успешное изменение");
        }, () => Price >= 0 && Inventory >= 0 && OrderLevel >= 0 && OrderBatch >= 0 && VAT >= 0);
    }
}
