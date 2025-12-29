#!/bin/bash
set -e

echo "Starting Blazor WebAssembly build..."

# Install .NET if not available
if ! command -v dotnet &> /dev/null; then
    echo "Installing .NET..."
    curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 9.0
    export PATH="$HOME/.dotnet:$PATH"
fi

# Clean previous build
echo "Cleaning previous build..."
rm -rf dist public
mkdir -p public

# Build the project
echo "Building Blazor WebAssembly project..."
dotnet publish src/Portfolio.Web/Portfolio.Web.csproj -c Release -o dist

# Copy files to public directory
echo "Copying files to public directory..."
cp -r dist/wwwroot/* public/

# List files for debugging
echo "Files in public directory:"
ls -la public/

echo "Build completed successfully!"