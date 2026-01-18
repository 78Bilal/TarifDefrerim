using Microsoft.EntityFrameworkCore;
using YemekTarifleri.Models;

namespace YemekTarifleri.Repositories
{
    /// <summary>
    /// Tarif repository implementasyonu - özel sorgular ve iş mantığı
    /// </summary>
    public class RecipeRepository : Repository<TarifModel>, IRecipeRepository
    {
        /// <summary>
        /// RecipeRepository constructor
        /// </summary>
        /// <param name="context">Entity Framework DbContext</param>
        public RecipeRepository(AppDbContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TarifModel>> GetByCategoryAsync(string kategori)
        {
            return await _dbSet
                .Where(t => t.Kategori == kategori)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TarifModel>> GetByIngredientAsync(string malzeme)
        {
            return await _dbSet
                .Where(t => t.Malzemeler != null && t.Malzemeler.Contains(malzeme, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TarifModel>> GetByMaxPreparationTimeAsync(int maxSure)
        {
            return await _dbSet
                .Where(t => t.HazirlamaSuresi.HasValue && t.HazirlamaSuresi <= maxSure)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TarifModel>> GetByDifficultyLevelAsync(string zorlukSeviyesi)
        {
            return await _dbSet
                .Where(t => t.ZorlukSeviyesi == zorlukSeviyesi)
                .ToListAsync();
        }
    }
}

