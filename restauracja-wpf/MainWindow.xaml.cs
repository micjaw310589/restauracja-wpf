using System.Text;
using System.Threading.Tasks;
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
using System.Text.RegularExpressions;

namespace restauracja_wpf;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        FillUpComboBoxAsync();
    }
    private void PreviewNumericInput(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+,");
        e.Handled = regex.IsMatch(e.Text);
    }

    private async void FillUpComboBoxAsync()        // wip
    {
        GenericDataService<Role> roleService = new GenericDataService<Role>(new RestaurantContextFactory());
        cmbRole.ItemsSource = await roleService.GetAll();
        cmbRole.DisplayMemberPath = "Name";
        cmbRole.SelectedValuePath = "Id";
        cmbRole.SelectedIndex = 0;
         
        // docelowo zamienić na wyszukiwanie restauracji po nazwie
        GenericDataService<Restaurant> restaurantService = new GenericDataService<Restaurant>(new RestaurantContextFactory());
        cmbRestaurant.ItemsSource = await restaurantService.GetAll();
        cmbRestaurant.DisplayMemberPath = "Name";
        cmbRestaurant.SelectedValuePath = "Id";
        cmbRestaurant.SelectedIndex = 0;

        // DISHES
        cmbDishAvaibility.Items.Add("Available");
        cmbDishAvaibility.Items.Add("Not Available");
    }

    private void btnAddUser_Click(object sender, RoutedEventArgs e)
    {
        if (txtFirstname.Text.IsNullOrEmpty() || txtLastname.Text.IsNullOrEmpty() ||
            txtLogin.Text.IsNullOrEmpty() || txtPassword.Password.IsNullOrEmpty() ||
            txtConfirmPassword.Password.IsNullOrEmpty() ||
            cmbRole.SelectedValue.ToString().IsNullOrEmpty())
        {
            MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return; // throw new Exception("Please fill in all fields.");
        }
        else if (txtPassword.Password != txtConfirmPassword.Password)
        {
            MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return; // throw new Exception("Passwords do not match.");
        }
        else if (cmbRestaurant.SelectedValue == null)
        {
            MessageBox.Show("Please select a restaurant.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return; // throw new Exception("Please select a restaurant.");
        }
        else if (cmbRole.SelectedValue == null)
        {
            MessageBox.Show("Please select a role.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return; // throw new Exception("Please select a role.");
        }

        string firstname = txtFirstname.Text;
        string lastname = txtLastname.Text;
        string login = txtLogin.Text;
        string password = txtPassword.Password;
        string restaurant = cmbRestaurant.Text;
        string role = cmbRole.Text;

        var messageBoxResult = MessageBox.Show($"Confirm User creation:\n" +
            $"Firstname: {firstname}\n" +
            $"Lastname: {lastname}\n" +
            $"Login: {login}\n" +
            $"Password: {password}\n" +
            $"Role: {role}\n" +
            $"Restaurant: {restaurant}", "Confirm", MessageBoxButton.OKCancel, MessageBoxImage.Question);

        if (messageBoxResult == MessageBoxResult.OK)
        {
            UserManagement userManagement = new(new GenericDataService<User>(new RestaurantContextFactory()));
            userManagement.AddUser(
                firstname,
                lastname,
                login,
                password,
                restaurant,
                role
                );
        }
    }

    private async void btnSearchUser_ClickAsync(object sender, RoutedEventArgs e)
    {
        UserManagement userManagement = new(new GenericDataService<User>(new RestaurantContextFactory()));
        string lastname = txtSearchUser.Text;

        if (string.IsNullOrEmpty(lastname))
        {
            MessageBox.Show("Please enter a lastname to search.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return; // throw new Exception("Please enter a lastname to search.");
        }

        var users = await userManagement.GetMatchingUsers(lastname);

        if (users == default || users == null)
        {
            MessageBox.Show("No users found with the given lastname.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
            return; // throw new Exception("No users found with the given lastname.");
        }

        lbxUserSearchResults.Items.Clear();
        lbxUserSearchResults.SelectedItem = null;
        foreach (var user in users)
        {
            lbxUserSearchResults.Items.Add($"{user.Id} {user.FirstName} {user.LastName} (login: {user.Login})");
        }
    }

    private async void OnSelected(object sender, SelectionChangedEventArgs e)
    {
        if (lbxUserSearchResults.SelectedItem != null)
        {
            string[] selectedUser = lbxUserSearchResults.SelectedItem.ToString().Split(" ");
            try
            {
                int id = Convert.ToInt32(selectedUser[0]);
                var genericDataService = new GenericDataService<User>(new RestaurantContextFactory());
                var user = await genericDataService.Get(id);
                if (user == null)
                {
                    MessageBox.Show("User not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; // throw new Exception("User not found.");
                }
                else
                {
                    txtFirstname.Text = user.FirstName;
                    txtLastname.Text = user.LastName;
                    txtLogin.Text = user.Login;
                    txtPassword.Password = user.PasswordHash; // Note: This should not be done in production, passwords should not be retrievable.
                    cmbRole.SelectedValue = user.RoleId;
                    cmbRestaurant.SelectedValue = user.RestaurantId;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Failed Id convertion.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // throw new Exception("Invalid user selection. Please select a valid user.");
            }
        }
    }

    private async void btnAddDish_Click(object sender, RoutedEventArgs e)
    {
        
        MessageBox.Show((Convert.ToDecimal(txtDishPrice.Text)).ToString());
    }

    private void rbtnDishTimeConst_Click(object sender, RoutedEventArgs e)
    {
        grpConstTime.IsEnabled = true;
    }

    private void rbtnDishTimeDynamic_Click(object sender, RoutedEventArgs e)
    {
        grpConstTime.IsEnabled = false;
    }
}