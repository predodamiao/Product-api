using FluentValidation;
using Infrastructure.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Service.Dtos;
using Service.Services;
using Service.Services.Interfaces;
using Service.Services.Validations;

namespace Service.IoC
{
    public static class ServicesIoC
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateProductDto>, CreateProductDtoValidator>();
            services.AddScoped<IValidator<UpdateProductDto>, UpdateProductDtoValidator>();
            services.AddScoped<IValidator<FindProductsDto>, FindProductsDtoValidator>();

            services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}
