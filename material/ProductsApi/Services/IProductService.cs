using DataAccessLayer.Entities;

namespace ProductsApi.Services
{
    public interface IProductService
    {
        public object[] Get();
        public object Get(int id);
        public Product Insert(Product myNewProduct);
        public (int, string) Update(int id, Product myNewProduct);
        public (int, string) Delete(int id);
    }
}