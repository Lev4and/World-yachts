using System.Collections.Generic;
using World_yachts.Model.Database.Models;
using World_yachts.Model.Logic;

namespace World_yachts.Model.Database.Interactions.DataActions
{
    public interface IGetable
    {
        int GetMinBasePrice();

        int GetMinNumberOfRowers();

        int GetMaxBasePrice();

        int GetMaxNumberOfRowers();

        double GetMinVATBoat();

        double GetMaxVATBoat();

        v_boat GetBoat(int idBoat);

        Colour GetColour(string nameColour);

        List<string> GetTypesModel();

        List<string> GetStringListBoatTypes();

        List<string> GetStringListColours();

        List<string> GetStringListWoods();

        List<BoatType> GetBoatTypes();

        List<Colour> GetColours();

        List<Wood> GetWoods();

        List<Role> GetRoles();

        List<v_boat> GetBoats(string model, List<string> listSelectedBoatTypes, List<string> listSelectedModelType, Range<int> rangeNumberOfRowers, bool? thereIsMast, List<string> listSelectedColours, List<string> listSelectedWoods, Range<int> rangeBasePrice, Range<double> rangeVAT);
    }
}
