FROM mcr.microsoft.com/dotnet/core/sdk:3.1
ARG BUILD_CONFIGURATION=Debug
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
EXPOSE 80

WORKDIR /src
COPY ["ExampleApp/ExampleApp.csproj", "ExampleApp/"]
COPY ["PdfLibCore/PdfLibCore.csproj", "PdfLibCore/"]
COPY . .
WORKDIR "/src/ExampleApp"
RUN dotnet build -c $BUILD_CONFIGURATION

RUN echo "exec dotnet run --no-build --no-launch-profile -c $BUILD_CONFIGURATION -- \"\$@\"" > /entrypoint.sh

ENTRYPOINT ["/bin/bash", "/entrypoint.sh"]