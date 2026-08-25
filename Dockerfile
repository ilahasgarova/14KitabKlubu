# Build mərhələsi (.NET 10.0)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Proqramın fayllarını köçürürük
COPY ["KitabKlubu.csproj", "./"]
RUN dotnet restore "KitabKlubu.csproj"

COPY . .
RUN dotnet publish -c Release -o /app/publish

# Runtime mərhələsi (.NET 10.0)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "KitabKlubu.dll"]