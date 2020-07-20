using System;
using System.Collections.Generic;
using World_yachts.Model.Database.Models;

namespace World_yachts.Model.Database.Interactions.DataActions
{
    public interface IFunctionable
    {
        bool NecessaryChagePassword(DateTime dateOfLastChangePassword);

        bool NecessaryChagePassword(User user);

        bool NecessaryChagePassword(v_user user);

        void EnterIntoContract(int idSalesPerson, int idCustomer, int idBoat, string deliveryAddress, string city, List<int> listSelectedAccessories, int depositPayed, int contractTotalPrice, int contractTotalPriceInclVAT);
    }
}
