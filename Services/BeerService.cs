using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Services
{
    public class BeerService : ICommonService<BeerDTO, BeerInsertDTO, BeerUpdateDTO>
    {

        private IRepository<Beer> _beerRepository;
        private IMapper _mapper;

        public List<string> Errors { get; }

        public BeerService(IRepository<Beer> beerRepository,
            IMapper mapper)
        {
            _beerRepository = beerRepository;
            _mapper = mapper;
            Errors = new List<string>();
        }

        public async Task<IEnumerable<BeerDTO>> Get()
        {
            var beers = await _beerRepository.Get();

            return beers.Select(b => _mapper.Map<BeerDTO>(b));
        }

        public async Task<BeerDTO> GetById(int id)
        {
            var beer = await _beerRepository.GetById(id);

            if (beer != null)
            {
                var beerDto = _mapper.Map<BeerDTO>(beer);

                return beerDto;  
            }

            return null;
        }

        public async Task<BeerDTO> Add(BeerInsertDTO beerInserDto)
        {
            var beer = _mapper.Map<Beer>(beerInserDto);

            await _beerRepository.Add(beer);
            await _beerRepository.Save();

            var beerDto = _mapper.Map<BeerDTO>(beer);

            return beerDto;
        }

        public async Task<BeerDTO> Update(int id, BeerUpdateDTO beerUpdateDto)
        {
            var beer = await _beerRepository.GetById(id);

            if (beer != null)
            {
                beer = _mapper.Map<BeerUpdateDTO, Beer>(beerUpdateDto, beer);

                 _beerRepository.Update(beer);
                await _beerRepository.Save();

                var beerDto = _mapper.Map<BeerDTO>(beer);

                return beerDto;
            }

            return null;
        }

        public async Task<BeerDTO> Delete(int id)
        {
            var beer = await _beerRepository.GetById(id);

            if (beer != null)
            {
                var beerDto = _mapper.Map<BeerDTO>(beer);

                _beerRepository.Delete(beer);
                await _beerRepository.Save();

                return beerDto;
            }

            return null;
        }

        public bool Validate(BeerInsertDTO beerInsertDto)
        {
          if(_beerRepository.Search(b => b.Name == beerInsertDto.Name).Count() > 0)
            {
                Errors.Add("the name is in use");
                return false;
            }

           return true;
        }

        public bool Validate(BeerUpdateDTO beerUpdateDto)
        {
            if (_beerRepository.Search(b => b.Name == beerUpdateDto.Name && beerUpdateDto.Id != b.BeerId).Count() > 0)
            {
                Errors.Add("the name is in use");
                return false;
            }

            return true;
        }
    }
}
