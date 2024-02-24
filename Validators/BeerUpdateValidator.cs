using FluentValidation;
using WebApplication1.DTOs;

namespace WebApplication1.Validators
{
    public class BeerUpdateValidator : AbstractValidator<BeerUpdateDTO>
    {
        public BeerUpdateValidator() {
            RuleFor(x => x.Id).NotNull().WithMessage("Id is required");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Name).Length(2, 20).WithMessage("the name must be a length of 2 to 20 characters");
            RuleFor(x => x.BrandID).NotNull().WithMessage(x => "the brand is required");
            RuleFor(x => x.BrandID).GreaterThan(0).WithMessage(x => "invalid value");
            RuleFor(x => x.Alcohol).GreaterThan(0).WithMessage(x => "the {PropertyName} is must be greater than 0");
        }
    }
}
