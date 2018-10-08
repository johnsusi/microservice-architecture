FROM microsoft/dotnet:2.2-sdk AS builder

WORKDIR /root/

COPY client.csproj .
RUN dotnet restore client.csproj

COPY . ./
RUN dotnet publish -c release -o dist client.csproj

FROM microsoft/dotnet:2.2-runtime

WORKDIR /root/
COPY --from=builder /root/dist .
COPY *.pem ./

CMD ["dotnet", "./client.dll"]