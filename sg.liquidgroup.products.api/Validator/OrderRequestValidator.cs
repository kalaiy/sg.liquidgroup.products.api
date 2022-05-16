using FluentValidation;
using Sg.LiquidGroup.Products.Domain.Entity;

namespace Sg.LiquidGroup.Products.Api.Validator
{
    public class OrderRequestValidator : AbstractValidator<OrderRequest>
    {
        public OrderRequestValidator()
        {
            RuleFor(r => r.CartId)
                .NotEmpty()
                .WithMessage("Please enter cart Id")
                .NotNull()
                .WithMessage("Please enter cart Id");

            RuleFor(r => r.Status)
                .NotNull()
                .WithMessage("Please choose order status");
                
        }
    }
}
