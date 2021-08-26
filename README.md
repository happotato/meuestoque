# MeuEstoque

## Dependencies

- [NodeJS](https://nodejs.org/)
- [.NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0)

## Setup

```bash
# Clone repository
git clone https://github.com/happotato/meuestoque.git

# Change to directory
cd meuestoque

# Install dependencies
dotnet restore

# Build
dotnet build
```

### Running

#### With SQLServer

```bash
dotnet run DB:SQLServer:ConnectionString="<string>"
```

#### With SQLite

```bash
dotnet run DB:SQLite:ConnectionString="<string>"
```

## License

[GPLv3](LICENSE.txt)