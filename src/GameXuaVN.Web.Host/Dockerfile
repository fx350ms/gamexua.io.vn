FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["src/GameXuaVN.Web.Host/GameXuaVN.Web.Host.csproj", "src/GameXuaVN.Web.Host/"]
COPY ["src/GameXuaVN.Web.Core/GameXuaVN.Web.Core.csproj", "src/GameXuaVN.Web.Core/"]
COPY ["src/GameXuaVN.Application/GameXuaVN.Application.csproj", "src/GameXuaVN.Application/"]
COPY ["src/GameXuaVN.Core/GameXuaVN.Core.csproj", "src/GameXuaVN.Core/"]
COPY ["src/GameXuaVN.EntityFrameworkCore/GameXuaVN.EntityFrameworkCore.csproj", "src/GameXuaVN.EntityFrameworkCore/"]
WORKDIR "/src/src/GameXuaVN.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/GameXuaVN.Web.Host", "src/GameXuaVN.Web.Host"]
COPY ["src/GameXuaVN.Web.Core", "src/GameXuaVN.Web.Core"]
COPY ["src/GameXuaVN.Application", "src/GameXuaVN.Application"]
COPY ["src/GameXuaVN.Core", "src/GameXuaVN.Core"]
COPY ["src/GameXuaVN.EntityFrameworkCore", "src/GameXuaVN.EntityFrameworkCore"]
WORKDIR "/src/src/GameXuaVN.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "GameXuaVN.Web.Host.dll"]
