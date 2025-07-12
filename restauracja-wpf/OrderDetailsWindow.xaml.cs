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
using restauracja_wpf.Interfaces;
using restauracja_wpf.Models;
using restauracja_wpf.Services;
using restauracja_wpf.Data;


namespace restauracja_wpf
{
    public partial class OrderDetailsWindow : Window
    {
        private Order order1;

        public OrderDetailsWindow()
        {
            InitializeComponent();

            if (SessionManager.CurrentUser.Role.Name == "Chef" || SessionManager.CurrentUser.Role.Name == "Admin")
            {
                btnTakeawayOrder.Visibility = Visibility.Visible;
                btnAcceptOrder_Kopiuj.Visibility = Visibility.Visible;
                btnRejectOrder.Visibility = Visibility.Visible;
            }
            else if (SessionManager.CurrentUser.Role.Name == "Waiter")
            {
                btnTakeawayOrder.Visibility = Visibility.Collapsed;
                btnAcceptOrder_Kopiuj.Visibility = Visibility.Collapsed;
                btnRejectOrder.Visibility = Visibility.Collapsed;
            }

        }

        public OrderDetailsWindow(Order order) : this()
        {
            order1 = order ?? throw new ArgumentNullException(nameof(order), "Order cannot be null.");
            loadContent();
        }

        private async void loadContent()
        {
            if (order1 == null)
            {
                return;
            }
            OrderDataService orderService = new(new GenericDataService<Order>(new RestaurantContextFactory()));
            IEnumerable<Order> orderDetails = await orderService.GetOrderDetails(order1.Id);
            OrderListBox.Items.Clear();
            foreach (var order in orderDetails)
            {
                OrderListBox.Items.Add($"{order.Id} - {order.Status.Name} - {order.DeliveryNumber}");
                foreach (var item in order.DishOrders)
                {
                    if (item == null || item.Dish == null)
                    {
                        continue;
                    }
                    else
                    {
                        OrderListBox.Items.Add($"{item.Dish.Name} - {item.Quantity} szt. - {item.PurchasePrice} zł");
                    }
                }
            }
        }
        //"Placed", "Rejected", "In progress", "Done", "Served", "Cancelled"
        private async void btnTakeawayOrder_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy chcesz potwierdzić wydanie zamówienia?", "Potwierdź:", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                GenericDataService<Order> orderManagement = new GenericDataService<Order>(new RestaurantContextFactory());

                var statuses = await new GenericDataService<OrderStatus>(new RestaurantContextFactory()).GetAll();
                var wydaneStatus = statuses.FirstOrDefault(s => s.Name == "Served");
                order1.StatusId = wydaneStatus.Id;
                orderManagement.Update(order1.Id, order1);

                MessageBox.Show("Order taken away.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Operation cancelled.", "Cancelled", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            this.Close();
        }

        private async void btnAcceptOrder_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy chcesz przyjąć zamówienie?", "Przyjmij:", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                GenericDataService<Order> orderManagement = new GenericDataService<Order>(new RestaurantContextFactory());

                var statuses = await new GenericDataService<OrderStatus>(new RestaurantContextFactory()).GetAll();
                var wydaneStatus = statuses.FirstOrDefault(s => s.Name == "In progress");
                order1.StatusId = wydaneStatus.Id;
                orderManagement.Update(order1.Id, order1);

                MessageBox.Show("Order accepted", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Operation cancelled.", "Cancelled", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            this.Close();
        }

        private async void btnRejectOrder_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy chcesz odrzucić zamówienie?", "Potwierdź:", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                GenericDataService<Order> orderManagement = new GenericDataService<Order>(new RestaurantContextFactory());

                var statuses = await new GenericDataService<OrderStatus>(new RestaurantContextFactory()).GetAll();
                var wydaneStatus = statuses.FirstOrDefault(s => s.Name == "Rejected");
                order1.StatusId = wydaneStatus.Id;
                orderManagement.Update(order1.Id, order1);

                MessageBox.Show("Order rejected", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Operation cancelled.", "Cancelled", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            this.Close();
        }

        private async void btnCancelOrder_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy chcesz potwierdzić wydanie zamówienia?", "Potwierdź:", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                GenericDataService<Order> orderManagement = new GenericDataService<Order>(new RestaurantContextFactory());

                var statuses = await new GenericDataService<OrderStatus>(new RestaurantContextFactory()).GetAll();
                var wydaneStatus = statuses.FirstOrDefault(s => s.Name == "In progress");
                order1.StatusId = wydaneStatus.Id;
                orderManagement.Update(order1.Id, order1);

                MessageBox.Show("Order cancelled.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Operation cancelled.", "Cancelled", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            this.Close();
        }
    }
}