# 🍳 Yemek Tarifleri - Senior Portfolio Project

Modern, ölçeklenebilir ve profesyonel bir ASP.NET Core MVC tarif defteri uygulaması. Bu proje, enterprise-level yazılım geliştirme pratiklerini ve modern web teknolojilerini sergilemek için geliştirilmiştir.

## 📋 İçindekiler

- [Özellikler](#özellikler)
- [Teknoloji Stack](#teknoloji-stack)
- [Mimari](#mimari)
- [Kurulum](#kurulum)
- [Kullanım](#kullanım)
- [Proje Yapısı](#proje-yapısı)
- [Katkıda Bulunma](#katkıda-bulunma)

## ✨ Özellikler

### 🎨 Modern UI/UX
- **Tailwind CSS** ile responsive ve modern arayüz
- Hover animasyonlu tarif kartları
- Step-by-step (adım adım) tarif ekleme formu
- Gelişmiş filtreleme sistemi (anlık filtreleme)
- Zorluk seviyesi badge sistemi (Kolay/Orta/Zor)
- Görsel yönetimi (wwwroot/images klasöründe saklama)

### 🏗️ Teknik Mimari
- **Repository Pattern** - Veri erişim katmanı soyutlaması
- **Service Layer** - İş mantığı ayrımı
- **AutoMapper** - Entity-DTO mapping
- **FluentValidation** - Güçlü form validasyonu
- **DTO Pattern** - Veri transfer nesneleri
- **Dependency Injection** - Loose coupling

### 🔍 Gelişmiş Özellikler
- Kategori bazlı filtreleme
- Malzeme ismine göre arama
- Hazırlama süresine göre filtreleme
- Zorluk seviyesine göre filtreleme
- Görsel yükleme ve yönetimi
- Responsive tasarım (mobil uyumlu)

## 🛠️ Teknoloji Stack

### Backend
- **.NET 9.0** - En son .NET framework
- **ASP.NET Core MVC** - Web framework
- **Entity Framework Core 9.0.4** - ORM
- **SQLite** - Veritabanı
- **AutoMapper 13.0.1** - Object mapping
- **FluentValidation 11.3.0** - Validation

### Frontend
- **Tailwind CSS** - Utility-first CSS framework
- **jQuery** - DOM manipulation
- **jQuery Validation** - Client-side validation

### Mimari Desenler
- Repository Pattern
- Service Layer Pattern
- DTO Pattern
- Dependency Injection

## 🏛️ Mimari

Proje, **Clean Architecture** prensiplerine uygun olarak katmanlı mimari ile geliştirilmiştir:

```
YemekTarifleri/
├── Controllers/          # MVC Controllers (sadece HTTP isteklerini yönetir)
├── Services/             # İş mantığı katmanı
│   ├── IRecipeService.cs
│   ├── RecipeService.cs
│   ├── IImageService.cs
│   └── ImageService.cs
├── Repositories/         # Veri erişim katmanı
│   ├── IRepository.cs
│   ├── Repository.cs
│   ├── IRecipeRepository.cs
│   └── RecipeRepository.cs
├── DTOs/                 # Veri transfer nesneleri
│   ├── RecipeDto.cs
│   ├── RecipeCreateDto.cs
│   ├── RecipeUpdateDto.cs
│   └── RecipeFilterDto.cs
├── Models/               # Entity modelleri
│   └── TarifModel.cs
├── Validators/           # FluentValidation validators
│   ├── RecipeCreateDtoValidator.cs
│   └── RecipeUpdateDtoValidator.cs
├── Mappings/             # AutoMapper profilleri
│   └── MappingProfile.cs
├── Data/                 # DbContext
│   └── AppDbContext.cs
└── Views/                # Razor views
```

### Katman Sorumlulukları

1. **Controllers**: HTTP isteklerini alır, Service katmanına yönlendirir, View'lara veri gönderir
2. **Services**: İş mantığını içerir, Repository'leri kullanır, DTO mapping yapar
3. **Repositories**: Veritabanı işlemlerini yönetir, CRUD operasyonları
4. **DTOs**: Veri transfer nesneleri, API ve View arasında veri taşır
5. **Models**: Entity modelleri, veritabanı tabloları ile eşleşir
6. **Validators**: FluentValidation ile form validasyonu
7. **Mappings**: AutoMapper konfigürasyonları

## 🚀 Kurulum

### Gereksinimler
- .NET 9.0 SDK
- Visual Studio 2022 veya VS Code
- Git

### Adımlar

1. **Projeyi klonlayın**
```bash
git clone <repository-url>
cd YemekTarifleri
```

2. **NuGet paketlerini yükleyin**
```bash
dotnet restore
```

3. **Veritabanı migration'larını çalıştırın**
```bash
dotnet ef database update
```

4. **Projeyi çalıştırın**
```bash
dotnet run
```

5. **Tarayıcıda açın**
```
https://localhost:5001
```

## 📝 Kullanım

### Yeni Tarif Ekleme
1. Ana sayfada "Tarif Ekle" butonuna tıklayın
2. Step-by-step formu doldurun:
   - **Adım 1**: Temel bilgiler (Ad, Kategori, Açıklama)
   - **Adım 2**: Malzemeler ve süre bilgileri
   - **Adım 3**: Yapılış adımları ve görsel yükleme
3. "Kaydet" butonuna tıklayın

### Tarif Filtreleme
Ana sayfada filtreleme panelini kullanarak:
- Kategoriye göre filtreleme
- Malzeme ismine göre arama
- Maksimum hazırlama süresine göre filtreleme
- Zorluk seviyesine göre filtreleme

Filtreleme anlık olarak çalışır (live filtering).

### Tarif Düzenleme ve Silme
- Tarif kartında "✏️" butonuna tıklayarak düzenleyebilirsiniz
- "🗑️" butonuna tıklayarak silebilirsiniz

## 📁 Proje Yapısı

```
YemekTarifleri/
├── Controllers/
│   ├── HomeController.cs
│   └── TarifController.cs
├── Services/
│   ├── IRecipeService.cs
│   ├── RecipeService.cs
│   ├── IImageService.cs
│   └── ImageService.cs
├── Repositories/
│   ├── IRepository.cs
│   ├── Repository.cs
│   ├── IRecipeRepository.cs
│   └── RecipeRepository.cs
├── DTOs/
│   ├── RecipeDto.cs
│   ├── RecipeCreateDto.cs
│   ├── RecipeUpdateDto.cs
│   └── RecipeFilterDto.cs
├── Models/
│   ├── TarifModel.cs
│   └── ErrorViewModel.cs
├── Validators/
│   ├── RecipeCreateDtoValidator.cs
│   └── RecipeUpdateDtoValidator.cs
├── Mappings/
│   └── MappingProfile.cs
├── Data/
│   └── AppDbContext.cs
├── Views/
│   ├── Tarif/
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   └── Details.cshtml
│   └── Shared/
│       └── _Layout.cshtml
├── wwwroot/
│   ├── images/
│   │   └── recipes/      # Yüklenen görseller
│   └── css/
└── Migrations/           # EF Core migrations
```

## 🔄 Migration Oluşturma

Yeni alanlar eklediğinizde migration oluşturmanız gerekir:

```bash
# Migration oluştur
dotnet ef migrations add AddNewFields

# Veritabanını güncelle
dotnet ef database update
```

**Not**: Mevcut veritabanı şemasına yeni alanlar eklerken (`HazirlamaSuresi`, `ZorlukSeviyesi`) migration oluşturmanız gerekecektir.

## 🎨 Renk Paleti

- **Arka Plan**: `#F9FAFB` (bg-[#F9FAFB])
- **Primary (Smaragd Green)**: `#10b981` (emerald-600)
- **Secondary (Warm Orange)**: `#f97316` (orange-500)
- **Accent**: `#059669` (emerald-700)

## 📚 Best Practices

Bu projede uygulanan best practices:

1. **Separation of Concerns**: Her katman kendi sorumluluğuna odaklanır
2. **Dependency Injection**: Loose coupling için DI kullanımı
3. **Repository Pattern**: Veri erişim soyutlaması
4. **DTO Pattern**: Entity'lerin direkt kullanımını önler
5. **Validation**: FluentValidation ile güçlü validasyon
6. **XML Documentation**: Tüm public metodlar dokümante edilmiştir
7. **Async/Await**: Asenkron programlama
8. **Error Handling**: Try-catch blokları ve uygun hata mesajları

## 🚧 Gelecek Geliştirmeler

- [ ] Cloudinary entegrasyonu (bulut görsel yönetimi)
- [ ] Kullanıcı authentication ve authorization
- [ ] Favori tarifler sistemi
- [ ] Yorum ve puanlama sistemi
- [ ] Tarif paylaşma (sosyal medya)
- [ ] API endpoint'leri (RESTful API)
- [ ] Unit testler ve integration testler
- [ ] Docker containerization
- [ ] CI/CD pipeline

## 📄 Lisans

Bu proje eğitim ve portfolio amaçlı geliştirilmiştir.

## 👨‍💻 Geliştirici

Senior Portfolio Project - ASP.NET Core MVC

---

**Not**: Bu proje, modern yazılım geliştirme pratiklerini ve enterprise-level mimari desenleri sergilemek için geliştirilmiştir.

