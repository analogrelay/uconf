## Build image
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build

# Work in the /src directory
WORKDIR /src

# Copy the csproj and restore it so that we can cache images with the NuGet packages downloaded
# NOTE: When we depend on other projects, we'll have to bring them in here as well.
COPY *.csproj /src/
RUN dotnet restore

# Copy the rest of the app in and publish
# Don't use standalone publish. We'll use a base image with the runtime.
COPY . /src/
RUN dotnet publish --output /publish

## Runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runtime
WORKDIR /app
COPY --from=build /publish /app
ENTRYPOINT [ "./uConf.Services.Playground" ]