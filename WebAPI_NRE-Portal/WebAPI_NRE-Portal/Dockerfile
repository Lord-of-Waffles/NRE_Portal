FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file from parent directory
COPY ["WebAPI_NRE-Portal.sln", "./"]

# Copy project files
COPY ["WebAPI_NRE-Portal/WebAPI_NRE-Portal.csproj", "WebAPI_NRE-Portal/"]
COPY ["DataLayer_NRE-Portal/DataLayer_NRE-Portal.csproj", "DataLayer_NRE-Portal/"]

# Restore dependencies
RUN dotnet restore "WebAPI_NRE-Portal/WebAPI_NRE-Portal.csproj"

# Copy all source code
COPY ["WebAPI_NRE-Portal/", "WebAPI_NRE-Portal/"]
COPY ["DataLayer_NRE-Portal/", "DataLayer_NRE-Portal/"]

# Build the application
WORKDIR "/src/WebAPI_NRE-Portal"
RUN dotnet build "WebAPI_NRE-Portal.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "WebAPI_NRE-Portal.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy published application
COPY --from=publish /app/publish .

# Copy CSV resources from build stage
COPY --from=build /src/DataLayer_NRE-Portal/Resources ./Resources

# Create directory for SQLite database
RUN mkdir -p /app/data

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "WebAPI_NRE-Portal.dll"]