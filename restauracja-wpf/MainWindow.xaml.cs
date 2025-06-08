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
using Microsoft.IdentityModel.Tokens;
using restauracja_wpf.Data;
using restauracja_wpf.Interfaces;
using restauracja_wpf.Models;
using restauracja_wpf.Services;

namespace restauracja_wpf;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        FillUpComboBoxAsync();
    }

    private async void FillUpComboBoxAsync()        // wip
    {
        GenericDataService<Role> roleService = new GenericDataService<Role>(new RestaurantContextFactory());
        cmbRole.ItemsSource = await roleService.GetAll();
        cmbRole.DisplayMemberPath = "Name";
        cmbRole.SelectedValuePath = "Id";
        cmbRole.SelectedIndex = 0;

        GenericDataService<Restaurant> restaurantService = new GenericDataService<Restaurant>(new RestaurantContextFactory());
        cmbRestaurant.ItemsSource = await restaurantService.GetAll();
        cmbRestaurant.DisplayMemberPath = "Name";
        cmbRestaurant.SelectedValuePath = "Id";
        cmbRestaurant.SelectedIndex = 0;
    }

    private void btnAddUser_Click(object sender, RoutedEventArgs e)
    {
        if (txtFirstname.Text.IsNullOrEmpty() || txtLastname.Text.IsNullOrEmpty() ||
            txtLogin.Text.IsNullOrEmpty() || txtPassword.Password.IsNullOrEmpty() ||
            txtConfirmPassword.Password.IsNullOrEmpty() ||
            cmbRole.SelectedValue.ToString().IsNullOrEmpty())
            return;     // throw
        else if (txtPassword.Password != txtConfirmPassword.Password)
            return;     // throw

        UserManagement userManagement = new();
        userManagement.AddUser(
            txtFirstname.Text,
            txtLastname.Text,
            txtLogin.Text,
            txtPassword.Password,
            cmbRestaurant.Text,
            cmbRole.Text
            );
    }
}