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

        public int GetMaxNumberOfRowers()
        {
            return _context.Boat.Max(b => b.NumberOfRowers);
        }

        public double GetMaxVATBoat()
        {
            return _context.Boat.Max(b => b.VAT);
        }

        public int GetMinBasePrice()
        {
            return _context.Boat.Min(b => b.BasePrice);
        }

        public int GetMinNumberOfRowers()
        {
            return _context.Boat.Min(b => b.NumberOfRowers);
        }

        public double GetMinVATBoat()
        {
            return _context.Boat.Min(b => b.VAT);
        }

        public List<Role> GetRoles()
        {
            return _context.Role.AsNoTracking().ToList();
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
    }
}
