# Consulte https://aka.ms/customizecontainer para aprender a personalizar su contenedor de depuración y cómo Visual Studio usa este Dockerfile para compilar sus imágenes para una depuración más rápida.

# Esta fase se usa cuando se ejecuta desde VS en modo rápido (valor predeterminado para la configuración de depuración)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS build
WORKDIR /app


COPY *.sln .
COPY NSPermissions.Api/*.csproj ./NSPermissions.Api/
COPY NSPermissions.Application/*.csproj ./NSPermissions.Application/
COPY NSPermissions.Domain/*.csproj ./NSPermissions.Domain/
COPY NSPermissions.Infrastructure/*.csproj ./NSPermissions.Infrastructure/

RUN dotnet restore

COPY . .
WORKDIR /app/NSPermissions.API
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/NSPermissions.API/out ./
ENTRYPOINT ["dotnet", "NSPermissions.Api.dll"]
