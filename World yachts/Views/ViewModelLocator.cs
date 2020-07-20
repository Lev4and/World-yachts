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
            services.AddTransient<BoatsViewModel>();
            services.AddTransient<AddBoatViewModel>();
            services.AddTransient<ChangeBoatViewModel>();
            services.AddTransient<AccessoriesViewModel>();
            services.AddTransient<AddAccessoryViewModel>();
            services.AddTransient<ChangeAccessoryViewModel>();
            services.AddTransient<UsersViewModel>();
            services.AddTransient<AddUserViewModel>();
            services.AddTransient<ChangeUserViewModel>();
            services.AddTransient<CustomersViewModel>();
            services.AddTransient<AddCustomerViewModel>();
            services.AddTransient<ChangeCustomerViewModel>();
            services.AddTransient<PartnersViewModel>();
            services.AddTransient<AddPartnerViewModel>();
            services.AddTransient<ChangePartnerViewModel>();
            services.AddTransient<ContractsViewModel>();
            services.AddTransient<AddContractViewModel>();
            services.AddTransient<ChangeContractViewModel>();

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

        public BoatsViewModel BoatsViewModel => _provider.GetRequiredService<BoatsViewModel>();

        public AddBoatViewModel AddBoatViewModel => _provider.GetRequiredService<AddBoatViewModel>();

        public ChangeBoatViewModel ChangeBoatViewModel => _provider.GetRequiredService<ChangeBoatViewModel>();

        public AccessoriesViewModel AccessoriesViewModel => _provider.GetRequiredService<AccessoriesViewModel>();

        public AddAccessoryViewModel AddAccessoryViewModel => _provider.GetRequiredService<AddAccessoryViewModel>();

        public ChangeAccessoryViewModel ChangeAccessoryViewModel => _provider.GetRequiredService<ChangeAccessoryViewModel>();

        public UsersViewModel UsersViewModel => _provider.GetRequiredService<UsersViewModel>();

        public AddUserViewModel AddUserViewModel => _provider.GetRequiredService<AddUserViewModel>();

        public ChangeUserViewModel ChangeUserViewModel => _provider.GetRequiredService<ChangeUserViewModel>();

        public CustomersViewModel CustomersViewModel => _provider.GetRequiredService<CustomersViewModel>();

        public AddCustomerViewModel AddCustomerViewModel => _provider.GetRequiredService<AddCustomerViewModel>();

        public ChangeCustomerViewModel ChangeCustomerViewModel => _provider.GetRequiredService<ChangeCustomerViewModel>();

        public PartnersViewModel PartnersViewModel => _provider.GetRequiredService<PartnersViewModel>();

        public AddPartnerViewModel AddPartnerViewModel => _provider.GetRequiredService<AddPartnerViewModel>();

        public ChangePartnerViewModel ChangePartnerViewModel => _provider.GetRequiredService<ChangePartnerViewModel>();

        public ContractsViewModel ContractsViewModel => _provider.GetRequiredService<ContractsViewModel>();

        public AddContractViewModel AddContractViewModel => _provider.GetRequiredService<AddContractViewModel>();

        public ChangeContractViewModel ChangeContractViewModel => _provider.GetRequiredService<ChangeContractViewModel>();
    }
}
