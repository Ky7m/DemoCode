FROM mcr.microsoft.com/dotnet/sdk:8.0
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AppHost/AppHost.csproj", "AppHost/"]
RUN dotnet workload install aspire
RUN dotnet restore "AppHost/AppHost.csproj"
COPY . .
WORKDIR "/src/AppHost"
RUN dotnet build "AppHost.csproj" -c $BUILD_CONFIGURATION -o /app/build
WORKDIR /app/build
ENTRYPOINT ["dotnet", "AppHost.dll"]
