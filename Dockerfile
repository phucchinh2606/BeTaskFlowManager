# Sử dụng image .NET 10 SDK để build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy các file .csproj và restore package
# Lưu ý: Sửa tên thư mục "API", "Application"... cho khớp chính xác với tên thư mục trong dự án của bạn
COPY ["API/API.csproj", "API/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "API/API.csproj"

# Copy toàn bộ source code còn lại và tiến hành publish
COPY . .
WORKDIR "/src/API"
RUN dotnet publish "API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Sử dụng image .NET 10 ASP.NET để chạy runtime (image này nhẹ hơn rất nhiều)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Từ .NET 8 trở đi (bao gồm .NET 10), port mặc định trong Docker container là 8080
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080

# Chạy ứng dụng (Đổi API.dll thành tên file dll được gen ra từ project chính của bạn)
ENTRYPOINT ["dotnet", "API.dll"]