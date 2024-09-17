FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /api
COPY src/ src/
COPY tests/ tests/
COPY functions/ functions/
RUN dotnet tool install --global dotnet-ef --version 8.*

# RUN dotnet build 

ENV PATH="$PATH:/root/.dotnet/tools"
ENV DBHOST="$DBHOST"
