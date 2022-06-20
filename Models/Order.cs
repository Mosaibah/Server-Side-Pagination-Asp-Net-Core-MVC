using System;
using System.Collections.Generic;

namespace ServerSidePagination.Models
{
    public partial class Order
    {
        public string Id { get; set; } = null!;
        public decimal Total { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedOnUtc { get; set; }
    }
}
