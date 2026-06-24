using System.Collections.Generic;
using Assignment.BusinessObjects;
using Assignment.DataAccess;

namespace Assignment.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public List<Customer> GetAllCustomers() => CustomerDAO.Instance.GetAll();

        public Customer? GetCustomerById(int id) => CustomerDAO.Instance.GetById(id);

        public Customer? GetCustomerByEmail(string email) => CustomerDAO.Instance.GetByEmail(email);

        public void AddCustomer(Customer customer) => CustomerDAO.Instance.Add(customer);

        public void UpdateCustomer(Customer customer) => CustomerDAO.Instance.Update(customer);

        public void DeleteCustomer(int id) => CustomerDAO.Instance.Delete(id);
    }
}
