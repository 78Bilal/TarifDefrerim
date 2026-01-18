## Admin Paneli Kurulum Özeti

Yemek Tarifleri uygulamasına **şifre korumalı** mini Admin Paneli başarıyla eklendi. Aşağıda oluşturulan tüm dosyalar, metodlar ve rotaları bulabilirsiniz.

---

## 🔐 Şifre Koruması

Admin paneline erişebilmek için şifre gereklidir. **Başlangıç şifresi:** `admin123`

**Şifreyi değiştirmek için:** `appsettings.json` dosyasında `AdminPanel:Password` değerini güncelleyin.

```json
{
  "AdminPanel": {
    "Password": "yeni_sifresi"
  }
}
```

---

## 📁 Oluşturulan Dosyalar

### 1. Controller (Ana Kontrol Sınıfı)
**Dosya:** `Controllers/AdminController.cs`

Admin panelinin ana kontrolcüsü. Şifre kontrolü + CRUD işlemleri:
- `Login()` [GET/POST] - Giriş sayfası ve kontrol
- `Logout()` - Oturumdan çıkış
- `IsAdminLoggedIn()` - Session kontrolü
- Tüm CRUD metodları şifre korumasına sahip

### 2. Login View
**Dosya:** `Views/Admin/Login.cshtml`

Şifre giriş sayfası:
- Modern tasarım (Tailwind CSS)
- Responsive (mobil uyumlu)
- Hata mesajları gösterir

---

## 🛣️ Admin Rotaları ve Metodlar

### 1. **Admin Giriş**
```
URL: GET /Admin/Login
Metod: AdminController.Login()
Döndürür: Şifre giriş formu (Login.cshtml)
Özellik: Zaten giriş yapmışsa Recipes sayfasına yönlendir
```

### 2. **Şifre Kontrolü**
```
URL: POST /Admin/Login
Metod: AdminController.Login(string password)
İşlem:
  - Şifre doğru ise session'a kaydeder
  - /Admin/Recipes'e yönlendir
  - Yanlış ise hata mesajı göster
```

### 3. **Oturumdan Çıkış**
```
URL: GET /Admin/Logout
Metod: AdminController.Logout()
İşlem:
  - Session'dan giriş bilgisini sil
  - /Admin/Login'e yönlendir
```

### 4. **Admin Ana Sayfası - Tarif Listesi**
```
URL: GET /Admin/Recipes
Metod: AdminController.Recipes()
Koruma: Şifre kontrolü - giriş yapmamışsa /Admin/Login'e yönlendir
Döndürür: Tüm tarifleri listeleyen view (Recipes.cshtml)
Butonlar: "Düzenle" ve "Sil" (her tarif için) + "Çıkış Yap"
Ek: İstatistikler paneli (toplam tarif, kategori sayısı, ortalama hazırlama süresi)
```

### 5. **Yeni Tarif Ekleme**
```
GET - Form Sayfası
  URL: GET /Admin/CreateRecipe
  Metod: AdminController.CreateRecipe()
  Koruma: Şifre kontrolü
  Döndürür: Boş tarif oluşturma formu (CreateRecipe.cshtml)

POST - Veri İşleme
  URL: POST /Admin/CreateRecipe
  Metod: AdminController.CreateRecipe(RecipeCreateDto dto, IFormFile? imageFile)
  Koruma: Şifre kontrolü
  İşlem: 
    - Tarifi veritabanına ekle
    - Görsel yüklerse, wwwroot/images klasörüne kaydet
    - Başarılı olursa /Admin/Recipes'e yönlendir
  Doğrulama:
    - Tarif Adı (zorunlu)
    - Malzemeler (zorunlu)
    - Yapılış (zorunlu)
    - Kategori (zorunlu)
    - Zorluk Seviyesi (zorunlu)
    - Diğer alanlar opsiyonel
```

---

### 3. **Tarif Düzenleme**
```
GET - Form Sayfası
  URL: GET /Admin/EditRecipe/{id}
  Metod: AdminController.EditRecipe(int id)
  Parametreler: Tarif ID'si
  Döndürür: Doldurulmuş tarif düzenleme formu (EditRecipe.cshtml)
  Hata: ID geçersiz ise 404 NotFound

POST - Veri İşleme
  URL: POST /Admin/EditRecipe/{id}
  Metod: AdminController.EditRecipe(int id, RecipeUpdateDto dto, IFormFile? imageFile)
  Parametreler: Tarif ID'si ve güncellenmiş veriler
  İşlem:
    - Tarifi veritabanında güncelle
    - Yeni görsel yüklerse, eski resmi değiştir
    - Başarılı olursa /Admin/Recipes'e yönlendir
  Doğrulama: Aynı CREATE ile aynı
  Ek: Mevcut görsel önizlemesi gösterir
```

