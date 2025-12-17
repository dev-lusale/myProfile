# Use the official .NET runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/Portfolio.Api/Portfolio.Api.csproj", "Portfolio.Api/"]
COPY ["src/Portfolio.Shared/Portfolio.Shared.csproj", "Portfolio.Shared/"]
RUN dotnet restore "Portfolio.Api/Portfolio.Api.csproj"

# Copy everything else and build
COPY src/ .
WORKDIR "/src/Portfolio.Api"
RUN dotnet build "Portfolio.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Portfolio.Api.csproj" -c Release -o /app/publish

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Portfolio.Api.dll"]