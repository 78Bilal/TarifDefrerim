namespace YemekTarifleri.Services
{
    /// <summary>
    /// Görsel yönetimi implementasyonu - wwwroot/images klasöründe görselleri saklar
    /// </summary>
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _imagesFolder = "images/recipes";
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private const long _maxFileSize = 5 * 1024 * 1024; // 5MB

        /// <summary>
        /// ImageService constructor
        /// </summary>
        /// <param name="environment">Web host environment</param>
        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        /// <inheritdoc/>
        public async Task<string?> SaveImageAsync(int recipeId, IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            // Dosya boyutu kontrolü
            if (imageFile.Length > _maxFileSize)
                throw new InvalidOperationException("Dosya boyutu 5MB'dan büyük olamaz.");

            // Dosya uzantısı kontrolü
            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                throw new InvalidOperationException("Geçersiz dosya formatı. Sadece JPG, PNG, GIF ve WEBP formatları desteklenir.");

            // Klasör yolu oluştur
            var imagesPath = Path.Combine(_environment.WebRootPath, _imagesFolder);
            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }

            // Dosya adı oluştur (GUID kullanarak benzersiz isim)
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(imagesPath, uniqueFileName);

            // Eski görseli sil (varsa)
            await DeleteImageAsync(recipeId);

            // Yeni görseli kaydet
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Web erişimi için yol döndür
            return $"/{_imagesFolder}/{uniqueFileName}";
        }

        /// <inheritdoc/>
        public Task<string?> GetImagePathAsync(int recipeId)
        {
            var imagesPath = Path.Combine(_environment.WebRootPath, _imagesFolder);
            if (!Directory.Exists(imagesPath))
                return Task.FromResult<string?>(null);

            // Model'de ImagePath varsa onu kullan
            // Eğer model'de yoksa, dosya sisteminde recipe_{id}_* pattern'i ile arama yap
            // (Eski görseller için geriye dönük uyumluluk)
            var files = Directory.GetFiles(imagesPath, $"recipe_{recipeId}_*");
            if (files.Length == 0)
                return Task.FromResult<string?>(null);

            // En yeni görseli al
            var latestFile = files.OrderByDescending(f => File.GetCreationTime(f)).First();
            var fileName = Path.GetFileName(latestFile);
            return Task.FromResult<string?>($"/{_imagesFolder}/{fileName}");
        }

        /// <inheritdoc/>
        public Task DeleteImageAsync(int recipeId)
        {
            var imagesPath = Path.Combine(_environment.WebRootPath, _imagesFolder);
            if (!Directory.Exists(imagesPath))
                return Task.CompletedTask;

            // Tarif ID'sine göre görselleri bul ve sil (eski pattern için geriye dönük uyumluluk)
            var files = Directory.GetFiles(imagesPath, $"recipe_{recipeId}_*");
            foreach (var file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch
                {
                    // Silme hatası durumunda sessizce devam et
                }
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task DeleteImageByPathAsync(string? imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return Task.CompletedTask;

            try
            {
                // Web path'ini fiziksel yola çevir (/images/recipes/file.jpg -> wwwroot/images/recipes/file.jpg)
                var relativePath = imagePath.TrimStart('/');
                var fullPath = Path.Combine(_environment.WebRootPath, relativePath);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch
            {
                // Silme hatası durumunda sessizce devam et
            }

            return Task.CompletedTask;
        }
    }
}

