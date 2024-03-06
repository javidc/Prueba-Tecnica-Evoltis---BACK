using Evoltis.Models.Dtos.ClubDtos;
using FluentValidation;

namespace Evoltis.Models.Validator
{
    public class ClubCreateDtoValidator : AbstractValidator<ClubCreateDto>
    {
        public ClubCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.CUIT).NotEmpty().Length(11);
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.StadiumName).NotEmpty();
            RuleFor(x => x.IdTournament).NotEmpty().GreaterThan(0);
            RuleFor(x => x.ImageLogo)
                .Must(BeAValidImage)
                .WithMessage("La imagen debe estar en formato .jpg o .png y pesar menos de 2MB");
        }

        private bool BeAValidImage(IFormFile file)
        {
            if (file == null)
            {
                return true;
            }

            string extension = Path.GetExtension(file.FileName).ToLower();
            long weight = file.Length;

            return (extension == ".jpg" || extension == ".png") && weight <= (2 * 1024 * 1024);
        }
    }
}
