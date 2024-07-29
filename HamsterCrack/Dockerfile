FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
RUN apk add clang binutils musl-dev build-base zlib-static
RUN apk add cmake
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HamsterCrack/HamsterCrack.csproj", "HamsterCrack/"]
RUN dotnet restore "HamsterCrack/HamsterCrack.csproj" -r linux-musl-x64
COPY . .
WORKDIR "/src/HamsterCrack"
RUN dotnet build "HamsterCrack.csproj" -c $BUILD_CONFIGURATION -o /app/build -r linux-musl-x64 --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HamsterCrack.csproj" -c $BUILD_CONFIGURATION -o /app/publish -r linux-musl-x64 --no-restore

FROM alpine:3.18 AS final
WORKDIR /app
COPY --from=publish /app/publish/HamsterCrack /app
ENTRYPOINT ["/app/HamsterCrack"]
