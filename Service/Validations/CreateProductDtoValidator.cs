using FluentValidation;
using Service.Dtos;

namespace Service.Services.Validations
{
    public class CreateProductDtoValidator: AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(product => product.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must be less than or equal to 100 characters");

            RuleFor(product => product.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(500).WithMessage("Description must be less than or equal to 500 characters");

            RuleFor(product => product.AvailableQuantity)
                .NotEmpty().WithMessage("AvailableQuantity is required")
                .GreaterThanOrEqualTo(0).WithMessage("AvailableQuantity must be greater than or equal to 0");

            RuleFor(product => product.Price)
                .NotEmpty().WithMessage("Price is required")
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0");
        }   
    }
}
