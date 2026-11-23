FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar el .csproj desde su ruta real respecto a la raíz del repositorio
COPY ["backend/Makoto.Api/Makoto.Api.csproj", "backend/Makoto.Api/"]
RUN dotnet restore "backend/Makoto.Api/Makoto.Api.csproj"

# Copiar el código fuente del backend y publicar
COPY ["backend/Makoto.Api/", "backend/Makoto.Api/"]
WORKDIR "/src/backend/Makoto.Api"
RUN dotnet publish "Makoto.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Copiar la carpeta de audios desde la raíz hacia /app/datos/audios
COPY ["datos/audios", "/app/datos/audios"]

ENTRYPOINT ["dotnet", "Makoto.Api.dll"]