FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build-env

COPY ["material/Frontend/Frontend.csproj", "material/Frontend/"]
RUN dotnet restore "material/Frontend/Frontend.csproj"
COPY "material/Frontend/" "material/Frontend/"
RUN dotnet build "material/Frontend/Frontend.csproj" -c Release -o /build

FROM build-env AS publish
RUN dotnet publish "material/Frontend/Frontend.csproj" -c Release -o /publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /publish/wwwroot /usr/local/webapp/nginx/html
COPY --from=build-env "material/Frontend/ngnix.conf" /etc/nginx/nginx.conf
EXPOSE 80