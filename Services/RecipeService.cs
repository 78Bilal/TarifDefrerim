using AutoMapper;
using YemekTarifleri.DTOs;
using YemekTarifleri.Models;
using YemekTarifleri.Repositories;

namespace YemekTarifleri.Services
{
    /// <summary>
    /// Tarif iş mantığı implementasyonu
    /// </summary>
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _repository;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        /// <summary>
        /// RecipeService constructor
        /// </summary>
        /// <param name="repository">Tarif repository</param>
        /// <param name="mapper">AutoMapper instance</param>
        /// <param name="imageService">Görsel yönetim servisi</param>
        public RecipeService(IRecipeRepository repository, IMapper mapper, IImageService imageService)
        {
            _repository = repository;
            _mapper = mapper;
            _imageService = imageService;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<RecipeDto>> GetAllRecipesAsync()
        {
            var recipes = await _repository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<RecipeDto>>(recipes);

            // Her tarif için görsel yolunu ekle (model'de yoksa dosya sisteminden al)
            foreach (var dto in dtos)
            {
                if (string.IsNullOrEmpty(dto.ImagePath))
                {
                    dto.ImagePath = await _imageService.GetImagePathAsync(dto.Id);
                }
            }

            return dtos;
        }

        /// <inheritdoc/>
        public async Task<RecipeDto?> GetRecipeByIdAsync(int id)
        {
            var recipe = await _repository.GetByIdAsync(id);
            if (recipe == null)
                return null;

            var dto = _mapper.Map<RecipeDto>(recipe);
            // Model'de ImagePath varsa onu kullan, yoksa dosya sisteminden al
            if (string.IsNullOrEmpty(dto.ImagePath))
            {
                dto.ImagePath = await _imageService.GetImagePathAsync(recipe.Id);
            }
            return dto;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<RecipeDto>> GetFilteredRecipesAsync(RecipeFilterDto filter)
        {
            var allRecipes = await _repository.GetAllAsync();
            var filteredRecipes = allRecipes.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(filter.Kategori))
            {
                filteredRecipes = filteredRecipes.Where(r => r.Kategori == filter.Kategori);
            }

            if (!string.IsNullOrWhiteSpace(filter.Malzeme))
            {
                filteredRecipes = filteredRecipes
                    .Where(r => r.Malzemeler != null && r.Malzemeler.Contains(filter.Malzeme, StringComparison.OrdinalIgnoreCase));
            }

            if (filter.MaxHazirlamaSuresi.HasValue)
            {
                filteredRecipes = filteredRecipes
                    .Where(r => r.HazirlamaSuresi.HasValue && r.HazirlamaSuresi <= filter.MaxHazirlamaSuresi);
            }

            if (!string.IsNullOrWhiteSpace(filter.ZorlukSeviyesi))
            {
                filteredRecipes = filteredRecipes
                    .Where(r => r.ZorlukSeviyesi == filter.ZorlukSeviyesi);
            }

            var recipes = filteredRecipes.ToList();
            var dtos = _mapper.Map<IEnumerable<RecipeDto>>(recipes);

            // Her tarif için görsel yolunu ekle (model'de yoksa dosya sisteminden al)
            foreach (var dto in dtos)
            {
                if (string.IsNullOrEmpty(dto.ImagePath))
                {
                    dto.ImagePath = await _imageService.GetImagePathAsync(dto.Id);
                }
            }

            return dtos;
        }

        /// <inheritdoc/>
        public async Task<RecipeDto> CreateRecipeAsync(RecipeCreateDto dto)
        {
            var recipe = _mapper.Map<TarifModel>(dto);
            await _repository.AddAsync(recipe);
            await _repository.SaveChangesAsync();

            // Görsel yolu varsa model'e kaydet
            var imagePath = await _imageService.GetImagePathAsync(recipe.Id);
            if (!string.IsNullOrEmpty(imagePath))
            {
                recipe.ImagePath = imagePath;
                await _repository.UpdateAsync(recipe);
                await _repository.SaveChangesAsync();
            }

            var result = _mapper.Map<RecipeDto>(recipe);
            result.ImagePath = recipe.ImagePath ?? imagePath;
            return result;
        }

        /// <inheritdoc/>
        public async Task<RecipeDto?> UpdateRecipeAsync(RecipeUpdateDto dto)
        {
            var existingRecipe = await _repository.GetByIdAsync(dto.Id);
            if (existingRecipe == null)
                return null;

            _mapper.Map(dto, existingRecipe);
            await _repository.UpdateAsync(existingRecipe);
            await _repository.SaveChangesAsync();

            // Görsel yolu varsa model'e kaydet
            var imagePath = await _imageService.GetImagePathAsync(existingRecipe.Id);
            if (!string.IsNullOrEmpty(imagePath))
            {
                existingRecipe.ImagePath = imagePath;
                await _repository.UpdateAsync(existingRecipe);
                await _repository.SaveChangesAsync();
            }

            var result = _mapper.Map<RecipeDto>(existingRecipe);
            result.ImagePath = existingRecipe.ImagePath ?? imagePath;
            return result;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteRecipeAsync(int id)
        {
            var recipe = await _repository.GetByIdAsync(id);
            if (recipe == null)
                return false;

            // İlişkili görseli de sil (model'de ImagePath varsa onu kullan)
            if (!string.IsNullOrEmpty(recipe.ImagePath))
            {
                await _imageService.DeleteImageByPathAsync(recipe.ImagePath);
            }
            else
            {
                // Eski pattern için geriye dönük uyumluluk
                await _imageService.DeleteImageAsync(id);
            }

            await _repository.DeleteAsync(recipe);
            await _repository.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            var recipes = await _repository.GetAllAsync();
            return recipes
                .Where(r => !string.IsNullOrWhiteSpace(r.Kategori))
                .Select(r => r.Kategori!)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateRecipeImagePathAsync(int recipeId, string imagePath)
        {
            var recipe = await _repository.GetByIdAsync(recipeId);
            if (recipe == null)
                return false;

            recipe.ImagePath = imagePath;
            await _repository.UpdateAsync(recipe);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}

