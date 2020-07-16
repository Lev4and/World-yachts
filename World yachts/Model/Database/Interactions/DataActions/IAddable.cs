using System.Collections.Generic;
using World_yachts.Model.Database.Models;

namespace World_yachts.Model.Database.Interactions.DataActions
{
    public interface IAddable
    {
        bool AddAccessory(string accName, string descriptionOfAccessory, int price, double VAT, int inventory, int orderLevel, int orderBatch, int idPartner, List<string> listSelectedCompatibleModelBoats);

        bool AddAccToBoats(int idAccessory, int idBoat);

        bool AddBoat(string model, int idBoatType, int numberOfRowers, bool mast, int idColour, int idWood, int basePrice, double VAT);

        bool AddColour(string nameColour);

        bool AddColour(string nameColour, out Colour colour);

        bool AddSalesPerson(int idSalesPerson, string firstName, string familyName);

        bool AddUser(int idRole, string login, string password);

        bool AddUser(int idRole, string login, string password, out User user);
    }
}
