FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
RUN ln -s ~/.dotnet/tools/libman /usr/local/bin/libman

WORKDIR /src
COPY ["src/GameXuaVN.Web.Mvc/GameXuaVN.Web.Mvc.csproj", "src/GameXuaVN.Web.Mvc/"]
COPY ["src/GameXuaVN.Web.Core/GameXuaVN.Web.Core.csproj", "src/GameXuaVN.Web.Core/"]
COPY ["src/GameXuaVN.Application/GameXuaVN.Application.csproj", "src/GameXuaVN.Application/"]
COPY ["src/GameXuaVN.Core/GameXuaVN.Core.csproj", "src/GameXuaVN.Core/"]
COPY ["src/GameXuaVN.EntityFrameworkCore/GameXuaVN.EntityFrameworkCore.csproj", "src/GameXuaVN.EntityFrameworkCore/"]
WORKDIR "/src/src/GameXuaVN.Web.Mvc"
RUN dotnet restore 

WORKDIR /src
COPY ["src/GameXuaVN.Web.Mvc", "src/GameXuaVN.Web.Mvc"]
COPY ["src/GameXuaVN.Web.Core", "src/GameXuaVN.Web.Core"]
COPY ["src/GameXuaVN.Application", "src/GameXuaVN.Application"]
COPY ["src/GameXuaVN.Core", "src/GameXuaVN.Core"]
COPY ["src/GameXuaVN.EntityFrameworkCore", "src/GameXuaVN.EntityFrameworkCore"]
WORKDIR "/src/src/GameXuaVN.Web.Mvc"
RUN libman restore
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "GameXuaVN.Web.Mvc.dll"]
