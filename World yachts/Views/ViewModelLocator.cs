using Microsoft.Extensions.DependencyInjection;
using World_yachts.Services;
using World_yachts.ViewModels;

namespace World_yachts.Views
{
    public class ViewModelLocator
    {
        private static ServiceProvider _provider;

        public static void Init()
        {
            var services = new ServiceCollection();

            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<AuthorizationViewModel>();
            services.AddTransient<ChangePasswordViewModel>();
            services.AddTransient<MenuViewModel>();
            services.AddTransient<RegistrationViewModel>();
            services.AddTransient<UserInformationSupplementViewModel>();

            services.AddSingleton<PageService>();

            _provider = services.BuildServiceProvider();

            foreach (var item in services)
                _provider.GetRequiredService(item.ServiceType);
        }

        public MainWindowViewModel MainWindowViewModel => _provider.GetRequiredService<MainWindowViewModel>();

        public MainViewModel MainViewModel => _provider.GetRequiredService<MainViewModel>();

        public AuthorizationViewModel AuthorizationViewModel => _provider.GetRequiredService<AuthorizationViewModel>();

        public ChangePasswordViewModel ChangePasswordViewModel => _provider.GetRequiredService<ChangePasswordViewModel>();

        public MenuViewModel MenuViewModel => _provider.GetRequiredService<MenuViewModel>();

        public RegistrationViewModel RegistrationViewModel => _provider.GetRequiredService<RegistrationViewModel>();

        public UserInformationSupplementViewModel UserInformationSupplementViewModel => _provider.GetRequiredService<UserInformationSupplementViewModel>();
    }
}
