# 🎯 Admin Paneli - Hızlı Referans

## 🔐 Şifre Koruması

**Başlangıç Şifresi:** `admin123`

Şifreyi değiştirmek için `appsettings.json`'da `AdminPanel:Password` değerini güncelleyin.

---

## Admin Paneli Rotaları

| İşlem | HTTP Metodu | URL | Controller | Action | Açıklama |
|-------|------------|-----|-----------|--------|----------|
| Admin Giriş Formu | GET | `/Admin/Login` | AdminController | Login() | Şifre giriş sayfası |
| Şifre Kontrolü | POST | `/Admin/Login` | AdminController | Login(...) | Şifre doğrulama |
| Çıkış Yap | GET | `/Admin/Logout` | AdminController | Logout() | Oturumdan çıkış |
| Tarif Listesi | GET | `/Admin/Recipes` | AdminController | Recipes() | Tüm tarifleri listeleyen ana sayfa (⚠️ Şifre gerekli) |
| Yeni Tarif Formu | GET | `/Admin/CreateRecipe` | AdminController | CreateRecipe() | Yeni tarif oluşturma formu (⚠️ Şifre gerekli) |
| Yeni Tarif Ekle | POST | `/Admin/CreateRecipe` | AdminController | CreateRecipe(...) | Tarifi veritabanına ekle (⚠️ Şifre gerekli) |
| Düzenleme Formu | GET | `/Admin/EditRecipe/{id}` | AdminController | EditRecipe(int) | Tarifi düzenleme formu (⚠️ Şifre gerekli) |
| Tarifi Güncelle | POST | `/Admin/EditRecipe/{id}` | AdminController | EditRecipe(int, ...) | Tarifi güncelle (⚠️ Şifre gerekli) |
| Silme Onay Sayfası | GET | `/Admin/DeleteRecipe/{id}` | AdminController | DeleteRecipe(int) | Silme onay sayfası (⚠️ Şifre gerekli) |
| Tarifi Sil | POST | `/Admin/DeleteRecipe/{id}` | AdminController | DeleteRecipeConfirmed(int) | Tarifi sil (⚠️ Şifre gerekli) |

---

## Controller Metodları Detaylı

### AdminController.cs

#### 0️⃣ Login() [GET] - Giriş Sayfası
```csharp
public IActionResult Login()
```
- **Rota:** GET `/Admin/Login`
- **Döndürü:** `Views/Admin/Login.cshtml`
- **Özellik:** Zaten giriş yapmışsa Recipes sayfasına yönlendir

#### 🔑 Login() [POST] - Şifre Kontrolü
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Login(string password)
```
- **Rota:** POST `/Admin/Login`
- **Parametreler:** `string password` - Admin şifresi
- **İşlem:**
  1. `appsettings.json`'dan şifreyi al
  2. Kullanıcı şifresi ile karşılaştır
  3. Doğru ise session'a "AdminLoggedIn" = "true" kaydet
  4. Recipes sayfasına yönlendir
  5. Yanlış ise hata mesajı göster

#### 🚪 Logout() - Oturumdan Çıkış
```csharp
public IActionResult Logout()
```
- **Rota:** GET `/Admin/Logout`
- **İşlem:**
  1. Session'dan "AdminLoggedIn" sil
  2. Login sayfasına yönlendir

#### 1️⃣ Recipes() - Tarif Yönetim Sayfası
```csharp
public async Task<IActionResult> Recipes()
```
- **Rota:** GET `/Admin/Recipes`
- **Koruma:** ⚠️ `IsAdminLoggedIn()` kontrolü - giriş yapmamışsa Login sayfasına yönlendir
- **Döndürü:** `Views/Admin/Recipes.cshtml`
- **Veri:** Tüm tariflerin listesi
- **Özellikleri:**
  - Tüm tarifleri tabloda listeler
  - "Düzenle" ve "Sil" butonları içerir
  - İstatistik kartları gösterir
  - Responsive tasarım

---

#### 2️⃣ CreateRecipe() [GET] - Yeni Tarif Formu
```csharp
public IActionResult CreateRecipe()
```
- **Rota:** GET `/Admin/CreateRecipe`
- **Döndürü:** `Views/Admin/CreateRecipe.cshtml`
- **Veri:** Kategoriler ve Zorluk Seviyeleri listesi (ViewBag)
- **Özellik:** Boş form gösterir

---

#### 3️⃣ CreateRecipe(...) [POST] - Tarifi Ekle
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> CreateRecipe(RecipeCreateDto dto, IFormFile? imageFile)
```
- **Rota:** POST `/Admin/CreateRecipe`
- **Parametreler:**
  - `RecipeCreateDto dto` - Tarif verisi
  - `IFormFile? imageFile` - Tarif görseli (opsiyonel)
- **İşlem:**
  1. Model doğrulama
  2. Tarifi `IRecipeService`'e göndererek veritabanına ekle
  3. Görsel varsa `IImageService`'e yükle
  4. Başarı → `/Admin/Recipes`'e yönlendir
  5. Hata → Formu tekrar göster (hata mesajıyla)
- **Doğrulama:**
  - Ad (zorunlu)
  - Malzemeler (zorunlu)
  - Yapılış (zorunlu)
  - Kategori (zorunlu)
  - ZorlukSeviyesi (zorunlu)

---

#### 4️⃣ EditRecipe(int) [GET] - Düzenleme Formu
```csharp
public async Task<IActionResult> EditRecipe(int id)
```
- **Rota:** GET `/Admin/EditRecipe/{id}`
- **Parametreler:** `int id` - Tarif ID'si
- **Döndürü:** `Views/Admin/EditRecipe.cshtml`
- **Veri:** `RecipeUpdateDto` (doldurulmuş form)
- **Hata Handling:** 404 Not Found (tarif bulunamazsa)
- **Özellik:** Mevcut görsel önizlemesi gösterir

