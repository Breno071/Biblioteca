#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["tests/Tests/Tests.csproj", "tests/Tests/"]
COPY ["src/API/API.csproj", "src/API/"]
COPY ["src/ApplicationCore/ApplicationCore.csproj", "src/ApplicationCore/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infraestructure/Infraestructure.csproj", "src/Infraestructure/"]
COPY ["src/Producer/RabbitMQ.Producer.csproj", "src/Producer/"]
RUN dotnet restore "./tests/Tests/./Tests.csproj"
COPY . .
WORKDIR "/src/tests/Tests"
RUN dotnet build "./Tests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tests.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tests.dll"]