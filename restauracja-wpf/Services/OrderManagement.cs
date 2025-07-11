using Microsoft.EntityFrameworkCore;
using restauracja_wpf.Data;
using restauracja_wpf.Models;
using restauracja_wpf.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace restauracja_wpf.Interfaces
{
    public class OrderDataService(GenericDataService<Order> orderService) : GenericDataService<Order>(new RestaurantContextFactory())
    {

        public async Task<IEnumerable<Order>> GetActiveOrders()
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                //"Placed", "Rejected", "In progress", "Done", "Served", "Cancelled"

                IEnumerable<Order> foundOrders = await context.Orders
                    .Include(o => o.Status)
                    .Include(o => o.User)
                    .Include(o => o.Reservation)
                    .Include(o => o.RegularCustomer)
                    .Include(o => o.Tables)
                    .Include(o => o.DishOrders)
                        .ThenInclude(dor => dor.Dish)
                    .Where(o => o.Status.Name.Contains("Placed") || o.Status.Name.Contains("In progress") || o.Status.Name.Contains("Done"))
                    .ToListAsync();

                return foundOrders;
            }
        }

        public async Task<IEnumerable<Order>> GetOrderDetails(int Id)
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                //"Placed", "Rejected", "In progress", "Done", "Served", "Cancelled"
                IEnumerable<Order> foundOrders = await context.Orders
                    .Include(o => o.Status)
                    .Include(o => o.User)
                    .Include(o => o.Reservation)
                    .Include(o => o.RegularCustomer)
                    .Include(o => o.Tables)
                    .Include(o => o.DishOrders)
                        .ThenInclude(dor => dor.Dish)
                        .Where(o => o.Id == Id)
                    .ToListAsync();
                return foundOrders;
            }
        }

        public async Task<IEnumerable<Order>> GetClosedOrders()
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                //"Placed", "Rejected", "In progress", "Done", "Served", "Cancelled"

                IEnumerable<Order> foundOrders = await context.Orders
                    .Include(o => o.Status)
                    .Include(o => o.User)
                    .Include(o => o.Reservation)
                    .Include(o => o.RegularCustomer)
                    .Include(o => o.Tables)
                    .Include(o => o.DishOrders)
                        .ThenInclude(dor => dor.Dish)
                    .Where(o => o.Status.Name.Contains("Rejected") || o.Status.Name.Contains("Cancelled") || o.Status.Name.Contains("Served"))
                    .ToListAsync();

                return foundOrders;
            }
        }
    }
}
