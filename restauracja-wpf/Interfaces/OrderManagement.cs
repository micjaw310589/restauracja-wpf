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
    public class OrderManagement
    {
        private readonly GenericDataService<Order> _orderService;

        public OrderManagement(GenericDataService<Order> orderService)
        {
            _orderService = orderService;
        }

        public async void AddOrder(int userId,
                                   bool isToGo,
                                   int statusId,
                                   OrderStatus status,
                                   User user = null,
                                   int? reservationId = null,
                                   Reservation reservation = null,
                                   int? regularCustomerId = null,
                                   int? tableId = null,
                                   DateTime? deliveryDate = null,
                                   sbyte? deliveryNumber = null)
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                var foundTable = await context.Tables
                    .FirstOrDefaultAsync(r => r.Id == tableId);

                if (foundTable == null)
                    throw new Exception("Table not found");

                await _orderService.Create(new Order()
                {
                    UserId = userId,
                    IsToGo = isToGo,
                    StatusId = statusId,
                    Status = status,
                    User = user,
                    ReservationId = reservationId,
                    Reservation = reservation,
                    RegularCustomerId = regularCustomerId,
                    TableId = tableId,
                    DeliveryDate = deliveryDate,
                    DeliveryNumber = deliveryNumber
                });
            }

        }

        //public async Task<IEnumerable<Order>> GetActiveOrders(string lastname)
        //{
        //    using (var context = new RestaurantContextFactory().CreateDbContext())
        //    {
        //        IEnumerable<User> foundUsers = await context.Users
        //            .Where(u => u.LastName.Contains(lastname))
        //            .ToListAsync();

        //        return foundUsers;
        //    }
        //}
    }
}
