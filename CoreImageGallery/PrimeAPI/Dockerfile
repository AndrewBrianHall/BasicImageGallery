FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY CoreImageGallery.sln ./
COPY PrimeAPI/PrimeAPI.csproj PrimeAPI/
COPY Primes.Lib/Primes.Lib.csproj Primes.Lib/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/PrimeAPI
RUN dotnet build PrimeAPI.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish PrimeAPI.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PrimeAPI.dll"]
