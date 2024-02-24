using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;

namespace WebApplication1.Services
{
    public interface ICommonService<T, TI, TU>
    {
        public List<string> Errors { get; }
        Task<IEnumerable<T>> Get();
        Task<T> GetById(int id);
        Task<T> Add(TI beerInserDto);
        Task<T> Update(int id, TU beerUpdateDto);
        Task<T> Delete(int id);
        bool Validate(TI dto);
        bool Validate(TU dto);
    }
}
