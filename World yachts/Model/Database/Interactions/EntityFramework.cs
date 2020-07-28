using DevExpress.Mvvm.Native;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using World_yachts.Model.Configurations;
using World_yachts.Model.Database.Models;
using World_yachts.Model.Logic;

namespace World_yachts.Model.Database.Interactions
{
    public class EntityFramework : IInteraction
    {
        private ConfigurationDatabase _config;
        private WorldYachtsContext _context;

        public EntityFramework()
        {
            _config = ConfigurationDatabase.GetConfiguration();
            _context = new WorldYachtsContext(_config.ConnectionString);

            SetCommandTimeout();
        }

        public bool AddAccessory(string accName, string descriptionOfAccessory, int price, double VAT, int inventory, int orderLevel, int orderBatch, int idPartner, List<string> listSelectedCompatibleModelBoats)
        {
            if(!ContainsAccessory(accName))
            {
                Accessory accessory = _context.Accessory.Add(new Accessory
                {
                    AccName = accName,
                    DescriptionOfAccessory = descriptionOfAccessory,
                    Price = price,
                    VAT = VAT,
                    Inventory = inventory,
                    OrderLevel = orderLevel,
                    OrderBatch = orderBatch,
                    IdPartner = idPartner
                });

                for (int i = 0; i < listSelectedCompatibleModelBoats.Count; i++)
                {
                    AddAccToBoats(accessory.IdAccessory, GetIdBoat(listSelectedCompatibleModelBoats[i]));
                }

                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public bool AddAccToBoats(int idAccessory, int idBoat)
        {
            if (!ContainsAccToBoats(idAccessory, idBoat))
            {
                _context.AccToBoats.Add(new AccToBoats
                {
                    IdAccessory = idAccessory,
                    IdBoat = idBoat
                });
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public bool AddBoat(string model, int idBoatType, int numberOfRowers, bool mast, int idColour, int idWood, int basePrice, double VAT)
        {
            if (!ContainsBoat(model))
            {
                _context.Boat.Add(new Boat
                {
                    Model = model,
                    ProductionStartDate = DateTime.Now,
                    IdBoatType = idBoatType,
                    NumberOfRowers = numberOfRowers,
                    Mast = mast,
                    IdColour = idColour,
                    IdWood = idWood,
                    BasePrice = basePrice,
                    VAT = VAT
                });
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public bool AddColour(string nameColour)
        {
            if(!ContainsColour(nameColour))
            {
                _context.Colour.Add(new Colour { NameColour = nameColour });
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public bool AddColour(string nameColour, out Colour colour)
        {
            colour = null;

            if (!ContainsColour(nameColour))
            {
                colour = _context.Colour.Add(new Colour { NameColour = nameColour });
                colour.Price = 5000;

                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public bool AddSalesPerson(int idSalesPerson, string firstName, string familyName)
        {
            if(!ContainsSalesPerson(idSalesPerson))
            {
                _context.SalesPerson.Add(new SalesPerson
                {
                    IdSalesPerson = idSalesPerson,
                    FirstName = firstName,
                    FamilyName = familyName
                });
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public bool AddUser(int idRole, string login, string password)
        {
            DateTime dateTime = DateTime.Now;

            if(!ContainsUser(login))
            {
                _context.User.Add(new User
                {
                    IdRole = idRole,
                    Login = login,
                    Password = password,
                    DateOfRegistration = dateTime,
                    DateOfLastChangePassword = dateTime,
                    WasOnline = null
                });
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public bool AddUser(int idRole, string login, string password, out User user)
        {
            user = null;

            DateTime dateTime = DateTime.Now;

            if (!ContainsUser(login))
            {
                user = _context.User.Add(new User
                {
                    IdRole = idRole,
                    Login = login,
                    Password = password,
                    DateOfRegistration = dateTime,
                    DateOfLastChangePassword = dateTime,
                    WasOnline = null
                });
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public void BlockingUsers()
        {
            _context.sp_toBlockUser();
        }

        public bool ContainsAccessory(string accName)
        {
            return _context.Accessory.SingleOrDefault(a => a.AccName == accName) != null;
        }

        public bool ContainsAccToBoats(int idAccessory, int idBoat)
        {
            return _context.AccToBoats.SingleOrDefault(a => a.IdAccessory == idAccessory && a.IdBoat == idBoat) != null;
        }

        public bool ContainsBoat(string model)
        {
            return _context.Boat.SingleOrDefault(b => b.Model == model) != null;
        }

        public bool ContainsColour(string nameColour)
        {
            return _context.Colour.SingleOrDefault(c => c.NameColour == nameColour) != null;
        }

        public bool ContainsSalesPerson(int idSalesPerson)
        {
            return _context.SalesPerson.SingleOrDefault(s => s.IdSalesPerson == idSalesPerson) != null;
        }

        public bool ContainsUser(string login)
        {
            return _context.User.SingleOrDefault(u => u.Login == login) != null;
        }

        public bool CorrectDataUser(string login, string password)
        {
            return _context.v_user.SingleOrDefault(u => u.Login == login && u.Password == password) != null;
        }

        public bool CorrectDataUser(string login, string password, out v_user user)
        {
            user = _context.v_user.SingleOrDefault(u => u.Login == login && u.Password == password);

            return user != null ? true : false;
        }

        public List<v_accessory> GetAccessories(string accName, Range<int> rangePrice, Range<double> rangeVAT, Range<int> rangeInventory, Range<int> rangeOrderLevel, Range<int> rangeOrderBatch, List<string> listSelectedPartners, List<string> listSelectedBoats)
        {
            return _context.v_accessory.Where(a =>
            (accName.Length > 0 ? a.AccName.ToLower().StartsWith(accName.ToLower()) : true) &&
            a.Price >= rangePrice.BeginValue &&
            a.Price <= rangePrice.EndValue &&
            a.VAT >= rangeVAT.BeginValue &&
            a.VAT <= rangeVAT.EndValue &&
            a.Inventory >= rangeInventory.BeginValue &&
            a.Inventory <= rangeInventory.EndValue &&
            a.OrderLevel >= rangeOrderLevel.BeginValue &&
            a.OrderLevel <= rangeOrderLevel.EndValue &&
            a.OrderBatch >= rangeOrderBatch.BeginValue &&
            a.OrderBatch <= rangeOrderBatch.EndValue &&
            (listSelectedPartners.Count > 0 ? listSelectedPartners.Contains(a.PartnerName) : true) &&
            (listSelectedBoats.Count > 0 ? listSelectedBoats.All(l => a.ListCompatibleModelBoats.ToLower().Contains(l.ToLower())) : true)).AsNoTracking().ToList();
        }

        public v_boat GetBoat(int idBoat)
        {
            return _context.v_boat.SingleOrDefault(b => b.IdBoat == idBoat);
        }

        public List<v_boat> GetBoats(string model, List<string> listSelectedBoatTypes, List<string> listSelectedModelType, Range<int> rangeNumberOfRowers, bool? thereIsMast, List<string> listSelectedColours, List<string> listSelectedWoods, Range<int> rangeBasePrice, Range<double> rangeVAT, Range<DateTime> rangeProductionStartDate)
        {
            return _context.v_boat.Where(b =>
                   (model.Length > 0 ? b.Model.ToLower().StartsWith(model.ToLower()) : true) &&
                   (listSelectedModelType.Count > 0 ? listSelectedModelType.Any(l => b.Model.ToLower().Contains(l.ToLower())) : true) &&
                   b.NumberOfRowers >= rangeNumberOfRowers.BeginValue &&
                   b.NumberOfRowers <= rangeNumberOfRowers.EndValue &&
                   (thereIsMast != null ? b.Mast == thereIsMast : true) &&
                   b.BasePrice >= rangeBasePrice.BeginValue &&
                   b.BasePrice <= rangeBasePrice.EndValue &&
                   b.VAT >= rangeVAT.BeginValue &&
                   b.VAT <= rangeVAT.EndValue &&
                   b.ProductionStartDate >= rangeProductionStartDate.BeginValue &&
                   b.ProductionStartDate <= rangeProductionStartDate.EndValue &&
                   (listSelectedBoatTypes.Count > 0 ? listSelectedBoatTypes.Contains(b.NameBoatType) : true) &&
                   (listSelectedColours.Count > 0 ? listSelectedColours.Contains(b.NameColour) : true) &&
                   (listSelectedWoods.Count > 0 ? listSelectedWoods.Contains(b.NameWood) : true)).AsNoTracking().ToList();
        }

        public List<v_boatSimplifiedInformation> GetBoats()
        {
            return _context.v_boatSimplifiedInformation.AsNoTracking().ToList();
        }

        public List<BoatType> GetBoatTypes()
        {
            return _context.BoatType.AsNoTracking().ToList();
        }

        public Colour GetColour(string nameColour)
        {
            return _context.Colour.SingleOrDefault(c => c.NameColour == nameColour);
        }

        public List<Colour> GetColours()
        {
            return _context.Colour.AsNoTracking().ToList();
        }

        public int GetMaxBasePrice()
        {
            return _context.Boat.Max(b => b.BasePrice);
        }

        public int GetMaxInventory()
        {
            return _context.Accessory.Max(a => a.Inventory);
        }

        public int GetMaxNumberOfRowers()
        {
            return _context.Boat.Max(b => b.NumberOfRowers);
        }

        public int GetMaxOrderBatch()
        {
            return _context.Accessory.Max(a => a.OrderBatch);
        }

        public int GetMaxOrderLevel()
        {
            return _context.Accessory.Max(a => a.OrderLevel);
        }

        public int GetMaxPriceAccessory()
        {
            return _context.Accessory.Max(a => a.Price);
        }

        public double GetMaxVATAccessory()
        {
            return _context.Accessory.Max(a => a.VAT);
        }

        public double GetMaxVATBoat()
        {
            return _context.Boat.Max(b => b.VAT);
        }

        public int GetMinBasePrice()
        {
            return _context.Boat.Min(b => b.BasePrice);
        }

        public int GetMinInventory()
        {
            return _context.Accessory.Min(a => a.Inventory);
        }

        public int GetMinNumberOfRowers()
        {
            return _context.Boat.Min(b => b.NumberOfRowers);
        }

        public int GetMinOrderBatch()
        {
            return _context.Accessory.Min(a => a.OrderBatch);
        }

        public int GetMinOrderLevel()
        {
            return _context.Accessory.Min(a => a.OrderLevel);
        }

        public int GetMinPriceAccessory()
        {
            return _context.Accessory.Min(a => a.Price);
        }

        public double GetMinVATAccessory()
        {
            return _context.Accessory.Min(a => a.VAT);
        }

        public double GetMinVATBoat()
        {
            return _context.Boat.Min(b => b.VAT);
        }

        public List<v_partner> GetPartners()
        {
            return _context.v_partner.AsNoTracking().ToList();
        }

        public List<Role> GetRoles()
        {
            return _context.Role.AsNoTracking().ToList();
        }

        public List<string> GetStringListBoats()
        {
            var list = new List<string>();

            GetBoats().ForEach(b => list.Add(b.Model));

            return list;
        }

        public List<string> GetStringListBoatTypes()
        {
            var list = new List<string>();

            GetBoatTypes().ForEach(b => list.Add(b.NameBoatType));

            return list;
        }

        public List<string> GetStringListColours()
        {
            var list = new List<string>();

            GetColours().ForEach(c => list.Add(c.NameColour));

            return list;
        }

        public List<string> GetStringListPartners()
        {
            var list = new List<string>();

            GetPartners().ForEach(p => list.Add(p.Name));

            return list;
        }

        public List<string> GetStringListWoods()
        {
            var list = new List<string>();

            GetWoods().ForEach(w => list.Add(w.NameWood));

            return list;
        }

        public List<string> GetTypesModel()
        {
            return new List<string>() { "Стандарт", "Эконом", "Юниор", "Супер Люкс", "Люкс" };
        }

        public List<Wood> GetWoods()
        {
            return _context.Wood.AsNoTracking().ToList();
        }

        public bool NecessaryChagePassword(DateTime dateOfLastChangePassword)
        {
            return (DateTime.Now - dateOfLastChangePassword).Days >= 14;
        }

        public bool NecessaryChagePassword(User user)
        {
            return (DateTime.Now - user.DateOfLastChangePassword).Days >= 14;
        }

        public bool NecessaryChagePassword(v_user user)
        {
            return (DateTime.Now - user.DateOfLastChangePassword).Days >= 14;
        }

        public void RemoveAccessory(int idAccessory)
        {
            var accessory = _context.Accessory.Single(a => a.IdAccessory == idAccessory);

            _context.Accessory.Remove(accessory);
            _context.SaveChanges();
        }

        public void RemoveAccToBoats(int idAccessory)
        {
            _context.AccToBoats.RemoveRange(_context.AccToBoats.Where(a => a.IdAccessory == idAccessory));
            _context.SaveChanges();
        }

        public void RemoveBoat(int idBoat)
        {
            var boat = _context.Boat.Single(b => b.IdBoat == idBoat);

            _context.Boat.Remove(boat);
            _context.SaveChanges();
        }

        public void UpdateBoat(int idBoat, int basePrice, double VAT)
        {
            var boat = _context.Boat.SingleOrDefault(b => b.IdBoat == idBoat);

            boat.BasePrice = basePrice;
            boat.VAT = VAT;

            _context.SaveChanges();
        }

        public void UpdateAccessory(int idAccessory, string descriptionOfAccessory, int price, double VAT, int inventory, int orderLevel, int orderBatch, List<string> listSelectedCompatibleModelBoats)
        {
            RemoveAccToBoats(idAccessory);

            Accessory accessory = _context.Accessory.Single(a => a.IdAccessory == idAccessory);

            accessory.DescriptionOfAccessory = descriptionOfAccessory;
            accessory.Price = price;
            accessory.VAT = VAT;
            accessory.Inventory = inventory;
            accessory.OrderLevel = orderLevel;
            accessory.OrderBatch = orderBatch;

            for (int i = 0; i < listSelectedCompatibleModelBoats.Count; i++)
            {
                AddAccToBoats(accessory.IdAccessory, GetIdBoat(listSelectedCompatibleModelBoats[i]));
            }

            _context.SaveChanges();
        }

        public void UpdatePassword(int idUser, string newPassword)
        {
            var user = _context.User.SingleOrDefault(u => u.IdUser == idUser);

            user.Password = newPassword;
            user.DateOfLastChangePassword = DateTime.Now;

            _context.SaveChanges();
        }

        public void UpdateValueWasOnline(int idUser)
        {
            var user = _context.User.SingleOrDefault(u => u.IdUser == idUser);
            user.WasOnline = DateTime.Now;

            _context.SaveChanges();
        }

        public void UpdateValueWasOnline(string login)
        {
            var user = _context.User.SingleOrDefault(u => u.Login == login);
            user.WasOnline = DateTime.Now;

            _context.SaveChanges();
        }

        public void UpdateValueWasOnline(User user)
        {
            user.WasOnline = DateTime.Now;

            _context.SaveChanges();
        }

        public void UpdateValueWasOnline(v_user user)
        {
            var obj = _context.User.SingleOrDefault(u => u.IdUser == user.IdUser);
            obj.WasOnline = DateTime.Now;

            _context.SaveChanges();
        }

        public int GetIdBoat(string model)
        {
            return _context.Boat.SingleOrDefault(b => b.Model == model).IdBoat;
        }

        public v_accessory GetAccessory(int idAccessory)
        {
            return _context.v_accessory.SingleOrDefault(a => a.IdAccessory == idAccessory);
        }

        public DateTime GetMinDateOfRegistration()
        {
            return _context.User.Min(u => u.DateOfRegistration);
        }

        public DateTime GetMinDateOfLastChangePassword()
        {
            return _context.User.Min(u => u.DateOfLastChangePassword);
        }

        public DateTime GetMinWasOnline()
        {
            return (DateTime)_context.User.Min(u => u.WasOnline);
        }

        public DateTime GetMaxDateOfRegistration()
        {
            return _context.User.Max(u => u.DateOfRegistration);
        }

        public DateTime GetMaxDateOfLastChangePassword()
        {
            return _context.User.Max(u => u.DateOfLastChangePassword);
        }

        public DateTime GetMaxWasOnline()
        {
            return (DateTime)_context.User.Max(u => u.WasOnline);
        }

        public User GetUser(int idUser)
        {
            return _context.User.SingleOrDefault(u => u.IdUser == idUser);
        }

        public List<v_user> GetUsers(string login, string roleName, Range<DateTime> rangeDateOfRegistration, Range<DateTime> rangeDateOfLastChangePassword, Range<DateTime> rangeWasOnline)
        {
            return _context.v_user.Where(u =>
            (login.Length > 0 ? u.Login.ToLower().StartsWith(login.ToLower()) : true) &&
            (roleName.Length > 0 ? u.RoleName == roleName : true) &&
            u.DateOfRegistration >= rangeDateOfRegistration.BeginValue &&
            u.DateOfRegistration <= rangeDateOfRegistration.EndValue &&
            u.DateOfLastChangePassword >= rangeDateOfLastChangePassword.BeginValue &&
            u.DateOfLastChangePassword <= rangeDateOfLastChangePassword.EndValue &&
            (u.WasOnline != null ? u.WasOnline >= rangeWasOnline.BeginValue &&
            u.WasOnline <= rangeWasOnline.EndValue : true)).AsNoTracking().ToList();
        }

        public void RemoveUser(int idUser)
        {
            var user = _context.User.Single(u => u.IdUser == idUser);

            _context.User.Remove(user);
            _context.SaveChanges();
        }

        public void UpdateUser(int idUser, int idRole, string password)
        {
            var user = _context.User.Single(u => u.IdUser == idUser);

            user.IdRole = idRole;
            user.Password = password;

            _context.SaveChanges();
        }

        public SalesPerson GetSalesPerson(int idSalesPerson)
        {
            return _context.SalesPerson.SingleOrDefault(s => s.IdSalesPerson == idSalesPerson);
        }

        public void AddCustomer(string firstName, string familyName, DateTime dateOfBirth, string organisationName, string address, string city, string email, string phone, string idNumber, string idDocumentName)
        {
            _context.Customer.Add(new Customer 
            {
                FirstName = firstName,
                FamilyName = familyName,
                DateOfBirth = dateOfBirth,
                OrganisationName = organisationName,
                Address = address,
                City = city,
                Email = email,
                Phone = phone,
                IdNumber = idNumber,
                IdDocumentName = idDocumentName
            });
            _context.SaveChanges();
        }

        public Customer GetCustomer(int idCustomer)
        {
            return _context.Customer.SingleOrDefault(c => c.IdCustomer == idCustomer);
        }

        public List<v_cityCustomer> GetCitiesCustomers()
        {
            return _context.v_cityCustomer.AsNoTracking().ToList();
        }

        public List<v_customer> GetCustomers(string fullName, string organisationName, string city)
        {
            return _context.v_customer.Where(c =>
            (fullName.Length > 0 ? c.FullName.ToLower().StartsWith(fullName.ToLower()) : true) &&
            (organisationName.Length > 0 ? c.OrganisationName == organisationName : true) &&
            (city.Length > 0 ? c.City == city : true)).AsNoTracking().ToList();
        }

        public List<v_organisationNameCustomer> GetOrganisationsNamesCustomers()
        {
            return _context.v_organisationNameCustomer.AsNoTracking().ToList();
        }

        public void RemoveCustomer(int idCustomer)
        {
            var customer = _context.Customer.Single(c => c.IdCustomer == idCustomer);

            _context.Customer.Remove(customer);
            _context.SaveChanges();
        }

        public void UpdateCustomer(int idCustomer, string organisationName, string address, string city, string email, string phone, string idNumber, string idDocumentName)
        {
            var customer = _context.Customer.Single(c => c.IdCustomer == idCustomer);

            customer.OrganisationName = organisationName;
            customer.Address = address;
            customer.City = city;
            customer.Email = email;
            customer.Phone = phone;
            customer.IdNumber = idNumber;
            customer.IdDocumentName = idDocumentName;

            _context.SaveChanges();
        }

        public bool AddPartner(string name, string address, string city)
        {
            if(!ContainsPartner(name, address, city))
            {
                _context.Partner.Add(new Partner
                {
                    Name = name,
                    Address = address,
                    City = city
                });
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public bool ContainsPartner(string name, string address, string city)
        {
            return _context.Partner.SingleOrDefault(p => p.Name == name && p.Address == address && p.City == city) != null;
        }

        public Partner GetPartner(int idPartner)
        {
            return _context.Partner.SingleOrDefault(p => p.IdPartner == idPartner);
        }

        public List<v_partner> GetPartners(string name, string city)
        {
            return _context.v_partner.Where(p =>
            (name.Length > 0 ? p.Name.ToLower().StartsWith(name.ToLower()) : true) &&
            (city.Length > 0 ? p.City == city : true)).AsNoTracking().ToList();
        }

        public List<v_cityPartner> GetCitiesPartners()
        {
            return _context.v_cityPartner.AsNoTracking().ToList();
        }

        public void RemovePartner(int idPartner)
        {
            var partner = _context.Partner.Single(p => p.IdPartner == idPartner);

            _context.Partner.Remove(partner);
            _context.SaveChanges();
        }

        public bool UpdatePartner(int idPartner, string name, string address, string city)
        {
            if(!ContainsPartner(name, address, city))
            {
                var partner = _context.Partner.Single(p => p.IdPartner == idPartner);

                partner.Name = name;
                partner.Address = address;
                partner.City = city;

                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public void AddContract(DateTime date, int depositPayed, int idOrder, int contractTotalPrice, int contractTotalPriceInclVAT, string productionProcess)
        {
            _context.Contract.Add(new Contract
            {
                Date = date,
                DepositPayed = depositPayed,
                IdOrder = idOrder,
                ContractTotalPrice = contractTotalPrice,
                ContractTotalPriceInclVAT = contractTotalPriceInclVAT,
                ProductionProcess = productionProcess
            });
            _context.SaveChanges();
        }

        public void AddOrderDetails(int idAccessory, int idOrder)
        {
            _context.OrderDetails.Add(new OrderDetails()
            {
                IdAccessory = idAccessory,
                IdOrder = idOrder
            });
            _context.SaveChanges();
        }

        public int GetMinDepositPayed()
        {
            return _context.Contract.Min(c => c.DepositPayed);
        }

        public int GetMinContractTotalPrice()
        {
            return _context.Contract.Min(c => c.ContractTotalPrice);
        }

        public int GetMinContractTotalPriceInclVAT()
        {
            return _context.Contract.Min(c => c.ContractTotalPriceInclVAT);
        }

        public int GetMaxDepositPayed()
        {
            return _context.Contract.Max(c => c.DepositPayed);
        }

        public int GetMaxContractTotalPrice()
        {
            return _context.Contract.Max(c => c.ContractTotalPrice);
        }

        public int GetMaxContractTotalPriceInclVAT()
        {
            return _context.Contract.Max(c => c.ContractTotalPriceInclVAT);
        }

        public DateTime GetMinDateOfConclusionContract()
        {
            return _context.Contract.Min(c => c.Date);
        }

        public DateTime GetMaxDateOfConclusionContract()
        {
            return _context.Contract.Max(c => c.Date);
        }

        public List<string> GetProductionProcess()
        {
            return new List<string>()
            {
                "Работы не начаты",
                "Начато производство",
                "25% готовности",
                "50% готовности",
                "75% готовности",
                "Отделка лодки"
            };
        }

        public List<v_cityOrder> GetCitiesOrders()
        {
            return _context.v_cityOrder.AsNoTracking().ToList();
        }

        public List<v_contract2> GetContracts(List<string> listSelectedModelsBoats, List<string> listSelectedAccessoriesAtOrder, List<string> listSelectedProductionProcess, List<string> listSelectedDeliveryAddressOrders, List<string> listSelectedCitiesOrders, Range<DateTime> rangeDateOfConclusionContract, Range<int> rangeDepositPayed, Range<int> rangeContractTotalPrice, Range<int> rangeContractTotalPriceInclVAT)
        {
            return _context.v_contract2.Where(c =>
            (listSelectedModelsBoats.Count > 0 ? listSelectedModelsBoats.Contains(c.Model) : true) &&
            (listSelectedAccessoriesAtOrder.Count > 0 ? listSelectedAccessoriesAtOrder.All(l => c.SelectedAccessoriesAtOrder.ToLower().Contains(l.ToLower())) : true) &&
            (listSelectedProductionProcess.Count > 0 ? listSelectedProductionProcess.Contains(c.ProductionProcess) : true) &&
            (listSelectedDeliveryAddressOrders.Count > 0 ? listSelectedDeliveryAddressOrders.Contains(c.DeliveryAddress) : true) &&
            (listSelectedCitiesOrders.Count > 0 ? listSelectedCitiesOrders.Contains(c.City) : true) &&
            c.Date >= rangeDateOfConclusionContract.BeginValue &&
            c.Date <= rangeDateOfConclusionContract.EndValue &&
            c.DepositPayed >= rangeDepositPayed.BeginValue &&
            c.DepositPayed <= rangeDepositPayed.EndValue &&
            c.ContractTotalPrice >= rangeContractTotalPrice.BeginValue &&
            c.ContractTotalPrice <= rangeContractTotalPrice.EndValue &&
            c.ContractTotalPriceInclVAT >= rangeContractTotalPriceInclVAT.BeginValue &&
            c.ContractTotalPriceInclVAT <= rangeContractTotalPriceInclVAT.EndValue).AsNoTracking().ToList();
        }

        public List<v_deliveryAddressOrder> GetDeliveryAddressOrders()
        {
            return _context.v_deliveryAddressOrder.AsNoTracking().ToList();
        }

        public void RemoveContract(int idContract)
        {
            var contract = _context.Contract.Single(c => c.IdContract == idContract);

            _context.Contract.Remove(contract);
            _context.SaveChanges();
        }

        public void RemoveOrder(int idOrder)
        {
            var order = _context.Order.Single(c => c.IdOrder == idOrder);

            _context.Order.Remove(order);
            _context.SaveChanges();
        }

        public void UpdateContract(int idContract, int depositPayed, string productionProcess)
        {
            var contract = _context.Contract.Single(c => c.IdContract == idContract);

            contract.DepositPayed = depositPayed;
            contract.ProductionProcess = productionProcess;

            _context.SaveChanges();
        }

        public void EnterIntoContract(int idSalesPerson, int idCustomer, int idBoat, string deliveryAddress, string city, List<int> listSelectedAccessories, int depositPayed, int contractTotalPrice, int contractTotalPriceInclVAT)
        {
            DateTime time = DateTime.Now;

            Order order = _context.Order.Add(new Order
            {
                Date = time,
                IdSalesPerson = idSalesPerson,
                IdCustomer = idCustomer,
                IdBoat = idBoat,
                DeliveryAddress = deliveryAddress,
                City = city
            });
            _context.SaveChanges();

            for (int i = 0; i < listSelectedAccessories.Count; i++)
            {
                AddOrderDetails(listSelectedAccessories[i], order.IdOrder);
            }

            AddContract(time, depositPayed, order.IdOrder, contractTotalPrice, contractTotalPriceInclVAT, "Работы не начаты");
        }

        public List<string> GetStringListAccessories()
        {
            var list = new List<string>();

            GetAccessories().ForEach(a => list.Add(a.AccName));

            return list;
        }

        public List<v_accessorySimplifiedInformation> GetAccessories()
        {
            return _context.v_accessorySimplifiedInformation.AsNoTracking().ToList();
        }

        public List<string> GetStringListDeliveryAddressOrders()
        {
            var list = new List<string>();

            GetDeliveryAddressOrders().ForEach(d => list.Add(d.DeliveryAddress));

            return list;
        }

        public List<string> GetStringListCitiesOrders()
        {
            var list = new List<string>();

            GetCitiesOrders().ForEach(c => list.Add(c.City));

            return list;
        }

        public DateTime GetMinProductionStartDate()
        {
            return _context.Boat.Min(b => b.ProductionStartDate);
        }

        public DateTime GetMaxProductionStartDate()
        {
            return _context.Boat.Max(b => b.ProductionStartDate);
        }

        public List<v_accessory> GetAccessories(string modelBoat)
        {
            return _context.v_accessory.Where(a =>
            a.ListCompatibleModelBoats.ToLower().Contains(modelBoat.ToLower())).AsNoTracking().ToList();
        }

        public List<v_customer> GetCustomers()
        {
            return _context.v_customer.AsNoTracking().ToList();
        }

        public v_contract GetContract(int idContract)
        {
            return _context.v_contract.Single(c => c.IdContract == idContract);
        }

        public Order GetOrder(int idOrder)
        {
            return _context.Order.Single(o => o.IdOrder == idOrder);
        }

        public void DatabaseAutocomplete()
        {
            Random rand = new Random();

            DateTime minDate = new DateTime(2019, 3, 8).Date;
            DateTime maxDate = DateTime.Now.Date;

            var users = GetUsers();
            var customers = GetCustomers();
            var boats = GetAllBoats();
            var cities = GetStringListCitiesOrders();

            List<v_accessory> accessories = null;

            for (DateTime dateTime = minDate; dateTime <= maxDate; dateTime = dateTime.AddDays(1))
            {
                int countContract = 3;

                var verifiedUsers = users.Where(u => u.RoleName == "Manager" && u.DateOfRegistration <= dateTime).ToList();

                for (int i = 0; i < countContract; i++)
                {
                    var verifiedBoats = boats.Where(b => b.ProductionStartDate <= dateTime).ToList();

                    var user = verifiedUsers[rand.Next(0, verifiedUsers.Count)];
                    var customer = customers[rand.Next(0, customers.Count)];
                    var boat = verifiedBoats[rand.Next(0, verifiedBoats.Count)];
                    var city = cities[rand.Next(0, cities.Count)];

                    string address = "";

                    switch(city)
                    {
                        case "Санкт-Петербург":
                            {
                                address = new string[2] { "Санкт-Петербург, порт", "Санкт-Петербург" }[rand.Next(0, 2)];
                            }
                            break;
                        case "Москва":
                            {
                                address = new string[2] { "Москва, Северный порт", "Москва" }[rand.Next(0, 2)];
                            }
                            break;
                        default:
                            {
                                address = city;
                            }
                            break;
                    }

                    accessories = GetAccessories(boat.Model);

                    var countSelectedAccessories = rand.Next(0, accessories.Count);
                    var selectedAccessories = new List<v_accessory>();

                    for(int j = 0; j < countSelectedAccessories; j++)
                    {
                        while(true)
                        {
                            var accessory = accessories[rand.Next(0, accessories.Count)];

                            if(!selectedAccessories.Contains(accessory))
                            {
                                selectedAccessories.Add(accessory);

                                break;
                            }
                        }
                    }

                    int depositPayed = 0;
                    int contractTotalPrice = 0;
                    int contractTotalPriceInclVAT = 0;

                    contractTotalPrice = boat.BasePrice;
                    contractTotalPriceInclVAT = (int)((double)boat.BasePrice + ((double)boat.BasePrice * (double)boat.VAT));

                    selectedAccessories.ForEach(sa =>
                    {
                        contractTotalPrice += sa.Price;
                        contractTotalPriceInclVAT += (int)((double)sa.Price + ((double)sa.Price * (double)sa.VAT));
                    });

                    depositPayed = (int)((double)contractTotalPrice / (double)3);

                    EnterIntoContract(dateTime, user.IdUser, customer.IdCustomer, boat.IdBoat, address, city, selectedAccessories, depositPayed, contractTotalPrice, contractTotalPriceInclVAT);
                }
            }
        }

        public void EnterIntoContract(int idSalesPerson, int idCustomer, int idBoat, string deliveryAddress, string city, List<v_accessory> listSelectedAccessories, int depositPayed, int contractTotalPrice, int contractTotalPriceInclVAT)
        {
            DateTime time = DateTime.Now;

            Order order = _context.Order.Add(new Order
            {
                Date = time,
                IdSalesPerson = idSalesPerson,
                IdCustomer = idCustomer,
                IdBoat = idBoat,
                DeliveryAddress = deliveryAddress,
                City = city
            });
            _context.SaveChanges();

            for (int i = 0; i < listSelectedAccessories.Count; i++)
            {
                AddOrderDetails(listSelectedAccessories[i].IdAccessory, order.IdOrder);
            }

            AddContract(time, depositPayed, order.IdOrder, contractTotalPrice, contractTotalPriceInclVAT, "Работы не начаты");
        }

        public List<v_user> GetUsers()
        {
            return _context.v_user.AsNoTracking().ToList();
        }

        public List<v_boat> GetAllBoats()
        {
            return _context.v_boat.AsNoTracking().ToList();
        }

        public void EnterIntoContract(DateTime dateOfConclusionContract, int idSalesPerson, int idCustomer, int idBoat, string deliveryAddress, string city, List<int> listSelectedAccessories, int depositPayed, int contractTotalPrice, int contractTotalPriceInclVAT)
        {
            Random rand = new Random();

            dateOfConclusionContract = dateOfConclusionContract.Date.AddHours(rand.Next(9, 21)).AddMinutes(rand.Next(0, 60)).AddMinutes(rand.Next(0, 60)).AddMilliseconds(rand.Next(0, 1000));

            Order order = _context.Order.Add(new Order
            {
                Date = dateOfConclusionContract,
                IdSalesPerson = idSalesPerson,
                IdCustomer = idCustomer,
                IdBoat = idBoat,
                DeliveryAddress = deliveryAddress,
                City = city
            });
            _context.SaveChanges();

            for (int i = 0; i < listSelectedAccessories.Count; i++)
            {
                AddOrderDetails(listSelectedAccessories[i], order.IdOrder);
            }

            AddContract(dateOfConclusionContract, depositPayed, order.IdOrder, contractTotalPrice, contractTotalPriceInclVAT, "Работы не начаты");
        }

        public void EnterIntoContract(DateTime dateOfConclusionContract, int idSalesPerson, int idCustomer, int idBoat, string deliveryAddress, string city, List<v_accessory> listSelectedAccessories, int depositPayed, int contractTotalPrice, int contractTotalPriceInclVAT)
        {
            Random rand = new Random();

            dateOfConclusionContract = dateOfConclusionContract.Date.AddHours(rand.Next(9, 21)).AddMinutes(rand.Next(0, 60)).AddMinutes(rand.Next(0, 60)).AddMilliseconds(rand.Next(0, 1000));

            Order order = _context.Order.Add(new Order
            {
                Date = dateOfConclusionContract,
                IdSalesPerson = idSalesPerson,
                IdCustomer = idCustomer,
                IdBoat = idBoat,
                DeliveryAddress = deliveryAddress,
                City = city
            });
            _context.SaveChanges();

            for (int i = 0; i < listSelectedAccessories.Count; i++)
            {
                AddOrderDetails(listSelectedAccessories[i].IdAccessory, order.IdOrder);
            }

            AddContract(dateOfConclusionContract, depositPayed, order.IdOrder, contractTotalPrice, contractTotalPriceInclVAT, "Работы не начаты");
        }

        public List<v_contract> GetContracts(List<string> listSelectedProductionProcess, Range<DateTime> rangeDateOfConclusionContract, Range<int> rangeDepositPayed, Range<int> rangeContractTotalPrice, Range<int> rangeContractTotalPriceInclVAT)
        {
            return _context.v_contract.Where(c =>
            (listSelectedProductionProcess.Count > 0 ? listSelectedProductionProcess.Contains(c.ProductionProcess) : true) &&
            c.Date >= rangeDateOfConclusionContract.BeginValue &&
            c.Date <= rangeDateOfConclusionContract.EndValue &&
            c.DepositPayed >= rangeDepositPayed.BeginValue &&
            c.DepositPayed <= rangeDepositPayed.EndValue &&
            c.ContractTotalPrice >= rangeContractTotalPrice.BeginValue &&
            c.ContractTotalPrice <= rangeContractTotalPrice.EndValue &&
            c.ContractTotalPriceInclVAT >= rangeContractTotalPriceInclVAT.BeginValue &&
            c.ContractTotalPriceInclVAT <= rangeContractTotalPriceInclVAT.EndValue).AsNoTracking().ToList();
        }

        public v_contract2 GetContractExtendedVersion(int idContract)
        {
            return _context.v_contract2.Single(c => c.IdContract == idContract);
        }

        public Colour GetColour(int idColour)
        {
            return _context.Colour.Single(c => c.IdColour == idColour);
        }

        public List<v_briefReportBestSalesPersonsAllTime> GetReportBestSalesPersons(List<string> listSelectedSalesPersons)
        {
            return _context.v_briefReportBestSalesPersonsAllTime.Where(r =>
            (listSelectedSalesPersons.Count > 0 ? listSelectedSalesPersons.Any(l => r.FullName == l) : false)).AsNoTracking().ToList();
        }

        public List<v_briefReportPopularBoatsAllTime> GetReportPopularBoats(List<string> listSelectedModelsBoats)
        {
            return _context.v_briefReportPopularBoatsAllTime.Where(r =>
            (listSelectedModelsBoats.Count > 0 ? listSelectedModelsBoats.Any(l => r.Model == l) : false)).AsNoTracking().ToList();
        }

        public List<v_detailDailyEconomicReportAllTime> GetDetailDailyEconomicReport(Range<DateTime> rangeDate)
        {
            return _context.v_detailDailyEconomicReportAllTime.Where(r =>
            r.Date >= rangeDate.BeginValue &&
            r.Date <= rangeDate.EndValue).OrderBy(r => r.Date).AsNoTracking().ToList();
        }

        public List<v_detailDailyReportBestSalesPersonsAllTime> GetDetailDailyReportBestSalesPersons(List<string> listSelectedSalesPersons, Range<DateTime> rangeDate)
        {
            return _context.v_detailDailyReportBestSalesPersonsAllTime.Where(r =>
            (listSelectedSalesPersons.Count > 0 ? listSelectedSalesPersons.Any(l => r.FullName == l) : false) &&
            r.Date >= rangeDate.BeginValue &&
            r.Date <= rangeDate.EndValue).OrderBy(r => r.Date).AsNoTracking().ToList();
        }

        public List<v_detailDailyReportPopularBoatsAllTime> GetDetailDailyReportPopularBoats(List<string> listSelectedModelsBoats, Range<DateTime> rangeDate)
        {
            return _context.v_detailDailyReportPopularBoatsAllTime.Where(r =>
            (listSelectedModelsBoats.Count > 0 ? listSelectedModelsBoats.Any(l => r.Model == l) : false) &&
            r.Date >= rangeDate.BeginValue &&
            r.Date <= rangeDate.EndValue).OrderBy(r => r.Date).AsNoTracking().ToList();
        }

        public List<v_detailWeeklyEconomicReportAllTime> GetDetailWeeklyEconomicReport(Date beginValueRangeDate, Date endValueRangeDate)
        {
            return _context.v_detailWeeklyEconomicReportAllTime.Where(r =>
            r.Year >= beginValueRangeDate.Year &&
            r.Year <= endValueRangeDate.Year &&
            (r.Year == beginValueRangeDate.Year ? r.Week >= beginValueRangeDate.Week : true) &&
            (r.Year == endValueRangeDate.Year ? r.Week <= endValueRangeDate.Week : true)).OrderBy(r => r.Year).ThenBy(r => r.Week).AsNoTracking().ToList();
        }

        public List<v_detailWeeklyReportBestSalesPersonsAllTime> GetDetailWeeklyReportBestSalesPersons(List<string> listSelectedSalesPersons, Date beginValueRangeDate, Date endValueRangeDate)
        {
            return _context.v_detailWeeklyReportBestSalesPersonsAllTime.Where(r =>
            (listSelectedSalesPersons.Count > 0 ? listSelectedSalesPersons.Any(l => r.FullName == l) : false) &&
            r.Year >= beginValueRangeDate.Year &&
            r.Year <= endValueRangeDate.Year &&
            (r.Year == beginValueRangeDate.Year ? r.Week >= beginValueRangeDate.Week : true) &&
            (r.Year == endValueRangeDate.Year ? r.Week <= endValueRangeDate.Week : true)).OrderBy(r => r.Year).ThenBy(r => r.Week).AsNoTracking().ToList();
        }

        public List<v_detailWeeklyReportPopularBoatsAllTime> GetDetailWeeklyReportPopularBoats(List<string> listSelectedModelsBoats, Date beginValueRangeDate, Date endValueRangeDate)
        {
            return _context.v_detailWeeklyReportPopularBoatsAllTime.Where(r =>
            (listSelectedModelsBoats.Count > 0 ? listSelectedModelsBoats.Any(l => r.Model == l) : false) &&
            r.Year >= beginValueRangeDate.Year &&
            r.Year <= endValueRangeDate.Year &&
            (r.Year == beginValueRangeDate.Year ? r.Week >= beginValueRangeDate.Week : true) &&
            (r.Year == endValueRangeDate.Year ? r.Week <= endValueRangeDate.Week : true)).OrderBy(r => r.Year).ThenBy(r => r.Week).AsNoTracking().ToList();
        }

        public List<v_detailMonthlyEconomicReportAllTime> GetDetailMonthlyEconomicReport(Date beginValueRangeDate, Date endValueRangeDate)
        {
            return _context.v_detailMonthlyEconomicReportAllTime.Where(r =>
            r.Year >= beginValueRangeDate.Year &&
            r.Year <= endValueRangeDate.Year &&
            (r.Year == beginValueRangeDate.Year ? r.Month >= beginValueRangeDate.Month : true) &&
            (r.Year == endValueRangeDate.Year ? r.Month <= endValueRangeDate.Month : true)).OrderBy(r => r.Year).ThenBy(r => r.Month).AsNoTracking().ToList();
        }

        public List<v_detailMonthlyReportBestSalesPersonsAllTime> GetDetailMonthlyReportBestSalesPersons(List<string> listSelectedSalesPersons, Date beginValueRangeDate, Date endValueRangeDate)
        {
            return _context.v_detailMonthlyReportBestSalesPersonsAllTime.Where(r =>
            (listSelectedSalesPersons.Count > 0 ? listSelectedSalesPersons.Any(l => r.FullName == l) : false) &&
            r.Year >= beginValueRangeDate.Year &&
            r.Year <= endValueRangeDate.Year &&
            (r.Year == beginValueRangeDate.Year ? r.Month >= beginValueRangeDate.Month : true) &&
            (r.Year == endValueRangeDate.Year ? r.Month <= endValueRangeDate.Month : true)).OrderBy(r => r.Year).ThenBy(r => r.Month).AsNoTracking().ToList();
        }

        public List<v_detailMonthlyReportPopularBoatsAllTime> GetDetailMonthlyReportPopularBoats(List<string> listSelectedModelsBoats, Date beginValueRangeDate, Date endValueRangeDate)
        {
            return _context.v_detailMonthlyReportPopularBoatsAllTime.Where(r =>
            (listSelectedModelsBoats.Count > 0 ? listSelectedModelsBoats.Any(l => r.Model == l) : false) &&
            r.Year >= beginValueRangeDate.Year &&
            r.Year <= endValueRangeDate.Year &&
            (r.Year == beginValueRangeDate.Year ? r.Month >= beginValueRangeDate.Month : true) &&
            (r.Year == endValueRangeDate.Year ? r.Month <= endValueRangeDate.Month : true)).OrderBy(r => r.Year).ThenBy(r => r.Month).AsNoTracking().ToList();
        }

        public List<v_detailQuarterlyEconomicReportAllTime> GetDetailQuarterlyEconomicReport(Date beginValueRangeDate, Date endValueRangeDate)
        {
            return _context.v_detailQuarterlyEconomicReportAllTime.Where(r =>
            r.Year >= beginValueRangeDate.Year &&
            r.Year <= endValueRangeDate.Year && 
            (r.Year == beginValueRangeDate.Year ? r.Quarter >= beginValueRangeDate.Quarter : true) &&
            (r.Year == endValueRangeDate.Year ? r.Quarter <= endValueRangeDate.Quarter : true)).OrderBy(r => r.Year).ThenBy(r => r.Quarter).AsNoTracking().ToList();
        }

        public List<v_detailQuarterlyReportBestSalesPersonsAllTime> GetDetailQuarterlyReportBestSalesPersons(List<string> listSelectedSalesPersons, Date beginValueRangeDate, Date endValueRangeDate)
        {
            return _context.v_detailQuarterlyReportBestSalesPersonsAllTime.Where(r =>
            (listSelectedSalesPersons.Count > 0 ? listSelectedSalesPersons.Any(l => r.FullName == l) : false) &&
            r.Year >= beginValueRangeDate.Year &&
            r.Year <= endValueRangeDate.Year &&
            (r.Year == beginValueRangeDate.Year ? r.Quarter >= beginValueRangeDate.Quarter : true) &&
            (r.Year == endValueRangeDate.Year ? r.Quarter <= endValueRangeDate.Quarter : true)).OrderBy(r => r.Year).ThenBy(r => r.Quarter).AsNoTracking().ToList();
        }

        public List<v_detailQuarterlyReportPopularBoatsAllTime> GetDetailQuarterlyReportPopularBoats(List<string> listSelectedModelsBoats, Date beginValueRangeDate, Date endValueRangeDate)
        {
            return _context.v_detailQuarterlyReportPopularBoatsAllTime.Where(r =>
            (listSelectedModelsBoats.Count > 0 ? listSelectedModelsBoats.Any(l => r.Model == l) : false) &&
            r.Year >= beginValueRangeDate.Year &&
            r.Year <= endValueRangeDate.Year &&
            (r.Year == beginValueRangeDate.Year ? r.Quarter >= beginValueRangeDate.Quarter : true) &&
            (r.Year == endValueRangeDate.Year ? r.Quarter <= endValueRangeDate.Quarter : true)).OrderBy(r => r.Year).ThenBy(r => r.Quarter).AsNoTracking().ToList();
        }

        public List<v_detailYearlyEconomicReportAllTime> GetDetailYearlyEconomicReport(Date beginValueRangeDate, Date endValueRangeDate)
        {
            return _context.v_detailYearlyEconomicReportAllTime.Where(r =>
            r.Year >= beginValueRangeDate.Year &&
            r.Year <= endValueRangeDate.Year).OrderBy(r => r.Year).AsNoTracking().ToList();
        }

        public List<v_detailYearlyReportBestSalesPersonsAllTime> GetDetailYearlyReportBestSalesPersons(List<string> listSelectedSalesPersons, Date beginValueRangeDate, Date endValueRangeDate)
        {
            return _context.v_detailYearlyReportBestSalesPersonsAllTime.Where(r =>
            (listSelectedSalesPersons.Count > 0 ? listSelectedSalesPersons.Any(l => r.FullName == l) : false) &&
            r.Year >= beginValueRangeDate.Year &&
            r.Year <= endValueRangeDate.Year).OrderBy(r => r.Year).AsNoTracking().ToList();
        }

        public List<v_detailYearlyReportPopularBoatsAllTime> GetDetailYearlyReportPopularBoats(List<string> listSelectedModelsBoats, Date beginValueRangeDate, Date endValueRangeDate)
        {
            return _context.v_detailYearlyReportPopularBoatsAllTime.Where(r =>
            (listSelectedModelsBoats.Count > 0 ? listSelectedModelsBoats.Any(l => r.Model == l) : false) &&
            r.Year >= beginValueRangeDate.Year &&
            r.Year <= endValueRangeDate.Year).OrderBy(r => r.Year).AsNoTracking().ToList();
        }

        public List<string> GetStringListSalesPeople()
        {
            var list = new List<string>();

            GetSalesPeople().ForEach(s => list.Add($"{s.FirstName} {s.FamilyName}"));

            return list;
        }

        public List<SalesPerson> GetSalesPeople()
        {
            return _context.SalesPerson.AsNoTracking().ToList();
        }

        private void SetCommandTimeout()
        {
            var adapter = (IObjectContextAdapter)_context;
            var objectContext = adapter.ObjectContext;

            objectContext.CommandTimeout = _config.CommandTimeout;
        }
    }
}
