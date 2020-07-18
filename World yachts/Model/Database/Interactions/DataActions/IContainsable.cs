namespace World_yachts.Model.Database.Interactions.DataActions
{
    public interface IContainsable
    {
        bool ContainsAccessory(string accName);

        bool ContainsAccToBoats(int idAccessory, int idBoat);

        bool ContainsBoat(string model);

        bool ContainsColour(string nameColour);

        bool ContainsSalesPerson(int idSalesPerson);

        bool ContainsPartner(string name, string address, string city);

        bool ContainsUser(string login);
    }
}
