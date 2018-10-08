FROM microsoft/dotnet:2.2-sdk AS builder

WORKDIR /root/

COPY server.csproj .
RUN dotnet restore server.csproj

COPY . ./
RUN dotnet publish -c release -o dist server.csproj

FROM microsoft/dotnet:2.2-runtime

WORKDIR /root/
COPY --from=builder /root/dist .
COPY *.pem ./

EXPOSE 80/tcp 443/tcp
CMD ["dotnet", "./server.dll"]