using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsApi.Services
{
    public class ProductService : IProductService
    {
        private AdventureworksContext _dbContext;

        public ProductService(AdventureworksContext dbContext)
        {
            _dbContext = dbContext;
        }

        public object[] Get()
        {
            return _dbContext.Products.Select(product => new
            {
                product.ProductId,
                product.Name,
                product.ListPrice,
            }).ToArray();
        }

        public object Get(int id)
        {
            if (_dbContext.Products.Any(x => x.ProductId == id))
            {
                return _dbContext.Products.Find(id);
            }
            else
            {
                return null;
            }
        }

        public Product Insert(Product myNewProduct)
        {
            try
            {
                if (myNewProduct == null)
                {
                    return null;
                }

                if (_dbContext.Products.Any(x => x.ProductId == myNewProduct.ProductId))
                {
                    return null;
                }
                else
                {
                    _dbContext.Products.Add(myNewProduct);
                    _dbContext.SaveChanges();

                    return myNewProduct;
                }
            }
            catch (Exception ex)
            {
                ex.InnerException.ToString();
                return null;
            }
        }

        public (int, string) Update(int id, Product myNewProduct)
        {
            if (myNewProduct == null)
            {
                return (404, "Producto no viene con la estructura correspondiente.");
            }

            if (_dbContext.Products.Any(x => x.ProductId == id))
            {
                var product = _dbContext.Products.Find(id);

                try
                {
                    product.Name = myNewProduct.Name;
                    product.ProductNumber = myNewProduct.ProductNumber;
                    product.Color = myNewProduct.Color;
                    product.StandardCost = myNewProduct.StandardCost;
                    product.ListPrice = myNewProduct.ListPrice;
                    product.Size = myNewProduct.Size;
                    product.Weight = myNewProduct.Weight;
                    product.ProductCategoryId = myNewProduct.ProductCategoryId;
                    product.ProductModelId = myNewProduct.ProductModelId;
                    product.SellStartDate = myNewProduct.SellStartDate;
                    product.DiscontinuedDate = myNewProduct.DiscontinuedDate;
                    product.ThumbNailPhoto = myNewProduct.ThumbNailPhoto;
                    product.ThumbnailPhotoFileName = myNewProduct.ThumbnailPhotoFileName;
                    product.ModifiedDate = DateTime.Now;
                }
                catch (Exception ex)
                {                    
                    return (500, ex.InnerException.ToString());
                }

                _dbContext.Products.Update(product);
                _dbContext.SaveChanges();

                return (200, "Producto correctamente actualizado.");
            }
            else
            {
                return (404, "Producto no encontrado.");
            }
        }

        public (int, string) Delete(int id)
        {
            if (_dbContext.Products.Any(x => x.ProductId == id))
            {
                var product = _dbContext.Products.Find(id);

                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();

                return (200, "Producto eliminado correctamente.");
            }
            else
            {
                return (404, "Producto no encontrado.");
            }
        }
    }
}
