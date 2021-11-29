using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoServices.Services
{
    interface ICustomerService
    {
        public object[] Get();
        public object Get(int id);
        public Customer Insert(Customer myNewCustomer);
        public (int, string) Update(int id, Customer myCustomer);
        public (int, string) Delete(int id);
    }
}
