using Converters;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using World_yachts.Model.Database.Interactions;
using World_yachts.Model.Database.Models;
using World_yachts.Model.Logic;
using World_yachts.Services;
using World_yachts.Views.Pages;
using World_yachts.Views.Windows;

namespace World_yachts.ViewModels
{
    public class BestSalesPersonsViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private EntityFramework _eF;

        public bool IsBackgroundTaskRunning { get; set; }

        public string TypeReport { get; set; }

        public DateTime MinValueDate { get; set; }

        public DateTime MaxValueDate { get; set; }

        public DateTime BeginValueDate { get; set; }

        public DateTime EndValueDate { get; set; }

        public string[] Labels { get; set; }

        public string[] ArrayDates { get; set; }

        public SeriesCollection Series1 { get; set; }

        public SeriesCollection Series2 { get; set; }

        public SeriesCollection Series3 { get; set; }

        public ObservableCollection<string> ListSelectedSalesPersons { get; set; }

        public List<string> ListDates { get; set; }

        public List<v_briefReportBestSalesPersonsAllTime> BriefBestSalesPersonsReport { get; set; }

        public List<v_detailDailyReportBestSalesPersonsAllTime> DailyBestSalesPersonsReport { get; set; }

        public List<v_detailWeeklyReportBestSalesPersonsAllTime> WeeklyBestSalesPersonsReport { get; set; }

        public List<v_detailMonthlyReportBestSalesPersonsAllTime> MonthlyBestSalesPersonsReport { get; set; }

        public List<v_detailQuarterlyReportBestSalesPersonsAllTime> QuarterlyBestSalesPersonsReport { get; set; }

        public List<v_detailYearlyReportBestSalesPersonsAllTime> YearlyBestSalesPersonsReport { get; set; }

        public ObservableCollection<string> SalesPersons { get; set; }

        public BestSalesPersonsViewModel(PageService pageService)
        {
            _pageService = pageService;
        }

        public ICommand Loaded => new DelegateCommand(() =>
        {
            _eF = new EntityFramework();

            TypeReport = "";

            MinValueDate = _eF.GetMinDateOfConclusionContract();
            MaxValueDate = _eF.GetMaxDateOfConclusionContract();

            BeginValueDate = MinValueDate;
            EndValueDate = MaxValueDate;

            SalesPersons = CollectionConverter<string>.ConvertToObservableCollection(_eF.GetStringListSalesPeople());

            ListDates = new List<string>();
            ListSelectedSalesPersons = new ObservableCollection<string>();
        });

        public ICommand Show => new DelegateCommand(() =>
        {
            Loading();
        }, () => (TypeReport != null ? TypeReport.Length > 0 : false) && IsBackgroundTaskRunning == false && ListSelectedSalesPersons.Count > 0);

