using DevExpress.Mvvm;
using System.Windows.Controls;
using World_yachts.Services;
using World_yachts.Views.Pages;

namespace World_yachts.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly PageService _pageService;

        public Page PageSource { get; set; }

        public MainWindowViewModel(PageService pageService)
        {
            _pageService = pageService;

            _pageService.OnPageChanged += (page) => PageSource = page;
            _pageService.ChangePage(new Main());
        }
    }
}
