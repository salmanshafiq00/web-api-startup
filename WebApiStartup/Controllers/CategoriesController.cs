using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiStartup.DTOs;
using WebApiStartup.Entity;
using WebApiStartup.Interfaces;
using WebApiStartup.Validations;

namespace WebApiStartup.Controllers
{
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository repository, IMapper mapper)
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

        [HttpPost("CreateCategory")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDTO createOrUpdateCategoryDTO)
        {
            //var validator = new CreateCategoryValidation();
            //var validationResult = validator.Validate(createOrUpdateCategoryDTO);
            
            //if(!validationResult.IsValid)
            //{
            //    foreach(var error  in validationResult.Errors)
            //    {
            //        ModelState.AddModelError("", error.ErrorMessage);
            //    }
            //    return BadRequest(ModelState);
            //}
            var category = await _repository.Create(_mapper.Map<Category>(createOrUpdateCategoryDTO));
            if (category is null)
                return NotFound();
            return CreatedAtAction(nameof(GetById), new {category.Id}, createOrUpdateCategoryDTO);
        }

        [HttpPut("UpdateCategory")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryDTO updateCategoryDTO)
        {
            var category = await _repository.Update(_mapper.Map<Category>(updateCategoryDTO));
            if (category is null)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            var category = await _repository.Delete(id);
            if (category is null)
                return NotFound();
            return NoContent(); 
        }
    }
}
