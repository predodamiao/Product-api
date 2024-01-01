using FluentValidation;
using Service.Dtos;

namespace Service.Services.Validations
{
    public class UpdateProductDtoValidator: AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(product => product.Name)
                .MaximumLength(100).WithMessage("Name must be less than or equal to 100 characters");

            RuleFor(product => product.Description)
                .MaximumLength(500).WithMessage("Description must be less than or equal to 500 characters");

            RuleFor(product => product.AvailableQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("AvailableQuantity must be greater than or equal to 0");

            RuleFor(product => product.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0");
        }   
    }
}
