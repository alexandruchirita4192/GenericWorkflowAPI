# Build from the repository root:
#   docker build -f src/GenericWorkflowAPI/Dockerfile -t genericworkflowapi-api .

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/GenericWorkflowAPI/GenericWorkflowAPI.csproj", "GenericWorkflowAPI/"]
COPY ["src/GenericWorkflowAPI.Core/GenericWorkflowAPI.Core.csproj", "GenericWorkflowAPI.Core/"]
COPY ["src/GenericWorkflowAPI.Database/GenericWorkflowAPI.Database.csproj", "GenericWorkflowAPI.Database/"]
COPY ["src/GenericWorkflowAPI.Domain/GenericWorkflowAPI.Domain.csproj", "GenericWorkflowAPI.Domain/"]
RUN dotnet restore "GenericWorkflowAPI/GenericWorkflowAPI.csproj"

COPY ["src/", "."]
WORKDIR "/src/GenericWorkflowAPI"
RUN dotnet publish "GenericWorkflowAPI.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GenericWorkflowAPI.dll"]
