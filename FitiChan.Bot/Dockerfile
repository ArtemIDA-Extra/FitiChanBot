#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FitiChan.Bot/FitiChan.Bot.csproj", "FitiChan.Bot/"]
COPY ["FitiChan.DL/FitiChan.DL.csproj", "FitiChan.DL/"]
RUN dotnet restore "FitiChan.Bot/FitiChan.Bot.csproj"
COPY . .
WORKDIR "/src/FitiChan.Bot"
RUN dotnet build "FitiChan.Bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FitiChan.Bot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FitiChan.Bot.dll"]