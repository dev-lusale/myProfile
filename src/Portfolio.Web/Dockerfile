# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/Portfolio.Web/Portfolio.Web.csproj", "Portfolio.Web/"]
COPY ["src/Portfolio.Shared/Portfolio.Shared.csproj", "Portfolio.Shared/"]
RUN dotnet restore "Portfolio.Web/Portfolio.Web.csproj"

# Copy everything else and build
COPY src/ .
WORKDIR "/src/Portfolio.Web"
RUN dotnet build "Portfolio.Web.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "Portfolio.Web.csproj" -c Release -o /app/publish

# Use nginx to serve the static files
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Copy the published application
COPY --from=build /app/publish/wwwroot .

# Copy custom nginx configuration
COPY src/Portfolio.Web/nginx.conf /etc/nginx/nginx.conf

EXPOSE 80