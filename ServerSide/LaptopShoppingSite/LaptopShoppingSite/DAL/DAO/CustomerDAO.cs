using LaptopShoppingSite.DAL.DomainClasses;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LaptopShoppingSite.DAL.DAO
{
    public class CustomerDAO
    {
        private StoreContext _db;
        public CustomerDAO(StoreContext ctx)
        {
            _db = ctx;
        }


        public async Task<Customer>Register(Customer customer)
        {
            await _db.Customers.AddAsync(customer);
            await _db.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> GetByEmail(string email)
        {
            Customer customer = _db.Customers.FirstOrDefault(u => u.Email == email);
            return customer;
        }


    }
}
