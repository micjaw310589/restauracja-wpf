using restauracja_wpf.Data;
using restauracja_wpf.Interfaces;
using restauracja_wpf.Models;
using restauracja_wpf.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace restauracja_wpf
{
    /// <summary>
    /// Interaction logic for ModifyUserWindow.xaml
    /// </summary>
    public partial class ModifyUserWindow : Window
    {
        private readonly UserDataService userService = new(new GenericDataService<User>(new RestaurantContextFactory()));
        private readonly User user;

        public ModifyUserWindow(User user)
        {
            InitializeComponent();

            this.user = user;

            txtFirstname.Text = user.FirstName;
            txtLastname.Text = user.LastName;
            txtLogin.Text = user.Login;
            FillUpComboboxAsync();
            cmbRole.SelectedValue = user.RoleId;
            //cmbRestaurant.SelectedValue = user.RestaurantId;
            ckbEnabled.IsChecked = user.Status;
        }

        private async void FillUpComboboxAsync()
        {
            RoleDataService roleService = new(new GenericDataService<Role>(new RestaurantContextFactory()));
            if (SessionManager.CurrentUser.Role.Name == "root")
                cmbRole.ItemsSource = await roleService.GetAllRoles(true);
            else
                cmbRole.ItemsSource = await roleService.GetAllRoles();

            cmbRole.DisplayMemberPath = "Name";
            cmbRole.SelectedValuePath = "Id";
            cmbRole.SelectedIndex = 0;

            //GenericDataService<Restaurant> restaurantService = new GenericDataService<Restaurant>(new RestaurantContextFactory());
            //cmbRestaurant.ItemsSource = await restaurantService.GetAll();
            //cmbRestaurant.DisplayMemberPath = "Name";
            //cmbRestaurant.SelectedValuePath = "Id";
            //cmbRestaurant.SelectedIndex = 0;
        }

        private async void btnConfirmChanges_Click(object sender, RoutedEventArgs e)
        {
            if (user.Role.Name == "root" || user.Role.Name == "admin")
            {
                MessageBox.Show("Only root can manage this account.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Equals(MessageBox.Show("Are you sure you want to update user details?", "Confirm Update", MessageBoxButton.YesNo, MessageBoxImage.Question), MessageBoxResult.No))
            {
                return; // User cancelled the update
            }
            else if (string.IsNullOrWhiteSpace(txtFirstname.Text) || string.IsNullOrWhiteSpace(txtLastname.Text) || string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // throw new Exception("Please fill in all fields.");
            }
            else if (txtChangePassword.Password != txtConfirmChangePassword.Password)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // throw new Exception("Passwords do not match.");
            }
            //else if (cmbRestaurant.SelectedValue == null)
            //{
            //    MessageBox.Show("Please select a restaurant.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return; // throw new Exception("Please select a restaurant.");
            //}
            else if (cmbRole.SelectedValue == null)
            {
                MessageBox.Show("Please select a role.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // throw new Exception("Please select a role.");
            }

            var usernameTaken = await userService.IsUsernameTaken(txtLogin.Text);

            if (usernameTaken)
            {
                MessageBox.Show("This username is already taken. Please choose a different one.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // throw new Exception("This login is already taken.");
            }

            try
            {
                await userService.Update(user.Id, new User()
                {
                    FirstName = txtFirstname.Text,
                    LastName = txtLastname.Text,
                    Login = txtLogin.Text,
                    PasswordHash = txtChangePassword.Password,
                    RoleId = Convert.ToInt32(cmbRole.SelectedValue),
                    RestaurantId = null!,
                    Status = ckbEnabled.IsChecked == true ? true : false
                });
                MessageBox.Show("User details updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating user details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            if (user.Role.Name == "root" || user.Role.Name == "Admin")
            {
                MessageBox.Show("Only root can manage this account.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (SessionManager.CurrentUser.Role.Name == "root")
            {
                MessageBox.Show($"You can't delete the root account.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Equals(MessageBox.Show($"DELETE user account?", "Confirm", MessageBoxButton.OKCancel, MessageBoxImage.Question), MessageBoxResult.OK))
            {
                await userService.Delete(user.Id);
                MessageBox.Show("Account deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }
    }
}
