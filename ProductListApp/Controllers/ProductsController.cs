using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProductListApp.Models;
using ProductListApp.Repository;

public class ProductsController : Controller
{
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    // GET: Products
    public async Task<IActionResult> Index()
    {
        var products = await _repository.GetAllAsync();
        return View(products);
    }

    // GET: Products/GetProducts
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _repository.GetAllAsync();
        return Json(products);
    }

    // POST: Products/AddProduct
    [HttpPost]
    public async Task<IActionResult> AddProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            await _repository.AddAsync(product);
            return Json(new { success = true });
        }

        return Json(new { success = false, message = "Validation failed." });
    }

    // POST: Products/UpdateProduct
    [HttpPost]
    public async Task<IActionResult> UpdateProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            await _repository.UpdateAsync(product);
            return Json(new { success = true });
        }

        return Json(new { success = false, message = "Validation failed." });
    }

    // POST: Products/DeleteProduct
    [HttpPost]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _repository.DeleteAsync(id);
        return Json(new { success = true });
    }
}
