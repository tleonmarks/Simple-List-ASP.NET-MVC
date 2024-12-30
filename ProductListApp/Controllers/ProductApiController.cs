 
using Microsoft.AspNetCore.Mvc;
using ProductListApp.Models;
using ProductListApp.Repository;
 

 namespace ProductListApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductsApiController(IProductRepository repository)
        {
            _repository = repository;
        }

        //GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _repository.GetAllAsync();
            return Ok(products); // Return 200 OK status code with the list of products
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound(); // Return 404 Not Found if the product is not found
            }

            return Ok(product); // Return 200 OK status code with the product details
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return 400 Bad Request if the model is invalid
            }

            await _repository.AddAsync(product);

            // Return 201 Created with a link to the newly created product
            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest(); // Return 400 Bad Request if the ID does not match
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return 400 Bad Request if the model is invalid
            }

            var existingProduct = await _repository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound(); // Return 404 Not Found if the product does not exist
            }

            await _repository.UpdateAsync(product);

            return NoContent(); // Return 204 No Content to indicate success without any content
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound(); 
            }

            await _repository.DeleteAsync(id);

            return NoContent(); // Return 204 No Content to indicate success
        }
    }
}
