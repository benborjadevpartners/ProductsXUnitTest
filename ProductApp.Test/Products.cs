using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Xunit;

namespace ProductsApp.Tests
{

    public class ProductsAppShould
    {
        [Fact]
        public void WhenProductNull_ShouldReturnException()
        {
            // Arrange
            var products = new Products();
            Product product = null;
            
            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => products.AddNew(product));
            
        }

        [Fact]
        public void WhenProductHasValue_ShouldReturnProductInList()
        {
            // Arrange
            var products = new Products();
            Product product = new Product
            {
                IsSold = false,
                Name = "Hammer"
            };

            // Act
            products.AddNew(product);
            var result = products.Items;

            // Assert
            Assert.NotEmpty(result); 
            Assert.Single(result.Where(x => x.Name  == product.Name));
            Assert.All(result.Select(x => x.Name), Name => Assert.True(Name == product.Name));
        }

        [Fact]
        public void WhenProductNameNotValid_ShouldReturnValidationError()
        {
            // Arrange
            var products = new Products();
            Product product = new Product
            {
                IsSold = false                
            };            

            // Act Assert
            Assert.Throws<NameRequiredException>(() => products.AddNew(product));
        }
    }

    internal class Products
    {
        private readonly List<Product> _products = new List<Product>();

        public IEnumerable<Product> Items => _products.Where(t => !t.IsSold);

        public void AddNew(Product product)
        {
            product = product ??
                throw new ArgumentNullException();
            product.Validate();
            _products.Add(product);
        }

        public void Sold(Product product)
        {
            product.IsSold = true;
        }

    }

    internal class Product
    {
        public bool IsSold { get; set; }
        public string Name { get; set; }

        internal void Validate()
        {
            Name = Name ??
                throw new NameRequiredException();
        }

    }

    [Serializable]
    internal class NameRequiredException : Exception
    {
        public NameRequiredException() { /* ... */ }

        public NameRequiredException(string message) : base(message) { /* ... */ }

        public NameRequiredException(string message, Exception innerException) : base(message, innerException) { /* ... */ }

        protected NameRequiredException(SerializationInfo info, StreamingContext context) : base(info, context) { /* ... */ }
    }
}