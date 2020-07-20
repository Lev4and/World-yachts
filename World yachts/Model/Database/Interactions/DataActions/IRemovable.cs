namespace World_yachts.Model.Database.Interactions.DataActions
{
    public interface IRemovable
    {
        void RemoveAccessory(int idAccessory);

        void RemoveAccToBoats(int idAccessory);

        void RemoveContract(int idContract);

        void RemoveCustomer(int idCustomer);

        void RemovePartner(int idPartner);

        void RemoveOrder(int idOrder);

        void RemoveBoat(int idBoat);

        void RemoveUser(int idUser);
    }
}
