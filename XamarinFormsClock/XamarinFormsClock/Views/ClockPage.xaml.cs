using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace XamarinFormsClock.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClockPage : ContentPage
    {
        private const int HourNeedleWidth = 8;
        private const int MinutesNeedleWidth = 4;
        private const int SecondsNeedleWidth = 2;
        private const int HourTickSize = 8;

        Xamarin.Forms.Shapes.Rectangle hourNeedle = new Xamarin.Forms.Shapes.Rectangle();
        Xamarin.Forms.Shapes.Rectangle minuteNeedle = new Xamarin.Forms.Shapes.Rectangle();
        Xamarin.Forms.Shapes.Rectangle secondsNeedle = new Xamarin.Forms.Shapes.Rectangle();

        public ClockPage()
        {
            InitializeComponent();
            Device.StartTimer(TimeSpan.FromSeconds(1), TimerCallback);
        }

        void OnAbsoluteLayoutSizeChanged(object sender, EventArgs args)
        {
            //define the outside circle 
            var circleDiameter = Math.Min(this.absoluteLayout.Width - 10, this.absoluteLayout.Height - 10);
            this.circle.WidthRequest = circleDiameter;
            this.circle.HeightRequest = circleDiameter;
            var circleStartPoint = new Point((this.absoluteLayout.Width - this.circle.WidthRequest) / 2, (this.absoluteLayout.Height - this.circle.HeightRequest) / 2);
            AbsoluteLayout.SetLayoutBounds(this.circle, new Xamarin.Forms.Rectangle(circleStartPoint, new Size(this.circle.WidthRequest, this.circle.HeightRequest)));
            // Get the center and radius of the circle.
            Point center = new Point(circleStartPoint.X + (this.circle.WidthRequest / 2), circleStartPoint.Y + (this.circle.HeightRequest / 2));
            double radius = 0.40 * Math.Min(this.circle.WidthRequest, this.circle.HeightRequest);

            this.DrawHoursTicks(center, radius);
            this.DrawNeedles(center, radius);
        }

        /// <summary>
        /// Draw the hour ticks (the 12 dots)
        /// </summary>
        /// <param name="center">the center of the clock</param>
        /// <param name="radius">the distance from the center of the clok of the hours ticks, and the large of seconds needle </param>
        private void DrawHoursTicks(Point center, double radius)
        {
            // Position, size, and rotate the 12 tick marks.
            for (int index = 0; index < 12; index++)
            {
                var radians = index * 2 * Math.PI / 12;
                var x = center.X + radius * Math.Sin(radians) - HourTickSize / 2;
                var y = center.Y - radius * Math.Cos(radians) - HourTickSize / 2;
                var hourTick = new Ellipse { Fill = Brush.Cyan, WidthRequest = HourTickSize, HeightRequest = HourTickSize, AnchorY = radius / HourTickSize };
                this.absoluteLayout.Children.Add(hourTick);
                AbsoluteLayout.SetLayoutBounds(hourTick, new Xamarin.Forms.Rectangle(x, y, HourTickSize, HourTickSize));
            }
        }

        /// <summary>
        /// Draw the 3 needles
        /// </summary>
        /// <param name="center">the center of the clock</param>
        /// <param name="radius">the distance from the center of the clok of the hours ticks, and the large of seconds needle </param>
        private void DrawNeedles(Point center, double radius)
        {
            this.hourNeedle = new Xamarin.Forms.Shapes.Rectangle() { Fill = Brush.Blue, AnchorY = 1, WidthRequest = HourNeedleWidth, HeightRequest = 0.8 * radius };
            this.minuteNeedle = new Xamarin.Forms.Shapes.Rectangle() { Fill = Brush.Red, AnchorY = 1, WidthRequest = MinutesNeedleWidth, HeightRequest = 0.9 * radius };
            this.secondsNeedle = new Xamarin.Forms.Shapes.Rectangle() { Fill = Brush.Gray, AnchorY = 1, WidthRequest = SecondsNeedleWidth, HeightRequest = radius };

            this.DrawSingleNeedle(this.secondsNeedle, center);
            this.DrawSingleNeedle(this.minuteNeedle, center);
            this.DrawSingleNeedle(this.hourNeedle, center);
        }

        /// <summary>
        /// Draw of individual needle
        /// </summary>
        /// <param name="needle">rectangle of the needle</param>
        /// <param name="center">the center of the clock</param>
        private void DrawSingleNeedle(Xamarin.Forms.Shapes.Rectangle needle, Point center)
        {
            this.absoluteLayout.Children.Add(needle);
            AbsoluteLayout.SetLayoutBounds(needle, new Xamarin.Forms.Rectangle(center.X - needle.WidthRequest / 2, center.Y - needle.HeightRequest, needle.WidthRequest, needle.HeightRequest));
        }

        /// <summary>
        /// Timer tick
        /// </summary>
        /// <returns></returns>
        private bool TimerCallback()
        {
            // Set clock
            var clockTextStartPoint = new Point((this.absoluteLayout.Width - this.clockText.Width) / 2, (this.absoluteLayout.Height / 2) - this.clockText.Height);
            AbsoluteLayout.SetLayoutBounds(this.clockText, new Xamarin.Forms.Rectangle(clockTextStartPoint, new Size(this.clockText.WidthRequest, this.clockText.HeightRequest)));
            this.clockText.Text = DateTime.Now.ToString((DateTime.Now.Second % 2) == 0 ? "HH:mm:ss" : "HH mm ss");
            this.clockText.IsVisible = true;
            // Set rotation angles for hour and minute hands.
            this.hourNeedle.Rotation = 30 * (DateTime.Now.Hour % 12);
            this.minuteNeedle.Rotation = 6 * DateTime.Now.Minute;
            this.secondsNeedle.Rotation = 6 * DateTime.Now.Second;

            return true;
        }
    }
}