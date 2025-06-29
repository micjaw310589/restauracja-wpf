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
using restauracja_wpf.Models;
using restauracja_wpf.Services;
using System.Text.RegularExpressions;
using restauracja_wpf.Interfaces;
using System.Diagnostics.Contracts;

namespace restauracja_wpf;

public partial class MainWindow : Window
{
    private User loggedUser;

    public MainWindow()
    {
        InitializeComponent();

        FillUpComboBoxAsync();
        GetActiveOrdersAsync();
    }

    private void PreviewNumericInput(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+,");
        e.Handled = regex.IsMatch(e.Text);
    }

    private decimal stringToPrice(string str)
    {
        str = str.Trim();
        if (str.Length == 0) { return 0; }
        else if (str.Contains(' '))
        {
            string str_new = "";
            foreach (string c in str.Split(' ')) { str_new += c; }
            ;
            return Convert.ToDecimal(str_new);
        }
        else
        {
            return Convert.ToDecimal(str);
        }
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
        cmbChangeDishAvaibility.Items.Add("Available");
        cmbChangeDishAvaibility.Items.Add("Not Available");
    }

    private void btnAddUser_Click(object sender, RoutedEventArgs e)
    {
        if (txtFirstname.Text.IsNullOrEmpty() || txtLastname.Text.IsNullOrEmpty() ||
            txtUsername.Text.IsNullOrEmpty() || txtPassword.Password.IsNullOrEmpty() ||
            txtConfirmPassword.Password.IsNullOrEmpty() ||
            cmbRole.Text.IsNullOrEmpty())
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
        string login = txtUsername.Text;
        string password = txtPassword.Password;
        string restaurant = cmbRestaurant.Text;
        string role = cmbRole.Text;
        bool status = ckbEnabled.IsChecked == true ? true : false;

        var messageBoxResult = MessageBox.Show($"Confirm User creation:\n" +
            $"Firstname: {firstname}\n" +
            $"Lastname: {lastname}\n" +
            $"Login: {login}\n" +
            $"Password: {password}\n" +
            $"Role: {role}\n" +
            $"Restaurant: {restaurant}\n" +
            $"Status (enabled?): {status}", "Confirm User Creation", MessageBoxButton.OKCancel, MessageBoxImage.Question);

        if (messageBoxResult == MessageBoxResult.OK)
        {
            UserDataService userService = new(new GenericDataService<User>(new RestaurantContextFactory()));
            userService.CreateUser(
                firstname,
                lastname,
                login,
                password,
                restaurant,
                role,
                status
                );

            MessageBox.Show("User added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private async void btnSearchUser_ClickAsync(object sender, RoutedEventArgs e)
    {
        UserDataService userService = new(new GenericDataService<User>(new RestaurantContextFactory()));
        string lastname = txtSearchUser.Text;

        if (string.IsNullOrEmpty(lastname))
        {
            MessageBox.Show("Please enter a lastname to search.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return; // throw new Exception("Please enter a lastname to search.");
        }

        var users = await userService.GetUserByLastname(lastname);

        if (users == default || users == null)
        {
            MessageBox.Show("No users found with the given lastname.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
            return; // throw new Exception("No users found with the given lastname.");
        }

        lbxUserSearchResults.Items.Clear();
        lbxUserSearchResults.SelectedItem = null;
        foreach (var user in users)
        {
            lbxUserSearchResults.Items.Add($"{user.Id} - {user.FirstName} {user.LastName} (login: {user.Login})");
        }
    }

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (lbxUserSearchResults.SelectedItem != null)
        {
            try
            {
                int user_id = Convert.ToInt32(lbxUserSearchResults.SelectedItem.ToString().Split(" ")[0]);
                UserDataService userService = new(new GenericDataService<User>(new RestaurantContextFactory()));
                var user = await userService.Get(user_id);
                ModifyUserWindow modifyUserWindow = new ModifyUserWindow(user);
                modifyUserWindow.ShowDialog();
            }
            catch (FormatException)
            {
                MessageBox.Show("Failed Id convertion.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    //--------------------------------------------DISH----------------------------------------------

    private async void btnAddDish_Click(object sender, RoutedEventArgs e)
    {
        if (txtDishName.Text.IsNullOrEmpty() || txtDishPrice.Text.IsNullOrEmpty() ||
            cmbDishAvaibility.Text.IsNullOrEmpty() || txtSeconds.Text.IsNullOrEmpty() ||
            txtMinutes.Text.IsNullOrEmpty())
        {
            MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return; // throw new Exception("Please fill in all fields.");
        }
        else if (cmbDishAvaibility.SelectedValue == null)
        {
            MessageBox.Show("Please select an avaibility state.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return; // throw new Exception("Please select a state.");
        }

        string name = txtDishName.Text;
        decimal price = stringToPrice(txtDishPrice.Text);
        bool avaibility = (cmbDishAvaibility.Text=="Available")? true : false;
        TimeSpan timeSpan = new TimeSpan(0, Convert.ToInt32(txtMinutes.Text), Convert.ToInt32(txtSeconds.Text));

        var messageBoxResult = MessageBox.Show($"Confirm Dish creation:\n" +
            $"Dish name: {name}\n" +
            $"Price: {price}\n" +
            $"Avaibility: {avaibility}\n" +
            $"Time to make (hh:mm:ss): {timeSpan}\n", "Confirm", MessageBoxButton.OKCancel, MessageBoxImage.Question);

        if (messageBoxResult == MessageBoxResult.OK)
        {
            DishDataService dishService = new(new GenericDataService<Dish>(new RestaurantContextFactory()));
            dishService.AddDish(
                name,
                price,
                avaibility,
                timeSpan
                );
        }
    }

    private void rbtnDishTimeConst_Click(object sender, RoutedEventArgs e)
    {
        grpConstTimeChangeDish.IsEnabled = true;
    }

    private void rbtnDishTimeDynamic_Click(object sender, RoutedEventArgs e)
    {
        grpConstTimeChangeDish.IsEnabled = false;
    }

    private async void btnSearchDish_ClickAsync(object sender, RoutedEventArgs e)
    {
        DishDataService dishService = new(new GenericDataService<Dish>(new RestaurantContextFactory()));
        string dishname = txtSearchDish.Text;

        if (string.IsNullOrEmpty(dishname))
        {
            MessageBox.Show("Please enter a dish name to search.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return; // throw new Exception("Please enter a dish name to search.");
        }

        var dishes = await dishService.GetMatchingDishes(dishname);

        if (dishes == default || dishes == null)
        {
            MessageBox.Show("No dishes found with the given name.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
            return; // throw new Exception("No dishes found with the given name.");
        }

        lbxDishSearchResults.Items.Clear();
        lbxDishSearchResults.SelectedItem = null;
        foreach (var dish in dishes)
        {
            lbxDishSearchResults.Items.Add($"{dish.Id} {dish.Name} {dish.Price}$ (avaibility: {dish.Available})");
        }
    }

    private async void btnChangeDish_Click(object sender, RoutedEventArgs e)
    {
        if (lbxDishSearchResults.SelectedItem != null)
        {
            string[] selectedUser = lbxDishSearchResults.SelectedItem.ToString().Split(" ");
            try
            {
                int id = Convert.ToInt32(selectedUser[0]);
                var genericDataService = new GenericDataService<Dish>(new RestaurantContextFactory());
                var dish = await genericDataService.Get(id);
                if (dish == null)
                {
                    MessageBox.Show("Dish not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; // throw new Exception("Dish not found.");
                }
                else
                {
                    if (MessageBox.Equals(MessageBox.Show("Are you sure you want to update dish data?", "Confirm Change", MessageBoxButton.YesNo, MessageBoxImage.Question), MessageBoxResult.No))
                    {
                        return; // User cancelled the update
                    }

                    dish.Name = txtChangeDishName.Text;
                    dish.Price = stringToPrice(txtChangeDishPrice.Text);
                    dish.Available = (cmbChangeDishAvaibility.SelectedItem == "Available") ? true : false;
                    dish.IsTimeCalculated = (rbtnDishTimeDynamic.IsChecked == true && dish.TimeCalculated != null);
                    dish.TimeConstant = new TimeSpan(0, Convert.ToInt32(txtChangeDishMinutes.Text), Convert.ToInt32(txtChangeDishSeconds.Text));
                    dish.DishOfTheDay = (chkChangeDishOfTheDay.IsChecked == true);
                    dish.Exclude = (chkChangeDishExclude.IsChecked == true);
                    genericDataService.Update(id, dish);
                }
            }

            catch (FormatException)
            {
                MessageBox.Show("Failed Id convertion.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // throw new Exception("Invalid user selection. Please select a valid user.");
            }
        }
    }

    private async void lbxDishSearchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (lbxDishSearchResults.SelectedItem == null)
        {
            return;
        }

        string[] selectedUser = lbxDishSearchResults.SelectedItem.ToString().Split(" ");
        try
        {
            int id = Convert.ToInt32(selectedUser[0]);
            var genericDataService = new GenericDataService<Dish>(new RestaurantContextFactory());
            var dish = await genericDataService.Get(id);
            if (dish == null)
            {
                MessageBox.Show("Dish not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // throw new Exception("Dish not found.");
            }
            else
            {
                txtChangeDishName.Text = dish.Name;
                txtChangeDishPrice.Text = dish.Price.ToString();
                if (dish.Available) cmbChangeDishAvaibility.SelectedIndex = 0;  else cmbChangeDishAvaibility.SelectedIndex = 1;
                try
                {
                    rbtnDishTimeDynamic.IsChecked = dish.IsTimeCalculated;
                    rbtnDishTimeConst.IsChecked = !(dish.IsTimeCalculated);
                }
                finally { }
                TimeSpan tempTime = TimeSpan.Zero;
                if(dish.TimeConstant != null)
                {
                    tempTime += (TimeSpan)dish.TimeConstant;
                }
                txtChangeDishMinutes.Text = tempTime.Minutes.ToString();
                txtChangeDishSeconds.Text = tempTime.Seconds.ToString();
                chkChangeDishOfTheDay.IsChecked = dish.DishOfTheDay;
                chkChangeDishExclude.IsChecked = dish.Exclude;
            }
        }
        catch
        {
            MessageBox.Show("Failed Id convertion.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return; // throw new Exception("Invalid user selection. Please select a valid user.");
        }

    }

    //private async void btnSetNewOrdersInDB_Click(object sender, RoutedEventArgs e)
    //{
    //    string[] Statuses = ["Placed", "Rejected", "In progress", "Done", "Served", "Cancelled"];
    //    StatusManagement statusManagement = new(new GenericDataService<OrderStatus>(new RestaurantContextFactory()));
    //    statusManagement.ClearAll();
    //    foreach (string stat in Statuses)
    //    {
    //        statusManagement.AddStatus(stat);
    //    }

    //    //--------ADD ROLES-----------

    //    string[] Roles = ["Admin", "Manager", "Waiter", "Cook"];
    //    RoleManagement roleManagement = new(new GenericDataService<Role>(new RestaurantContextFactory()));
    //    roleManagement.ClearRoles();
    //    foreach (string role in Roles) {
    //        roleManagement.AddRole(role);
    //    }

    //    MessageBox.Show("Done!");
    //}

    private async void btnManageOrder_Click(object sender, RoutedEventArgs e, IEnumerable<Order> orders2, IEnumerable<Order> orders)
    {
        OrderManagement orderManagement = new(new GenericDataService<Order>(new RestaurantContextFactory()));
        var _orders = await orderManagement.GetActiveOrders();
        if (_orders != null)
        {
            foreach (var order in _orders)
            {
                lbxPendingOrders.Items.Add(order);
            }
        }
    }

    private void btnAddRestaurant_Click(object sender, RoutedEventArgs e)
    {

        string address = txtRestaurantAddress.Text;
        string restaurantName = txtRestaurantName.Text;
        string city = txtRestaurantCity.Text;
        bool isOpen = chxRestaurantIsOpen.IsEnabled;
        if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(restaurantName) || string.IsNullOrEmpty(city))
        {
            MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return; // throw new Exception("Please fill in all fields.");
        }

        var messageBoxResult = MessageBox.Show($"Confirm Restaurant creation:\n" +
            $"Name: {restaurantName}\n" +
            $"Address: {address}\n" +
            $"City: {city}\n" +
            $"Is Open: {isOpen}", "Confirm", MessageBoxButton.OKCancel, MessageBoxImage.Question);

        if (messageBoxResult == MessageBoxResult.OK)
        {
            RestaurantDataService restaurantService = new(new GenericDataService<Restaurant>(new RestaurantContextFactory()));
            restaurantService.AddRestaurant(
                restaurantName,
                address,
                city,
                isOpen
                );
        }

    }

    private async void btnSearchRestaurant_Click(object sender, RoutedEventArgs e)
    {
        RestaurantDataService restaurantService = new(new GenericDataService<Restaurant>(new RestaurantContextFactory()));

        string restaurantName = txtSearchRestaurant.Text;
        if (string.IsNullOrEmpty(restaurantName))
        {
            MessageBox.Show("Please enter a restaurant name to search.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return; // throw new Exception("Please enter a restaurant name to search.");
        }

        var restaurants = await restaurantService.GetMatchingRestaurants(restaurantName);

        if (restaurants == default || restaurants == null)
        {
            MessageBox.Show("No restaurants found with the given name.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
            return; // throw new Exception("No restaurants found with the given name.");
        }

        lbxRestaurantSearchResults.Items.Clear();
        lbxRestaurantSearchResults.SelectedItem = null;
        foreach (var restaurant in restaurants)
        {
            lbxRestaurantSearchResults.Items.Add($"{restaurant.Id} {restaurant.Name} ({restaurant.City})");
        }

        if (lbxRestaurantSearchResults.Items.Count == 0)
        {
            MessageBox.Show("No restaurants found with the given name.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            MessageBox.Show($"{lbxRestaurantSearchResults.Items.Count} restaurants found.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        if (lbxRestaurantSearchResults.Items.Count > 0)
        {
            lbxRestaurantSearchResults.SelectedIndex = 0; // Select the first item by default
        }

    }

    private async void btnRestaurantChange_Click(object sender, RoutedEventArgs e)
    {
        RestaurantDataService restaurantService = new(new GenericDataService<Restaurant>(new RestaurantContextFactory()));

        if (lbxRestaurantSearchResults.SelectedItem == null)
        {
            MessageBox.Show("Please select a restaurant to change.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return; // throw new Exception("Please select a restaurant to change.");
        }

        string[] selectedRestaurant = lbxRestaurantSearchResults.SelectedItem.ToString().Split(" ");

        try
        {
            int id = Convert.ToInt32(selectedRestaurant[0]);
            var genericDataService = new GenericDataService<Restaurant>(new RestaurantContextFactory());
            var restaurant = await genericDataService.Get(id);


            if (restaurant == null)
            {
                MessageBox.Show("Restaurant not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // throw new Exception("Restaurant not found.");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtChangeRestaurantName.Text) ||
                    string.IsNullOrWhiteSpace(txtChangeRestaurantAddress.Text) ||
                    string.IsNullOrWhiteSpace(txtChangeRestaurantCity.Text))
                 {
                        MessageBox.Show("All fields must be filled!", "Validation Error",
                                        MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                }
                else
                {

                        restaurant.Name = txtChangeRestaurantName.Text;
                    restaurant.Address = txtChangeRestaurantAddress.Text;
                    restaurant.City = txtChangeRestaurantCity.Text;
                    restaurant.IsOpen = chxChangeIsOpen.IsChecked ?? false;
                    var messageBoxResult = MessageBox.Show($"Confirm Restaurant change:\n" +
                        $"Name: {restaurant.Name}\n" +
                        $"Address: {restaurant.Address}\n" +
                        $"City: {restaurant.City}\n" +
                        $"Is Open: {restaurant.IsOpen}", "Confirm", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    if (messageBoxResult == MessageBoxResult.OK)
                    {
                        await genericDataService.Update(restaurant.Id, restaurant);
                        MessageBox.Show("Restaurant updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
        catch (FormatException)
        {
            MessageBox.Show("Failed Id convertion.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return; // throw new Exception("Invalid restaurant selection. Please select a valid restaurant.");
        }

    }

    private async void btnDeleteRestaurant_Click(object sender, RoutedEventArgs e)
    {
        RestaurantDataService restaurantService = new(new GenericDataService<Restaurant>(new RestaurantContextFactory()));

        if (lbxRestaurantSearchResults.SelectedItem == null)
        {
            MessageBox.Show("Please select a restaurant to delete.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return; // throw new Exception("Please select a restaurant to delete.");
        }
        string[] selectedRestaurant = lbxRestaurantSearchResults.SelectedItem.ToString().Split(" ");


        int id = Convert.ToInt32(selectedRestaurant[0]);
        var genericDataService = new GenericDataService<Restaurant>(new RestaurantContextFactory());
        var restaurant = await genericDataService.Get(id);

        if (restaurant != null)
        {
            var messageBoxResult = MessageBox.Show($"Are you sure you want to delete the restaurant:\n" +
                $"Name: {restaurant.Name}\n" +
                $"Address: {restaurant.Address}\n" +
                $"City: {restaurant.City}\n" +
                $"Is Open: {restaurant.IsOpen}", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                await genericDataService.Delete(restaurant.Id);
                MessageBox.Show("Restaurant deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        else
        {
            MessageBox.Show("Restaurant not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        }

    }

    private async void btnLogIn_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            UserDataService userService = new(new GenericDataService<User>(new RestaurantContextFactory()));
            loggedUser = await userService.Login(txtLogin.Text, txtLoginPassword.Password);
            txtbLoginInfo.Text = $"Logged in as: {loggedUser.FirstName} {loggedUser.LastName} ({loggedUser.Role.Name})";
        }
        catch (Exception ex)
        {
            txtbLoginInfo.Text = ex.Message;
            return; // throw new Exception("Login failed. Please check your credentials.");
        }
    }

    private async void btnDeleteDish_Click(object sender, RoutedEventArgs e)
    {
        DishDataService dishService = new(new GenericDataService<Dish>(new RestaurantContextFactory()));
        if (MessageBox.Equals(MessageBox.Show($"Remove dish?", "Confirm", MessageBoxButton.OKCancel, MessageBoxImage.Question), MessageBoxResult.OK))
        {
            if (lbxDishSearchResults.SelectedItem != null)
            {
                string[] selectedDish = lbxDishSearchResults.SelectedItem.ToString().Split(" ");
                try
                {
                    await dishService.DeleteDish(Convert.ToInt32(selectedDish[0]));
                    MessageBox.Show("Dish deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Failed Id convertion.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; // throw new Exception("Invalid user selection. Please select a valid user.");
                }
            }
        }
    }

    private void recalculateOrderPrice(ref decimal totalPrice)
    {
        foreach (var item in lbxOrder.Items.Cast<string>().ToList())
        {
            totalPrice += Convert.ToDecimal(item.Split(" - ")[2].Split(' ')[0]) * Convert.ToInt32(item.Split(' ')[0].Trim('(', ')'));
        }

    }

    private void btnNewOrder_Click(object sender, RoutedEventArgs e)
    {
        GridLength currentColumnWidth = gridMainPage.ColumnDefinitions[1].Width;
        GridLength collapsed = new (0, GridUnitType.Star);
        GridLength visible = new (1, GridUnitType.Star);
        if (currentColumnWidth == collapsed)
        {
            btnRefreshMenu_Click(sender, e);
            gridMainPage.ColumnDefinitions[1].Width = visible;
        }
        else
            gridMainPage.ColumnDefinitions[1].Width = collapsed;

        //REFRESH ACTIVE ORDERS
        GetActiveOrdersAsync();
    }

    private async void GetActiveOrdersAsync()
    {
        OrderManagement orderManagement = new(new GenericDataService<Order>(new RestaurantContextFactory()));
        var pendingOrders = await orderManagement.GetActiveOrders();
        //pendingOrders = await filtrujListę(pendingOrders, o => o.Status.Name == "Placed" || o.Status.Name == "In progress" || o.Status.Name == "Accepted");
        //"Placed", "Rejected", "In progress", "Done", "Served", "Cancelled"
        lbxPendingOrders.Items.Clear();
        lbxPendingOrders.SelectedItem = null;
        foreach (var order in pendingOrders)
        {
            int dish_count = order.DishOrders.Sum(dishOrder => dishOrder.Quantity);
            decimal sum = order.DishOrders.Sum(dishOrder => dishOrder.PurchasePrice);
            lbxPendingOrders.Items.Add($"{order.Id} - Status: {order.Status.Name} - {order.OrderDate} - Dania: {order.DishOrders.Count} - Cena: {sum} zł");
        }
    }

    private async void lbxPendingOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if(lbxPendingOrders.SelectedItem != null)
        {
            GenericDataService<Order> orderService = new(new RestaurantContextFactory());
            Order selectedOrder = await orderService.Get(Convert.ToInt32(lbxPendingOrders.SelectedItem.ToString().Split(' ')[0]));
            OrderDetailsWindow orderDetailsWindow = new OrderDetailsWindow(selectedOrder);
            orderDetailsWindow.ShowDialog();
        }
    }

    private async void btnRefreshOrders_Click(object sender, RoutedEventArgs e)
    {
        GetActiveOrdersAsync();
    }

    private void btnAddToOrder_Click(object sender, RoutedEventArgs e)
    {
        var selectedItems = lbxMenu.SelectedItems.Cast<string>().ToList();
        decimal totalPrice = 0.0M;
        int counter = 1;
        bool found = false;

        foreach (var item in selectedItems)
        {
            if (lbxOrder.Items.IsEmpty)
            {
                lbxOrder.Items.Add("(" + counter + ") " + item);
                continue;
            }

            foreach (var existingItem in lbxOrder.Items.Cast<string>().ToList())
            {
                if (Convert.ToInt32(existingItem.Split(' ')[1]) == Convert.ToInt32(item.Split(' ')[0]))
                {
                    counter = Convert.ToInt32(existingItem.Split(' ')[0].Trim('(', ')'));
                    counter++;
                    lbxOrder.Items.Remove(existingItem);
                    lbxOrder.Items.Add("(" + counter + ") " + item);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                counter = 1;
                lbxOrder.Items.Add("(" + counter + ") " + item);
            }
        }

        recalculateOrderPrice(ref totalPrice);
        txtTotalPrice.Text = totalPrice.ToString("F2") + " zł";

    }

    private async void btnRefreshMenu_Click(object sender, RoutedEventArgs e)
    {
        GenericDataService<Dish> dishService = new(new RestaurantContextFactory());
        lbxMenu.Items.Clear();
        lbxMenu.SelectedItem = null;
        var all_dishes = await dishService.GetAll();
        foreach (var dish in all_dishes)
        {
            if (!dish.Exclude)
            {
                lbxMenu.Items.Add($"{dish.Id} - {dish.Name} - {dish.Price} zł - (ToS: {dish.TimeConstant})");
            }
        }
    }

    private void btnRemoveFromOrder_Click(object sender, RoutedEventArgs e)
    {
        lbxOrder.SelectedItems.Cast<string>().ToList().ForEach(item => lbxOrder.Items.Remove(item));

        decimal totalPrice = 0.0M;

        recalculateOrderPrice(ref totalPrice);

        txtTotalPrice.Text = totalPrice.ToString("F2") + " zł";
    }

    private async void btnPlaceOrder_Click(object sender, RoutedEventArgs e)
    {
        GenericDataService<Dish> dishService = new(new RestaurantContextFactory());
        GenericDataService<Order> orderService = new(new RestaurantContextFactory());
        List<DishOrder> dishOrders = new List<DishOrder>();

        foreach (var item in lbxOrder.Items.Cast<string>().ToList())
        {
            int dishId = Convert.ToInt32(item.Split(" ")[1]);
            Dish searchForDish = await dishService.Get(dishId);

            if (searchForDish != null)
            {
                dishOrders.Add(new DishOrder
                {
                    DishId = searchForDish.Id,
                    Quantity = Convert.ToInt32(item.Split(' ')[0].Trim('(', ')')),
                    PurchasePrice = searchForDish.Price * Convert.ToInt32(item.Split(' ')[0].Trim('(', ')'))
                });
            }
            else
            {
                MessageBox.Show($"Dish not found in DB. Aborting.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        Order newOrder = new Order
        {
            UserId = null,
            StatusId = 1,
            OrderDate = DateTime.Now,
            DishOrders = dishOrders
        };

        await orderService.Create(newOrder);

        lbxOrder.Items.Clear();
        txtTotalPrice.Text = "0.00 zł";
    }

    private void btnPlus1_Click(object sender, RoutedEventArgs e)
    {
        if (lbxOrder.SelectedItem == null)
        {
            return;
        }

        var selectedItem = lbxOrder.SelectedItem;
        int counter = Convert.ToInt32(selectedItem.ToString().Split(' ')[0].Trim('(', ')'));
        counter++;
        lbxOrder.Items.Remove(selectedItem);
        lbxOrder.Items.Add("(" + counter + ") " + selectedItem.ToString().Substring(selectedItem.ToString().IndexOf(' ') + 1));
      
        decimal totalPrice = 0.0M;
        recalculateOrderPrice(ref totalPrice);
        txtTotalPrice.Text = totalPrice.ToString("F2") + " zł";
    }

    private void btnMinus1_Click(object sender, RoutedEventArgs e)
    {
        if (lbxOrder.SelectedItem == null)
        {
            return;
        }

        var selectedItem = lbxOrder.SelectedItem;
        int counter = Convert.ToInt32(selectedItem.ToString().Split(' ')[0].Trim('(', ')'));
        counter--;

        if (counter <= 0)
        {
            lbxOrder.Items.Remove(selectedItem);
        }
        else
        {
            lbxOrder.Items.Remove(selectedItem);
            lbxOrder.Items.Add("(" + counter + ") " + selectedItem.ToString().Substring(selectedItem.ToString().IndexOf(' ') + 1));
        }

        decimal totalPrice = 0.0M;
        recalculateOrderPrice(ref totalPrice);
        txtTotalPrice.Text = totalPrice.ToString("F2") + " zł";
    }
}