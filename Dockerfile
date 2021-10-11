FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY MeuEstoque.Web/MeuEstoque.Web.csproj MeuEstoque.Web/MeuEstoque.Web.csproj
COPY MeuEstoque.Domain/MeuEstoque.Domain.csproj MeuEstoque.Domain/MeuEstoque.Domain.csproj
COPY MeuEstoque.Infrastrucure/MeuEstoque.Infrastrucure.csproj MeuEstoque.Infrastrucure/MeuEstoque.Infrastrucure.csproj
COPY MeuEstoque.sln MeuEstoque.sln

RUN dotnet restore
COPY . ./

RUN dotnet publish -c Release -o out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY --from=build-env /app/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "MeuEstoque.dll", "--urls", "http://*:80"]