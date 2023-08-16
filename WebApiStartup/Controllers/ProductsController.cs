using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiStartup.DTOs;
using WebApiStartup.Entity;
using WebApiStartup.Interfaces;

namespace WebApiStartup.Controllers
{
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repository.GetAll());
        }

        [HttpGet]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllNew()
        {
            return Ok(await _repository.GetAll());
        }


        [HttpGet("{id:int}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await _repository.GetAll());
        }

        [HttpPost("CreateProduct")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO createOrUpdateProductDTO)
        {
            var product = await _repository.Create(_mapper.Map<Product>(createOrUpdateProductDTO));
            if (product is null)
                return NotFound();
            return CreatedAtAction(nameof(GetById), new {product.Id}, createOrUpdateProductDTO);
        }

        [HttpPut("UpdateProduct")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDTO updateProductDTO)
        {
            var product = await _repository.Update(_mapper.Map<Product>(updateProductDTO));
            if (product is null)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            var product = await _repository.Delete(id);
            if (product is null)
                return NotFound();
            return NoContent(); 
        }
    }
}
