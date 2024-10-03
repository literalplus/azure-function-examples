FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /App

COPY . ./

RUN dotnet restore

RUN dotnet build -c Release -o out

FROM cmr.microsoft.com/dotnet/runtime:8.0 AS runner
WORKDIR /App

COPY --from=builder /App/out .

ENTRYPOINT [ "dotnet", "az-redis.dll" ]
