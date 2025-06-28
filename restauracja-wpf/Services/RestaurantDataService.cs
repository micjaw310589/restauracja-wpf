using Microsoft.EntityFrameworkCore;
using restauracja_wpf.Data;
using restauracja_wpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace restauracja_wpf.Services
{
    public class RestaurantDataService(GenericDataService<Restaurant> restaurantService) : GenericDataService<Restaurant>(new Data.RestaurantContextFactory())
    {
        public async void AddRestaurant(string name, string address, string city, bool isOpen)
        {

            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                var existingRestaurant = await context.Restaurants
                .FirstOrDefaultAsync(r => r.Name == name);

                if (existingRestaurant != null)
                    throw new Exception("Restaurant with this name already exists");

                await restaurantService.Create(new Restaurant()
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
