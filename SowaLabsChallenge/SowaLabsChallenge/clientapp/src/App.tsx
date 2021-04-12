import React, { FC, useEffect, useState } from 'react';
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
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [orderBook, setOrderBook] = useState<OrderBookDepth>();

  const apiUrl = process.env.REACT_APP_API_URL as string;

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
          data: [],
          color: '#03a7a8',
        },
        {
          name: 'Asks',
          type: 'area',
          data: [],
          color: '#fc5857',
        },
      ],
    };

    const setUpSignalRConnection = async () => {
      const connection = new HubConnectionBuilder()
        .withUrl(apiUrl)
        .withAutomaticReconnect()
        .build();

      connection.on('updateOrderBook', (orderBookDepth: string) => {
        const orderBookDepthData = JSON.parse(orderBookDepth) as OrderBookDepth;
        const options: Highcharts.Options = {
          series: [
            {
              name: 'Bids',
              type: 'area',
              data: orderBookDepthData.bids,
              color: '#03a7a8',
            },
            {
              name: 'Asks',
              type: 'area',
              data: orderBookDepthData.asks,
              color: '#fc5857',
            },
          ],
        };

        setChartOptions(options);
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

      // setHubConnection(connection);
    };
    // setUpSignalRConnection();
    setChartOptions(config);
  }, []);

  return (
    <div>
      {isLoading ? (
        <div>Loading Data</div>
      ) : (
        <div>
          <HighchartsReact highcharts={Highcharts} options={chartOptions} />
          <OrderBookListContainer>
            <OrderBookTitle>Order Book</OrderBookTitle>
            <AsksContainer></AsksContainer>
            <BidsContainer></BidsContainer>
            {orderBook?.top10Asks.map((ask) => {
              return (
                <h1>
                  {ask[0]}
                  {ask[1]}
                </h1>
              );
            })}
            {orderBook?.top10Bids.map((bid) => {
              return (
                <div>
                  <h1>{bid[0]}</h1>
                  <h2>{bid[1]}</h2>
                </div>
              );
            })}
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
  align-items: center;
  background-color: red;
`;

const OrderBookTitle = styled.h2`
  color: black;
`;

const AsksContainer = styled.div``;
const BidsContainer = styled.div``;
