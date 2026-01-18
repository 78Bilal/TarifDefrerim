using Microsoft.AspNetCore.Mvc;
using YemekTarifleri.DTOs;
using YemekTarifleri.Services;

namespace YemekTarifleri.Controllers
{
    /// <summary>
    /// Admin paneli - Tarif yönetimi (CRUD işlemleri)
    /// Şifre koruması ile erişim kontrol edilir
    /// </summary>
    public class AdminController : Controller
    {
        private readonly IRecipeService _recipeService;
        private readonly IImageService _imageService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// AdminController constructor
        /// </summary>
        /// <param name="recipeService">Tarif iş mantığı servisi</param>
        /// <param name="imageService">Görsel yönetim servisi</param>
        /// <param name="configuration">Uygulama yapılandırması</param>
        public AdminController(IRecipeService recipeService, IImageService imageService, IConfiguration configuration)
        {
            _recipeService = recipeService;
            _imageService = imageService;
            _configuration = configuration;
        }

        /// <summary>
        /// Admin oturumunun açık olup olmadığını kontrol eder
        /// </summary>
        private bool IsAdminLoggedIn()
        {
            return HttpContext.Session.GetString("AdminLoggedIn") == "true";
        }

        /// <summary>
        /// Admin giriş formu
        /// GET: /Admin/Login
        /// </summary>
        public IActionResult Login()
        {
            // Zaten giriş yapmışsa, doğrudan Recipes sayfasına yönlendir
            if (IsAdminLoggedIn())
                return RedirectToAction(nameof(Recipes));

            return View();
        }

        /// <summary>
        /// Admin şifre kontrolü ve oturum açma
        /// POST: /Admin/Login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string password)
        {
            // Yapılandırmadan admin şifresini al
            var adminPassword = _configuration["AdminPanel:Password"] ?? "admin123";

            // Şifre doğru mu kontrol et
            if (!string.IsNullOrEmpty(password) && password == adminPassword)
            {
                // Session'a giriş bilgisini kaydet
                HttpContext.Session.SetString("AdminLoggedIn", "true");
                return RedirectToAction(nameof(Recipes));
            }

            // Hatalı şifre
            ViewBag.Error = "Şifre yanlış! Lütfen tekrar deneyiniz.";
            return View();
        }

        /// <summary>
        /// Admin oturumundan çıkış
        /// GET: /Admin/Logout
        /// </summary>
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminLoggedIn");
            return RedirectToAction(nameof(Login));
        }

        /// <summary>
        /// Admin paneli ana sayfası - Tüm tarifleri listeler
        /// GET: /Admin/Recipes
        /// </summary>
        /// <returns>Tarif yönetimi sayfası</returns>
        public async Task<IActionResult> Recipes()
        {
            // Şifre kontrolü
            if (!IsAdminLoggedIn())
                return RedirectToAction(nameof(Login));

            var tarifler = await _recipeService.GetAllRecipesAsync();
            return View(tarifler);
        }

        /// <summary>
        /// Yeni tarif oluşturma formu
        /// GET: /Admin/CreateRecipe
        /// </summary>
        /// <returns>Tarif oluşturma formu</returns>
        public IActionResult CreateRecipe()
        {
            // Şifre kontrolü
            if (!IsAdminLoggedIn())
                return RedirectToAction(nameof(Login));

            ViewBag.Kategoriler = new[] { "Tatlı", "Ana Yemek", "Çorba", "Salata", "Kahvaltılık" };
            ViewBag.ZorlukSeviyeleri = new[] { "Kolay", "Orta", "Zor" };
            return View();
        }

