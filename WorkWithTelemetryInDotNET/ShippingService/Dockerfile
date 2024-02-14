FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-chiseled AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0.101-jammy AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ShippingService/ShippingService.csproj", "ShippingService/"]
RUN dotnet restore "ShippingService/ShippingService.csproj"
COPY . .
WORKDIR "/src/ShippingService"
RUN dotnet build "ShippingService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ShippingService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShippingService.dll"]
