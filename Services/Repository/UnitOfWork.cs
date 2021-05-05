using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Services.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository CustomerRepository { get; }
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICustomerInfoRepository CustomerInfoRepository { get; }
        IOrderRepository OrderRepository { get;}
        IUtilityRepository UtilityRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        INewsRepository NewsRepository { get; }
        void Commit();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private ICustomerRepository _customerRepository;
        private IProductRepository _productRepository;
        private ICategoryRepository _categoryRepository;
        private ICustomerInfoRepository _customerInfoRepository;
        private IOrderRepository _orderRepository;
        private IUtilityRepository _utilityRepository;
        private IEmployeeRepository _employeeRepository;
        private INewsRepository _newsRepository;
        private bool _disposed;

        public UnitOfWork(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public ICustomerRepository CustomerRepository
        {
            get { return _customerRepository ?? (_customerRepository = new CustomerRepository(_transaction)); }
        }

        public IProductRepository ProductRepository
        {
            get { return _productRepository ?? (_productRepository = new ProductRepository(_transaction)); }
        }

        public ICategoryRepository CategoryRepository
        {
            get { return _categoryRepository ?? (_categoryRepository = new CategoryRepository(_transaction)); }
        }

        public ICustomerInfoRepository CustomerInfoRepository
        {
            get { return _customerInfoRepository ?? (_customerInfoRepository = new CustomerInfoRepository(_transaction)); }
        }

        public IOrderRepository OrderRepository
        {
            get { return _orderRepository ?? (_orderRepository = new OrderRepository(_transaction)); }
        }

        public IUtilityRepository UtilityRepository
        {
            get { return _utilityRepository ?? (_utilityRepository = new UtilityRepository(_transaction)); }
        }

        public IEmployeeRepository EmployeeRepository
        {
            get { return _employeeRepository ?? (_employeeRepository = new EmployeeRepository(_transaction)); }
        }

        public INewsRepository NewsRepository
        {
            get { return _newsRepository ?? (_newsRepository = new NewsRepository(_transaction)); }
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                resetRepositories();
            }
        }

        private void resetRepositories()
        {
            _customerRepository = null;
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            dispose(false);
        }
    }
}
