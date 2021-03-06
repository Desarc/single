FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["singleApp/singleApp.csproj", "./"]
RUN dotnet restore "./singleApp.csproj"
COPY . .
WORKDIR "/src/singleApp"
RUN dotnet build "singleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "singleApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "singleApp.dll"]