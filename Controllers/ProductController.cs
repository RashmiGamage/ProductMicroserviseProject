using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductMicroserviseProject.Models;
using ProductMicroserviseProject.Repository;
using System.Transactions;

namespace ProductMicroserviseProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }



        // GET:api/Product
        [HttpGet]
        public IActionResult Get()
        {
            var products = _productRepository.GetProducts();
            return new OkObjectResult(products);
        }

        // GET:api/Product/5
        [HttpGet("{id}",Name = "Get")]
        public IActionResult Get(int id)
        {
            var products = _productRepository.GetProductById(id);
            return new OkObjectResult(products);
        }

        //POST:api/Product
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {

            using (var scope = new TransactionScope())
            {
                _productRepository.InsertProduct(product);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new {id = product.Id },product);
            }
        }

        //PUT:api/Product
        [HttpPut]
        public IActionResult Put([FromBody] Product product)
        {
            if (product != null)
            {
                using (var scope = new TransactionScope())
                {
                    _productRepository.UpdateProduct(product);
                    scope.Complete();
                    return new OkResult();
                }
            }
            return new NoContentResult();

        }

        // DELETE:api/ApiWithActions/5
        [HttpDelete("{id}")]

        public IActionResult Delete(int id)
        {
            _productRepository.DeleteProduct(id);
            return new OkResult();
        }

       
    }
}
