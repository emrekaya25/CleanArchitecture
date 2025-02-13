# Application Katmanı 
# Domainde oluşturduğumuz entitylerin CRUD işlemlerinin yazılacağı katmandır.
# Bu işlemleri 'CQRS Pattern' kullanarak gerçekleştireceğiz.(MediatR kütüphanesi kuruldu.)
# Validation işlemleri için 'FluentValidation' kullanıldı.
# Bu katmanı her Entity için klasörleme yaparak kullanıyoruz.Bu da DDD(Domain-Driven-Design) yapısı için
# Mapleme işlemleri için 'Mapster' kuruldu.


## CQRS Nedir ?
## CQRS(Command Query Responsibility Segregation) Okuma (Query) ve Yazma (Command) işlemlerini birbirinden ayırarak sistemin performansını, ölçeklenebilirliğini ve güvenilirliğini artırmaktır.

# CQRS’in Temel Mantığı
# CQRS, veri okuma ve veri yazma işlemlerini iki farklı modelle yönetir:
# 1️⃣ Query (Sorgu) Tarafı – Sistemdeki verileri okumak için kullanılır.
# 2️⃣ Command (Komut) Tarafı – Sistemdeki verileri değiştirmek (ekleme, güncelleme, silme) için kullanılır.

# 📌 Örnek:
# Bir e-ticaret sisteminde, kullanıcı sipariş listesi görüntülemek için Query,
# Yeni bir sipariş oluşturmak için Command kullanır.