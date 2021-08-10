#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["src/ui.blazor/Works.Paradigma.App.Blazor.csproj", "src/ui.blazor/"]
COPY ["../../works.framework/impl/src/tools.log4net/Works.Log4Net.csproj", "../../works.framework/impl/src/tools.log4net/"]
COPY ["../../works.framework/impl/src/works.shared/Works.Shared.csproj", "../../works.framework/impl/src/works.shared/"]
COPY ["../../works.framework/impl/src/works/Works.csproj", "../../works.framework/impl/src/works/"]
COPY ["../../works.framework/impl/src/tools.automapper/Works.AutoMapper.csproj", "../../works.framework/impl/src/tools.automapper/"]
COPY ["src/ui.razor/Works.Paradigma.Ui.Razor.csproj", "src/ui.razor/"]
COPY ["src/application/Works.Paradigma.Application.csproj", "src/application/"]
COPY ["src/dataaccess/Works.Paradigma.DataAccess.csproj", "src/dataaccess/"]
COPY ["src/domain/Works.Paradigma.Domain.csproj", "src/domain/"]
COPY ["src/core/Works.Paradigma.Core.csproj", "src/core/"]
COPY ["../../works.framework/impl/src/dataaccess.fluentnhibernate/Works.FluentNHibernate.csproj", "../../works.framework/impl/src/dataaccess.fluentnhibernate/"]
COPY ["../../works.framework/impl/src/dataaccess.nhibernate/Works.NHibernate.csproj", "../../works.framework/impl/src/dataaccess.nhibernate/"]
COPY ["../../works.framework/impl/src/dataaccess/Works.DataAccess.csproj", "../../works.framework/impl/src/dataaccess/"]
COPY ["../../works.framework/impl/src/ui.blazor/Works.Ui.Blazor.csproj", "../../works.framework/impl/src/ui.blazor/"]
COPY ["../../works.framework/impl/src/tools.reports/Works.Reports.csproj", "../../works.framework/impl/src/tools.reports/"]
COPY ["../../works.framework/impl/src/web.common/Works.Web.Common.csproj", "../../works.framework/impl/src/web.common/"]
COPY ["../../works.framework/impl/src/web.aspnetcore/Works.AspNetCore.csproj", "../../works.framework/impl/src/web.aspnetcore/"]
RUN dotnet restore "src/ui.blazor/Works.Paradigma.App.Blazor.csproj"
COPY . .
WORKDIR "/src/src/ui.blazor"
RUN dotnet build "Works.Paradigma.App.Blazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Works.Paradigma.App.Blazor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Works.Paradigma.App.Blazor.dll"]