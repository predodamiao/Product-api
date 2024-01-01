using Domain.Models;
using FluentValidation;
using Infrastructure.Dtos;

namespace Service.Services.Validations
{
    /// <summary>
    /// Find Products Dto Validation with FluentValidation
    /// </summary>
    public class FindProductsDtoValidator : AbstractValidator<FindProductsDto>
    {
        /// <summary>
        /// Execute Validations on FindProductsDto
        /// </summary>
        public FindProductsDtoValidator()
        {
            RuleFor(product => product.NameToFind)
                .MaximumLength(100).WithMessage("NameToFind must be less than or equal to 100 characters");

            RuleFor(product => product.PropertyToOrderBy)
                .Must((propertyToOrderBy) => propertyToOrderBy == null || IsPropertyFromProduct(propertyToOrderBy));

            RuleFor(product => product.Pagination)
                .NotNull().WithMessage("Pagination is required");

            RuleFor(product => product.Pagination.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("PageNumber must be greater than or equal to 1");

            RuleFor(product => product.Pagination.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("PageSize must be greater than or equal to 1")
                .LessThanOrEqualTo(200).WithMessage("PageSize must be less than or equal to 200");
        }


        private static bool IsPropertyFromProduct(string? propertyName)
        {
            var productProperties = typeof(Product).GetProperties().Select(property => property.Name);
            return productProperties.Contains(propertyName);
        }   
    }
}
