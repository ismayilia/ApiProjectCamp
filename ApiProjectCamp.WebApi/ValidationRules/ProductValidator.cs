using ApiProjectCamp.WebApi.Entities;
using FluentValidation;

namespace ApiProjectCamp.WebApi.ValidationRules
{
	public class ProductValidator : AbstractValidator<Product>
	{
		public ProductValidator()
		{
			RuleFor(x => x.ProductName).NotEmpty().WithMessage("Product name don`t be empty " +
															  "Please add Product name!");
			RuleFor(x => x.ProductName).MinimumLength(2).WithMessage("Enter Min length 2 chracter!");
			RuleFor(x => x.ProductName).MaximumLength(50).WithMessage("Enter Max length 50 chracter!");

			RuleFor(x => x.Price).NotEmpty().WithMessage("Don`t be empty product price")
											.GreaterThan(0).WithMessage("Don`t be negative product price")
											.LessThan(1000).WithMessage("Product price max 1000!");

			RuleFor(x => x.ProductDescription).NotEmpty().WithMessage("Don`t be empty product desc");
		}
	}
}
