using Core.Entities;
using Core.Interfaces;

using Infrastructure.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;
        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            return Ok(await _repo.GetProductsAsync(brand, type, sort));
        }

        [HttpGet("{id:int}")]  //api/product/2
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _repo.GetProductByIdAsync(id);

            if (product == null) return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _repo.AddProduct(product);

            if (await _repo.SaveChangesAsync()) return BadRequest("Problem creating product");

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id  || !_repo.ProductExists(id)) return BadRequest("Cannot update this product");

            _repo.UpdateProduct(product);

            if (await _repo.SaveChangesAsync()) return BadRequest("Problem updating the product");

            return NoContent();

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _repo.GetProductByIdAsync(id);

            if (product == null) return NotFound();

            _repo.DeleteProduct(product);

            if (await _repo.SaveChangesAsync()) return BadRequest("Problem deleting the product");

            return NoContent();
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            return Ok(await _repo.GetBrandsAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            return Ok(await _repo.GetTypesAsync());
        }

    }
}
