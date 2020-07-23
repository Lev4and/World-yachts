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

namespace World_yachts.ViewModels
{
    public class PopularBoatsViewModel : BindableBase
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

        public ObservableCollection<string> ListSelectedBoats { get; set; }

        public List<string> ListDates { get; set; }

        public List<v_briefReportPopularBoatsAllTime> BriefPopularBoatsReport { get; set; }

        public List<v_detailDailyReportPopularBoatsAllTime> DailyPopularBoatsReport { get; set; }

        public List<v_detailWeeklyReportPopularBoatsAllTime> WeeklyPopularBoatsReport { get; set; }

        public List<v_detailMonthlyReportPopularBoatsAllTime> MonthlyPopularBoatsReport { get; set; }

        public List<v_detailQuarterlyReportPopularBoatsAllTime> QuarterlyPopularBoatsReport { get; set; }

        public List<v_detailYearlyReportPopularBoatsAllTime> YearlyPopularBoatsReport { get; set; }

        public ObservableCollection<string> Boats { get; set; }

        public PopularBoatsViewModel(PageService pageService)
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

            Boats = CollectionConverter<string>.ConvertToObservableCollection(_eF.GetStringListBoats());

            ListDates = new List<string>();
            ListSelectedBoats = new ObservableCollection<string>();
        });

        public ICommand Show => new DelegateCommand(() =>
        {
            Loading();
        }, () => (TypeReport != null ? TypeReport.Length > 0 : false) && IsBackgroundTaskRunning == false && ListSelectedBoats.Count > 0);

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
            BriefPopularBoatsReport = _eF.GetReportPopularBoats(ListSelectedBoats.ToList());

            switch (TypeReport)
            {
                case "Группировка по дням":
                    {
                        DailyPopularBoatsReport = _eF.GetDetailDailyReportPopularBoats(ListSelectedBoats.ToList(), new Range<DateTime>(BeginValueDate, EndValueDate));
                        DailyPopularBoatsReport.ForEach(i => 
                        { 
                            if(!ListDates.Contains(((DateTime)i.Date).ToString("yyyy-MM-dd")))
                                ListDates.Add(((DateTime)i.Date).ToString("yyyy-MM-dd"));
                        });
                    }
                    break;
                case "Группировка по неделям":
                    {
                        WeeklyPopularBoatsReport = _eF.GetDetailWeeklyReportPopularBoats(ListSelectedBoats.ToList(), new Date(BeginValueDate), new Date(EndValueDate));
                        WeeklyPopularBoatsReport.ForEach(i => 
                        { 
                            if(!ListDates.Contains(i.Date))
                                ListDates.Add(i.Date);
                        });
                    }
                    break;
                case "Группировка по месяцам":
                    {
                        MonthlyPopularBoatsReport = _eF.GetDetailMonthlyReportPopularBoats(ListSelectedBoats.ToList(), new Date(BeginValueDate), new Date(EndValueDate));
                        MonthlyPopularBoatsReport.ForEach(i =>
                        {
                            if (!ListDates.Contains(i.Date))
                                ListDates.Add(i.Date);
                        });
                    }
                    break;
                case "Группировка по кварталам":
                    {
                        QuarterlyPopularBoatsReport = _eF.GetDetailQuarterlyReportPopularBoats(ListSelectedBoats.ToList(), new Date(BeginValueDate), new Date(EndValueDate));
                        QuarterlyPopularBoatsReport.ForEach(i =>
                        {
                            if (!ListDates.Contains(i.Date))
                                ListDates.Add(i.Date);
                        });
                    }
                    break;
                case "Группировка по годам":
                    {
                        YearlyPopularBoatsReport = _eF.GetDetailYearlyReportPopularBoats(ListSelectedBoats.ToList(), new Date(BeginValueDate), new Date(EndValueDate));
                        YearlyPopularBoatsReport.ForEach(i =>
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

            for (int i = 0; i < ListSelectedBoats.Count; i++)
            {
                Series1.Add(new LineSeries { Title = $"{ListSelectedBoats[i]}", Values = new ChartValues<double>(GetCollection(ListSelectedBoats[i], "Count")) });
                Series2.Add(new LineSeries { Title = $"{ListSelectedBoats[i]}", Values = new ChartValues<double>(GetCollection(ListSelectedBoats[i], "Rate")) });
                Series3.Add(new PieSeries { Title = $"{ListSelectedBoats[i]}", Values = new ChartValues<int>(new List<int> { (int)BriefPopularBoatsReport.Single(r => r.Model == ListSelectedBoats[i]).Count })});
            }

            ArrayDates = ListDates.ToArray();
        }

        private int GetIndexBoat(string modelBoat)
        {
            for(int i = 0; i < Boats.Count; i++)
            {
                if (Boats[i] == modelBoat)
                    return i;
            }

            return 0;
        }

        private IEnumerable<double> GetCollection(string modelBoat, string property)
        {
            var collection = new List<double>();

            switch (TypeReport)
            {
                case "Группировка по дням":
                    {
                        if(property == "Count")
                        {
                            foreach(var date in ListDates)
                            {
                                if (DailyPopularBoatsReport.Find(r => r.Model == modelBoat && ((DateTime)r.Date).ToString("yyyy-MM-dd") == date) == null)
                                    collection.Add(double.NaN);
                            }

                            DailyPopularBoatsReport.Where(r => r.Model == modelBoat).ForEach(i => collection.Add((double)i.Count));
                        }

                        if (property == "Rate")
                        {
                            foreach (var date in ListDates)
                            {
                                if (DailyPopularBoatsReport.Find(r => r.Model == modelBoat && ((DateTime)r.Date).ToString("yyyy-MM-dd") == date) == null)
                                    collection.Add(double.NaN);
                            }

                            DailyPopularBoatsReport.Where(r => r.Model == modelBoat).ForEach(i => collection.Add((double)i.Rate));
                        }
                    }
                    break;
                case "Группировка по неделям":
                    {
                        if (property == "Count")
                        {
                            foreach (var date in ListDates)
                            {
                                if (WeeklyPopularBoatsReport.Find(r => r.Model == modelBoat && r.Date == date) == null)
                                    collection.Add(double.NaN);
                            }

                            WeeklyPopularBoatsReport.Where(r => r.Model == modelBoat).ForEach(i => collection.Add((double)i.Count));
                        }

                        if (property == "Rate")
                        {
                            foreach (var date in ListDates)
                            {
                                if (WeeklyPopularBoatsReport.Find(r => r.Model == modelBoat && r.Date == date) == null)
                                    collection.Add(double.NaN);
                            }

                            WeeklyPopularBoatsReport.Where(r => r.Model == modelBoat).ForEach(i => collection.Add((double)i.Rate));
                        }
                    }
                    break;
                case "Группировка по месяцам":
                    {
                        if (property == "Count")
                        {
                            foreach (var date in ListDates)
                            {
                                if (MonthlyPopularBoatsReport.Find(r => r.Model == modelBoat && r.Date == date) == null)
                                    collection.Add(double.NaN);
                            }

                            MonthlyPopularBoatsReport.Where(r => r.Model == modelBoat).ForEach(i => collection.Add((double)i.Count));
                        }

                        if (property == "Rate")
                        {
                            foreach (var date in ListDates)
                            {
                                if (MonthlyPopularBoatsReport.Find(r => r.Model == modelBoat && r.Date == date) == null)
                                    collection.Add(double.NaN);
                            }

                            MonthlyPopularBoatsReport.Where(r => r.Model == modelBoat).ForEach(i => collection.Add((double)i.Rate));
                        }
                    }
                    break;
                case "Группировка по кварталам":
                    {
                        if (property == "Count")
                        {
                            foreach (var date in ListDates)
                            {
                                if (QuarterlyPopularBoatsReport.Find(r => r.Model == modelBoat && r.Date == date) == null)
                                    collection.Add(double.NaN);
                            }

                            QuarterlyPopularBoatsReport.Where(r => r.Model == modelBoat).ForEach(i => collection.Add((double)i.Count));
                        }

                        if (property == "Rate")
                        {
                            foreach (var date in ListDates)
                            {
                                if (QuarterlyPopularBoatsReport.Find(r => r.Model == modelBoat && r.Date == date) == null)
                                    collection.Add(double.NaN);
                            }

                            QuarterlyPopularBoatsReport.Where(r => r.Model == modelBoat).ForEach(i => collection.Add((double)i.Rate));
                        }
                    }
                    break;
                case "Группировка по годам":
                    {
                        if (property == "Count")
                        {
                            foreach (var date in ListDates)
                            {
                                if (YearlyPopularBoatsReport.Find(r => r.Model == modelBoat && r.Date == date) == null)
                                    collection.Add(double.NaN);
                            }

                            YearlyPopularBoatsReport.Where(r => r.Model == modelBoat).ForEach(i => collection.Add((double)i.Count));
                        }

                        if(property == "Rate")
                        {
                            foreach (var date in ListDates)
                            {
                                if (YearlyPopularBoatsReport.Find(r => r.Model == modelBoat && r.Date == date) == null)
                                    collection.Add(double.NaN);
                            }

                            YearlyPopularBoatsReport.Where(r => r.Model == modelBoat).ForEach(i => collection.Add((double)i.Rate));
                        }
                    }
                    break;
            }

            return collection;
        }
    }
}
