using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoServices.Services
{
    public interface IReviewService
    {
        public object[] Get();
        public object Get(int id);
        public ProductReview Insert(ProductReview myNewProductReview);
        public (int, string) Update(int id, ProductReview myProductReview);
        public (int, string) Delete(int id);
    }
}
