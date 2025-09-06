# ContactsApp

Программа "Записная книжка" на C# с использованием PostgreSQL и Entity Framework Core.

---

## Описание

Приложение позволяет:

- Просматривать все контакты
- Искать контакты по имени, фамилии, телефону, e-mail или по всем полям одновременно
- Добавлять новые контакты с валидацией данных
- Сохранять данные в базе PostgreSQL

---

## Требования

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- Visual Studio Code (или другой редактор C#)
- PostgreSQL и pgAdmin 4
- Расширение **C#** для VS Code

---

## Установка PostgreSQL

1. Установите PostgreSQL.
2. Откройте **pgAdmin 4**.

## Установка зависимостей и запуск

1. Установите пакеты EF Core и PostgreSQL-провайдера:

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet tool install --global dotnet-ef
```
2. Собрать пакеты
```bash
dotnet build
```
3. Запустить программу
```bash
dotnet run
```

## Настройка подключения к базе данных

В файле `ContactsContext.cs` найдите метод `OnConfiguring` и укажите свои данные подключения к PostgreSQL:

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    // Замените параметры на свои
    optionsBuilder.UseNpgsql("Host=localhost;Database=contactsdb;Username=ваш_пользователь;Password=ваш_пароль");
}