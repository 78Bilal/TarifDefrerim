using YemekTarifleri.DTOs;

namespace YemekTarifleri.Services
{
    /// <summary>
    /// Tarif iş mantığı için service interface
    /// </summary>
    public interface IRecipeService
    {
        /// <summary>
        /// Tüm tarifleri getirir
        /// </summary>
        /// <returns>Tarif listesi</returns>
        Task<IEnumerable<RecipeDto>> GetAllRecipesAsync();

        /// <summary>
        /// ID'ye göre tarif getirir
        /// </summary>
        /// <param name="id">Tarif ID'si</param>
        /// <returns>Tarif bilgisi veya null</returns>
        Task<RecipeDto?> GetRecipeByIdAsync(int id);

        /// <summary>
        /// Filtrelenmiş tarifleri getirir
        /// </summary>
        /// <param name="filter">Filtre kriterleri</param>
        /// <returns>Filtrelenmiş tarif listesi</returns>
        Task<IEnumerable<RecipeDto>> GetFilteredRecipesAsync(RecipeFilterDto filter);

        /// <summary>
        /// Yeni tarif oluşturur
        /// </summary>
        /// <param name="dto">Tarif oluşturma DTO'su</param>
        /// <returns>Oluşturulan tarif</returns>
        Task<RecipeDto> CreateRecipeAsync(RecipeCreateDto dto);

        /// <summary>
        /// Mevcut tarifi günceller
        /// </summary>
        /// <param name="dto">Tarif güncelleme DTO'su</param>
        /// <returns>Güncellenen tarif</returns>
        Task<RecipeDto?> UpdateRecipeAsync(RecipeUpdateDto dto);

        /// <summary>
        /// Tarifi siler
        /// </summary>
        /// <param name="id">Silinecek tarif ID'si</param>
        /// <returns>Silme işlemi başarılı mı?</returns>
        Task<bool> DeleteRecipeAsync(int id);

        /// <summary>
        /// Tüm kategorileri getirir
        /// </summary>
        /// <returns>Kategori listesi</returns>
        Task<IEnumerable<string>> GetCategoriesAsync();

        /// <summary>
        /// Tarif görsel yolunu günceller
        /// </summary>
        /// <param name="recipeId">Tarif ID'si</param>
        /// <param name="imagePath">Görsel dosya yolu</param>
        /// <returns>Güncelleme başarılı mı?</returns>
        Task<bool> UpdateRecipeImagePathAsync(int recipeId, string imagePath);
    }
}

