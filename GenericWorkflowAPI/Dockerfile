#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["GenericWorkflowAPI/GenericWorkflowAPI.csproj", "GenericWorkflowAPI/"]
COPY ["GenericWorkflowAPI.Database/GenericWorkflowAPI.Database.csproj", "GenericWorkflowAPI.Database/"]
COPY ["GenericWorkflowAPI.Domain/GenericWorkflowAPI.Domain.csproj", "GenericWorkflowAPI.Domain/"]
COPY ["GenericWorkflowAPI.Core/GenericWorkflowAPI.Core.csproj", "GenericWorkflowAPI.Core/"]
RUN dotnet restore "GenericWorkflowAPI/GenericWorkflowAPI.csproj"
COPY . .
WORKDIR "/src/GenericWorkflowAPI"
RUN dotnet build "GenericWorkflowAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GenericWorkflowAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GenericWorkflowAPI.dll"]