FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

ENV NUGET_PACKAGES=/root/.nuget/packages
ENV NUGET_FALLBACK_PACKAGES=

# cria config limpa
RUN printf '<configuration>\n  <packageSources>\n    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />\n  </packageSources>\n</configuration>' > /root/nuget.config

COPY ["FCG.Reviews.API/FCG.Reviews.API.csproj", "FCG.Reviews.API/"]
COPY ["FCG.Reviews.Application/FCG.Reviews.Application.csproj", "FCG.Reviews.Application/"]
COPY ["FCG.Reviews.Domain/FCG.Reviews.Domain.csproj", "FCG.Reviews.Domain/"]
COPY ["FCG.Reviews.Infrastructure/FCG.Reviews.Infrastructure.csproj", "FCG.Reviews.Infrastructure/"]

RUN dotnet restore "FCG.Reviews.API/FCG.Reviews.API.csproj" --configfile /root/nuget.config --no-cache

COPY . .

RUN dotnet build "FCG.Reviews.API/FCG.Reviews.API.csproj" \
    -c $BUILD_CONFIGURATION \
    --no-restore \
    /p:RestoreFallbackFolders=""

RUN dotnet publish "FCG.Reviews.API/FCG.Reviews.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    --no-build \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FCG.Reviews.API.dll"]