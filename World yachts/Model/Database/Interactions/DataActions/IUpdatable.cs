using System.Collections.Generic;
using World_yachts.Model.Database.Models;

namespace World_yachts.Model.Database.Interactions.DataActions
{
    public interface IUpdatable
    {
        void UpdateAccessory(int idAccessory, string descriptionOfAccessory, int price, double VAT, int inventory, int orderLevel, int orderBatch, List<string> listSelectedCompatibleModelBoats);

        void UpdateBoat(int idBoat, int basePrice, double VAT);

        void UpdatePassword(int idUser, string newPassword);

        void UpdateValueWasOnline(int idUser);

        void UpdateValueWasOnline(string login);

        void UpdateValueWasOnline(User user);

        void UpdateValueWasOnline(v_user user);
    }
}
