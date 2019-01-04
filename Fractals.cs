using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForTask7
{
    public class Candle
    {
        public long TimeStamp { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public DateTime Time { get; set; }
    }

    public class Queue_Report
    {
        public Queue<Candle> candles { get; set; } = new Queue<Candle>();

        public Queue_Report()
        { }

        public Candle Pop()
        {
            return candles.Dequeue();
        }

        public Candle Peek()
        {
            return candles.Peek();
        }

        public void Push(Candle candle)
        {
            candles.Enqueue(candle);
        }
    }

    public interface IId_Calculation
    {
        IId_Serie Calculation(Queue_Report r);
    }

    public interface IId_Serie : IEnumerable<double?>
    {
        Queue<double?> Data { get; set; }
        double? Pop();
        void Push(double? value);
    }

    public class Id_Serie : IId_Serie
    {
        public Queue<double?> Data { get; set; } = new Queue<double>();

        public double? Pop()
        {
            return Data.Dequeue();
        }

        public void Push(double? value)
        {
            Data.Enqueue(value);
        }

        public IEnumerator<double?> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Data.GetEnumerator();
        }
    }

    public class Fractals : IId_Calculation
    {
        public IId_Serie Calculation(Queue_Report report)
        {
            IId_Serie return_serie = new Id_Serie();
            var candles_serie = report.candles.ToList();

            for (int i = 0; i < candles_serie.Count; i++)
            {
                if (i < candles_serie.Count - 5)
                {
                    List<double?> candels_high = new List<double?>(5);
                    List<double?> candels_low = new List<double?>(5);
                    for (int j = i; j < i + 5; j++)
                    {
                        candels_high.Add((double?)candles_serie[j].High);
                        candels_low.Add((double?)candles_serie[j].Low);
                    }
                    double max = candels_high.Max(), min = candels_low.Min();
                    return_serie.Push(0); return_serie.Push(0);
                    if (min == candels_low[2])
                        return_serie.Push(min);
                    else if (max == candels_high[2])
                        return_serie.Push(max);
                    else return_serie.Push(0);
                    return_serie.Push(0); return_serie.Push(0);
                }
                
            }

            return return_serie;
        }
    }
}
