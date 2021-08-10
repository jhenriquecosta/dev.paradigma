FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
VOLUME 

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src

# Skip extraction of XML documentation for nuget packages
ENV NUGET_XMLDOC_MODE=skip 

COPY ["src/common/Works.Imob.Common.csproj", "src/common/"]
COPY ["src/domain/Works.Imob.Domain.csproj", "src/domain/"]
COPY ["src/dataaccess/Works.Imob.DataAccess.csproj", "src/dataaccess/"]
COPY ["src/application/Works.Imob.Application.csproj", "src/application/"]
COPY ["src/ui.razor/Works.Imob.Ui.Razor.csproj", "src/ui.razor/"]
COPY ["src/ui.blazor/Works.Imob.Ui.Blazor.csproj", "src/ui.blazor/"]

RUN dotnet restore --source /nupkg

WORKDIR "/src/src/ui.blazor"
 

RUN dotnet build "Works.Imob.Ui.Blazor.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "Works.Imob.Ui.Blazor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Works.Imob.Ui.Blazor.dll"]