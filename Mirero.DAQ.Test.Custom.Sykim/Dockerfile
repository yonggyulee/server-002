FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Mirero.DAQ.Test.Custom.Sykim/Mirero.DAQ.Test.Custom.Sykim.csproj", "Mirero.DAQ.Test.Custom.Sykim/"]
RUN dotnet restore "Mirero.DAQ.Test.Custom.Sykim/Mirero.DAQ.Test.Custom.Sykim.csproj"
COPY . .
WORKDIR "/src/Mirero.DAQ.Test.Custom.Sykim"
RUN dotnet build "Mirero.DAQ.Test.Custom.Sykim.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mirero.DAQ.Test.Custom.Sykim.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mirero.DAQ.Test.Custom.Sykim.dll"]
