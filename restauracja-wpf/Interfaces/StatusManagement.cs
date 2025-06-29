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
    public class StatusManagement
    {
        private readonly GenericDataService<OrderStatus> _statusService;

        public StatusManagement(GenericDataService<OrderStatus> statusService) { 
            _statusService = statusService;
        }

        public async void AddStatus(string name)
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                await _statusService.Create(new OrderStatus()
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
                    .FirstOrDefaultAsync(s => s.Name == name);
                if (foundStatus == null)
                {
                    AddStatus(name);
                    foundStatus = await context.OrderStatuses
                    .FirstOrDefaultAsync(s => s.Name == name);
                    if (foundStatus == null)
                    {
                        throw new Exception($"Status with name '{name}' could not be found or created.");
                    }
                }
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
