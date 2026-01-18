using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YemekTarifleri.Models
{
    /// <summary>
    /// Tarif entity modeli - veritabanı tablosu ile eşleşir
    /// </summary>
    public class TarifModel
    {
        /// <summary>
        /// Tarif benzersiz ID'si
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
        /// Tarif görsel dosya yolu (wwwroot/images/recipes klasöründen)
        /// </summary>
        public string? ImagePath { get; set; }
    }
}