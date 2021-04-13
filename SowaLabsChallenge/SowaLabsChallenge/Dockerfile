FROM node:15.14.0-alpine as build-fe
WORKDIR /orderbookapp
COPY clientapp/package.json .
RUN npm install
COPY clientapp/ .
RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:5.0 as build-environment
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-environment /app/out .
COPY --from=build-fe /orderbookapp/build ./clientapp/build
ENTRYPOINT ["dotnet", "SowaLabsChallenge.dll"]