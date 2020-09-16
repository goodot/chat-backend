#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ChatAPI/ChatAPI.csproj", "ChatAPI/"]
COPY ["ChatAPI.data/ChatAPI.Data.csproj", "ChatAPI.data/"]
RUN dotnet restore "ChatAPI/ChatAPI.csproj"
CMD dotnet migrate
COPY . .
WORKDIR "/src/ChatAPI"
RUN dotnet build "ChatAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChatAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatAPI.dll"]
