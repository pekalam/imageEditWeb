FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
RUN apt-get update
RUN apt-get -y install netcat
COPY ["E2ETests/E2ETests.csproj", "E2ETests/"]
COPY ["ImageEdit.Core/ImageEdit.Core.csproj", "ImageEdit.Core/"]
COPY ["TestUtils/TestUtils.csproj", "TestUtils/"]
RUN dotnet restore "E2ETests/E2ETests.csproj"
COPY . .
WORKDIR "/src/E2ETests"
COPY ["E2ETests/docker/e2etests/wait-for", "/bin/wait-for"]
COPY ["E2ETests/docker/e2etests/wait-for-services.sh", "/bin/wait-for-services.sh"]
COPY ["E2ETests/docker/e2etests/docker-entrypoint.sh", "/bin/docker-entrypoint.sh"]
RUN chmod +x /bin/wait-for /bin/wait-for-services.sh /bin/docker-entrypoint.sh
RUN dotnet build "E2ETests.csproj" -c Debug
VOLUME [ "/reports" ]
CMD ["docker-entrypoint.sh"]
