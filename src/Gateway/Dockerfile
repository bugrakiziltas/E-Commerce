FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app
EXPOSE 80

# copy all .csproj files and restore distinct layers. Use of the same COPY command for every dockerfile in the project to take
# advantage of docker caching.
COPY MicroserviceGameStoreProject.sln MicroserviceGameStoreProject.sln
COPY src/ProductService/ProductService.csproj src/ProductService/ProductService.csproj
COPY src/SearchService/SearchService.csproj src/SearchService/SearchService.csproj
COPY src/OrderService/OrderService.csproj src/OrderService/OrderService.csproj
COPY src/ShoppingCartService/ShoppingCartService.csproj src/ShoppingCartService/ShoppingCartService.csproj
COPY src/StripeService/StripeService.csproj src/StripeService/StripeService.csproj
COPY src/Gateway/Gateway.csproj src/Gateway/Gateway.csproj
COPY src/Contracts/Contracts.csproj src/Contracts/Contracts.csproj
COPY src/IdentityServerAspNetIdentity/IdentityServerAspNetIdentity.csproj src/IdentityServerAspNetIdentity/IdentityServerAspNetIdentity.csproj

#Restore package deps
RUN dotnet restore MicroserviceGameStoreProject.sln

#COPY the app folders over with the dependencies
COPY src/Gateway src/Gateway
WORKDIR /app/src/Gateway
RUN dotnet publish -c Release -o /app/src/out

#Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 
WORKDIR /app
COPY --from=build /app/src/out .
ENTRYPOINT [ "dotnet", "Gateway.dll" ]