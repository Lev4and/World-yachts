using DevExpress.Mvvm;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using World_yachts.Model.Database.Interactions;
using World_yachts.Model.Database.Models;
using World_yachts.Model.Logic;
using World_yachts.Services;
using World_yachts.Views.Pages;
using World_yachts.Views.Windows;

namespace World_yachts.ViewModels
{
    public class EconomicReportViewModel : BindableBase
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

        public SeriesCollection Series4 { get; set; }

        public List<int> ListCount { get; set; }

        public List<int> ListTotalCount { get; set; }

        public List<decimal> ListDepositPayed { get; set; }

        public List<decimal> ListContractTotalPrice { get; set; }

        public List<decimal> ListContractTotalPriceInclVAT { get; set; }

        public List<decimal> ListTotalDepositPayed { get; set; }

        public List<decimal> ListTotalContractTotalPrice { get; set; }

        public List<decimal> ListTotalContractTotalPriceInclVAT { get; set; }

        public List<string> ListDates { get; set; }

        public List<v_detailDailyEconomicReportAllTime> DailyEconomicReport { get; set; }

        public List<v_detailWeeklyEconomicReportAllTime> WeeklyEconomicReport { get; set; }

        public List<v_detailMonthlyEconomicReportAllTime> MonthlyEconomicReport { get; set; }

        public List<v_detailQuarterlyEconomicReportAllTime> QuarterlyEconomicReport { get; set; }

        public List<v_detailYearlyEconomicReportAllTime> YearlyEconomicReport { get; set; }

        public EconomicReportViewModel(PageService pageService)
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

            ListCount = new List<int>();
            ListDepositPayed = new List<decimal>();
            ListContractTotalPrice = new List<decimal>();
            ListContractTotalPriceInclVAT = new List<decimal>();

            ListTotalCount = new List<int>();
            ListTotalDepositPayed = new List<decimal>();
            ListTotalContractTotalPrice = new List<decimal>();
            ListTotalContractTotalPriceInclVAT = new List<decimal>();

