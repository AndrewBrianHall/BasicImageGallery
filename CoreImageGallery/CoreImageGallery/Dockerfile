FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["CoreImageGallery/CoreImageGallery.csproj", "CoreImageGallery/"]
COPY ["ImageGallery.Model/ImageGallery.Model.csproj", "ImageGallery.Model/"]
RUN dotnet restore "CoreImageGallery/CoreImageGallery.csproj"
COPY . .
WORKDIR "/src/CoreImageGallery"
RUN dotnet build "CoreImageGallery.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoreImageGallery.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoreImageGallery.dll"]