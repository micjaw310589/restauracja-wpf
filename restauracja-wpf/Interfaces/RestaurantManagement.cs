using Microsoft.EntityFrameworkCore;
using restauracja_wpf.Data;
using restauracja_wpf.Models;
using restauracja_wpf.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace restauracja_wpf.Interfaces
{
    public class RestaurantManagement
    {
        private readonly GenericDataService<Restaurant> _restaurantService;

        public RestaurantManagement(GenericDataService<Restaurant> restaurantService)
        {
            _restaurantService = restaurantService;
        }

        public async void AddRestaurant(string name, string address, string city, bool isOpen)
        {

            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                var existingRestaurant = await context.Restaurants
                .FirstOrDefaultAsync(r => r.Name == name);

                if (existingRestaurant != null)
                    throw new Exception("Restaurant with this name already exists");


                await _restaurantService.Create(new Restaurant()
                {
                    Name = name,
                    Address = address,
                    City = city,
                    IsOpen = isOpen
                });
            }
        }

        public async Task<IEnumerable<Restaurant>> GetMatchingRestaurants(string name)
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                IEnumerable<Restaurant> foundRestaurants = await context.Restaurants
                    .Where(r => r.Name.Contains(name))
                    .ToListAsync();
                return foundRestaurants;
            }
        }

    }
}
