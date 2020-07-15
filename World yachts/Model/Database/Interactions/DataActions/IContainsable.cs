namespace World_yachts.Model.Database.Interactions.DataActions
{
    public interface IContainsable
    {
        bool ContainsBoat(string model);

        bool ContainsColour(string nameColour);

        bool ContainsSalesPerson(int idSalesPerson);

        bool ContainsUser(string login);
    }
}
