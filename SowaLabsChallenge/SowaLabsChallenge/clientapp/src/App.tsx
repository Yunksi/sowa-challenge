import React, { ChangeEvent, FC, useEffect, useState } from 'react';
import styled from 'styled-components';

import * as Highcharts from 'highcharts';
import HighchartsReact from 'highcharts-react-official';
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
} from '@microsoft/signalr';
import { OrderBookDepth } from './interfaces/OrderBookDepth';

export const App: FC = () => {
  const [chartOptions, setChartOptions] = useState<Highcharts.Options>();
  const [currency, setCurrency] = useState<string>('BTCEUR');
  const [hubConnection, setHubConnection] = useState<HubConnection>();
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [orderBook, setOrderBook] = useState<OrderBookDepth>();
  const [bids, setBids] = useState<number[][]>();
  const [asks, setAsks] = useState<number[][]>();

  const apiUrl = process.env.REACT_APP_API_URL as string;

  useEffect(() => {
    const setUpSignalRConnection = async () => {
      const connection = new HubConnectionBuilder()
        .withUrl(apiUrl)
        .withAutomaticReconnect()
        .build();

      connection.on('updateOrderBook', (orderBookDepth: string) => {
        const orderBookDepthData = JSON.parse(orderBookDepth) as OrderBookDepth;
        setBids(orderBookDepthData.bids.reverse());
        setAsks(orderBookDepthData.asks);
        setOrderBook(orderBookDepthData);
        if (isLoading) setIsLoading(false);
      });

      try {
        await connection.start();
      } catch (err) {
        console.error(err);
      }

      if (connection.state === HubConnectionState.Connected) {
        connection.invoke('AddToGroup', currency).catch((err: Error) => {
          return console.error(err.toString());
        });
      }

      setHubConnection(connection);
    };
    setUpSignalRConnection();
  }, []);

  useEffect(() => {
    const config: Highcharts.Options = {
      chart: {
        type: 'area',
        zoomType: 'xy',
      },
      title: {
        text: 'Market Depth',
      },
      xAxis: {
        minPadding: 0,
        maxPadding: 0,
        title: {
          text: 'Price',
        },
      },
      yAxis: [
        {
          lineWidth: 1,
          gridLineWidth: 1,
          title: undefined,
          tickWidth: 1,
          tickLength: 5,
          tickPosition: 'inside',
          labels: {
            align: 'left',
            x: 8,
          },
        },
        {
          opposite: true,
          linkedTo: 0,
          lineWidth: 1,
          gridLineWidth: 0,
          title: undefined,
          tickWidth: 1,
          tickLength: 5,
          tickPosition: 'inside',
          labels: {
            align: 'right',
            x: -8,
          },
        },
      ],
      legend: {
        enabled: false,
      },
      plotOptions: {
        area: {
          fillOpacity: 0.2,
          lineWidth: 1,
          step: 'center',
        },
      },
      tooltip: {
        headerFormat:
          '<span style="font-size=10px;">Price: {point.key}</span><br/>',
        valueDecimals: 2,
      },
      series: [
        {
          name: 'Bids',
          type: 'area',
          data: bids,
          color: '#03a7a8',
        },
        {
          name: 'Asks',
          type: 'area',
          data: asks,
          color: '#fc5857',
        },
      ],
    };

    setChartOptions(config);
  }, [bids, asks]);

  const currencyPairChanged = (event: ChangeEvent<HTMLSelectElement>) => {
    hubConnection?.invoke('RemoveFromGroup', currency);
    setCurrency(event.target.value);
    hubConnection?.invoke('AddToGroup', event.target.value);
    setIsLoading(true);
  };

  return (
    <div>
      {isLoading ? (
        <div>Loading Data</div>
      ) : (
        <div>
          <HighchartsReact highcharts={Highcharts} options={chartOptions} />
          <OrderBookListContainer>
            <div style={{ display: 'flex', justifyContent: 'space-between' }}>
              <OrderBookTitle>Order Book</OrderBookTitle>
              <select
                name='currencyPair'
                id='currencyPair'
                value={currency}
                onChange={(event) => currencyPairChanged(event)}
              >
                <option value='BTCEUR'>BTCEUR</option>
                <option value='BTCUSD'>BTCUSD</option>
              </select>
            </div>

            <OrderBookHeader>
              <div>Price(USD)</div>
              <div>Amount(BTC)</div>
              <div>Total</div>

              {/* {orderBook?.top10Asks.map((ask) => {
                return (
                  <div key={ask[0]}>
                    <h1>{ask[0]}</h1>
                    <h2>{ask[1]}</h2>
                  </div>
                );
              })} */}
            </OrderBookHeader>
            <OrderBookList>
              <h3 style={{ alignSelf: 'center' }}>Asks</h3>
              <div style={{ display: 'flex', flexDirection: 'column-reverse' }}>
                {orderBook?.top10Asks.map((ask) => {
                  return (
                    <Item key={ask[0]}>
                      <p>{ask[0].toFixed(2)}</p>
                      <p>{ask[1].toFixed(5)}</p>
                      <p>{(ask[0] * ask[1]).toFixed(2)}</p>
                    </Item>
                  );
                })}
              </div>

              <h3 style={{ alignSelf: 'center' }}>Bids</h3>
              {orderBook?.top10Bids.map((bid) => {
                return (
                  <Item key={bid[0]}>
                    <p>{bid[0].toFixed(2)}</p>
                    <p>{bid[1].toFixed(5)}</p>
                    <p>{(bid[0] * bid[1]).toFixed(2)}</p>
                  </Item>
                );
              })}
            </OrderBookList>
            {/* <BidsContainer></BidsContainer> */}

            {/* {orderBook?.top10Bids.map((bid) => {
              return (
                <div key={bid[0]}>
                  <h1>{bid[0]}</h1>
                  <h2>{bid[1]}</h2>
                </div>
              );
            })} */}
            {/* <OrderBookList>Order Book</OrderBookList> */}
          </OrderBookListContainer>
        </div>
      )}
    </div>
  );
};

const OrderBookListContainer = styled.div`
  display: flex;
  flex-direction: column;
`;

const OrderBookTitle = styled.h3`
  color: black;
  align-self: center;
`;

const OrderBookHeader = styled.div`
  margin: 16px 16px;
  flex-direction: row;
  justify-content: space-between;
  display: flex;
`;

const OrderBookList = styled.div`
  display: flex;
  flex-direction: column;
`;

const Item = styled.div`
  display: flex;
  margin: 0px 16px;
  flex-direction: row;
  justify-content: space-between;
`;
