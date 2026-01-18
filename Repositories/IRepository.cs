using System.Linq.Expressions;

namespace YemekTarifleri.Repositories
{
    /// <summary>
    /// Generic repository interface - tüm repository'ler için temel sözleşme
    /// </summary>
    /// <typeparam name="T">Entity tipi</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Tüm kayıtları getirir
        /// </summary>
        /// <returns>Kayıt listesi</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Belirli bir koşula göre kayıtları getirir
        /// </summary>
        /// <param name="predicate">Filtreleme koşulu</param>
        /// <returns>Filtrelenmiş kayıt listesi</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// ID'ye göre tek bir kayıt getirir
        /// </summary>
        /// <param name="id">Kayıt ID'si</param>
        /// <returns>Bulunan kayıt veya null</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Yeni bir kayıt ekler
        /// </summary>
        /// <param name="entity">Eklenecek entity</param>
        /// <returns>Eklenen entity</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Mevcut bir kaydı günceller
        /// </summary>
        /// <param name="entity">Güncellenecek entity</param>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Bir kaydı siler
        /// </summary>
        /// <param name="entity">Silinecek entity</param>
        Task DeleteAsync(T entity);

        /// <summary>
        /// ID'ye göre bir kaydı siler
        /// </summary>
        /// <param name="id">Silinecek kayıt ID'si</param>
        Task DeleteAsync(int id);

        /// <summary>
        /// Değişiklikleri veritabanına kaydeder
        /// </summary>
        Task SaveChangesAsync();
    }
}

