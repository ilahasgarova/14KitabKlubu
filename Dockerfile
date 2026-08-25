# Build mərhələsi
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Proqramın fayllarını köçürürük
COPY ["KitabKlubu.csproj", "./"]
RUN dotnet restore "KitabKlubu.csproj"

COPY . .
RUN dotnet publish -c Release -o /app/publish

# Runtime (işə salma) mərhələsi
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render üçün port tənzimləməsi
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "KitabKlubu.dll"]