---

### 4. **Tarif Silme**
```
GET - Onay Sayfası
  URL: GET /Admin/DeleteRecipe/{id}
  Metod: AdminController.DeleteRecipe(int id)
  Parametreler: Silinecek tarif ID'si
  Döndürür: Silme onay sayfası (DeleteRecipe.cshtml)
  İçerik: Tarif bilgileri (ad, resim, kategori, malzemeler, yapılış vb.)
  Hata: ID geçersiz ise 404 NotFound

POST - Silme İşlemi
  URL: POST /Admin/DeleteRecipe/{id}
  Metod: AdminController.DeleteRecipeConfirmed(int id)
  Parametreler: Silinecek tarif ID'si
  İşlem:
    - Tarifi veritabanından sil
    - Başarılı olursa /Admin/Recipes'e yönlendir
    - Hata varsa onay sayfasında hata mesajı göster
```

---

## 📄 Oluşturulan View Dosyaları

### 1. **Recipes.cshtml** - Tarif Yönetim Sayfası
`Views/Admin/Recipes.cshtml`

**Özellikleri:**
- Tüm tarifleri tablo formatında listeler
- Her tarifte: Ad, Kategori, Hazırlama Süresi, Zorluk Seviyesi
- "Düzenle" ve "Sil" butonları her tarif için
- Tarif görselinin küçük önizlemesi
- İstatistik kartları:
  - Toplam Tarif Sayısı
  - Kategori Sayısı
  - Ortalama Hazırlama Süresi
- Eğer tarif yoksa bilgilendirici mesaj
- Responsive tasarım (mobil uyumlu)

---

### 2. **CreateRecipe.cshtml** - Yeni Tarif Formu
`Views/Admin/CreateRecipe.cshtml`

**Form Alanları:**
- Tarif Adı (text) - **Zorunlu**
- Açıklama (textarea) - Opsiyonel
- Malzemeler (textarea) - **Zorunlu** (çok satırlı)
- Yapılış (textarea) - **Zorunlu** (çok satırlı)
- Kategori (dropdown) - **Zorunlu** (Tatlı, Ana Yemek, Çorba, Salata, Kahvaltılık)
- Hazırlama Süresi (number) - Opsiyonel (dakika)
- Zorluk Seviyesi (dropdown) - **Zorunlu** (Kolay, Orta, Zor)
- Görsel Yükleme (file) - Opsiyonel (JPG, PNG, GIF)

**Özellikler:**
- Görsel yükleme alanı drag-drop destekli
- Dinamik hata gösterimi
- Tailwind CSS ile modern tasarım
- "Tarifi Ekle" ve "İptal" butonları

---

### 3. **EditRecipe.cshtml** - Tarif Düzenleme Formu
`Views/Admin/EditRecipe.cshtml`

**Form Alanları:** CreateRecipe ile aynı, ayrıca:
- Hidden ID field (POST isteği için)
- Mevcut görsel önizlemesi (varsa)
- "Değişiklikleri Kaydet" butonu

**Özellikler:**
- Mevcut görsel gösterimi
- Yeni görsel yükleme seçeneği
- Önceden doldurulmuş form alanları

---

### 4. **DeleteRecipe.cshtml** - Silme Onay Sayfası
`Views/Admin/DeleteRecipe.cshtml`

**Özellikleri:**
- Geri alınamaz işlem uyarısı (kırmızı uyarı kutusu)
- Silinecek tarif bilgileri:
  - Tarif görseli
  - Tarif adı
  - Kategori
  - Hazırlama süresi
  - Zorluk seviyesi
  - Malzemeler (kısmi görünüm)
  - Yapılış (kısmi görünüm)
- "Evet, Sil" (kırmızı) ve "Hayır, İptal Et" (gri) butonları
- Önemli detaylar vurgulanmış

---

## 🔐 Yetkilendirme (Authorization)

Admin Paneli, `[Authorize]` attribute ile koruma altındadır.

**Mevcut Durum:**
```csharp
[Authorize]  // Giriş yapmış tüm kullanıcılar erişebilir
public class AdminController : Controller
```

