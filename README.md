# ForumApp

.NET 10 ile geliştirdiğim basit bir forum API projesi.

Projeyi Minimal API yapısını için Carter, JWT authentication, Entity Framework Core ve PostgreSQL ile geliştirildi.

## Proje Linki

http://forumapp-api.omerdumlupinar.com/scalar/v1

## Kullanılan Teknolojiler

- .NET 10
- Minimal API
- Carter
- Entity Framework Core
- PostgreSQL
- ASP.NET Core Identity
- JWT
- Scalar

## Projede Neler Var?

- Kullanıcı kayıt olabilir ve giriş yapabilir.
- Kullanıcı konu açabilir.
- Açılan konular listelenebilir.
- Konulara yorum yapılabilir.
- Yorumlara cevap verilebilir.
- Yorumlara 1-5 arasında puan verilebilir.
- Kullanıcı kendi yorumuna puan veremez.
- Bir kullanıcı aynı yoruma bir kez puan verebilir.
- Konuyu açan kullanıcı konuyu kapatabilir.
- Kapatılmış konuya yeni yorum yapılamaz.
- Soft delete yapısı bulunmaktadır.
- Rate limiting kullanılmaktadır.

## Veritabanı

Projede PostgreSQL kullanılıyor.

Migrationları çalıştırmak için:

```bash
dotnet ef database update
