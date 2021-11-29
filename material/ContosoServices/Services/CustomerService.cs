using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoServices.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AdventureworksContext _dbContext;

        public CustomerService(AdventureworksContext dbContext)
        {
            _dbContext = dbContext;
        }

        public object[] Get()
        {
            return _dbContext.Customers.Select(customer => new
            {
                customer.CustomerId,
                customer.FirstName,
                customer.MiddleName,
                customer.LastName,
                customer.Phone,
                customer.EmailAddress,
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

        public Customer Insert(Customer myNewCustomer)
        {
            try
            {
                if (myNewCustomer == null)
                {
                    return null;
                }

                if (_dbContext.Customers.Any(x => x.CustomerId == myNewCustomer.CustomerId))
                {
                    return null;
                }
                else
                {
                    _dbContext.Customers.Add(myNewCustomer);
                    _dbContext.SaveChanges();

                    return myNewCustomer;
                }
            }
            catch (Exception ex)
            {
                ex.InnerException.ToString();
                return null;
            }
        }

        public (int, string) Update(int id, Customer myCustomer)
        {
            if (myCustomer == null)
            {
                return (404, "Cliente no viene con la estructura correspondiente.");
            }

            if (_dbContext.Products.Any(x => x.ProductId == id))
            {
                var customer = _dbContext.Customers.Find(id);

                try
                {
                    customer.FirstName = myCustomer.FirstName;
                    customer.MiddleName = myCustomer.MiddleName;
                    customer.LastName = myCustomer.LastName;
                    customer.Phone = myCustomer.Phone;
                    customer.EmailAddress = myCustomer.EmailAddress;
                    customer.ModifiedDate = DateTime.Now;
                }
                catch (Exception ex)
                {
                    return (500, ex.InnerException.ToString());
                }

                _dbContext.Customers.Update(customer);
                _dbContext.SaveChanges();

                return (200, "Cliente correctamente actualizado.");
            }
            else
            {
                return (404, "Cliente no encontrado.");
            }
        }

        public (int, string) Delete(int id)
        {
            if (_dbContext.Customers.Any(x => x.CustomerId == id))
            {
                var product = _dbContext.Customers.Find(id);

                _dbContext.Customers.Remove(product);
                _dbContext.SaveChanges();

                return (200, "Cliente eliminado correctamente.");
            }
            else
            {
                return (404, "Cliente no encontrado.");
            }
        }
    }
}
