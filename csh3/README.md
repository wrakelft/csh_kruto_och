# ContactsApp

Программа "Записная книжка" на C# с использованием PostgreSQL, Entity Framework Core и ASP.NET Core Web API

---

## Описание

Приложение позволяет:

- Просматривать все контакты
- Искать контакты по имени, фамилии, телефону, e-mail или по всем полям одновременно
- Добавлять новые контакты с валидацией данных
- Сохранять данные в базе PostgreSQL
- Получать доступ к данным посредством REST API

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

1. Установите пакеты EF Core, PostgreSQL-провайдера и Core Web API:

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet tool install --global dotnet-ef
dotnet add package Swashbuckle.AspNetCore
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

В файле `appsettings.json` найдите метод `DefaultConnection` и укажите свои данные подключения к PostgreSQL:

```csharp
    "DefaultConnection": "Host=localhost;Database=contactsdb;Username=имя_пользователя;Password=пароль"
```

## Как юзать??????

После запуска в терминале появится вся инфа, дальше взаимодействовать лучше через swagger по ссылке
```bash
http://localhost:ПОРТ/swagger
```
