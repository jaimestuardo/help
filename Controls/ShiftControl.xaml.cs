using Microsoft.Maui;
using TimeApp.Helpers;
using static System.Net.Mime.MediaTypeNames;

namespace TimeApp.Controls
{
    public partial class ShiftControl : ContentView
    {
        public static readonly BindableProperty StartProperty = BindableProperty.Create(nameof(StartTime), typeof(DateTime), typeof(ShiftControl), DateTime.Today, propertyChanged: (bindable, oldValue, newValue) =>
        {
            UpdateShift((ShiftControl)bindable);
        });

        public static readonly BindableProperty EndProperty = BindableProperty.Create(nameof(EndTime), typeof(DateTime), typeof(ShiftControl), DateTime.Today.LastTimeInDay(), propertyChanged: (bindable, oldValue, newValue) =>
        {
            UpdateShift((ShiftControl)bindable);
        });

        public static readonly BindableProperty CurrentProperty = BindableProperty.Create(nameof(CurrentTime), typeof(DateTime), typeof(ShiftControl), DateTime.Now, propertyChanged: (bindable, oldValue, newValue) =>
        {
            UpdateShift((ShiftControl)bindable);
        });

        public ShiftControl()
        {
            InitializeComponent();

            var timer = new System.Timers.Timer(60000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CurrentTime = DateTime.Now;
        }

        public DateTime? StartTime { 
            get => GetValue(StartProperty) as DateTime?;
            set => SetValue(StartProperty, value); 
        }

        public DateTime? EndTime
        {
            get => GetValue(EndProperty) as DateTime?;
            set => SetValue(EndProperty, value);
        }

        public DateTime? CurrentTime
        {
            get => GetValue(CurrentProperty) as DateTime?;
            set => SetValue(CurrentProperty, value);
        }

        private static void UpdateShift(ShiftControl control)
        {
            ((ElapsedTimeDrawable)control.ProgressView.Drawable).StartTime = control.StartTime ?? DateTime.Today;
            ((ElapsedTimeDrawable)control.ProgressView.Drawable).EndTime = control.EndTime ?? DateTime.Today.LastTimeInDay();
            ((ElapsedTimeDrawable)control.ProgressView.Drawable).CurrentTime = control.CurrentTime ?? DateTime.Now;
            control.ProgressView.Invalidate();
        }
    }

    public class ElapsedTimeDrawable : IDrawable
    {
        public DateTime StartTime { private get; set; }
        public DateTime EndTime { private get; set; }
        public DateTime CurrentTime { private get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Color.FromRgba("6599ff");
            canvas.StrokeSize = 5;
            canvas.DrawArc(15, 15, dirtyRect.Width - 30, dirtyRect.Height - 30, 0, 360, false, false);

            canvas.StrokeColor = Colors.Silver;
            canvas.StrokeSize = 1;
            canvas.DrawLine(dirtyRect.Width / 2, 10, dirtyRect.Width / 2, dirtyRect.Height - 10);
            canvas.DrawLine(10, dirtyRect.Height / 2, dirtyRect.Width - 10, dirtyRect.Height / 2);
            
            canvas.FontSize = 12;
            canvas.DrawString("0 h", dirtyRect.Width / 2, 5, HorizontalAlignment.Center);
            canvas.DrawString("6 h", dirtyRect.Width - 2, dirtyRect.Height / 2, HorizontalAlignment.Center);
            canvas.DrawString("12 h", dirtyRect.Width / 2, dirtyRect.Height, HorizontalAlignment.Center);
            canvas.DrawString("18 h", 0, dirtyRect.Height / 2, HorizontalAlignment.Center);

            canvas.StrokeColor = Colors.Green;
            canvas.StrokeSize = 2;
            canvas.DrawPath(GetPathForTime(StartTime, dirtyRect));

            canvas.StrokeColor = Colors.Red;
            canvas.DrawPath(GetPathForTime(EndTime, dirtyRect));

            canvas.FillColor = Color.FromRgba(0x60, 0xFF, 0x60, 112);

            //canvas.FillArc(20, 20, dirtyRect.Width - 40, dirtyRect.Height - 40, 0, 90, false);
            canvas.FillPath(GetPathForShift(dirtyRect), WindingMode.EvenOdd);

            //canvas.FillArc(17, 17, dirtyRect.Width - 34, dirtyRect.Height - 34, 90, 315, false);
        }

