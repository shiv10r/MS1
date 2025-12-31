# ----------------------------
# Base runtime image
# ----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80   # HTTP
EXPOSE 443  # HTTPS (optional, not configured in this example)

# ----------------------------
# Build stage
# ----------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["MS1.csproj", "."]
RUN dotnet restore "./MS1.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "./MS1.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ----------------------------
# Publish stage
# ----------------------------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MS1.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ----------------------------
# Final runtime image
# ----------------------------
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Run the app
ENTRYPOINT ["dotnet", "MS1.dll"]
