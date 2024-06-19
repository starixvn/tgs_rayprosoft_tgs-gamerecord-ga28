#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameRecord.GA28/Tgs.GameRecord.GA28.csproj", "Tgs.GameRecord.GA28/"]
COPY ["Tgs.GameRecord.GA28.Common/Tgs.GameRecord.GA28.Common.csproj", "Tgs.GameRecord.GA28.Common/"]
COPY ["Tgs.GameRecord.GA28.Core/Tgs.GameRecord.GA28.Core.csproj", "Tgs.GameRecord.GA28.Core/"]
COPY ["Tgs.GameRecord.GA28.Repositories/Tgs.GameRecord.GA28.Repositories.csproj", "Tgs.GameRecord.GA28.Repositories/"]
RUN dotnet restore "./Tgs.GameRecord.GA28/Tgs.GameRecord.GA28.csproj"
COPY . .
WORKDIR "/src/Tgs.GameRecord.GA28"
RUN dotnet build "./Tgs.GameRecord.GA28.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tgs.GameRecord.GA28.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameRecord.GA28.dll"]