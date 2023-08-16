using FluentValidation;
using WebApiStartup.DTOs;

namespace WebApiStartup.Validations
{
    public class CreateCategoryValidation : AbstractValidator<CreateCategoryDTO>
    {
        public CreateCategoryValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Category Name is required")
                .MinimumLength(6)
                .WithMessage("Minimum 6 charachters required");

            RuleFor(x => x.Description)
                .NotEmpty()
                .NotNull()
                .WithMessage("Description Name is required")
                .MinimumLength(6)
                .WithMessage("Minimum 6 charachters required");
        }
    }
}
