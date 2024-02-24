using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeerController : ControllerBase
    {
        private IValidator<BeerInsertDTO> _beerInsertValidator;
        private IValidator<BeerUpdateDTO> _beerUpdateValidator;
        private ICommonService<BeerDTO, BeerInsertDTO, BeerUpdateDTO> _beerService;

        public BeerController(
            IValidator<BeerInsertDTO> beerInsertValidator,
            IValidator<BeerUpdateDTO> beerUpdateValidator,
            [FromKeyedServices("beerService")] ICommonService<BeerDTO, BeerInsertDTO, BeerUpdateDTO> beerService
            ) 
        {
            _beerInsertValidator = beerInsertValidator;
            _beerUpdateValidator = beerUpdateValidator;
            _beerService = beerService;
        }

        [HttpGet]
        public async Task<IEnumerable<BeerDTO>> Get() => await _beerService.Get();

        [HttpGet("{id}")]
        public async Task<ActionResult<BeerDTO>>  GetById(int id)
        {
             var beerDto = await _beerService.GetById(id);
            return beerDto == null ? NotFound() : Ok(beerDto);
        }

        [HttpPost]
        public async Task <ActionResult<BeerDTO>> Add(BeerInsertDTO beerInserDto)
        {
            var validationResult = await _beerInsertValidator.ValidateAsync(beerInserDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_beerService.Validate(beerInserDto))
            {
                return BadRequest(_beerService.Errors);
            }

            var beerDto = await _beerService.Add(beerInserDto);
            return CreatedAtAction(nameof(GetById), new {id = beerDto.Id}, beerDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BeerDTO>> Update(int id, BeerUpdateDTO beerUpdateDto)
        {
            var validationResult = await _beerUpdateValidator.ValidateAsync(beerUpdateDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_beerService.Validate(beerUpdateDto))
            {
                return BadRequest(_beerService.Errors);
            }

            var beerDto = await  _beerService.Update(id, beerUpdateDto);

            return beerDto == null ? NotFound() : Ok(beerDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BeerDTO>> Delete(int id)
        {
            var beerDto =  await _beerService.Delete(id);
            return beerDto == null ? NotFound() : Ok(beerDto);
        }
    }
}
