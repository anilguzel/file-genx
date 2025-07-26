aşağıdaki tüm isterleri kapsayan geliştirmeyi yap


✅ 1. Proje Kurulumu ve Temel Yapı
Yapılacaklar:
 dotnet new console -n DistributedFileStorage komutu ile .NET 8 veya 9 tabanlı bir Console uygulaması oluştur.

 Aşağıdaki temel projeleri/katmanları oluştur:

DistributedFileStorage.Core: Arayüzler, modeller, servis tanımları

DistributedFileStorage.Infrastructure: Storage provider implementasyonları

DistributedFileStorage.Persistence: In-Memory DB + repository katmanı

DistributedFileStorage.App: Console uygulaması, IoC tanımları

 Microsoft.Extensions.DependencyInjection, Microsoft.Extensions.Logging, Serilog, EFCore.InMemory, System.IO.Abstractions, System.Security.Cryptography NuGet paketlerini yükle.

✅ 2. Dosya Chunk'lama Mekanizması
Yapılacaklar:
 IFileChunker interface'i tanımlanacak.

 DynamicFileChunker sınıfı dosya boyutuna göre parçalama yapacak şekilde geliştirilecek.

Dosya boyutuna göre optimal chunk boyutu belirlenecek (örnek: <10MB → 1MB, 10–100MB → 5MB, >100MB → 10MB).

 Chunk modeli (FileChunk) aşağıdaki alanları içerecek:

Id, ChunkIndex, OriginalFileName, Data, Checksum, StorageProviderName, CreatedAt

 Her chunk'ın SHA256 checksum'ı alınacak.

✅ 3. Storage Provider Mimarisi
Yapılacaklar:
 IStorageProvider interface’i tanımlanacak: StoreAsync(FileChunk chunk), RetrieveAsync(Guid chunkId) vs.

 Aşağıdaki implementasyonlar geliştirilecek:

 FileSystemStorageProvider: Chunk'ları local diske kaydeder.

 DatabaseStorageProvider: Chunk'ları EF Core In-Memory DB’ye kaydeder.

 StorageProviderFactory veya DI üzerinden provider seçim stratejisi oluşturulacak.

✅ 4. Veritabanı & Metadata Saklama
Yapılacaklar:
 EF Core In-Memory DB context (ChunkDbContext) yapılandırılacak.

 ChunkMetadata entity’si ile aşağıdaki alanlar tanımlanacak:

FileId, ChunkId, ChunkIndex, Checksum, StorageProviderName, CreatedAt

 CRUD işlemleri için IChunkRepository ve ChunkRepository geliştirilecek.

 DI üzerinden DbContext, repository’ler ve provider’lar inject edilecek.

✅ 5. Dosya Birleştirme ve Bütünlük Kontrolü
Yapılacaklar:
 IFileRebuilder interface’i tanımlanacak.

 FileRebuilder sınıfı:

Tüm chunk’ları sırayla ilgili provider'dan çeker.

Chunk'ları birleştirerek geçici bir dosya oluşturur.

SHA256 checksum ile orijinal dosya bütünlüğünü doğrular.

 Hata durumları loglanmalı ve kullanıcıya bildirilmelidir.

✅ 6. Logging ve İzlenebilirlik
Yapılacaklar:
 ILogger<T> altyapısı DI üzerinden sağlanmalı.

 Serilog yapılandırması yapılmalı.

 Aşağıdaki işlemler loglanmalı:

Dosya yükleme, chunk’a ayırma, her chunk'ın kaydedilmesi

Metadata kaydı, dosya birleştirme, checksum doğrulama

Hatalar ve uyarılar

✅ 7. Design ve SOLID Prensipleri
Uygulanacak İlkeler:
S: Tüm bileşenler tek sorumluluk taşıyacak şekilde yapılandırılacak (Chunker, Storage, Rebuilder vs.).

O: Yeni storage provider eklenebilir olmalı (örneğin CloudStorageProvider).

L: Tüm provider’lar aynı interface’i uygulayacak.

I: Bileşenler küçük interface’lere bölünecek.

D: Tüm bağımlılıklar IoC container üzerinden çözülecek.

✅ 8. Ekstra Özellik Fikirleri (Ekstra Puan)
 Dosya upload sırasında ilerleme göstergesi.

 Storage provider yük dağıtım algoritması (örnek: round-robin).

 Config üzerinden storage öncelik sırası.

 Web API ile ileride genişletilebilecek endpoint hazırlığı (modüler yapı).

 Retry mekanizması başarısız storage kayıtları için.

 Toplam işlem süresi ölçümü ve loglanması.

✅ 9. README ve Proje Teslimatı
README İçeriği:
 Projenin amacı

 Kullanılan teknoloji ve mimari özetleri

 Uygulamanın nasıl çalıştırılacağı:

bash
Copy
Edit
dotnet build
dotnet run --project DistributedFileStorage.App
 Genişletme ve test önerileri

 Ekstra geliştirilen özellikler
