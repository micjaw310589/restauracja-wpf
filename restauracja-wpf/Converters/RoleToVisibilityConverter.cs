using restauracja_wpf.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace restauracja_wpf.Converters
{
    public class RoleToVisibilityConverter : IValueConverter
    {
        public string RequiredRole { get; set; } = "";

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var user = SessionManager.CurrentUser;
            return user != null && user.Role.Name == RequiredRole ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
