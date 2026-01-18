using Microsoft.AspNetCore.Mvc;
using YemekTarifleri.DTOs;
using YemekTarifleri.Services;

namespace YemekTarifleri.Controllers
{
    /// <summary>
    /// Tarif yönetimi için controller - CRUD işlemleri ve filtreleme
    /// </summary>
    public class TarifController : Controller
    {
        private readonly IRecipeService _recipeService;
        private readonly IImageService _imageService;

        /// <summary>
        /// TarifController constructor
        /// </summary>
        /// <param name="recipeService">Tarif iş mantığı servisi</param>
        /// <param name="imageService">Görsel yönetim servisi</param>
        public TarifController(IRecipeService recipeService, IImageService imageService)
        {
            _recipeService = recipeService;
            _imageService = imageService;
        }

        /// <summary>
        /// Ana sayfa - tüm tarifleri listeler
        /// </summary>
        /// <param name="kategori">Kategori filtresi (opsiyonel)</param>
        /// <returns>Tarif listesi view'ı</returns>
        public async Task<IActionResult> Index(string? kategori)
        {
            var filter = new RecipeFilterDto { Kategori = kategori };
            var tarifler = await _recipeService.GetFilteredRecipesAsync(filter);
            
            ViewBag.SeciliKategori = kategori;
            ViewBag.Kategoriler = await _recipeService.GetCategoriesAsync();
            
            return View(tarifler);
        }

        /// <summary>
        /// Tarif detay sayfası
        /// </summary>
        /// <param name="id">Tarif ID'si</param>
        /// <returns>Tarif detay view'ı</returns>
        public async Task<IActionResult> Details(int id)
        {
            var tarif = await _recipeService.GetRecipeByIdAsync(id);
            if (tarif == null)
                return NotFound();

            return View(tarif);
        }

        /// <summary>
        /// AJAX ile filtreleme endpoint'i
        /// </summary>
        /// <param name="filter">Filtre kriterleri</param>
        /// <returns>JSON formatında filtrelenmiş tarif listesi</returns>
        [HttpPost]
        public async Task<IActionResult> Filter([FromBody] RecipeFilterDto filter)
        {
            var tarifler = await _recipeService.GetFilteredRecipesAsync(filter);
            return Json(tarifler);
        }
    }
}
