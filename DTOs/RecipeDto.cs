namespace YemekTarifleri.DTOs
{
    /// <summary>
    /// Tarif bilgilerini görüntüleme için kullanılan veri transfer nesnesi
    /// </summary>
    public class RecipeDto
    {
        /// <summary>
        /// Tarif ID'si
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tarif adı
        /// </summary>
        public string? Ad { get; set; }

        /// <summary>
        /// Tarif açıklaması
        /// </summary>
        public string? Aciklama { get; set; }

        /// <summary>
        /// Gerekli malzemeler listesi
        /// </summary>
        public string? Malzemeler { get; set; }

        /// <summary>
        /// Yapılış adımları
        /// </summary>
        public string? Yapilis { get; set; }

        /// <summary>
        /// Tarif kategorisi
        /// </summary>
        public string? Kategori { get; set; }

        /// <summary>
        /// Hazırlama süresi (dakika cinsinden)
        /// </summary>
        public int? HazirlamaSuresi { get; set; }

        /// <summary>
        /// Zorluk seviyesi (Kolay, Orta, Zor)
        /// </summary>
        public string? ZorlukSeviyesi { get; set; }

        /// <summary>
        /// Görsel dosya yolu (varsa)
        /// </summary>
        public string? ImagePath { get; set; }
    }
}

