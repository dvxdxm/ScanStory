#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["eStoryContainer.Web/eStoryContainer.Web.csproj", "eStoryContainer.Web/"]
RUN dotnet restore "eStoryContainer.Web/eStoryContainer.Web.csproj"
COPY . .
WORKDIR "/src/eStoryContainer.Web"
RUN dotnet build "eStoryContainer.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eStoryContainer.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eStoryContainer.Web.dll"]