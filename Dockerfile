FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["QuokkaServiceRegistry.csproj", "./"]
RUN dotnet restore "QuokkaServiceRegistry.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "QuokkaServiceRegistry.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# Version information build arguments
ARG APP_VERSION=dev
ARG APP_COMMIT_HASH=unknown
ARG APP_BUILD_DATE

RUN dotnet publish "QuokkaServiceRegistry.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

# Set version information as environment variables
ENV APP_VERSION=$APP_VERSION
ENV APP_COMMIT_HASH=$APP_COMMIT_HASH
ENV APP_BUILD_DATE=$APP_BUILD_DATE

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuokkaServiceRegistry.dll"]
