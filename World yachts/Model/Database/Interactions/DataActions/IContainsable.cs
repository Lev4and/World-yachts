namespace World_yachts.Model.Database.Interactions.DataActions
{
    public interface IContainsable
    {
        bool ContainsSalesPerson(int idSalesPerson);

        bool ContainsUser(string login);
    }
}
