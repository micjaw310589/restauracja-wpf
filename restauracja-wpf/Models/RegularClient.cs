﻿using EntityFrameworkCore.EncryptColumn.Attribute;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restauracja_wpf.Models
{
    [Table("RegularClients")]
    public class RegularClient : DomainObject
    {
        //public int RegularClientId { get; set; }

        [MaxLength(15)]
        public string FirstName { get; set; } = "";

        [MaxLength(20)]
        public string LastName { get; set; } = "";

        [EncryptColumn]
        [MaxLength(12)]
        public string? PhoneNumber { get; set; } = null;

        [EncryptColumn]
        [MaxLength(10)]
        public string? Email { get; set; } = null;

        public int? DiscountId { get; set; } = null;
        public Discount? Discount { get; set; } = null;

        public bool IsActive { get; set; } = true;


        public List<Order> Orders { get; set; } = new List<Order>();

    }
}
