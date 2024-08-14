using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeApp.Models
{
    public class AttendanceItem : INotifyPropertyChanged
    {
        public int Orden { get; set; }
        public ShiftItem TurnoActual { get; set; }
        public DateTime? HoraEntrada { get; set; }
        public DateTime? HoraSalida { get; set; }
        public TimeSpan HorasTrabajadas { 
            get { 
                if (HoraEntrada is null && HoraSalida is null)
                    return TimeSpan.Zero;

                DateTime llegada = HoraEntrada ?? DateTime.Today.Add(TurnoActual.Inicio.TimeOfDay);
                DateTime salida = HoraSalida ?? DateTime.Now;

                return salida - llegada;
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public AttendanceItem()
        {
            var timer = new System.Timers.Timer(60000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HorasTrabajadas)));
        }
    }
}
