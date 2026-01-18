namespace YemekTarifleri.Services
{
    /// <summary>
    /// Görsel yönetimi için service interface
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// Tarif görselini kaydeder
        /// </summary>
        /// <param name="recipeId">Tarif ID'si</param>
        /// <param name="imageFile">Yüklenecek görsel dosyası</param>
        /// <returns>Kaydedilen görsel yolu</returns>
        Task<string?> SaveImageAsync(int recipeId, IFormFile? imageFile);

        /// <summary>
        /// Tarif görselini getirir
        /// </summary>
        /// <param name="recipeId">Tarif ID'si</param>
        /// <returns>Görsel dosya yolu veya null</returns>
        Task<string?> GetImagePathAsync(int recipeId);

        /// <summary>
        /// Tarif görselini siler
        /// </summary>
        /// <param name="recipeId">Tarif ID'si</param>
        Task DeleteImageAsync(int recipeId);

        /// <summary>
        /// Belirtilen görsel yolundaki dosyayı siler
        /// </summary>
        /// <param name="imagePath">Silinecek görsel dosya yolu</param>
        Task DeleteImageByPathAsync(string? imagePath);
    }
}

