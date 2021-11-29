using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoServices.Services
{
    public interface ISaleOrderService
    {
        public object[] Get();
        public object Get(int id);
        public SalesOrderHeader Insert(SalesOrderHeader myNewSaleOrder);
        public (int, string) Update(int id, SalesOrderHeader mySaleOrder);
        public (int, string) Delete(int id);
    }
}
