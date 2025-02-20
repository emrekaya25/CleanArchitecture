## OData (Open Data Protocol) Nedir ?
- OData (Open Data Protocol), Microsoft tarafından geliştirilen ve REST API'leri için standart bir protokol sağlayan bir veri erişim protokolüdür.
- ✔ REST tabanlıdır ve HTTP üzerinden veri sorgulama, filtreleme, sıralama gibi işlemleri kolaylaştırır.
- ✔ Veriyi standart bir formatta sunar ve istemcilerin API ile daha esnek ve verimli iletişim kurmasını sağlar.
- ✔ .NET, Java, JavaScript, Python gibi farklı teknolojilerle kolayca entegre edilebilir.

## 🔹 OData Neden Kullanılır?
💡 Daha Esnek ve Güçlü Sorgular
- Standart REST API’lerde genellikle sabit uç noktalar (endpoints) vardır, ancak OData ile istemciler dinamik sorgular yapabilir.
- Örneğin, GET /products yerine filtreleme, sıralama, sayfalama gibi işlemleri aynı uç nokta üzerinden yapabilirsin.

🚀 Performans Optimizasyonu
- Sunucuya gereksiz yük binmesini önler.
- SELECT işlemi gibi verinin sadece belirli kısımlarını alabilir ($select).
- Sayfalama desteği ile büyük verilerin yüklenmesini optimize eder ($top, $skip).

🔄 Standartlaşmış Veri Alışverişi
- API’nin istemci ile nasıl iletişim kuracağı OData standartlarıyla belirlenmiştir.
- JSON, XML gibi formatlarla veri alıp gönderebilir.

📊 Dinamik Filtreleme, Sıralama, ve Sayfalama
- REST API’lerde genellikle filtreleme için özel endpoint’ler yazmak gerekir.
- OData sayesinde istemciler URL içinden dinamik sorgular çalıştırabilir.

🔌 Entegrasyon Kolaylığı
- Microsoft Excel, Power BI, SAP, Dynamics gibi birçok yazılım OData’yı doğrudan destekler.
- Özellikle büyük kurumsal uygulamalarda veri paylaşımı için kullanılır.

## Modules Klasörü ne işe yarar? 
✔ Modules klasöründe bizim entitylerimizin Add-Delete-Update işlemlerinin endpointleri yazılır. MinimalAPI yazdığımız kısım orasıdır. Sonrasında ise genel bir RouteRegistrar.cs yardımıyla yazdığımız tüm MinimalAPI endpointlerini program.cs de çağırıp oluşturabiliriz.
✔ ** GetAll Metodunu OData ile çağırdık çünkü direk OData üzerinden filtreleme yapabiliyoruz.

## MinimalAPI nedir?
- Minimal API, .NET 6 ile tanıtılan, daha basit ve hafif bir API geliştirme modelidir. Bu model, geleneksel Controller tabanlı yapıya kıyasla daha az kodla hızlıca API geliştirmeyi mümkün kılar.
- Minimal API ile  controller, model binding, view rendering gibi ekstra katmanlar olmadan doğrudan HTTP endpoint’leri oluşturabilirsiniz.
- Ekstra gereksiz katmanlardan kaçındığı için, Minimal API daha hızlı çalışabilir ve daha az bellek kullanabilir.

## ExceptionHandler.cs nedir ?
✔ ExceptionHandler.cs bizim FluentValidation'dan ve kendi yazdığımız errorları toplayarak tek bir kalıp haline getiriyor böylelikle UI tarafında tek bir patternle çalışabileceğiz. 

## OpenAPI' kuruyoruz. 
✔ Endpointlerimizi json formatına çeviriyor.( OpenAPI kullanılabilmesi için 'Cors' politikası yazmak zorundayız.)

## ✔ Görsel bir araç olarak "Swagger" kullanıyorduk. Artık .NET 9 ile gelen "Scalar" kullanacağız. Nugetten kuruyoruz.

## ExtensionsMiddleware.cs nedir ?

