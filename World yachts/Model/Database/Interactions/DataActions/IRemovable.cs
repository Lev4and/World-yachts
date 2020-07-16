namespace World_yachts.Model.Database.Interactions.DataActions
{
    public interface IRemovable
    {
        void RemoveAccessory(int idAccessory);

        void RemoveAccToBoats(int idAccessory);

        void RemoveBoat(int idBoat);
    }
}
