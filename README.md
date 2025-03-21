# Clean Architecture Setup

- 2025 yılı için projelerde kullanılabilecek modern bir Clean Architecture yapısı sunulmaktadır.
- Detaylı anlatım için katmanların içindeki .md dosyalarını inceleyebilirsiniz.

### Mimari Yapı
- **Architectural Pattern**: Clean Architecture
- **Design Patterns**:
  - Result Pattern
  - Repository Pattern
  - CQRS Pattern
  - UnitOfWork Pattern

### Kullanılan Kütüphaneler
- **MediatR**: CQRS ve mesajlaşma işlemleri için kullanıldı.
-**Mapster**: Nesne eşleşmeleri(mapleme) için kullanıldı.
- **FluentValidation**: Doğrulama işlemleri için kullanıldı.
- **EntityFrameworkCore**: ORM(Object-Relational Mapping) için kullanıldı.
- **OData**: Sorgulama ve veri erişiminde esneklik sağlamak için kullanıldı.
- **Scrutor**: Dependency Injection yönetimi ve dinamik servis kaydı için kullanıldı.
- **Microsoft.Identity**: Kimlik doğrulama (authentication) ve yetkilendirme (authorization) işlemlerini yönetmek için kullanılan bir kütüphanedir. Kullanıcı yönetimi, roller, şifreleme, token doğrulama gibi birçok güvenlik özelliğini sağlar.


## Kurulum ve Kullanım
1. **Depoyu Klonlayın**:

   ```sh
   git clone https://github.com/emrekaya25/CleanArchitecture.git
   cd CleanArchitecture