        public ICommand Back => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new Reports());
        }, () => IsBackgroundTaskRunning == false);

        private async void Loading()
        {
            await Task.Run(() =>
            {
                IsBackgroundTaskRunning = true;

                Thread.Sleep(2000);

                ClearLists();

                GettingAndFillData();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    SetSeries();
                });

                IsBackgroundTaskRunning = false;
            });
        }

        private void ClearLists()
        {
            ListDates.Clear();
        }

        private void GettingAndFillData()
        {
            BriefBestSalesPersonsReport = _eF.GetReportBestSalesPersons(ListSelectedSalesPersons.ToList());

            switch (TypeReport)
            {
                case "Группировка по дням":
                    {
                        DailyBestSalesPersonsReport = _eF.GetDetailDailyReportBestSalesPersons(ListSelectedSalesPersons.ToList(), new Range<DateTime>(BeginValueDate, EndValueDate));
                        DailyBestSalesPersonsReport.ForEach(i =>
                        {
                            if (!ListDates.Contains(((DateTime)i.Date).ToString("yyyy-MM-dd")))
                                ListDates.Add(((DateTime)i.Date).ToString("yyyy-MM-dd"));
                        });
                    }
                    break;
                case "Группировка по неделям":
                    {
                        WeeklyBestSalesPersonsReport = _eF.GetDetailWeeklyReportBestSalesPersons(ListSelectedSalesPersons.ToList(), new Date(BeginValueDate), new Date(EndValueDate));
                        WeeklyBestSalesPersonsReport.ForEach(i =>
                        {
                            if (!ListDates.Contains(i.Date))
                                ListDates.Add(i.Date);
                        });
                    }
                    break;
                case "Группировка по месяцам":
                    {
                        MonthlyBestSalesPersonsReport = _eF.GetDetailMonthlyReportBestSalesPersons(ListSelectedSalesPersons.ToList(), new Date(BeginValueDate), new Date(EndValueDate));
                        MonthlyBestSalesPersonsReport.ForEach(i =>
                        {
                            if (!ListDates.Contains(i.Date))
                                ListDates.Add(i.Date);
                        });
                    }
                    break;
                case "Группировка по кварталам":
                    {
                        QuarterlyBestSalesPersonsReport = _eF.GetDetailQuarterlyReportBestSalesPersons(ListSelectedSalesPersons.ToList(), new Date(BeginValueDate), new Date(EndValueDate));
                        QuarterlyBestSalesPersonsReport.ForEach(i =>
                        {
                            if (!ListDates.Contains(i.Date))
                                ListDates.Add(i.Date);
                        });
                    }
                    break;
                case "Группировка по годам":
                    {
                        YearlyBestSalesPersonsReport = _eF.GetDetailYearlyReportBestSalesPersons(ListSelectedSalesPersons.ToList(), new Date(BeginValueDate), new Date(EndValueDate));
                        YearlyBestSalesPersonsReport.ForEach(i =>
                        {
                            if (!ListDates.Contains(i.Date))
                                ListDates.Add(i.Date);
                        });
                    }
                    break;
            }
        }

        private void SetSeries()
        {
            Series1 = new SeriesCollection();
            Series2 = new SeriesCollection();
            Series3 = new SeriesCollection();

            for (int i = 0; i < ListSelectedSalesPersons.Count; i++)
            {
                Series1.Add(new LineSeries { Title = $"{ListSelectedSalesPersons[i]}", Values = new ChartValues<double>(GetCollection(ListSelectedSalesPersons[i], "Count")) });
                Series2.Add(new LineSeries { Title = $"{ListSelectedSalesPersons[i]}", Values = new ChartValues<double>(GetCollection(ListSelectedSalesPersons[i], "Rate")) });
                Series3.Add(new PieSeries { Title = $"{ListSelectedSalesPersons[i]}", Values = new ChartValues<int>(new List<int> { (int)BriefBestSalesPersonsReport.Single(r => r.FullName == ListSelectedSalesPersons[i]).Count }) });
            }

            ArrayDates = ListDates.ToArray();
        }

        private IEnumerable<double> GetCollection(string fullName, string property)
        {
            var collection = new List<double>();

            switch (TypeReport)
            {
                case "Группировка по дням":
                    {
                        if (property == "Count")
                        {
                            foreach (var date in ListDates)
                            {
                                if (DailyBestSalesPersonsReport.Find(r => r.FullName == fullName && ((DateTime)r.Date).ToString("yyyy-MM-dd") == date) == null)
                                    collection.Add(double.NaN);
                            }

                            DailyBestSalesPersonsReport.Where(r => r.FullName == fullName).ForEach(i => collection.Add((double)i.Count));
                        }

                        if (property == "Rate")
                        {
                            foreach (var date in ListDates)
                            {
                                if (DailyBestSalesPersonsReport.Find(r => r.FullName == fullName && ((DateTime)r.Date).ToString("yyyy-MM-dd") == date) == null)
                                    collection.Add(double.NaN);
                            }

                            DailyBestSalesPersonsReport.Where(r => r.FullName == fullName).ForEach(i => collection.Add((double)i.Rate));
                        }
                    }
                    break;
                case "Группировка по неделям":
                    {
                        if (property == "Count")
                        {
                            foreach (var date in ListDates)
                            {
                                if (WeeklyBestSalesPersonsReport.Find(r => r.FullName == fullName && r.Date == date) == null)
                                    collection.Add(double.NaN);
                            }

                            WeeklyBestSalesPersonsReport.Where(r => r.FullName == fullName).ForEach(i => collection.Add((double)i.Count));
                        }

                        if (property == "Rate")
                        {
                            foreach (var date in ListDates)
                            {
                                if (WeeklyBestSalesPersonsReport.Find(r => r.FullName == fullName && r.Date == date) == null)
                                    collection.Add(double.NaN);
                            }

                            WeeklyBestSalesPersonsReport.Where(r => r.FullName == fullName).ForEach(i => collection.Add((double)i.Rate));
                        }
                    }
                    break;
                case "Группировка по месяцам":
                    {
                        if (property == "Count")
                        {
                            foreach (var date in ListDates)
                            {
                                if (MonthlyBestSalesPersonsReport.Find(r => r.FullName == fullName && r.Date == date) == null)
                                    collection.Add(double.NaN);
                            }

                            MonthlyBestSalesPersonsReport.Where(r => r.FullName == fullName).ForEach(i => collection.Add((double)i.Count));
                        }

                        if (property == "Rate")
                        {
                            foreach (var date in ListDates)
                            {
                                if (MonthlyBestSalesPersonsReport.Find(r => r.FullName == fullName && r.Date == date) == null)
                                    collection.Add(double.NaN);
                            }

                            MonthlyBestSalesPersonsReport.Where(r => r.FullName == fullName).ForEach(i => collection.Add((double)i.Rate));
                        }
                    }
                    break;
                case "Группировка по кварталам":
                    {
                        if (property == "Count")
                        {
                            foreach (var date in ListDates)
                            {
                                if (QuarterlyBestSalesPersonsReport.Find(r => r.FullName == fullName && r.Date == date) == null)
                                    collection.Add(double.NaN);
                            }

                            QuarterlyBestSalesPersonsReport.Where(r => r.FullName == fullName).ForEach(i => collection.Add((double)i.Count));
                        }

                        if (property == "Rate")
                        {
                            foreach (var date in ListDates)
                            {
                                if (QuarterlyBestSalesPersonsReport.Find(r => r.FullName == fullName && r.Date == date) == null)
                                    collection.Add(double.NaN);
                            }

                            QuarterlyBestSalesPersonsReport.Where(r => r.FullName == fullName).ForEach(i => collection.Add((double)i.Rate));
                        }
                    }
                    break;
                case "Группировка по годам":
                    {
                        if (property == "Count")
                        {
                            foreach (var date in ListDates)
                            {
                                if (YearlyBestSalesPersonsReport.Find(r => r.FullName == fullName && r.Date == date) == null)
                                    collection.Add(double.NaN);
                            }

                            YearlyBestSalesPersonsReport.Where(r => r.FullName == fullName).ForEach(i => collection.Add((double)i.Count));
                        }

                        if (property == "Rate")
                        {
                            foreach (var date in ListDates)
                            {
                                if (YearlyBestSalesPersonsReport.Find(r => r.FullName == fullName && r.Date == date) == null)
                                    collection.Add(double.NaN);
                            }

                            YearlyBestSalesPersonsReport.Where(r => r.FullName == fullName).ForEach(i => collection.Add((double)i.Rate));
                        }
                    }
                    break;
            }

            return collection;
        }
    }
}
