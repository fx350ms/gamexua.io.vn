# Sử dụng SDK .NET 8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Cài đặt công cụ libman
RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
RUN ln -s ~/.dotnet/tools/libman /usr/local/bin/libman

# Thư mục làm việc cho quá trình build
WORKDIR /src

# Sao chép file .csproj
COPY ["src/GameXuaVN.Web.Mvc/GameXuaVN.Web.Mvc.csproj", "GameXuaVN.Web.Mvc/"]
COPY ["src/GameXuaVN.Web.Core/GameXuaVN.Web.Core.csproj", "GameXuaVN.Web.Core/"]
COPY ["src/GameXuaVN.Application/GameXuaVN.Application.csproj", "GameXuaVN.Application/"]
COPY ["src/GameXuaVN.Core/GameXuaVN.Core.csproj", "GameXuaVN.Core/"]
COPY ["src/GameXuaVN.EntityFrameworkCore/GameXuaVN.EntityFrameworkCore.csproj", "GameXuaVN.EntityFrameworkCore/"]

# Restore các dependency
WORKDIR /src/GameXuaVN.Web.Mvc
RUN dotnet restore 

# Sao chép toàn bộ mã nguồn
WORKDIR /src
COPY src/ .

# Restore libman và build ứng dụng
WORKDIR /src/GameXuaVN.Web.Mvc
RUN libman restore
RUN dotnet publish -c Release -o /publish --no-restore

# Image chạy ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "GameXuaVN.Web.Mvc.dll"]
