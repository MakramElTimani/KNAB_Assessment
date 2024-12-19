# KNAB_Assessment

This is a dotnet aspire project

You can run this easily from Visual Studio using the debug option

or you through the terminal

```
cd KNAB_Assessment/KNAB_Assessment.AppHost

dotnet run

```

then navigate to the URl that is logged

https://localhost:17034/


You will be presented with dotnet aspire dashboard with 2 services, API and WebApp

You can visit the WebApp URL to see the simple UI that I created for the test

Alternatively, you can use API url `https://localhost:7380/api/exchange/{symbol}/{comma-separated-currencies}` to test the API

### Setting up api key

After getting the api key from CoinMarketCap add it to user secrets

```

cd .\KNAB_Assessment.ApiService\

dotnet user-secrets set "CoinMarketCapSettings:ApiKey" "<REDACTED>"

```