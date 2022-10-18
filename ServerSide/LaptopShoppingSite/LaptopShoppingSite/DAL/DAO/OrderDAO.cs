using LaptopShoppingSite.DAL.DomainClasses;
using LaptopShoppingSite.DAL.Helpers;
using Microsoft.EntityFrameworkCore;
using System;

namespace LaptopShoppingSite.DAL.DAO
{
    public class OrderDAO
    {
        private StoreContext _db;
        public OrderDAO(StoreContext ctx)
        {
            _db = ctx;
        }
        public async Task<int> AddOrder(int userid, OrderSelectionHelper[] selections)
        {
            int orderId = -1;
            using (_db)
            {
                // we need a transaction as multiple entities involved
                using (var _trans = await _db.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Order
                        Order order = new Order();
                        order.UserId = userid;
                        order.OrderDate = System.DateTime.Now;
                        order.OrderAmount = 0.0M;

                        // Product
                        ProductDAO dao = new ProductDAO(_db);
                        // calculate the total and then add the order row to the table
                        foreach (OrderSelectionHelper selection in selections)
                        {
                            order.OrderAmount += Convert.ToDecimal(selection.Product.MRSP) * selection.Qty;
                        }
                        await _db.Orders.AddAsync(order);
                        await _db.SaveChangesAsync();
                        // then add each item to the order line items table
                        foreach (OrderSelectionHelper selection in selections)
                        {
                            OrderLineItem oLineItem = new OrderLineItem();
                            oLineItem.ProductId = selection.Product.Id;
                            oLineItem.OrderId = order.Id;
                            oLineItem.SellingPrice = Convert.ToDecimal(selection.Product.MRSP);
                            Product product = await dao.GetById(selection.Product.Id);

                            // Calculating qtys
                            if (selection.Qty < product.QtyOnHand)
                            {
                                product.QtyOnHand -= selection.Qty;
                                oLineItem.QtySold = selection.Qty;
                                oLineItem.QtyOrdered = selection.Qty;
                                oLineItem.QtyBackOrdered = 0;
                            }
                            else
                            {
                                product.QtyOnBackOrder += selection.Qty - product.QtyOnHand;
                                oLineItem.QtySold = product.QtyOnHand;
                                oLineItem.QtyOrdered = selection.Qty;
                                oLineItem.QtyBackOrdered = selection.Qty - product.QtyOnHand;
                                product.QtyOnHand = 0;
                            }

                            // Saving changes
                            await _db.OrderLineItems.AddAsync(oLineItem);
                            await _db.SaveChangesAsync();
                            _db.Products.Update(product);
                        }
                        await _trans.CommitAsync();
                        orderId = order.Id;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        await _trans.RollbackAsync();
                    }
                }
            }
            return orderId;
        }

        public async Task<List<Order>> GetAll(int id)
        {
            return await _db.Orders.Where(order => order.UserId == id).ToListAsync();
        }

        public async Task<List<OrderDetailsHelper>> GetOrderDetails(int oid, string email)
        {
            Customer customer = _db.Customers.FirstOrDefault(customer => customer.Email == email);
            List<OrderDetailsHelper> allDetails = new List<OrderDetailsHelper>();
            // LINQ way of doing INNER JOINS
            var results = from o in _db.Orders
                          join oi in _db.OrderLineItems on o.Id equals oi.OrderId
                          join p in _db.Products on oi.ProductId equals p.Id
                          where (o.UserId == customer.Id && o.Id == oid)
                          select new OrderDetailsHelper
                          {
                              OrderId = o.Id,
                              ProductId = oi.ProductId,
                              UserId = customer.Id,
                              Name = p.ProductName,
                              QtySold = oi.QtySold,
                              QtyOrdered = oi.QtyOrdered,
                              QtyBackOrdered = oi.QtyBackOrdered,
                              SellingPrice = oi.SellingPrice,
                              OrderDate = o.OrderDate.ToString("yyyy/MM/dd - hh:mm tt")
                          };
            allDetails = await results.ToListAsync();
            return allDetails;
        }
    }
}
