namespace YemekTarifleri.DTOs
{
    /// <summary>
    /// Tarif filtreleme için kullanılan veri transfer nesnesi
    /// </summary>
    public class RecipeFilterDto
    {
        /// <summary>
        /// Kategoriye göre filtreleme
        /// </summary>
        public string? Kategori { get; set; }

        /// <summary>
        /// Malzeme ismine göre filtreleme
        /// </summary>
        public string? Malzeme { get; set; }

        /// <summary>
        /// Maksimum hazırlama süresi (dakika)
        /// </summary>
        public int? MaxHazirlamaSuresi { get; set; }

        /// <summary>
        /// Zorluk seviyesine göre filtreleme
        /// </summary>
        public string? ZorlukSeviyesi { get; set; }
    }
}

