FROM microsoft/dotnet:1.1-sdk AS build-env
WORKDIR /app

# Copiar csproj e restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Build da aplicacao
COPY . ./
RUN dotnet publish -c Release -o out

# Build da imagem
FROM microsoft/dotnet:1.1.0-runtime
WORKDIR /app
COPY --from=build-env /app/out .
CMD ["dotnet", "qap4.dll"]

EXPOSE 80/tcp
EXPOSE 80/udp