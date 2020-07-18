using DevExpress.Mvvm.Native;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using World_yachts.Model.Database.Models;
using World_yachts.Model.Logic;

namespace World_yachts.Model.Database.Interactions
{
    public class EntityFramework : IInteraction
    {
        private WorldYachtsContext _context;

        public EntityFramework()
        {
            _context = new WorldYachtsContext();
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

        public List<v_boat> GetBoats(string model, List<string> listSelectedBoatTypes, List<string> listSelectedModelType, Range<int> rangeNumberOfRowers, bool? thereIsMast, List<string> listSelectedColours, List<string> listSelectedWoods, Range<int> rangeBasePrice, Range<double> rangeVAT)
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

        public List<Partner> GetPartners()
        {
            return _context.Partner.AsNoTracking().ToList();
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
    }
}
