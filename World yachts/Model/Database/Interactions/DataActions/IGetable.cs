using System;
using System.Collections.Generic;
using World_yachts.Model.Database.Models;
using World_yachts.Model.Logic;

namespace World_yachts.Model.Database.Interactions.DataActions
{
    public interface IGetable
    {
        int GetIdBoat(string model);

        int GetMinPriceAccessory();

        int GetMinBasePrice();

        int GetMinNumberOfRowers();

        int GetMinInventory();

        int GetMinOrderLevel();

        int GetMinOrderBatch();

        int GetMaxPriceAccessory();

        int GetMaxBasePrice();

        int GetMaxNumberOfRowers();

        int GetMaxInventory();

        int GetMaxOrderLevel();

        int GetMaxOrderBatch();

        double GetMinVATAccessory();

        double GetMinVATBoat();

        double GetMaxVATAccessory();

        double GetMaxVATBoat();

        DateTime GetMinDateOfRegistration();

        DateTime GetMinDateOfLastChangePassword();

        DateTime GetMinWasOnline();

        DateTime GetMaxDateOfRegistration();

        DateTime GetMaxDateOfLastChangePassword();

        DateTime GetMaxWasOnline();

        v_accessory GetAccessory(int idAccessory);

        v_boat GetBoat(int idBoat);

        Colour GetColour(string nameColour);

        SalesPerson GetSalesPerson(int idSalesPerson);

        User GetUser(int idUser);

        List<string> GetTypesModel();

        List<string> GetStringListBoatTypes();

        List<string> GetStringListColours();

        List<string> GetStringListWoods();

        List<string> GetStringListPartners();

        List<string> GetStringListBoats();

        List<BoatType> GetBoatTypes();

        List<Colour> GetColours();

        List<Partner> GetPartners();

        List<Wood> GetWoods();

        List<Role> GetRoles();

        List<v_accessory> GetAccessories(string accName, Range<int> rangePrice, Range<double> rangeVAT, Range<int> rangeInventory, Range<int> rangeOrderLevel, Range<int> rangeOrderBatch, List<string> listSelectedPartners, List<string> listSelectedBoats);

        List<v_boat> GetBoats(string model, List<string> listSelectedBoatTypes, List<string> listSelectedModelType, Range<int> rangeNumberOfRowers, bool? thereIsMast, List<string> listSelectedColours, List<string> listSelectedWoods, Range<int> rangeBasePrice, Range<double> rangeVAT);

        List<v_boatSimplifiedInformation> GetBoats();

        List<v_user> GetUsers(string login, string roleName, Range<DateTime> rangeDateOfRegistration, Range<DateTime> rangeDateOfLastChangePassword, Range<DateTime> rangeWasOnline);
    }
}
