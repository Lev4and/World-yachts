using World_yachts.Model.Database.Models;

namespace World_yachts.Model.Database.Interactions.DataActions
{
    public interface IAddable
    {
        bool AddSalesPerson(int idSalesPerson, string firstName, string familyName);

        bool AddUser(int idRole, string login, string password);

        bool AddUser(int idRole, string login, string password, out User user);
    }
}