---

#### 5️⃣ EditRecipe(...) [POST] - Tarifi Güncelle
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> EditRecipe(int id, RecipeUpdateDto dto, IFormFile? imageFile)
```
- **Rota:** POST `/Admin/EditRecipe/{id}`
- **Parametreler:**
  - `int id` - Tarif ID'si
  - `RecipeUpdateDto dto` - Güncellenmiş tarif verisi
  - `IFormFile? imageFile` - Yeni görsel (opsiyonel)
- **İşlem:**
  1. ID doğrulama (yol ID'si vs form ID'si)
  2. Model doğrulama
  3. Tarifi güncelle
  4. Yeni görsel varsa değiştir
  5. Başarı → `/Admin/Recipes`'e yönlendir
  6. Hata → Formu tekrar göster (hata mesajıyla)
- **Doğrulama:** CreateRecipe ile aynı

---

#### 6️⃣ DeleteRecipe(int) [GET] - Silme Onay Sayfası
```csharp
public async Task<IActionResult> DeleteRecipe(int id)
```
- **Rota:** GET `/Admin/DeleteRecipe/{id}`
- **Parametreler:** `int id` - Silinecek tarif ID'si
- **Döndürü:** `Views/Admin/DeleteRecipe.cshtml`
- **Veri:** Silinecek `RecipeDto`
- **Hata Handling:** 404 Not Found (tarif bulunamazsa)
- **Özellik:** 
  - Geri alınamaz işlem uyarısı
  - Tarif bilgileri (resim, ad, kategori, malzemeler, yapılış)
  - "Evet, Sil" ve "Hayır, İptal Et" butonları

---

#### 7️⃣ DeleteRecipeConfirmed(int) [POST] - Tarifi Sil
```csharp
[HttpPost, ActionName("DeleteRecipe")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteRecipeConfirmed(int id)
```
- **Rota:** POST `/Admin/DeleteRecipe/{id}` (ActionName = "DeleteRecipe")
- **Parametreler:** `int id` - Silinecek tarif ID'si
- **İşlem:**
  1. `IRecipeService.DeleteRecipeAsync(id)` çağır
  2. Başarı → `/Admin/Recipes`'e yönlendir
  3. Hata → Onay sayfasını hata mesajıyla göster
- **Hata Handling:** Try-catch ile hata mesajları

---

## 🔐 Yetkilendirme

```csharp
[Authorize]  // Giriş yapan tüm kullanıcılar
public class AdminController : Controller
```

**Not:** Identity sistemi yoksa bu şu anda çalışmayacaktır. Gelecekte:
```csharp
[Authorize(Roles = "Admin")]  // Sadece Admin rolü
```

---

## 📝 DTO Sınıfları

### RecipeCreateDto
```
Ad (string) - **Zorunlu**
Aciklama (string) - Opsiyonel
Malzemeler (string) - **Zorunlu**
Yapilis (string) - **Zorunlu**
Kategori (string) - **Zorunlu**
HazirlamaSuresi (int?) - Opsiyonel
ZorlukSeviyesi (string) - **Zorunlu**
```

### RecipeUpdateDto
RecipeCreateDto'nun tümü + `Id` (int)

---

## 🎨 View Dosyaları

| View | URL | Amaç |
|------|-----|------|
| `Recipes.cshtml` | `/Admin/Recipes` | Tarif listesi ve yönetim |
| `CreateRecipe.cshtml` | `/Admin/CreateRecipe` [GET] | Yeni tarif formu |
| `EditRecipe.cshtml` | `/Admin/EditRecipe/{id}` [GET] | Düzenleme formu |
| `DeleteRecipe.cshtml` | `/Admin/DeleteRecipe/{id}` [GET] | Silme onay sayfası |

---

## 🔄 İşlem Akışı

### Yeni Tarif Ekleme
```
GET /Admin/CreateRecipe 
  ↓ [Formu doldur]
POST /Admin/CreateRecipe
  ↓ [Doğrulama ✓]
[Veri Tabanına Ekle]
  ↓
GET /Admin/Recipes [Başarı mesajı ile]
```

### Tarif Düzenleme
```
GET /Admin/EditRecipe/{id}
  ↓ [Formu düzenle]
POST /Admin/EditRecipe/{id}
  ↓ [Doğrulama ✓]
[Veri Tabanında Güncelle]
  ↓
GET /Admin/Recipes [Başarı mesajı ile]
```

### Tarif Silme
```
GET /Admin/DeleteRecipe/{id}
  ↓ [Bilgileri kontrol et]
POST /Admin/DeleteRecipe/{id}
  ↓ [Onay ✓]
[Veri Tabanından Sil]
  ↓
GET /Admin/Recipes [Başarı mesajı ile]
```

---

## 🧪 Test URL'leri

```
Tarif Listesi:
https://localhost:xxxx/Admin/Recipes

Yeni Tarif Ekleme:
https://localhost:xxxx/Admin/CreateRecipe

Tarif Düzenleme (ID=1):
https://localhost:xxxx/Admin/EditRecipe/1

Tarif Silme (ID=1):
https://localhost:xxxx/Admin/DeleteRecipe/1
```

---

## ⚡ Hızlı Başlangıç

1. Tarayıcıda `/Admin/Recipes` gidin
2. "Yeni Tarif" butonuna tıklayın
3. Formu doldurun ve gönder
4. Listede görüntüleyin, düzenleyin veya silin

**Tamamlandı!** 🎉