            ListDates = new List<string>();
        });

        public ICommand Show => new DelegateCommand(() =>
        {
            Loading();
        }, () => (TypeReport != null ? TypeReport.Length > 0 : false) && IsBackgroundTaskRunning == false);

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
            ListCount.Clear();
            ListDepositPayed.Clear();
            ListContractTotalPrice.Clear();
            ListContractTotalPriceInclVAT.Clear();

            ListTotalCount.Clear();
            ListTotalDepositPayed.Clear();
            ListTotalContractTotalPrice.Clear();
            ListTotalContractTotalPriceInclVAT.Clear();

            ListDates.Clear();
        }

        private void GettingAndFillData()
        {
            switch (TypeReport)
            {
                case "Группировка по дням":
                    {
                        DailyEconomicReport = _eF.GetDetailDailyEconomicReport(new Range<DateTime>(BeginValueDate, EndValueDate));

                        DailyEconomicReport.ForEach(r =>
                        {
                            ListCount.Add((int)r.Count);
                            ListDepositPayed.Add((decimal)r.DepositPayed);
                            ListContractTotalPrice.Add((decimal)r.ContractTotalPrice);
                            ListContractTotalPriceInclVAT.Add((decimal)r.ContractTotalPriceInclVAT);

                            ListTotalCount.Add((int)r.TotalCount);
                            ListTotalDepositPayed.Add((decimal)r.TotalDepositPayed);
                            ListTotalContractTotalPrice.Add((decimal)r.TotalContractTotalPrice);
                            ListTotalContractTotalPriceInclVAT.Add((decimal)r.TotalContractTotalPriceInclVAT);
                            ListDates.Add(((DateTime)r.Date).ToString("yyyy-MM-dd"));
                        });
                    }
                    break;
                case "Группировка по неделям":
                    {
                        WeeklyEconomicReport = _eF.GetDetailWeeklyEconomicReport(new Date(BeginValueDate), new Date(EndValueDate));

                        WeeklyEconomicReport.ForEach(r =>
                        {
                            ListCount.Add((int)r.Count);
                            ListDepositPayed.Add((decimal)r.DepositPayed);
                            ListContractTotalPrice.Add((decimal)r.ContractTotalPrice);
                            ListContractTotalPriceInclVAT.Add((decimal)r.ContractTotalPriceInclVAT);

                            ListTotalCount.Add((int)r.TotalCount);
                            ListTotalDepositPayed.Add((decimal)r.TotalDepositPayed);
                            ListTotalContractTotalPrice.Add((decimal)r.TotalContractTotalPrice);
                            ListTotalContractTotalPriceInclVAT.Add((decimal)r.TotalContractTotalPriceInclVAT);
                            ListDates.Add(r.Date);
                        });
                    }
                    break;
                case "Группировка по месяцам":
                    {
                        MonthlyEconomicReport = _eF.GetDetailMonthlyEconomicReport(new Date(BeginValueDate), new Date(EndValueDate));

                        MonthlyEconomicReport.ForEach(r =>
                        {
                            ListCount.Add((int)r.Count);
                            ListDepositPayed.Add((decimal)r.DepositPayed);
                            ListContractTotalPrice.Add((decimal)r.ContractTotalPrice);
                            ListContractTotalPriceInclVAT.Add((decimal)r.ContractTotalPriceInclVAT);

                            ListTotalCount.Add((int)r.TotalCount);
                            ListTotalDepositPayed.Add((decimal)r.TotalDepositPayed);
                            ListTotalContractTotalPrice.Add((decimal)r.TotalContractTotalPrice);
                            ListTotalContractTotalPriceInclVAT.Add((decimal)r.TotalContractTotalPriceInclVAT);
                            ListDates.Add(r.Date);
                        });
                    }
                    break;
                case "Группировка по кварталам":
                    {
                        QuarterlyEconomicReport = _eF.GetDetailQuarterlyEconomicReport(new Date(BeginValueDate), new Date(EndValueDate));

                        QuarterlyEconomicReport.ForEach(r =>
                        {
                            ListCount.Add((int)r.Count);
                            ListDepositPayed.Add((decimal)r.DepositPayed);
                            ListContractTotalPrice.Add((decimal)r.ContractTotalPrice);
                            ListContractTotalPriceInclVAT.Add((decimal)r.ContractTotalPriceInclVAT);

                            ListTotalCount.Add((int)r.TotalCount);
                            ListTotalDepositPayed.Add((decimal)r.TotalDepositPayed);
                            ListTotalContractTotalPrice.Add((decimal)r.TotalContractTotalPrice);
                            ListTotalContractTotalPriceInclVAT.Add((decimal)r.TotalContractTotalPriceInclVAT);
                            ListDates.Add(r.Date);
                        });
                    }
                    break;
                case "Группировка по годам":
                    {
                        YearlyEconomicReport = _eF.GetDetailYearlyEconomicReport(new Date(BeginValueDate), new Date(EndValueDate));

                        YearlyEconomicReport.ForEach(r =>
                        {
                            ListCount.Add((int)r.Count);
                            ListDepositPayed.Add((decimal)r.DepositPayed);
                            ListContractTotalPrice.Add((decimal)r.ContractTotalPrice);
                            ListContractTotalPriceInclVAT.Add((decimal)r.ContractTotalPriceInclVAT);

                            ListTotalCount.Add((int)r.TotalCount);
                            ListTotalDepositPayed.Add((decimal)r.TotalDepositPayed);
                            ListTotalContractTotalPrice.Add((decimal)r.TotalContractTotalPrice);
                            ListTotalContractTotalPriceInclVAT.Add((decimal)r.TotalContractTotalPriceInclVAT);
                            ListDates.Add(r.Date);
                        });
                    }
                    break;
            }
        }

        private void SetSeries()
        {
            Series1 = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Общий депозит в ₽", Values = new ChartValues<decimal>(ListTotalDepositPayed)
                },

                new LineSeries
                {
                    Title = "Общая стоимость контрактов в ₽", Values = new ChartValues<decimal>(ListTotalContractTotalPrice)
                },

                new LineSeries
                {
                    Title = "Общая стоимость контрактов с НДС в ₽", Values = new ChartValues<decimal>(ListTotalContractTotalPriceInclVAT)
                },
            };

            Series2 = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Депозит в ₽", Values = new ChartValues<decimal>(ListDepositPayed)
                },

                new LineSeries
                {
                    Title = "Стоимость контракта в ₽", Values = new ChartValues<decimal>(ListContractTotalPrice)
                },

                new LineSeries
                {
                    Title = "Стоимость контракта с НДС в ₽", Values = new ChartValues<decimal>(ListContractTotalPriceInclVAT)
                },
            };

            Series3 = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Договоры (кол-во)", Values = new ChartValues<int>(ListCount), Fill = new SolidColorBrush(Colors.Lime),
                },
            };

            Series4 = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Договоры (общее кол-во)", Values = new ChartValues<int>(ListTotalCount), Fill = new SolidColorBrush(Colors.Orange),
                },
            };

            ArrayDates = ListDates.ToArray();
        }
    }
}