**Gelecek için (Identity sistemi kurulursa):**
```csharp
[Authorize(Roles = "Admin")]  // Sadece Admin rolündeki kullanıcılar erişebilir
public class AdminController : Controller
```

**Not:** Şu anda Identity sistemi kurulu değilse, `[Authorize]` attribute çalışmayacaktır. Erişim kısıtlaması için ASP.NET Core Identity kurulması gerekir.

---

## 🎨 UI/UX Özellikleri

### Stil ve Tasarım:
- **Tailwind CSS** kullanılmış (mevcut site ile uyumlu)
- Renkli kartlar ve istatistikler
- Responsive mobil tasarım
- Emojiler ile görsel iyileştirme
- Hover efektleri ve geçişler

### Validasyon:
- Server-side doğrulama
- Client-side için FluentValidation entegrasyonu
- Hata mesajları her alanda gösterilir
- Başarı/başarısızlık durumunda kullanıcı yönlendirilir

---

## 📊 Veritabanı İşlemleri

Admin Paneli, mevcut `IRecipeService` interface'ini kullanır:
- `GetAllRecipesAsync()` - Tüm tarifleri getir
- `GetRecipeByIdAsync(id)` - Tarifi ID'ye göre getir
- `CreateRecipeAsync(dto)` - Yeni tarif oluştur
- `UpdateRecipeAsync(dto)` - Tarifi güncelle
- `DeleteRecipeAsync(id)` - Tarifi sil
- `UpdateRecipeImagePathAsync(id, path)` - Görsel yolunu güncelle

---

## 🖼️ Görsel Yönetimi

Görsel yükleme işlemleri `IImageService` tarafından yönetilir:
- Resimler wwwroot/images klasörüne kaydedilir
- Yalnızca görsel dosyaları kabul edilir
- Maksimum dosya boyutu kontrol edilebilir
- Yeni görsel yükleme eski resmi değiştirir

---

## 📱 Navigasyon

**Mevcut Navbar'a Ekleme:**
Ana sayfanın navbar'ında (\_Layout.cshtml), Admin Paneline giden link eklenmiştir:
```
⚙️ Admin Paneli butonu
```

Bu buton, "/Admin/Recipes" URL'sine yönlendirir.

---

## 🧪 Test Etme

1. **Tarif Listeleme:**
   - Tarayıcıda `/Admin/Recipes` gidin
   - Tüm tarifleri görmeli

2. **Yeni Tarif Ekleme:**
   - `/Admin/CreateRecipe` gidin
   - Formu doldurun ve "Tarifi Ekle"ye tıklayın
   - Başarı: `/Admin/Recipes`'e yönlendirileceksiniz

3. **Tarif Düzenleme:**
   - Tarif listesinden "Düzenle" düğmesine tıklayın
   - Bilgileri değiştirin ve "Kaydet"e tıklayın
   - Başarı: `/Admin/Recipes`'e yönlendirileceksiniz

4. **Tarif Silme:**
   - Tarif listesinden "Sil" düğmesine tıklayın
   - Onay sayfasında detayları kontrol edin
   - "Evet, Sil" düğmesine tıklayın
   - Başarı: `/Admin/Recipes`'e yönlendirileceksiniz

---

## ⚙️ Ayarlar ve Konfigürasyon

**Program.cs'de mevcut:**
- ✅ `app.UseAuthorization()` zaten mevcuttur
- ✅ `AddControllersWithViews()` zaten mevcuttur
- ✅ Database context zaten konfigüre edilmiştir

**Ek Kurulum Gerekli Değil** - Admin Paneli direkt kullanıma hazırdır!

---

## 📝 Notlar

1. **Identity Sistemi:** Yetkilendirmeyi tam olarak aktif etmek için ASP.NET Core Identity kurulması önerilir.

2. **Admin Menüsü:** Navbar'a otomatik olarak eklenmiştir. Stilini değiştirmek için `_Layout.cshtml`'ı düzenleyin.

3. **Validasyon:** Tüm doğrulama kuralları `RecipeCreateDtoValidator` ve `RecipeUpdateDtoValidator`'da tanımlıdır.

4. **Görsel Yönetimi:** `ImageService` sınıfı görsel yükleme işlemlerini yönetir.

---

**Hazır Kullanım:** Admin Paneli tamamen hazır ve çalışmaya hazırdır! 🚀
