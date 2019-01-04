using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryForTask7
{
    public interface IData
    {
        Queue_Report GetReport();
    }

    public class Monitoring : IDisposable
    {
        public delegate void Event_Candle(Candle c, double? id);
        public event Event_Candle get_candle;
        Thread thread;
        IData source_data;
        Queue_Report stream_data;
        IId_Calculation c;
        IId_Serie s;

        public Monitoring(IData s, IId_Calculation c)
        {
            source_data = s;
            this.c = c;
        }

        public void RunMonitor()
        {
            stream_data = source_data.GetReport();
            s = c.Calculation(stream_data);

            thread = new Thread(new ThreadStart(UpdateMonitor));
            thread.Start();
        }

        protected void UpdateMonitor()
        {
            while (stream_data.candles.Count != 0)
            {
                get_candle?.Invoke(s.Pop(), stream_data.Pop());

                Thread.Sleep(1000);
            }
        }

        public void Dispose()
        {
            thread.Abort();
        }
    }
}
