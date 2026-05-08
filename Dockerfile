# ----- Build Stage -----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution + all projects
COPY . .

# Restore only WebApi
RUN dotnet restore ToDoApp.WebApi/ToDoApp.WebApi.csproj

# Build & publish
RUN dotnet publish ToDoApp.WebApi/ToDoApp.WebApi.csproj -c Release -o /app/publish

# ----- Runtime Stage -----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ToDoApp.WebApi.dll"]