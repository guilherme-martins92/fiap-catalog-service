using fiap_catalog_service.Models;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace fiap_catalog_service.Validators
{
    [ExcludeFromCodeCoverage]
    public class VehicleValidator : AbstractValidator<Vehicle>
    {
        public VehicleValidator()
        {
            RuleFor(vehicle => vehicle.Model)
                .NotEmpty()
                .WithMessage("O Modelo é obrigatório.")
                .Length(2, 100)
                .WithMessage("O Modelo deve ter entra 2 e 100 caracteres.");

            RuleFor(vehicle => vehicle.Brand)
                .NotEmpty()
                .WithMessage("A Marca é obrigatória.")
                .Length(2, 100)
                .WithMessage("A Marca deve ter entre 2 e 100 caracteres.");

            RuleFor(vehicle => vehicle.Color)
                .NotEmpty()
                .WithMessage("A Cor é obrigatória.")
                .Length(2, 50)
                .WithMessage("A Cor deve ter entre 2 e 50 caracteres.");

            RuleFor(vehicle => vehicle.Year)
                .InclusiveBetween(1886, DateTime.Now.Year)
                .WithMessage($"O Ano deve estar entre 1886 e {DateTime.Now.Year}.");

            RuleFor(vehicle => vehicle.Price)
                .GreaterThan(0)
                .WithMessage("O Preço deve ser maior que zero.");
        }
    }
}
