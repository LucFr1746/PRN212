using System.Collections.Generic;
using Assignment.BusinessObjects;

namespace Assignment.Repositories
{
    public interface ICustomerRepository
    {
        List<Customer> GetAllCustomers();
        Customer? GetCustomerById(int id);
        Customer? GetCustomerByEmail(string email);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int id);
    }
}
