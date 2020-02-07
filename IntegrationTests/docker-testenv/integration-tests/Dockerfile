FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
RUN apt-get update
RUN apt-get -y install netcat
COPY ["IntegrationTests/IntegrationTests.csproj", "IntegrationTests/"]
COPY ["ImageEdit.Core/ImageEdit.Core.csproj", "ImageEdit.Core/"]
RUN dotnet restore "IntegrationTests/IntegrationTests.csproj"
COPY . .
WORKDIR "/src/IntegrationTests"
COPY ["IntegrationTests/docker-testenv/integration-tests/wait-for", "/bin/wait-for"]
COPY ["IntegrationTests/docker-testenv/integration-tests/wait-for-services.sh", "/bin/wait-for-services.sh"]
COPY ["IntegrationTests/docker-testenv/integration-tests/docker-entrypoint.sh", "/bin/docker-entrypoint.sh"]
RUN chmod +x /bin/wait-for /bin/wait-for-services.sh /bin/docker-entrypoint.sh
RUN dotnet build "IntegrationTests.csproj" -c Debug
VOLUME [ "/reports" ]
CMD ["docker-entrypoint.sh"]
