using FluentValidation;
using YemekTarifleri.DTOs;

namespace YemekTarifleri.Validators
{
    /// <summary>
    /// RecipeCreateDto için FluentValidation validator
    /// </summary>
    public class RecipeCreateDtoValidator : AbstractValidator<RecipeCreateDto>
    {
        /// <summary>
        /// RecipeCreateDtoValidator constructor - validasyon kurallarını tanımlar
        /// </summary>
        public RecipeCreateDtoValidator()
        {
            RuleFor(x => x.Ad)
                .NotEmpty().WithMessage("Tarif adı gereklidir.")
                .MaximumLength(200).WithMessage("Tarif adı en fazla 200 karakter olabilir.");

            RuleFor(x => x.Malzemeler)
                .NotEmpty().WithMessage("Malzemeler gereklidir.")
                .MinimumLength(10).WithMessage("Malzemeler en az 10 karakter olmalıdır.");

            RuleFor(x => x.Yapilis)
                .NotEmpty().WithMessage("Yapılış adımları gereklidir.")
                .MinimumLength(20).WithMessage("Yapılış adımları en az 20 karakter olmalıdır.");

            RuleFor(x => x.Kategori)
                .NotEmpty().WithMessage("Kategori seçimi gereklidir.")
                .Must(k => new[] { "Tatlı", "Ana Yemek", "Çorba", "Salata", "Kahvaltılık" }.Contains(k))
                .WithMessage("Geçerli bir kategori seçiniz.");

            RuleFor(x => x.HazirlamaSuresi)
                .GreaterThan(0).WithMessage("Hazırlama süresi 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(1440).WithMessage("Hazırlama süresi 24 saatten (1440 dakika) fazla olamaz.")
                .When(x => x.HazirlamaSuresi.HasValue);

            RuleFor(x => x.ZorlukSeviyesi)
                .Must(z => z == null || new[] { "Kolay", "Orta", "Zor" }.Contains(z))
                .WithMessage("Zorluk seviyesi 'Kolay', 'Orta' veya 'Zor' olmalıdır.");

            RuleFor(x => x.Aciklama)
                .MaximumLength(1000).WithMessage("Açıklama en fazla 1000 karakter olabilir.")
                .When(x => !string.IsNullOrWhiteSpace(x.Aciklama));
        }
    }
}

