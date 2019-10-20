using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FancyTodoList.Helpers.Converters
{
    class StringToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var dateString = value.ToString();
            DateTime datetime = DateTime.Parse(dateString);

            if (datetime.Date == DateTime.Now.Date)
                return "Hoy";

            var numDays = DateTime.Now.Date - datetime.Date;
            if (numDays.Days > 0 && numDays.Days < 8)
            {
                if (numDays.Days == 1)
                    return "Ayer";
                else
                    return new CultureInfo("Es-Es").DateTimeFormat.GetDayName(datetime.DayOfWeek);
            }

            //datetime.ToLocalTime().ToString("g")
            return datetime.Date.ToString("dd/MM/yy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
