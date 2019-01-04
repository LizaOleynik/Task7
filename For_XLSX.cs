using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;

namespace LibraryForTask7
{
    public class For_XLSX : IData
    {
        private string string_doc;

        public For_XLSX(string str)
        {
            string_doc = str;
        }

        public Queue_Report GetReport()
        {
            Queue_Report r = new Queue_Report();

            using (ExcelPackage pack_xml = new ExcelPackage(new FileInfo(string_doc)))
            {
                var text = pack_xml.Workbook.Worksheets.First();
                var columns = text.Dimension.End.Column;
                var rows = text.Dimension.End.Row;

                for (int i = 2; i <= rows; i++)
                {
                    var row = text.Cells[i, 1, i, columns].Select(c => c.Value == null ? string.Empty : c.Value.ToString()).ToArray();

                    string data_t_row = row[2] + " " + row[3];

                    DateTime data_time = DateTime.ParseExact(data_t_row, "yyyyMMdd HHmmss", null);

                    Candle candle = new Candle()
                    {
                        High = int.Parse(row[5]),
                        Low = int.Parse(row[6]),
                        Open = int.Parse(row[4]),
                        Close = int.Parse(row[7]),
                        Time = data_time,
                        TimeStamp = ((DateTimeOffset)data_time).ToUnixTimeMilliseconds(),
                    };

                    r.Push(candle);
                }
            }

            return r;
        }
    }
}
