#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Core.RBAC.Web/Core.RBAC.Web.csproj", "Core.RBAC.Web/"]
COPY ["Core.Components/Core.Components.csproj", "Core.Components/"]
COPY ["Core.Services/Core.Services.csproj", "Core.Services/"]
COPY ["Core.Models/Core.Models.csproj", "Core.Models/"]
COPY ["Core.Repo/Core.Repo.csproj", "Core.Repo/"]
COPY ["Core.Handlers/Core.Handlers.csproj", "Core.Handlers/"]
RUN dotnet restore "./Core.RBAC.Web/./Core.RBAC.Web.csproj"
COPY . .
WORKDIR "/src/Core.RBAC.Web"
RUN dotnet build "./Core.RBAC.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Core.RBAC.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Core.RBAC.Web.dll"]