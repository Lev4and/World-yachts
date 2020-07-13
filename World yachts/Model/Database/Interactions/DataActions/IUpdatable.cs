using World_yachts.Model.Database.Models;

namespace World_yachts.Model.Database.Interactions.DataActions
{
    public interface IUpdatable
    {
        void UpdateValueWasOnline(int idUser);

        void UpdateValueWasOnline(string login);

        void UpdateValueWasOnline(User user);

        void UpdateValueWasOnline(v_user user);
    }
}
