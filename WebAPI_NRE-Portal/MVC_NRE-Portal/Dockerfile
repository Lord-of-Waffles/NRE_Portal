FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file from parent directory
COPY ["WebAPI_NRE-Portal.sln", "./"]

# Copy project file
COPY ["MVC_NRE-Portal/MVC_NRE-Portal.csproj", "MVC_NRE-Portal/"]

# Restore dependencies
RUN dotnet restore "MVC_NRE-Portal/MVC_NRE-Portal.csproj"

# Copy all source code
COPY ["MVC_NRE-Portal/", "MVC_NRE-Portal/"]

# Build the application
WORKDIR "/src/MVC_NRE-Portal"
RUN dotnet build "MVC_NRE-Portal.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "MVC_NRE-Portal.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy published application
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ApiBaseUrl=http://webapi:8080

ENTRYPOINT ["dotnet", "MVC_NRE-Portal.dll"]