using YemekTarifleri.Models;

namespace YemekTarifleri.Repositories
{
    /// <summary>
    /// Tarif repository için özel metodlar içeren interface
    /// </summary>
    public interface IRecipeRepository : IRepository<TarifModel>
    {
        /// <summary>
        /// Kategoriye göre tarifleri getirir
        /// </summary>
        /// <param name="kategori">Kategori adı</param>
        /// <returns>Kategoriye ait tarifler</returns>
        Task<IEnumerable<TarifModel>> GetByCategoryAsync(string kategori);

        /// <summary>
        /// Malzeme ismine göre tarifleri getirir
        /// </summary>
        /// <param name="malzeme">Malzeme adı</param>
        /// <returns>Belirtilen malzemeyi içeren tarifler</returns>
        Task<IEnumerable<TarifModel>> GetByIngredientAsync(string malzeme);

        /// <summary>
        /// Hazırlama süresine göre tarifleri getirir
        /// </summary>
        /// <param name="maxSure">Maksimum hazırlama süresi (dakika)</param>
        /// <returns>Belirtilen süreye eşit veya daha kısa süreli tarifler</returns>
        Task<IEnumerable<TarifModel>> GetByMaxPreparationTimeAsync(int maxSure);

        /// <summary>
        /// Zorluk seviyesine göre tarifleri getirir
        /// </summary>
        /// <param name="zorlukSeviyesi">Zorluk seviyesi</param>
        /// <returns>Belirtilen zorluk seviyesindeki tarifler</returns>
        Task<IEnumerable<TarifModel>> GetByDifficultyLevelAsync(string zorlukSeviyesi);
    }
}

