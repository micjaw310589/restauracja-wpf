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
            OrderManagement orderManagement = new OrderManagement(new GenericDataService<Order>(new RestaurantContextFactory()));
            IEnumerable<Order> orderDetails = await orderManagement.GetOrderDetails(order1.Id);
            OrderListBox.Items.Clear();
            foreach (var order in orderDetails)
            {
                OrderListBox.Items.Add($"{order.Id} - {order.Status} - {order.DeliveryNumber}");
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

        private async void btnAcceptOrder_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Czy chcesz potwierdzić wydanie zamówienia?", "Potwierdź:", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                GenericDataService<Order> orderManagement = new GenericDataService<Order>(new RestaurantContextFactory());

                var statuses = await new GenericDataService<OrderStatus>(new RestaurantContextFactory()).GetAll();
                var wydaneStatus = statuses.FirstOrDefault(s => s.Name == "Wydane");
                order1.StatusId = wydaneStatus.Id;
                orderManagement.Update(order1.Id, order1);

                MessageBox.Show("Możesz wydać zamówienie.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Anulowano wydanie zamówienia.", "Anulowano", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            this.Close();
        }
    }
}
