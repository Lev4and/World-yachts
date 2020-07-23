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

        int GetMinDepositPayed();

        int GetMinContractTotalPrice();

        int GetMinContractTotalPriceInclVAT();

        int GetMaxPriceAccessory();

        int GetMaxBasePrice();

        int GetMaxNumberOfRowers();

        int GetMaxInventory();

        int GetMaxOrderLevel();

        int GetMaxOrderBatch();

        int GetMaxDepositPayed();

        int GetMaxContractTotalPrice();

        int GetMaxContractTotalPriceInclVAT();

        double GetMinVATAccessory();

        double GetMinVATBoat();

        double GetMaxVATAccessory();

        double GetMaxVATBoat();

        DateTime GetMinDateOfRegistration();

        DateTime GetMinDateOfLastChangePassword();

        DateTime GetMinWasOnline();

        DateTime GetMinDateOfConclusionContract();

        DateTime GetMinProductionStartDate();

        DateTime GetMaxDateOfRegistration();

        DateTime GetMaxDateOfLastChangePassword();

        DateTime GetMaxWasOnline();

        DateTime GetMaxDateOfConclusionContract();

        DateTime GetMaxProductionStartDate();

        v_accessory GetAccessory(int idAccessory);

        v_boat GetBoat(int idBoat);

        Colour GetColour(int idColour);

        Colour GetColour(string nameColour);

        v_contract GetContract(int idContract);

        v_contract2 GetContractExtendedVersion(int idContract);

        Customer GetCustomer(int idCustomer);

        SalesPerson GetSalesPerson(int idSalesPerson);

        Partner GetPartner(int idPartner);

        Order GetOrder(int idOrder);

        User GetUser(int idUser);

        List<string> GetTypesModel();

        List<string> GetStringListAccessories();

        List<string> GetStringListBoatTypes();

        List<string> GetStringListCitiesOrders();

        List<string> GetStringListColours();

        List<string> GetStringListDeliveryAddressOrders();

        List<string> GetStringListWoods();

        List<string> GetStringListPartners();

        List<string> GetStringListBoats();

        List<string> GetStringListSalesPeople();

        List<string> GetProductionProcess();

        List<BoatType> GetBoatTypes();

        List<Colour> GetColours();

        List<v_partner> GetPartners();

        List<v_partner> GetPartners(string name, string city);

        List<SalesPerson> GetSalesPeople();

        List<Wood> GetWoods();

        List<Role> GetRoles();

        List<v_accessorySimplifiedInformation> GetAccessories();

        List<v_accessory> GetAccessories(string accName, Range<int> rangePrice, Range<double> rangeVAT, Range<int> rangeInventory, Range<int> rangeOrderLevel, Range<int> rangeOrderBatch, List<string> listSelectedPartners, List<string> listSelectedBoats);

        List<v_accessory> GetAccessories(string modelBoat);

        List<v_boat> GetAllBoats();

        List<v_boat> GetBoats(string model, List<string> listSelectedBoatTypes, List<string> listSelectedModelType, Range<int> rangeNumberOfRowers, bool? thereIsMast, List<string> listSelectedColours, List<string> listSelectedWoods, Range<int> rangeBasePrice, Range<double> rangeVAT, Range<DateTime> rangeProductionStartDate);

        List<v_boatSimplifiedInformation> GetBoats();

        List<v_cityCustomer> GetCitiesCustomers();

        List<v_cityPartner> GetCitiesPartners();

        List<v_cityOrder> GetCitiesOrders();

        List<v_contract> GetContracts(List<string> listSelectedProductionProcess, Range<DateTime> rangeDateOfConclusionContract, Range<int> rangeDepositPayed, Range<int> rangeContractTotalPrice, Range<int> rangeContractTotalPriceInclVAT);

        List<v_contract2> GetContracts(List<string> listSelectedModelsBoats, List<string> listSelectedAccessoriesAtOrder, List<string> listSelectedProductionProcess, List<string> listSelectedDeliveryAddressOrders, List<string> listSelectedCitiesOrders, Range<DateTime> rangeDateOfConclusionContract, Range<int> rangeDepositPayed, Range<int> rangeContractTotalPrice, Range<int> rangeContractTotalPriceInclVAT);

        List<v_customer> GetCustomers();
        
        List<v_customer> GetCustomers(string fullName, string organisationName, string city);

        List<v_deliveryAddressOrder> GetDeliveryAddressOrders();

        List<v_organisationNameCustomer> GetOrganisationsNamesCustomers();

        List<v_user> GetUsers();

        List<v_user> GetUsers(string login, string roleName, Range<DateTime> rangeDateOfRegistration, Range<DateTime> rangeDateOfLastChangePassword, Range<DateTime> rangeWasOnline);

        List<v_briefReportBestSalesPersonsAllTime> GetReportBestSalesPersons(List<string> listSelectedSalesPersons);

        List<v_briefReportPopularBoatsAllTime> GetReportPopularBoats(List<string> listSelectedModelsBoats);

        List<v_detailDailyEconomicReportAllTime> GetDetailDailyEconomicReport(Range<DateTime> rangeDate);

        List<v_detailDailyReportBestSalesPersonsAllTime> GetDetailDailyReportBestSalesPersons(List<string> listSelectedSalesPersons, Range<DateTime> rangeDate);

        List<v_detailDailyReportPopularBoatsAllTime> GetDetailDailyReportPopularBoats(List<string> listSelectedModelsBoats, Range<DateTime> rangeDate);

        List<v_detailWeeklyEconomicReportAllTime> GetDetailWeeklyEconomicReport(Date beginValueRangeDate, Date endValueRangeDate);

        List<v_detailWeeklyReportBestSalesPersonsAllTime> GetDetailWeeklyReportBestSalesPersons(List<string> listSelectedSalesPersons, Date beginValueRangeDate, Date endValueRangeDate);

        List<v_detailWeeklyReportPopularBoatsAllTime> GetDetailWeeklyReportPopularBoats(List<string> listSelectedModelsBoats, Date beginValueRangeDate, Date endValueRangeDate);

        List<v_detailMonthlyEconomicReportAllTime> GetDetailMonthlyEconomicReport(Date beginValueRangeDate, Date endValueRangeDate);

        List<v_detailMonthlyReportBestSalesPersonsAllTime> GetDetailMonthlyReportBestSalesPersons(List<string> listSelectedSalesPersons, Date beginValueRangeDate, Date endValueRangeDate);

        List<v_detailMonthlyReportPopularBoatsAllTime> GetDetailMonthlyReportPopularBoats(List<string> listSelectedModelsBoats, Date beginValueRangeDate, Date endValueRangeDate);

        List<v_detailQuarterlyEconomicReportAllTime> GetDetailQuarterlyEconomicReport(Date beginValueRangeDate, Date endValueRangeDate);

        List<v_detailQuarterlyReportBestSalesPersonsAllTime> GetDetailQuarterlyReportBestSalesPersons(List<string> listSelectedSalesPersons, Date beginValueRangeDate, Date endValueRangeDate);

        List<v_detailQuarterlyReportPopularBoatsAllTime> GetDetailQuarterlyReportPopularBoats(List<string> listSelectedModelsBoats, Date beginValueRangeDate, Date endValueRangeDate);

        List<v_detailYearlyEconomicReportAllTime> GetDetailYearlyEconomicReport(Date beginValueRangeDate, Date endValueRangeDate);

        List<v_detailYearlyReportBestSalesPersonsAllTime> GetDetailYearlyReportBestSalesPersons(List<string> listSelectedSalesPersons, Date beginValueRangeDate, Date endValueRangeDate);

        List<v_detailYearlyReportPopularBoatsAllTime> GetDetailYearlyReportPopularBoats(List<string> listSelectedModelsBoats, Date beginValueRangeDate, Date endValueRangeDate);
    }
}
