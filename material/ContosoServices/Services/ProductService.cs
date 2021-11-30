using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoServices.Services
{
    public class ProductService : IProductService
    {
        private readonly AdventureworksContext _dbContext;

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
                product.ListPrice
            }).ToArray();
        }

        public object[] Get(int id, string name, int categoryId, string propertyOrder, string typeOrder, int limit)
        {
            List<Product> productsReturn = new();

            if (id == 0 && name == null && categoryId == 0)
            {
                productsReturn = _dbContext.Products.Select(product => new Product
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    ListPrice = product.ListPrice,
                    ProductCategoryId = product.ProductCategoryId,
                    ModifiedDate = product.ModifiedDate,
                    Weight = _dbContext.ProductReviews.Where(x => x.ProductId == product.ProductId).Average(x => x.Rating)
                }).ToList();

                if (string.IsNullOrEmpty(propertyOrder))
                {
                    propertyOrder = " ";
                }
            }

            if (id > 0)
            {
                if (_dbContext.Products.Any(x => x.ProductId == id))
                {
                    productsReturn = _dbContext.Products.Where(x => x.ProductId == id).Select(product => new Product
                    {
                        ProductId = product.ProductId,
                        Name = product.Name,
                        ListPrice = product.ListPrice,
                        ProductCategoryId = product.ProductCategoryId,
                        ModifiedDate = product.ModifiedDate,
                        Weight = _dbContext.ProductReviews.Where(x => x.ProductId == product.ProductId).Average(x => x.Rating)
                    }).ToList();
                }
            }

            if (!string.IsNullOrEmpty(name))
            {
                if (productsReturn.Count == 0)
                {
                    productsReturn = _dbContext.Products.Where(x => x.Name.Contains(name)).Select(product => new Product
                    {
                        ProductId = product.ProductId,
                        Name = product.Name,
                        ListPrice = product.ListPrice,
                        ProductCategoryId = product.ProductCategoryId,
                        ModifiedDate = product.ModifiedDate,
                        Weight = _dbContext.ProductReviews.Where(x => x.ProductId == product.ProductId).Average(x => x.Rating)
                    }).ToList();
                }
                else
                {
                    productsReturn = productsReturn.Where(x => x.Name.Contains(name)).ToList();
                }
            }

            if (categoryId > 0)
            {
                if (productsReturn.Count == 0)
                {
                    productsReturn = _dbContext.Products.Where(x => x.ProductCategoryId == categoryId).Select(product => new Product
                    {
                        ProductId = product.ProductId,
                        Name = product.Name,
                        ListPrice = product.ListPrice,
                        ProductCategoryId = product.ProductCategoryId,
                        ModifiedDate = product.ModifiedDate,
                        Weight = _dbContext.ProductReviews.Where(x => x.ProductId == product.ProductId).Average(x => x.Rating)
                    }).ToList();
                }
                else
                {
                    productsReturn = productsReturn.Where(x => x.ProductCategoryId == categoryId).ToList();
                }
            }

            if (!string.IsNullOrEmpty(propertyOrder))
            {
                if (productsReturn.Count > 0)
                {
                    if (string.IsNullOrEmpty(typeOrder))
                    {
                        typeOrder = "asc";
                    }

                    switch (propertyOrder.ToLower())
                    {
                        case "new":
                            {
                                if (typeOrder.ToLower().Equals("asc"))
                                {
                                    productsReturn = productsReturn.OrderBy(x => x.ModifiedDate).ToList();
                                }
                                else
                                {
                                    productsReturn = productsReturn.OrderByDescending(x => x.ModifiedDate).ToList();
                                }
                                break;
                            };
                        case "voted":
                            {
                                if (typeOrder.ToLower().Equals("asc"))
                                {
                                    productsReturn = productsReturn.OrderBy(x => x.Weight ?? 0).ToList();
                                }
                                else
                                {
                                    productsReturn = productsReturn.OrderByDescending(x => x.Weight ?? 0).ToList();
                                }
                                break;
                            };
                        case "trending":
                            {
                                if (typeOrder.ToLower().Equals("asc"))
                                {
                                    productsReturn = productsReturn.OrderBy(x => x.ProductId).ToList();
                                }
                                else
                                {
                                    productsReturn = productsReturn.OrderByDescending(x => x.ProductId).ToList();
                                }
                                break;
                            };
                        case "price":
                            {
                                if (typeOrder.ToLower().Equals("asc"))
                                {
                                    productsReturn = productsReturn.OrderBy(x => x.ListPrice).ToList();
                                }
                                else
                                {
                                    productsReturn = productsReturn.OrderByDescending(x => x.ListPrice).ToList();
                                }
                                break;
                            };
                        case "name":
                            {
                                if (typeOrder.ToLower().Equals("asc"))
                                {
                                    productsReturn = productsReturn.OrderBy(x => x.Name).ToList();
                                }
                                else
                                {
                                    productsReturn = productsReturn.OrderByDescending(x => x.Name).ToList();
                                }
                                break;
                            };
                        default:
                            {
                                if (typeOrder.ToLower().Equals("asc"))
                                {
                                    productsReturn = productsReturn.OrderBy(x => x.ProductId).ToList();
                                }
                                else
                                {
                                    productsReturn = productsReturn.OrderByDescending(x => x.ProductId).ToList();
                                }
                                break;
                            };
                    }
                }
            }

            if (limit > 0)
            {
                productsReturn = productsReturn.Take(limit).ToList();
            }

            return productsReturn.Select(product => new
            {
                product.ProductId,
                product.Name,
                product.ListPrice
            }).ToArray(); ;
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
