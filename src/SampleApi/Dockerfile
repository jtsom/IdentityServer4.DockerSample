FROM microsoft/dotnet:latest

COPY ./bin/Debug/netcoreapp1.0/publish /app

WORKDIR /app

ENTRYPOINT ["dotnet", "SampleApi.dll"]