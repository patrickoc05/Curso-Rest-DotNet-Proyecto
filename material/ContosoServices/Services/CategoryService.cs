using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoServices.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AdventureworksContext _dbContext;

        public CategoryService(AdventureworksContext dbContext)
        {
            _dbContext = dbContext;
        }

        public object[] Get()
        {
            return _dbContext.ProductCategories.Select(product => new
            {
                product.ProductCategoryId,
                product.ParentProductCategoryId,
                product.Name,
                Products = _dbContext.Products.Where(x => x.ProductCategoryId == product.ProductCategoryId).Select(productCat => new
                {
                    productCat.ProductId,
                    productCat.Name,
                    productCat.ListPrice,
                }).ToList()
            }).ToArray();
        }

        public object Get(int id)
        {
            if (_dbContext.ProductCategories.Any(x => x.ProductCategoryId == id))
            {
                var MyCategory = _dbContext.ProductCategories.Find(id);

                var category = new
                {
                    MyCategory.ProductCategoryId,
                    MyCategory.ParentProductCategoryId,
                    MyCategory.Name,
                    Products = _dbContext.Products.Where(x => x.ProductCategoryId == MyCategory.ProductCategoryId).Select(productCat => new
                    {
                        productCat.ProductId,
                        productCat.Name,
                        productCat.ListPrice,
                    }).ToList()
                };

                return category;
            }
            else
            {
                return null;
            }
        }

        public ProductCategory Insert(ProductCategory myNewProductCategory)
        {
            try
            {
                if (myNewProductCategory == null)
                {
                    return null;
                }

                if (_dbContext.ProductCategories.Any(x => x.ProductCategoryId == myNewProductCategory.ProductCategoryId))
                {
                    return null;
                }
                else
                {
                    _dbContext.ProductCategories.Add(myNewProductCategory);
                    _dbContext.SaveChanges();

                    return myNewProductCategory;
                }
            }
            catch (Exception ex)
            {
                ex.InnerException.ToString();
                return null;
            }
        }

        public (int, string) Update(int id, ProductCategory myNewProductCategory)
        {
            if (myNewProductCategory == null)
            {
                return (404, "Categoría de producto no viene con la estructura correspondiente.");
            }

            if (_dbContext.ProductCategories.Any(x => x.ProductCategoryId == id))
            {
                var product = _dbContext.ProductCategories.Find(id);

                try
                {
                    product.Name = myNewProductCategory.Name;
                    product.ModifiedDate = DateTime.Now;
                }
                catch (Exception ex)
                {
                    return (500, ex.InnerException.ToString());
                }

                _dbContext.ProductCategories.Update(myNewProductCategory);
                _dbContext.SaveChanges();

                return (200, "Categoría de producto correctamente actualizado.");
            }
            else
            {
                return (404, "Categoría de producto no encontrado.");
            }
        }

        public (int, string) Delete(int id)
        {
            if (_dbContext.ProductCategories.Any(x => x.ProductCategoryId == id))
            {
                var productCategory = _dbContext.ProductCategories.Find(id);

                _dbContext.ProductCategories.Remove(productCategory);
                _dbContext.SaveChanges();

                return (200, "Categoría de producto eliminado correctamente.");
            }
            else
            {
                return (404, "Categoría de producto no encontrado.");
            }
        }
    }
}
