using Domain.Models;
using FluentValidation;
using Infrastructure.Dtos;

namespace Service.Services.Validations
{
    public class FindProductsDtoValidator : AbstractValidator<FindProductsDto>
    {
        public FindProductsDtoValidator()
        {
            RuleFor(product => product.NameToFind)
                .MaximumLength(100).WithMessage("NameToFind must be less than or equal to 100 characters");

            RuleFor(product => product.PropertyToOrderBy)
                .Must((propertyToOrderBy) => propertyToOrderBy == null || isPropertyFromProduct(propertyToOrderBy));

            RuleFor(product => product.Pagination)
                .NotNull().WithMessage("Pagination is required");

            RuleFor(product => product.Pagination.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("PageNumber must be greater than or equal to 1");

            RuleFor(product => product.Pagination.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("PageSize must be greater than or equal to 1")
                .LessThanOrEqualTo(200).WithMessage("PageSize must be less than or equal to 200");
        }


        private bool isPropertyFromProduct(string? propertyName)
        {
            var productProperties = typeof(Product).GetProperties().Select(property => property.Name);
            return productProperties.Contains(propertyName);
        }   
    }
}
