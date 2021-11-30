using DataAccessLayer.Entities;
using System.Collections.Generic;

namespace ContosoServices.Services
{
    public interface IProductService
    {
        public object[] Get();
        public object[] Get(int id, string name, int categoryId, string propertyOrder, string typeOrder, int limit);
        public object Get(int id);        
        public Product Insert(Product myNewProduct);
        public (int, string) Update(int id, Product myProduct);
        public (int, string) Delete(int id);
    }
}
