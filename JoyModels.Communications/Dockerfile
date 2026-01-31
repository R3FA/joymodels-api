FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /build
COPY ["./JoyModels.Communications/JoyModels.Communications.csproj", "JoyModels.Communications/"]
COPY ["./JoyModels.Models/JoyModels.Models.csproj", "JoyModels.Models/"]
COPY ["./JoyModels.Utilities/JoyModels.Utilities.csproj", "JoyModels.Utilities/"]
RUN dotnet restore "./JoyModels.Communications/JoyModels.Communications.csproj"
COPY . .
WORKDIR "/build/JoyModels.Communications/"
RUN dotnet build "JoyModels.Communications.csproj" -c Release -o /build/bin/
RUN dotnet publish "JoyModels.Communications.csproj" -c Release -o /build/publish/

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /build/publish .
CMD ["sh", "-c", "dotnet /app/JoyModels.Communications.dll"]