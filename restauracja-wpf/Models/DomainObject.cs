﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace restauracja_wpf.Models
{
    public class DomainObject
    {
        public int Id { get; set; }
        public bool isDeleted { get; set; } = false;
    }
}
