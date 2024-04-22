FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
EXPOSE 80
# Copy csproj files and restore as distinct layers
COPY ["WeatherApi/WeatherApi.csproj", "WeatherApi/"]
COPY ["WeatherApi.BusinessLogic/WeatherApi.BusinessLogic.csproj", "WeatherApi.BusinessLogic/"]
COPY ["WeatherApi.BusinessLogic.Abstraction/WeatherApi.BusinessLogic.Abstraction.csproj", "WeatherApi.BusinessLogic.Abstraction/"]
COPY ["WeatherApi.Data/WeatherApi.Data.csproj", "WeatherApi.Data/"]

# Restore NuGet packages
RUN dotnet restore "WeatherApi/WeatherApi.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish "WeatherApi/WeatherApi.csproj" -c Release -o /app/publish

# Generate the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WeatherApi.dll"]
