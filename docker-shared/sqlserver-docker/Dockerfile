FROM mcr.microsoft.com/mssql/server:2017-CU17-ubuntu
RUN apt-get update && apt-get -y install netcat
COPY ["container-scripts", "/container-scripts/"]
RUN chmod +x /container-scripts/docker-entrypoint.sh /container-scripts/listen-on-health-port.sh /container-scripts/wait-for
HEALTHCHECK --start-period=10m --interval=50s --retries=2 CMD /container-scripts/healthcheck.sh || exit 1
EXPOSE 32112
ENTRYPOINT [ "/container-scripts/docker-entrypoint.sh" ]