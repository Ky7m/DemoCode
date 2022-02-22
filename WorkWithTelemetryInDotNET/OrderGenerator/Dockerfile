FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OrderGenerator/OrderGenerator.csproj", "OrderGenerator/"]
RUN dotnet restore "OrderGenerator/OrderGenerator.csproj"
COPY . .
WORKDIR "/src/OrderGenerator"
RUN dotnet build "OrderGenerator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderGenerator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderGenerator.dll"]
