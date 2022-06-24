FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Mirero.DAQ.Test.Integration.Service.Workflow/Mirero.DAQ.Test.Integration.Service.Workflow.csproj", "Mirero.DAQ.Test.Integration.Service.Workflow/"]
RUN dotnet restore "Mirero.DAQ.Test.Integration.Service.Workflow/Mirero.DAQ.Test.Integration.Service.Workflow.csproj"
COPY . .
WORKDIR "/src/Mirero.DAQ.Test.Integration.Service.Workflow"
RUN dotnet build "Mirero.DAQ.Test.Integration.Service.Workflow.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mirero.DAQ.Test.Integration.Service.Workflow.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mirero.DAQ.Test.Integration.Service.Workflow.dll"]
