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

        public bool CorrectDataUser(string login, string password)
        {
            return _context.v_user.SingleOrDefault(u => u.Login == login && u.Password == password) != null ? true : false;
        }

        public bool CorrectDataUser(string login, string password, out v_user user)
        {
            user = _context.v_user.SingleOrDefault(u => u.Login == login && u.Password == password);

            return user != null ? true : false;
        }
    }
}
