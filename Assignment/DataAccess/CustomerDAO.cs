using System;
using System.Collections.Generic;
using System.Linq;
using Assignment.BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace Assignment.DataAccess
{
    public class CustomerDAO
    {
        private static CustomerDAO? instance;
        private static readonly object instanceLock = new object();

        private CustomerDAO() { }

        public static CustomerDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CustomerDAO();
                    }
                    return instance;
                }
            }
        }

        public List<Customer> GetAll()
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.Customers.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving customers: {ex.Message}");
            }
        }

        public Customer? GetById(int id)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.Customers.FirstOrDefault(c => c.CustomerId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving customer: {ex.Message}");
            }
        }

        public Customer? GetByEmail(string email)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.Customers.FirstOrDefault(c => c.EmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving customer by email: {ex.Message}");
            }
        }

        public void Add(Customer customer)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                // Ensure email uniqueness is preserved before inserting
                if (context.Customers.Any(c => c.EmailAddress == customer.EmailAddress))
                {
                    throw new Exception("Email address is already in use.");
                }
                context.Customers.Add(customer);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding customer: {ex.Message}");
            }
        }

        public void Update(Customer customer)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                // Verify email uniqueness (ignoring the customer being updated)
                if (context.Customers.Any(c => c.EmailAddress == customer.EmailAddress && c.CustomerId != customer.CustomerId))
                {
                    throw new Exception("Email address is already in use by another customer.");
                }
                context.Entry(customer).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating customer: {ex.Message}");
            }
        }

        public void Delete(int id)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                var customer = context.Customers.Find(id);
                if (customer != null)
                {
                    context.Customers.Remove(customer);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting customer: {ex.Message}");
            }
        }
    }
}
