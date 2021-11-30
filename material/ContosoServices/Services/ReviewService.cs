using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoServices.Services
{
    public class ReviewService : IReviewService
    {
        private readonly AdventureworksContext _dbContext;

        public ReviewService(AdventureworksContext dbContext)
        {
            _dbContext = dbContext;
        }

        public object[] Get()
        {
            return _dbContext.ProductReviews.Select(productReview => new
            {
                productReview.ReviewId,
                productReview.ProductId,
                productReview.CustomerId,
                productReview.Review,
                productReview.Rating,
                productReview.ModifiedDate
            }).ToArray();
        }

        public object Get(int id)
        {
            if (_dbContext.ProductReviews.Any(x => x.ProductId == id))
            {
                var myReview = _dbContext.ProductReviews.Find(id);

                var productReview = new
                {
                    myReview.ReviewId,
                    myReview.ProductId,
                    myReview.CustomerId,
                    myReview.Review,
                    myReview.Rating,
                    myReview.ModifiedDate
                };

                return productReview;
            }
            else
            {
                return null;
            }
        }

        public ProductReview Insert(ProductReview myNewProductReview)
        {
            try
            {
                if (myNewProductReview == null)
                {
                    return null;
                }

                if (_dbContext.ProductReviews.Any(x => x.ReviewId == myNewProductReview.ReviewId))
                {
                    return null;
                }
                else
                {
                    _dbContext.ProductReviews.Add(myNewProductReview);
                    _dbContext.SaveChanges();

                    return myNewProductReview;
                }
            }
            catch (Exception ex)
            {
                ex.InnerException.ToString();
                return null;
            }
        }

        public (int, string) Update(int id, ProductReview myNewProductReview)
        {
            if (myNewProductReview == null)
            {
                return (404, "Product review no viene con la estructura correspondiente.");
            }

            if (_dbContext.ProductReviews.Any(x => x.ReviewId == id))
            {
                var productReview = _dbContext.ProductReviews.Find(id);

                try
                {
                    productReview.Review = myNewProductReview.Review;
                    productReview.Rating = myNewProductReview.Rating;
                    productReview.ModifiedDate = DateTime.Now;
                }
                catch (Exception ex)
                {
                    return (500, ex.InnerException.ToString());
                }

                _dbContext.ProductReviews.Update(productReview);
                _dbContext.SaveChanges();

                return (200, "Producto review correctamente actualizado.");
            }
            else
            {
                return (404, "Producto review no encontrado.");
            }
        }

        public (int, string) Delete(int id)
        {
            if (_dbContext.ProductReviews.Any(x => x.ReviewId == id))
            {
                var productReview = _dbContext.ProductReviews.Find(id);

                _dbContext.ProductReviews.Remove(productReview);
                _dbContext.SaveChanges();

                return (200, "Product review eliminado correctamente.");
            }
            else
            {
                return (404, "Producto review no encontrado.");
            }
        }
    }
}
