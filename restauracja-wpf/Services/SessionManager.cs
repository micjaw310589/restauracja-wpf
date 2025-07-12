using restauracja_wpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace restauracja_wpf.Services
{
    public static class SessionManager
    {
        public static User CurrentUser { get; set; } = null!;
    }
}
