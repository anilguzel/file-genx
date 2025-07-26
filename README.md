# DistributedFileStorage

Bu proje, `todo.md` dosyasında belirtilen adımların çoğunu kapsayan basit bir örnek çözüm için oluşturulmuştur. 
Uygulama .NET 8 kullanımı hedefler ve Clean Architecture prensipleri doğrultusunda 
Core, Infrastructure, Persistence ve App katmanlarına ayrılmıştır.

"dotnet" yüklenemediği için derleme adımları çalıştırılamamıştır. 

## Kullanım

Projenin inşatı için normalde aşağıdaki komutlar çalıştırılır:

```bash
dotnet build

dotnet run --project src/DistributedFileStorage.App
```

Bu ortamda `dotnet` aracı mevcut olmadığından komutlar çalıştırılamamıştır.
