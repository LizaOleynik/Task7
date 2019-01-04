using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LibraryForTask7
{
    public class For_JSON : IData
    {
        private string string_doc;

        public For_JSON(string str)
        {
            string_doc = str;
        }

        public Queue_Report GetReport()
        {
            Queue_Report r = new Queue_Report();

            dynamic json = JsonConvert.DeserializeObject(File.ReadAllText(string_doc));

            var high = ((JArray)json.h);
            var low = ((JArray)json.l);

            var open = ((JArray)json.o);
            var close = ((JArray)json.c);
            var timestamps = ((JArray)json.t);

            for (int i = 0; i < timestamps.Count; i++)
            {
                Candle candle = new Candle()
                {
                    High = (decimal)high[i],
                    Low = (decimal)low[i],
                    Open = (decimal)open[i],
                    Close = (decimal)close[i],
                    Time = DateTimeOffset.FromUnixTimeSeconds((long)timestamps[i]).UtcDateTime,
                    TimeStamp = (long)timestamps[i],
                };

                r.Push(candle);

            }

            return r;
        }
    }
}
