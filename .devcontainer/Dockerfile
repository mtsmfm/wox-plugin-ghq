FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env

ENV LANG=C.UTF-8 LC_ALL=C.UTF-8

RUN apt update && apt install -y zsh less zip

RUN useradd --create-home --user-group --uid 1000 app
RUN mkdir -p /app /original
RUN chown -R app /app /original

WORKDIR /app

USER app

ENV SHELL=/bin/zsh

# # Copy csproj and restore as distinct layers
# COPY *.csproj ./
# RUN dotnet restore

# # Copy everything else and build
# COPY . ./
# RUN dotnet publish -c Release -o out

# # Build runtime image
# FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
# WORKDIR /app
# COPY --from=build-env /app/out .
# ENTRYPOINT ["dotnet", "aspnetapp.dll"]