        private static PointF GetPoint(double hours, RectF dirtyRect)
        {
            double radio = (dirtyRect.Width - 30) / 2;

            // Tira la línea a la hora de inicio
            double theta;
            double y;
            double x;
            if (hours >= 0 && hours <= 6)   // cuadrante superior derecha
            {
                theta = hours * 360 / 24 - 90;
                y = dirtyRect.Height - radio * Math.Sin(theta * Math.PI / 180); // El positivo del Y va hacia abajo
                x = dirtyRect.Width / 2 + Math.Sqrt(Math.Pow(radio, 2) - Math.Pow(y - dirtyRect.Height / 2, 2));
            }
            else if (hours > 6 && hours <= 12)  // cuadrante inferior derecha
            {
                theta = hours * 360 / 24 - 90;
                y = dirtyRect.Height / 2 + radio * Math.Sin(theta * Math.PI / 180); // El positivo del Y va hacia abajo
                x = dirtyRect.Width / 2 + Math.Sqrt(Math.Pow(radio, 2) - Math.Pow(y - dirtyRect.Height / 2, 2));
            }
            else if (hours > 12 && hours <= 18)  // cuadrante inferior izquierda
            {
                theta = hours * 360 / 24 - 90;
                y = dirtyRect.Height / 2 + radio * Math.Sin(theta * Math.PI / 180); // El positivo del Y va hacia abajo
                x = dirtyRect.Width / 2 - Math.Sqrt(Math.Pow(radio, 2) - Math.Pow(y - dirtyRect.Height / 2, 2));
            }
            else // cuadrante superior izquierda
            {
                theta = hours * 360 / 24 - 90;
                y = dirtyRect.Height / 2 - radio * Math.Sin(theta * Math.PI / 180); // El positivo del Y va hacia abajo
                x = dirtyRect.Width / 2 - Math.Sqrt(Math.Pow(radio, 2) - Math.Pow(y - dirtyRect.Height / 2, 2));
            }

            return new PointF((float)x, (float)y);
        }

        private static double GetAngle(double hours)
        {
            double theta;
            if (hours >= 0 && hours <= 6)   // cuadrante superior derecha
            {
                theta = -1 * (hours - 6) * 360 / 24;
            }
            else if (hours > 6 && hours <= 12)  // cuadrante inferior derecha
            {
                theta = -1 * (hours - 6) * 360 / 24;
            }
            else if (hours > 12 && hours <= 18)  // cuadrante inferior izquierda
            {
                theta = -1 * (hours - 6) * 360 / 24;
            }
            else // cuadrante superior izquierda
            {
                theta = -1 * (hours - 6) * 360 / 24;
            }

            return theta;
        }

        private static PathF GetPathForTime(DateTime time, RectF dirtyRect)
        {
            PathF path = new();
            path.MoveTo(dirtyRect.Center.X, dirtyRect.Center.Y);

            path.LineTo(GetPoint(time.TimeOfDay.TotalHours, dirtyRect));

            return path;
        }

        private PathF GetPathForShift(RectF dirtyRect)
        {
            PathF path = new();

            double ahora = CurrentTime.TimeOfDay.TotalHours;
            PointF firstPoint = GetPoint(StartTime.TimeOfDay.TotalHours, dirtyRect);

            path.MoveTo(dirtyRect.Center.X, dirtyRect.Center.Y);
            path.LineTo(firstPoint.X, firstPoint.Y);

            float startAngle = (float)GetAngle(StartTime.TimeOfDay.TotalHours);
            float endAngle = (float)GetAngle(ahora);
            path.AddArc(18, 18, dirtyRect.Width - 18, dirtyRect.Height - 18, startAngle, endAngle, true);
            path.LineTo(dirtyRect.Center.X, dirtyRect.Center.Y);

            return path;
        }
    }
}
