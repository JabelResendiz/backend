# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and projects
COPY ["Finlay.PharmaVigilance.sln", ""]
COPY ["Finlay.PharmaVigilance.API/Finlay.PharmaVigilance.API.csproj", "Finlay.PharmaVigilance.API/"]
COPY ["Finlay.PharmaVigilance.Application/Finlay.PharmaVigilance.Application.csproj", "Finlay.PharmaVigilance.Application/"]
COPY ["Finlay.PharmaVigilance.Domain/Finlay.PharmaVigilance.Domain.csproj", "Finlay.PharmaVigilance.Domain/"]
COPY ["Finlay.PharmaVigilance.Infrastructure/Finlay.PharmaVigilance.Infrastructure.csproj", "Finlay.PharmaVigilance.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "Finlay.PharmaVigilance.sln"

# Copy remaining files
COPY . .

# Build the solution
RUN dotnet build "Finlay.PharmaVigilance.sln" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Finlay.PharmaVigilance.API/Finlay.PharmaVigilance.API.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Expose port
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Start application
ENTRYPOINT ["dotnet", "Finlay.PharmaVigilance.API.dll"]
