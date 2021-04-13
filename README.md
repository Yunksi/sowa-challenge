# sowa-challenge

## The task
Display the BTC/EUR market depth chart to the end user on a simple website; you can get the data from Bitstamp or any other exchange. The back-end should be written in .NET, for the front-end you can choose any framework you wish or evendo it without a framework.To make our task a bit more challenging, you should keep an audit log of every order book snapshot that is [potentially] displayed to the end user with the timestamp of when it was acquired.

## Project structure
1. SowaLabsChallenge contains solution files
2. SowaLabsChallenge/clientapp contains files of the FE
3. SowaLabsChallenge.Tests contains unit tests

## Running
1. Open Terminal
2. Go to SowaLabsChallenge project folder
3. Run `docker compose up`
4. API is hosted on `localhost:8080`, clientapp on `localhost:8080/clientapp`


## Known Issues
If you want to run application in docker, you must comment out Audit section (works ok, when running in Visual Studio)

For any other questions don't hesitate to ask me. 
