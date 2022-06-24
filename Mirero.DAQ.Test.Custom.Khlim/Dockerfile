FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Sample.Test.Custom/Sample.Test.Custom.csproj", "Sample.Test.Custom/"]
RUN dotnet restore "Sample.Test.Custom/Sample.Test.Custom.csproj"
COPY . .
WORKDIR "/src/Sample.Test.Custom"
RUN dotnet build "Sample.Test.Custom.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sample.Test.Custom.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.Test.Custom.dll"]
