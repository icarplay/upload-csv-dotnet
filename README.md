# upload-csv-dotnet

# Etapas para executar:

1. execute a migration: `dotnet ef database update`
2. execute o projeto: `dotnet run watch`
3. abra o endpoint do swagger: `localhost:{port}/swagger/index.html`


# Bibliotecas utilizadas:

- Microsoft Entity Framework Core:
`dotnet add package Microsoft.EntityFrameworkCore`

- Microsoft Entity Framework Core Tools:
`dotnet add package Microsoft.EntityFrameworkCore.Tools`

- Microsoft Entity Framework Core Sqlite:
`dotnet add package Microsoft.EntityFrameworkCore.Sqlite`

- CsvHelper:
`dotnet add package CsvHelper`

- Entity Framework Core Bulk Extensions:
`dotnet add package EFCore.BulkExtensions`

- Auto Mapper:
`dotnet add package AutoMapper`

- Auto Mapper Dependency Injection:
`dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection`
