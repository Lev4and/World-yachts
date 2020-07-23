using Converters;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using World_yachts.Model.Database.Interactions;
using World_yachts.Model.Database.Models;

namespace World_yachts.ViewModels
{
    public class AddAccessoryViewModel : BindableBase
    {
        private EntityFramework _eF;

        public int Price { get; set; }

        public int Inventory { get; set; }

        public int OrderLevel { get; set; }

        public int OrderBatch { get; set; }

        public int? SelectedIdPartner { get; set; }

        public double VAT { get; set; }

        public string AccName { get; set; }

        public string DescriptionOfAccessory { get; set; }

        public ObservableCollection<string> ListSelectedModelBoats { get; set; }

        public ObservableCollection<string> Boats { get; set; }

        public ObservableCollection<v_partner> Partners { get; set; }

        public AddAccessoryViewModel()
        {

        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _eF = new EntityFramework();

            Price = 0;
            Inventory = 0;
            OrderLevel = 0;
            OrderBatch = 0;
            SelectedIdPartner = null;
            VAT = 0;
            AccName = "";
            DescriptionOfAccessory = "";
            ListSelectedModelBoats = new ObservableCollection<string>();
            Boats = CollectionConverter<string>.ConvertToObservableCollection(_eF.GetStringListBoats());
            Partners = CollectionConverter<v_partner>.ConvertToObservableCollection(_eF.GetPartners());
        });

        public ICommand Add => new DelegateCommand(() =>
        {
            if (_eF.AddAccessory(AccName, DescriptionOfAccessory, Price, PercentConverter.ConvertToFractionalFormat(VAT), Inventory, OrderLevel, OrderBatch, (int)SelectedIdPartner, ListSelectedModelBoats.ToList()))
            {
                MessageBox.Show("Успешное добавление");
            }
            else
            {
                MessageBox.Show("Ошибка");
            }
        }, () => Price >= 0 && Inventory >= 0 && OrderLevel >= 0 && OrderBatch >= 0 && SelectedIdPartner != null && VAT >= 0 && AccName.Length > 0);
    }
}
