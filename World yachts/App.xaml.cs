using System.Windows;
using World_yachts.Views;

namespace World_yachts
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ViewModelLocator.Init();

            base.OnStartup(e);
        }
    }
}
