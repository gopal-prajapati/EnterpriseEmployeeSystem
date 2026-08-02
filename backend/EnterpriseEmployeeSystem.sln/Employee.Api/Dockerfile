# ---------- Build Stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file
COPY EnterpriseEmployeeSystem.Api.csproj .

# Restore dependencies
RUN dotnet restore

# Copy remaining source code
COPY . .

# Publish application
RUN dotnet publish -c Release -o /app/publish

# ---------- Runtime Stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published files from build stage
COPY --from=build /app/publish .

# ASP.NET Core listens on port 8080 inside container
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "EnterpriseEmployeeSystem.Api.dll"]
