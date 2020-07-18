using System;
using System.Windows;
using System.Windows.Controls;

namespace World_yachts.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для DoubleDatePicker.xaml
    /// </summary>
    public partial class DoubleDatePicker : UserControl
    {
        public DateTime? MinSelectedDate
        {
            get { return (DateTime?)GetValue(MinSelectedDateProperty); }
            set { SetValue(MinSelectedDateProperty, value); }
        }

        public static readonly DependencyProperty MinSelectedDateProperty =
            DependencyProperty.Register("MinSelectedDate", typeof(DateTime?), typeof(DoubleDatePicker), new PropertyMetadata((DateTime?)DateTime.MinValue));


        public DateTime? BeginSelectedDate
        {
            get { return (DateTime?)GetValue(BeginSelectedDateProperty); }
            set { SetValue(BeginSelectedDateProperty, value); }
        }

        public static readonly DependencyProperty BeginSelectedDateProperty =
            DependencyProperty.Register("BeginSelectedDate", typeof(DateTime?), typeof(DoubleDatePicker), new PropertyMetadata((DateTime?)DateTime.MinValue));

        public DateTime? EndSelectedDate
        {
            get { return (DateTime?)GetValue(EndSelectedDateProperty); }
            set { SetValue(EndSelectedDateProperty, value); }
        }

        public static readonly DependencyProperty EndSelectedDateProperty =
            DependencyProperty.Register("EndSelectedDate", typeof(DateTime?), typeof(DoubleDatePicker), new PropertyMetadata((DateTime?)DateTime.MaxValue));

        public DateTime? MaxSelectedDate
        {
            get { return (DateTime?)GetValue(MaxSelectedDateProperty); }
            set { SetValue(MaxSelectedDateProperty, value); }
        }

        public static readonly DependencyProperty MaxSelectedDateProperty =
            DependencyProperty.Register("MaxSelectedDate", typeof(DateTime?), typeof(DoubleDatePicker), new PropertyMetadata((DateTime?)DateTime.MaxValue));

        public double HeightGrid { get; set; } = 60;
        public Thickness MarginForGrid1 { get; set; } = new Thickness(0, 0, 0, 0);
        public Thickness MarginForGrid2 { get; set; } = new Thickness(0, 30, 0, 0);

        public DoubleDatePicker()
        {
            InitializeComponent();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            HeightGrid = _this.Height / 2;

            MarginForGrid1 = new Thickness(0, 0, 0, 0);
            MarginForGrid2 = new Thickness(0, HeightGrid, 0, 0);
        }
    }
}
