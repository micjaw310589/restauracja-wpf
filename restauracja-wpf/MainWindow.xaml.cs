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
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        IDataService<Role> roleService = new GenericDataService<Role>(new RestaurantContextFactory());
        //var result = await roleService.Create(new Role
        //{
        //    Name = "Kucharz"
        //});

        var result = await roleService.GetAll();
        MessageBox.Show(result.ElementAt(0).Name + " " + result.ElementAt(1).Name);
    }
}