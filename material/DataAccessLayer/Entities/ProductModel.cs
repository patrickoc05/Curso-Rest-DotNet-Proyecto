using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccessLayer.Entities
{
    public partial class ProductModel
    {
        public ProductModel()
        {
            ProductModelProductDescriptions = new HashSet<ProductModelProductDescription>();
        }

        public int ProductModelId { get; set; }
        public string Name { get; set; }
        public string CatalogDescription { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<ProductModelProductDescription> ProductModelProductDescriptions { get; set; }
    }
}
