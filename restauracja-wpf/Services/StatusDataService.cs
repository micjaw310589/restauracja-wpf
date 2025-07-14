using Microsoft.EntityFrameworkCore;
using restauracja_wpf.Data;
using restauracja_wpf.Models;
using restauracja_wpf.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace restauracja_wpf.Interfaces
{
    public class StatusDataService(GenericDataService<OrderStatus> statusService) : GenericDataService<OrderStatus>(new RestaurantContextFactory())
    {

        private async void AddStatus(string name)
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                await statusService.Create(new OrderStatus()
                {
                    Name = name
                });
            }
        }

        public async Task<int> GetStatusIdByName(string name)
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                var foundStatus = await context.OrderStatuses
                    .Where(s => !s.isDeleted)
                    .FirstOrDefaultAsync(s => s.Name == name);

                return foundStatus.Id;
            }
        }

        public async Task<bool> ClearAll()
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                foreach (var item in context.OrderStatuses)
                {
                    context.OrderStatuses.Remove(item);
                }
                await context.SaveChangesAsync();
                return true;
            }
        }
    }
}
