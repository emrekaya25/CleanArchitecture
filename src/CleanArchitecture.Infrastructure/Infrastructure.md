# Infrastructure Katmanı

- ## AddScoped işlemleri için "scrutor" kütüphanesini kuruyoruz ve otomatik DI(Dependency Injection) işlemini yapıyor.

## Options klasörü ne işe yarar ?
- JwtOptions.cs tokenin içine gömdüğümüz bilgileri karşılayacak fieldların tutulduğu model görevini görür.
- JwtOptionsSetup.cs tokenin belirli ayarlarının yapıldığı sınıftır.

## Services klasörü ne işe yarar ?
- JwtProvider.cs verilen bilgilerle token oluşturma işlevini gören kodların yazıldığı sınıftır.
