using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoServices.Services
{
    public class SaleOrderService : ISaleOrderService
    {
        private readonly AdventureworksContext _dbContext;

        public SaleOrderService(AdventureworksContext dbContext)
        {
            _dbContext = dbContext;
        }

        public object[] Get()
        {
            return _dbContext.SalesOrderHeaders.Select(saleOrder => new
            {
                saleOrder.SalesOrderId,
                saleOrder.Customer.CustomerId,
                saleOrder.Customer.FirstName,
                saleOrder.Customer.MiddleName,
                saleOrder.Customer.LastName,
                saleOrder.Comment,
                saleOrder.DueDate,
                saleOrder.Status,
                saleOrder.SubTotal,
                saleOrder.TaxAmt,
                saleOrder.TotalDue,
                SalesOrderDetails = _dbContext.SalesOrderDetails.Where(x => x.SalesOrderId == saleOrder.SalesOrderId).Select(x => new
                {
                    x.Product.ProductId,
                    x.Product.Name,
                    x.Product.ProductCategoryId,
                    x.OrderQty,
                    x.UnitPrice,
                    x.UnitPriceDiscount,
                    x.ModifiedDate
                }).ToList()
            }).ToArray();
        }

        public object Get(int id)
        {
            if (_dbContext.SalesOrderHeaders.Any(x => x.SalesOrderId == id))
            {
                var mySaleOrder = _dbContext.SalesOrderHeaders.Find(id);
                mySaleOrder.SalesOrderDetails = _dbContext.SalesOrderDetails.Where(x => x.SalesOrderId == mySaleOrder.SalesOrderId).ToList();

                mySaleOrder.SalesOrderDetails.ToList().ForEach(x => { 
                    x.Product = _dbContext.Products.Find(x.ProductId);
                });

                var myCustomer = _dbContext.Customers.Find(mySaleOrder.CustomerId);

                if (mySaleOrder.SalesOrderDetails.Count > 0)
                {
                    var saleOrder = new
                    {
                        mySaleOrder.SalesOrderId,
                        CustomerId = myCustomer == null ? 0 : myCustomer.CustomerId,
                        FirstName = myCustomer == null ? "" : myCustomer.FirstName,
                        MiddleName = myCustomer == null ? "" : myCustomer.MiddleName,
                        LastName = myCustomer == null ? "" : myCustomer.LastName,
                        mySaleOrder.Comment,
                        mySaleOrder.DueDate,
                        mySaleOrder.Status,
                        mySaleOrder.SubTotal,
                        mySaleOrder.TaxAmt,
                        mySaleOrder.TotalDue,
                        Details = mySaleOrder.SalesOrderDetails.Select(x => new
                        {
                            x.Product.ProductId,
                            x.Product.Name,
                            x.Product.ProductCategoryId,
                            x.OrderQty,
                            x.UnitPrice,
                            x.UnitPriceDiscount,
                            x.ModifiedDate
                        }).ToList()
                    };

                    return saleOrder;
                }
                else
                {
                    var saleOrder = new
                    {
                        mySaleOrder.SalesOrderId,
                        CustomerId = myCustomer == null ? 0 : myCustomer.CustomerId,
                        FirstName = myCustomer == null ? "" : myCustomer.FirstName,
                        MiddleName = myCustomer == null ? "" : myCustomer.MiddleName,
                        LastName = myCustomer == null ? "" : myCustomer.LastName,
                        mySaleOrder.Comment,
                        mySaleOrder.DueDate,
                        mySaleOrder.Status,
                        mySaleOrder.SubTotal,
                        mySaleOrder.TaxAmt,
                        mySaleOrder.TotalDue,
                        Details = Array.Empty<object>()
                    };

                    return saleOrder;
                }
            }
            else
            {
                return null;
            }
        }

        public SalesOrderHeader Insert(SalesOrderHeader myNewSaleOrder)
        {
            try
            {
                if (myNewSaleOrder == null)
                {
                    return null;
                }

                if (_dbContext.SalesOrderHeaders.Any(x => x.SalesOrderId == myNewSaleOrder.SalesOrderId))
                {
                    return null;
                }
                else
                {
                    _dbContext.SalesOrderHeaders.Add(myNewSaleOrder);
                    _dbContext.SalesOrderDetails.AddRange(myNewSaleOrder.SalesOrderDetails);
                    _dbContext.SaveChanges();

                    return myNewSaleOrder;
                }
            }
            catch (Exception ex)
            {
                ex.InnerException.ToString();
                return null;
            }
        }

        public (int, string) Update(int id, SalesOrderHeader mySaleOrder)
        {
            if (mySaleOrder == null)
            {
                return (404, "Orden de compra no tiene la estructura correspondiente.");
            }

            if (_dbContext.SalesOrderHeaders.Any(x => x.SalesOrderId == id))
            {
                var saleOrder = _dbContext.SalesOrderHeaders.Find(id);

                try
                {
                    saleOrder.Comment = mySaleOrder.Comment;
                    saleOrder.DueDate = mySaleOrder.DueDate;
                    saleOrder.Status = mySaleOrder.Status;
                    saleOrder.SubTotal = mySaleOrder.SubTotal;
                    saleOrder.TaxAmt = mySaleOrder.TaxAmt;
                    saleOrder.TotalDue = mySaleOrder.TotalDue;
                    saleOrder.ModifiedDate = DateTime.Now;

                    foreach (var detail in mySaleOrder.SalesOrderDetails)
                    {

                        //si ya existe
                        if (saleOrder.SalesOrderDetails.Any(x => x.ProductId == detail.ProductId))
                        {
                            var mySpecificSaleDetail = saleOrder.SalesOrderDetails.Where(x => x.ProductId == detail.ProductId).FirstOrDefault();

                            mySpecificSaleDetail.OrderQty = detail.OrderQty;
                            mySpecificSaleDetail.UnitPrice = detail.UnitPrice;
                            mySpecificSaleDetail.UnitPriceDiscount = detail.UnitPriceDiscount;
                            mySpecificSaleDetail.ModifiedDate = DateTime.Now;

                            _dbContext.SalesOrderDetails.Update(mySpecificSaleDetail);
                        }
                        else
                        {
                            //si no existe se crea
                            _dbContext.SalesOrderDetails.Add(detail);
                        }
                    }

                    foreach (var recordedDetail in saleOrder.SalesOrderDetails)
                    {
                        if (!mySaleOrder.SalesOrderDetails.Any(x => x.ProductId == recordedDetail.ProductId))
                        {
                            _dbContext.SalesOrderDetails.Remove(recordedDetail);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return (500, ex.InnerException.ToString());
                }

                _dbContext.SalesOrderHeaders.Update(mySaleOrder);
                _dbContext.SaveChanges();

                return (200, "Orden de compra correctamente actualizada.");
            }
            else
            {
                return (404, "Orden de compra no encontrada.");
            }
        }

        public (int, string) Delete(int id)
        {
            if (_dbContext.SalesOrderHeaders.Any(x => x.SalesOrderId == id))
            {
                var saleOrder = _dbContext.SalesOrderHeaders.Find(id);
                var saleOrderDetails = _dbContext.SalesOrderDetails.Where(x => x.SalesOrderId == id).ToList();

                _dbContext.SalesOrderDetails.RemoveRange(saleOrderDetails);
                _dbContext.SalesOrderHeaders.Remove(saleOrder);
                _dbContext.SaveChanges();

                return (200, "Orden de compra eliminada correctamente.");
            }
            else
            {
                return (404, "Orden de compra no encontrada.");
            }
        }
    }
}
