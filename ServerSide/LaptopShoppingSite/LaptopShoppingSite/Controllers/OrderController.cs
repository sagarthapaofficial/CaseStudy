using LaptopShoppingSite.DAL;
using LaptopShoppingSite.DAL.DAO;
using LaptopShoppingSite.DAL.DomainClasses;
using LaptopShoppingSite.DAL.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LaptopShoppingSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        StoreContext _ctx;
        public OrderController(StoreContext context) // injected here
        {
            _ctx = context;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<string>> Index(OrderHelper helper)
        {
            string retVal = "";
            bool backOrdered = false;
            try
            {
                CustomerDAO cDao = new CustomerDAO(_ctx);
                Customer orderOwner = await cDao.GetByEmail(helper.email);
                OrderDAO oDao = new OrderDAO(_ctx);
                int orderId = await oDao.AddOrder(orderOwner.Id, helper.selections);
                if (orderId > 0)
                {
                    retVal = "Order " + orderId + " completed!";
                }
                else
                {
                    retVal = "Order not completed";
                }

                // Check if any order was backordered
                foreach (OrderSelectionHelper selection in helper.selections)
                {
                    if (selection.Qty > selection.Product.QtyOnHand)
                    {
                        backOrdered = true;
                    }
                }

                if (backOrdered)
                {
                    retVal += " Some goods backordered!";
                }
            }
            catch (Exception ex)
            {
                retVal = "Order not completed " + ex.Message;
            }
            return retVal;
        }

        [Route("{email}")]
        public async Task<ActionResult<List<Order>>> List(string email)
        {
            List<Order> trays = new List<Order>();
            CustomerDAO cDao = new CustomerDAO(_ctx);
            Customer cartOwner = await cDao.GetByEmail(email);
            OrderDAO oDao = new OrderDAO(_ctx);
            trays = await oDao.GetAll(cartOwner.Id);
            return trays;
        }

        [Route("{orderid}/{email}")]
        public async Task<ActionResult<List<OrderDetailsHelper>>> GetOrderDetails(int orderid, string email)
        {
            OrderDAO dao = new OrderDAO(_ctx);
            return await dao.GetOrderDetails(orderid, email);
        }
    }
}
