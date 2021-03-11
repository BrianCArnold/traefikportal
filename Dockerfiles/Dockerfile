FROM mcr.microsoft.com/dotnet/sdk:5.0

WORKDIR /app

RUN apt-get update -y && \
    curl -fsSL https://deb.nodesource.com/setup_15.x | bash - && \
    apt-get install -y nodejs
