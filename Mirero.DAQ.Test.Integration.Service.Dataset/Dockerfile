FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Mirero.DAQ.Test.Integration.Service.Dataset/Mirero.DAQ.Test.Integration.Service.Dataset.csproj", "Mirero.DAQ.Test.Integration.Service.Dataset/"]
RUN dotnet restore "Mirero.DAQ.Test.Integration.Service.Dataset/Mirero.DAQ.Test.Integration.Service.Dataset.csproj"
COPY . .
WORKDIR "/src/Mirero.DAQ.Test.Integration.Service.Dataset"
RUN dotnet build "Mirero.DAQ.Test.Integration.Service.Dataset.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mirero.DAQ.Test.Integration.Service.Dataset.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mirero.DAQ.Test.Integration.Service.Dataset.dll"]
