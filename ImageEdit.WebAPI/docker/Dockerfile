FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic AS build
WORKDIR /src

COPY ["ImageEdit.Core", "ImageEdit.Core/"]
COPY ["ImageEdit.WebAPI", "ImageEdit.WebAPI/"]
RUN dotnet restore ImageEdit.WebAPI/ImageEdit.WebAPI.csproj

FROM build AS publish
RUN dotnet publish --no-restore "ImageEdit.WebAPI/ImageEdit.WebAPI.csproj" -c Release -o /app
COPY "ImageEdit.WebAPI/docker/docker-entrypoint.sh" /app
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT [ "./docker-entrypoint.sh" ]