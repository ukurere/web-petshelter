# 1. Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копіюємо все в контейнер
COPY . .

# Відновлюємо пакети
RUN dotnet restore

# Публікуємо у Release
RUN dotnet publish -c Release -o /app/publish

# 2. Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Копіюємо опублікований білд
COPY --from=build /app/publish .

# Слухаємо на 8080 всередині контейнера
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# !!! Якщо твій проект називається інакше, поміняй ім'я DLL тут
ENTRYPOINT ["dotnet", "web-petshelter.dll"]
