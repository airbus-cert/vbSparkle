FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

EXPOSE 8000/tcp

ENV ASPNETCORE_URLS "http://+"

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Sources/vbSparkle.Web/vbSparkle.Web.csproj", "Sources/vbSparkle.Web/"]
COPY ["Sources/vbSparkle/vbSparkle.csproj", "Sources/vbSparkle/"]

RUN dotnet restore "Sources/vbSparkle.Web/vbSparkle.Web.csproj"
COPY . .
WORKDIR "/src/Sources/vbSparkle.Web"
RUN dotnet build "vbSparkle.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "vbSparkle.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "vbSparkle.Web.dll"]