        /// <summary>
        /// Yeni tarif oluşturma işlemi
        /// POST: /Admin/CreateRecipe
        /// </summary>
        /// <param name="dto">Tarif oluşturma DTO'su</param>
        /// <param name="imageFile">Yüklenecek görsel dosyası (opsiyonel)</param>
        /// <returns>Başarılı ise Recipes sayfasına yönlendirme, hata varsa form view'ı</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRecipe(RecipeCreateDto dto, IFormFile? imageFile)
        {
            // Şifre kontrolü
            if (!IsAdminLoggedIn())
                return RedirectToAction(nameof(Login));

            if (!ModelState.IsValid)
            {
                ViewBag.Kategoriler = new[] { "Tatlı", "Ana Yemek", "Çorba", "Salata", "Kahvaltılık" };
                ViewBag.ZorlukSeviyeleri = new[] { "Kolay", "Orta", "Zor" };
                return View(dto);
            }

            try
            {
                var recipe = await _recipeService.CreateRecipeAsync(dto);

                // Görsel yükleme (varsa)
                if (imageFile != null && imageFile.Length > 0)
                {
                    var imagePath = await _imageService.SaveImageAsync(recipe.Id, imageFile);
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        await _recipeService.UpdateRecipeImagePathAsync(recipe.Id, imagePath);
                    }
                }

                return RedirectToAction(nameof(Recipes));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Hata: {ex.Message}");
                ViewBag.Kategoriler = new[] { "Tatlı", "Ana Yemek", "Çorba", "Salata", "Kahvaltılık" };
                ViewBag.ZorlukSeviyeleri = new[] { "Kolay", "Orta", "Zor" };
                return View(dto);
            }
        }

        /// <summary>
        /// Tarif düzenleme formu
        /// GET: /Admin/EditRecipe/{id}
        /// </summary>
        /// <param name="id">Düzenlenecek tarif ID'si</param>
        /// <returns>Tarif düzenleme formu</returns>
        public async Task<IActionResult> EditRecipe(int id)
        {
            // Şifre kontrolü
            if (!IsAdminLoggedIn())
                return RedirectToAction(nameof(Login));

            var tarif = await _recipeService.GetRecipeByIdAsync(id);
            if (tarif == null)
                return NotFound();

            var updateDto = new RecipeUpdateDto
            {
                Id = tarif.Id,
                Ad = tarif.Ad,
                Aciklama = tarif.Aciklama,
                Malzemeler = tarif.Malzemeler,
                Yapilis = tarif.Yapilis,
                Kategori = tarif.Kategori,
                HazirlamaSuresi = tarif.HazirlamaSuresi,
                ZorlukSeviyesi = tarif.ZorlukSeviyesi,
                ImagePath = tarif.ImagePath
            };

            ViewBag.Kategoriler = new[] { "Tatlı", "Ana Yemek", "Çorba", "Salata", "Kahvaltılık" };
            ViewBag.ZorlukSeviyeleri = new[] { "Kolay", "Orta", "Zor" };
            return View(updateDto);
        }

        /// <summary>
        /// Tarif düzenleme işlemi
        /// POST: /Admin/EditRecipe/{id}
        /// </summary>
        /// <param name="id">Düzenlenecek tarif ID'si</param>
        /// <param name="dto">Tarif güncelleme DTO'su</param>
        /// <param name="imageFile">Yeni görsel dosyası (opsiyonel)</param>
        /// <returns>Başarılı ise Recipes sayfasına yönlendirme, hata varsa form view'ı</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRecipe(int id, RecipeUpdateDto dto, IFormFile? imageFile)
        {
            // Şifre kontrolü
            if (!IsAdminLoggedIn())
                return RedirectToAction(nameof(Login));

            if (id != dto.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Kategoriler = new[] { "Tatlı", "Ana Yemek", "Çorba", "Salata", "Kahvaltılık" };
                ViewBag.ZorlukSeviyeleri = new[] { "Kolay", "Orta", "Zor" };
                return View(dto);
            }

            try
            {
                await _recipeService.UpdateRecipeAsync(dto);

                // Yeni görsel yükleme (varsa)
                if (imageFile != null && imageFile.Length > 0)
                {
                    var imagePath = await _imageService.SaveImageAsync(id, imageFile);
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        await _recipeService.UpdateRecipeImagePathAsync(id, imagePath);
                    }
                }

                return RedirectToAction(nameof(Recipes));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Hata: {ex.Message}");
                ViewBag.Kategoriler = new[] { "Tatlı", "Ana Yemek", "Çorba", "Salata", "Kahvaltılık" };
                ViewBag.ZorlukSeviyeleri = new[] { "Kolay", "Orta", "Zor" };
                return View(dto);
            }
        }

        /// <summary>
        /// Tarif silme onay sayfası
        /// GET: /Admin/DeleteRecipe/{id}
        /// </summary>
        /// <param name="id">Silinecek tarif ID'si</param>
        /// <returns>Silme onay sayfası</returns>
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            // Şifre kontrolü
            if (!IsAdminLoggedIn())
                return RedirectToAction(nameof(Login));

            var tarif = await _recipeService.GetRecipeByIdAsync(id);
            if (tarif == null)
                return NotFound();

            return View(tarif);
        }

        /// <summary>
        /// Tarif silme işlemi
        /// POST: /Admin/DeleteRecipe/{id}
        /// </summary>
        /// <param name="id">Silinecek tarif ID'si</param>
        /// <returns>Başarılı ise Recipes sayfasına yönlendirme</returns>
        [HttpPost, ActionName("DeleteRecipe")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRecipeConfirmed(int id)
        {
            // Şifre kontrolü
            if (!IsAdminLoggedIn())
                return RedirectToAction(nameof(Login));

            try
            {
                await _recipeService.DeleteRecipeAsync(id);
                return RedirectToAction(nameof(Recipes));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Silme işlemi başarısız: {ex.Message}");
                var tarif = await _recipeService.GetRecipeByIdAsync(id);
                return View("DeleteRecipe", tarif);
            }
        }
    }
}
