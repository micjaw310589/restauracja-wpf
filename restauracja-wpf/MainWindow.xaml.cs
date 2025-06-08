using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
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

    public async void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (tabUserManagement.IsSelected)
        {
            IDataService<Role> roleService = new GenericDataService<Role>(new RestaurantContextFactory());
            var result = roleService.GetAll().Result;
            cmbRole.ItemsSource = result;
            cmbRole.DisplayMemberPath = result.ElementAt(0).Name;
            cmbRole.SelectedValuePath = Convert.ToString(result.ElementAt(0).Id);
            cmbRole.SelectedIndex = 0;
        }
    }
}