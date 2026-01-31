FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /build
COPY ["./JoyModels.API/JoyModels.API.csproj", "JoyModels.API/"]
COPY ["./JoyModels.Models/JoyModels.Models.csproj", "JoyModels.Models/"]
COPY ["./JoyModels.Services/JoyModels.Services.csproj", "JoyModels.Services/"]
COPY ["./JoyModels.Utilities/JoyModels.Utilities.csproj", "JoyModels.Utilities/"]
RUN dotnet restore "./JoyModels.API/JoyModels.API.csproj"
COPY . .
WORKDIR "/build/JoyModels.API/"
RUN dotnet build "JoyModels.API.csproj" -c Release -o /build/bin/
RUN dotnet publish "JoyModels.API.csproj" -c Release -o /build/publish/

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=build /build/publish .
CMD ["sh", "-c", "dotnet /app/JoyModels.API.dll"]