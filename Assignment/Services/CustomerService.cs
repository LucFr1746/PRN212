using System;
using System.Collections.Generic;
using Assignment.BusinessObjects;
using Assignment.Repositories;

namespace Assignment.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService()
        {
            _customerRepository = new CustomerRepository();
        }

        public List<Customer> GetAllCustomers() => _customerRepository.GetAllCustomers();

        public Customer? GetCustomerById(int id) => _customerRepository.GetCustomerById(id);

        public Customer? GetCustomerByEmail(string email) => _customerRepository.GetCustomerByEmail(email);

        public void AddCustomer(Customer customer) => _customerRepository.AddCustomer(customer);

        public void UpdateCustomer(Customer customer) => _customerRepository.UpdateCustomer(customer);

        public void DeleteCustomer(int id) => _customerRepository.DeleteCustomer(id);

        public Customer? Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            var customer = _customerRepository.GetCustomerByEmail(email);
            if (customer != null && customer.Password == password && customer.CustomerStatus == 1)
            {
                return customer;
            }

            return null;
        }
    }
}
