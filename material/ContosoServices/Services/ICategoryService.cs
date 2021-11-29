using DataAccessLayer.Entities;

namespace ContosoServices.Services
{
    public interface ICategoryService
    {
        public object[] Get();
        public object Get(int id);
        public ProductCategory Insert(ProductCategory myNewProductCategory);
        public (int, string) Update(int id, ProductCategory myProductCategory);
        public (int, string) Delete(int id);
    }
}
