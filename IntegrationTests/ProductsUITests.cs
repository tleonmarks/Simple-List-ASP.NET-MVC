using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace IntegrationTests
{
    [TestClass]
    public class ProductsUITests
    {
        private IWebDriver _driver;

        [TestInitialize]
        public void SetUp()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://localhost:5001"); // Your app URL
        }

        [TestMethod]
        public void CreateProduct_ValidData_AddsProductToList()
        {
            // Open the modal
            var createButton = _driver.FindElement(By.Id("createProductBtn"));
            createButton.Click();

            // Fill the form fields
            var nameField = _driver.FindElement(By.Id("productName"));
            var priceField = _driver.FindElement(By.Id("productPrice"));
            var submitButton = _driver.FindElement(By.Id("submitProductBtn"));

            nameField.SendKeys("Test Product");
            priceField.SendKeys("19.99");

            // Submit the form (AJAX request)
            submitButton.Click();

            // Wait for the AJAX request to complete and update the UI
            Thread.Sleep(2000);

            // Verify that the new product appears in the product list
            var productList = _driver.FindElement(By.Id("productList"));
            Assert.IsTrue(productList.Text.Contains("Test Product"));
        }

        [TestCleanup]
        public void TearDown()
        {
            _driver.Quit();
        }
    }
}
