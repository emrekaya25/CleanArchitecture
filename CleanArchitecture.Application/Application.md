# Application Katmanı 
# Domainde oluşturduğumuz entitylerin CRUD işlemlerinin yazılacağı katmandır.
# Bu işlemleri 'CQRS Pattern' kullanarak gerçekleştireceğiz.(MediatR kütüphanesi kuruldu.)
# Validation işlemleri için 'FluentValidation' kullanıldı.
# Bu katmanı her Entity için klasörleme yaparak kullanıyoruz.Bu da DDD(Domain-Driven-Design) yapısı için
# Mapleme işlemleri için 'Mapster' kuruldu.

## ----------------------------------------------------------


## CQRS Nedir ?
# 📌 CQRS(Command Query Responsibility Segregation) Okuma (Query) ve Yazma (Command) işlemlerini birbirinden ayırarak sistemin performansını, ölçeklenebilirliğini ve güvenilirliğini artırmaktır.

# CQRS’in Temel Mantığı
# CQRS, veri okuma ve veri yazma işlemlerini iki farklı modelle yönetir:
# 1️⃣ Query (Sorgu) Tarafı – Sistemdeki verileri okumak için kullanılır.
# 2️⃣ Command (Komut) Tarafı – Sistemdeki verileri değiştirmek (ekleme, güncelleme, silme) için kullanılır.

# 📌 Örnek:
# Bir e-ticaret sisteminde, kullanıcı sipariş listesi görüntülemek için Query,
# Yeni bir sipariş oluşturmak için Command kullanır.

## ----------------------------------------------------------

## Pipeline nedir ?
# 📌 Pipeline, bir işlemin veya veri akışının aşamalar halinde işlendiği yapıdır. Birçok sistemde, özellikle yazılım geliştirme, veri işleme ve mesaj kuyruğu sistemlerinde kullanılır.

# 1️⃣ Middleware = HTTP isteklerini işleyen özel bir pipeline mekanizmasıdır.
# 2️⃣ Pipeline = Middleware’i de kapsayan genel bir süreç yönetim modelidir.
## Middleware’ler pipeline içinde alt bileşenler gibi düşünülebilir.

## ----------------------------------------------------------

## Behavior nedir ?
# 📌Genellikle iş kurallarını, validation (doğrulama) mekanizmalarını ve özel pipeline davranışlarını içeren dosyaları saklamak için kullanılır.
## 1️⃣ Validation Behaviors (Doğrulama Davranışları)

## FluentValidation gibi kütüphanelerle request'leri doğrulamak için kullanılır.

## 2️⃣ Logging Behaviors (Loglama Davranışları)

## API’ye gelen istekleri loglamak için kullanılır.
## Hangi request'in hangi parametrelerle geldiğini görmek için Middleware gibi çalışır.
## 3️⃣ Performance Behaviors (Performans İzleme Davranışları)

## İşlemlerin ne kadar sürede çalıştığını takip eder.
## Slow request'leri (yavaş işlemleri) loglamak için kullanılır.
## 4️⃣ Transaction Behaviors (İşlem Yönetimi Davranışları)

## UnitOfWork veya Database Transaction yönetimi yapmak için kullanılır.
## İşlemler başarısız olursa rollback (geri alma) işlemlerini yönetir.

## ----------------------------------------------------------


## ApplicationRegistrar.cs nedir ?
# 📌 Bulunduğu katmanın servislerini, bağımlılıklarını veya bileşenlerini kaydetmek için kullanılan bir yapıdır. Özellikle Dependency Injection konteynırına servisleri eklemek için kullanılır.