using System;
using System.Linq;
using World_yachts.Model.Database.Models;

namespace World_yachts.Model.Database.Interactions
{
    public class EntityFramework : IInteraction
    {
        private WorldYachtsContext _context;

        public EntityFramework()
        {
            _context = new WorldYachtsContext();
        }

        public void BlockingUsers()
        {
            _context.sp_toBlockUser();
        }

        public bool CorrectDataUser(string login, string password)
        {
            return _context.v_user.SingleOrDefault(u => u.Login == login && u.Password == password) != null ? true : false;
        }

        public bool CorrectDataUser(string login, string password, out v_user user)
        {
            user = _context.v_user.SingleOrDefault(u => u.Login == login && u.Password == password);

            return user != null ? true : false;
        }

        public bool NecessaryChagePassword(DateTime dateOfLastChangePassword)
        {
            return (DateTime.Now - dateOfLastChangePassword).Days >= 14 ? true : false;
        }

        public bool NecessaryChagePassword(User user)
        {
            return (DateTime.Now - user.DateOfLastChangePassword).Days >= 14 ? true : false;
        }

        public bool NecessaryChagePassword(v_user user)
        {
            return (DateTime.Now - user.DateOfLastChangePassword).Days >= 14 ? true : false;
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
