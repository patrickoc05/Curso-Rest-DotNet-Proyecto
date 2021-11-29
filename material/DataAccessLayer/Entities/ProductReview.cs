using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccessLayer
{
    public partial class ProductReview
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public int? CustomerId { get; set; }
        public string Review { get; set; }
        public decimal Rating { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }
}
