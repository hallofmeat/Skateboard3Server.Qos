FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app 

# Restore
COPY *.sln .
COPY src/Skateboard3Server.Qos/*.csproj ./src/Skateboard3Server.Qos/
COPY tests/Skateboard3Server.Qos.Tests/*.csproj ./tests/Skateboard3Server.Qos.Tests/ 
RUN dotnet restore 

# Build/Publish
COPY src/Skateboard3Server.Qos/. ./src/Skateboard3Server.Qos/
COPY tests/Skateboard3Server.Qos.Tests/. ./tests/Skateboard3Server.Qos.Tests/ 
WORKDIR /app/src/Skateboard3Server.Qos
#TODO: --no-restore
RUN dotnet publish -c Release -o /app/publish

#TODO: unit tests

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app 

# qos http
EXPOSE 17502
# qos ping/bandwidth
EXPOSE 17499
# qos firewall
EXPOSE 17500
# qos firewall 2
#EXPOSE 17500

COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "Skateboard3Server.Qos.dll"]