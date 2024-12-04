using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

using WebAPI.RequestHelpers;

namespace WebAPI.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _repo;
        public ProductsController(IGenericRepository<Product> repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery]ProductSpecParams specParams)
        {
            var spec = new ProductSpecification(specParams);

            return await CreatePagedResult(_repo, spec, specParams.PageIndex, specParams.PageSize);
        }

        [HttpGet("{id:int}")]  //api/product/2
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _repo.GetByIdAsync(id);

            if (product == null) return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _repo.Add(product);

            if (await _repo.SaveAllAsync() == false) return BadRequest("Problem creating product");

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id  || !_repo.Exists(id)) return BadRequest("Cannot update this product");

            _repo.Update(product);

            if (await _repo.SaveAllAsync() == false) return BadRequest("Problem updating the product");

            return NoContent();

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _repo.GetByIdAsync(id);

            if (product == null) return NotFound();

            _repo.Remove(product);

            if (await _repo.SaveAllAsync() == false) return BadRequest("Problem deleting the product");

            return NoContent();
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var spec = new BrandListSpecification();
            return Ok(await _repo.ListAsync(spec));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new TypeListSpecification();
            return Ok(await _repo.ListAsync(spec));
        }

    }
}
