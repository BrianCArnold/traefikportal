FROM mcr.microsoft.com/dotnet/sdk:5.0 as build-env

WORKDIR /app

RUN apt-get update -y && \
    curl -fsSL https://deb.nodesource.com/setup_15.x | bash - && \
    apt-get install -y nodejs

COPY *.csproj ./
RUN dotnet restore

COPY . ./
WORKDIR /app/ClientApp
RUN npm install
WORKDIR /app
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["/app/TraefikPortal"]