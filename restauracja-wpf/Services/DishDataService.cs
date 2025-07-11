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
    public class DishDataService(GenericDataService<Dish> dishService) : GenericDataService<Dish>(new RestaurantContextFactory())
    {
        public async void AddDish(string name, decimal price, bool avaibility, TimeSpan time)
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                await dishService.Create(new Dish()
                {
                    Name = name,
                    Price = price,
                    Available = avaibility,
                    TimeConstant = time
                });
            }
        }

        public async Task<IEnumerable<Dish>> GetDishesByName(string name)
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                IEnumerable<Dish> foundDishes = await context.Set<Dish>()
                    .Where(d => d.Name.Contains(name))
                    .Where(d => d.isDeleted == false)
                    .ToListAsync();
                return foundDishes;
            }
        }

        public async Task<IEnumerable<Dish>> GetAllDishes(bool onlyAvailable)
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                IEnumerable<Dish> foundDishes;

                foundDishes = await context.Set<Dish>()
                    .Where(d => d.Available == onlyAvailable)
                    .Where(d => d.Exclude == false)
                    .Where(d => d.isDeleted == false)
                    .ToListAsync();

                return foundDishes;
            }
        }

    }
}
