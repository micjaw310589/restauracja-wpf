using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using restauracja_wpf.Data;
using restauracja_wpf.Models;
using restauracja_wpf.Services;

namespace restauracja_wpf;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        IDataService<User> userService = new GenericDataService<User>(new RestaurantContextFactory());
        userService.Create(new User
        {
            FirstName = "Jan",
            LastName = "Kowalski"
        }).Wait();
    }

}