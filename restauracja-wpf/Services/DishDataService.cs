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

        public async Task<IEnumerable<Dish>> GetMatchingDishes(string name)
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                IEnumerable<Dish> foundDishes = await context.Set<Dish>()
                    .Where(d => d.Name.Contains(name))
                    .ToListAsync();
                return foundDishes;
            }
        }

        public async Task<IEnumerable<Dish>> GetAllDishes()
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                IEnumerable<Dish> foundDishes = await context.Set<Dish>().ToListAsync();
                return foundDishes;
            }
        }

        public async Task<IEnumerable<Dish>> GetAllAvalibleDishes()
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                IEnumerable<Dish> foundDishes = await context.Set<Dish>()
                    .Where(d => d.Available == true)
                    .ToListAsync();
                return foundDishes;
            }
        }

        public async Task<Dish> GetDish(int id)
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                Dish dish = await context.Set<Dish>().FirstOrDefaultAsync((d) => d.Id == id);
                return dish;
            }
        }

        public async Task<bool> DeleteDish(int id)
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                Dish dish = await context.Set<Dish>().FirstOrDefaultAsync((d) => d.Id == id);
                if (dish == null)
                {
                    return false; // Dish not found
                }
                context.Set<Dish>().Remove(dish);
                await context.SaveChangesAsync();
                return true;
            }

        }

    }
}
