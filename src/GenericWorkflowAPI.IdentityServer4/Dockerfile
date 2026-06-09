# Build from the repository root:
#   docker build -f src/GenericWorkflowAPI.IdentityServer4/Dockerfile -t genericworkflowapi-identityserver4 .

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY ["src/GenericWorkflowAPI.IdentityServer4/GenericWorkflowAPI.IdentityServer4.csproj", "GenericWorkflowAPI.IdentityServer4/"]
COPY ["src/GenericWorkflowAPI.Core/GenericWorkflowAPI.Core.csproj", "GenericWorkflowAPI.Core/"]
COPY ["src/GenericWorkflowAPI.Database/GenericWorkflowAPI.Database.csproj", "GenericWorkflowAPI.Database/"]
COPY ["src/GenericWorkflowAPI.Domain/GenericWorkflowAPI.Domain.csproj", "GenericWorkflowAPI.Domain/"]
RUN dotnet restore "GenericWorkflowAPI.IdentityServer4/GenericWorkflowAPI.IdentityServer4.csproj"

COPY ["src/", "."]
WORKDIR "/src/GenericWorkflowAPI.IdentityServer4"
RUN dotnet publish "GenericWorkflowAPI.IdentityServer4.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GenericWorkflowAPI.IdentityServer4.dll"]
