using DevExpress.Mvvm;
using System.Windows.Input;
using World_yachts.Services;
using World_yachts.Views.Pages;

namespace World_yachts.ViewModels
{
    public class ReportsViewModel : BindableBase
    {
        private readonly PageService _pageService;

        public ReportsViewModel(PageService pageService)
        {
            _pageService = pageService;
        }

        public ICommand EconomicReport => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new EconomicReport());
        });

        public ICommand PopularBoats => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new PopularBoats());
        });

        public ICommand BestSalesPersons => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new BestSalesPersons());
        });

        public ICommand Back => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new Menu());
        });
    }
}
