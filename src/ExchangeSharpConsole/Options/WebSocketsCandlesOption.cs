using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using ExchangeSharp;
using ExchangeSharpConsole.Options.Interfaces;

namespace ExchangeSharpConsole.Options
{
	[Verb("ws-candles", HelpText =
		"Connects to the given exchange websocket and keeps printing the candles from that exchange." +
		"If market symbol is not set then uses all.")]
	public class WebSocketsCandlesOption : BaseOption, IOptionPerExchange, IOptionWithMultipleMarketSymbol
	{
		public override async Task RunCommand()
		{
			async Task<IWebSocket> GetWebSocket(IExchangeAPI api)
			{
				var symbols = await ValidateMarketSymbolsAsync(api, MarketSymbols.ToArray());

				return await api.GetCandlesWebSocketAsync(message =>
					{
						Console.WriteLine($"{message.Symbol}: {message.Candle}");
						return Task.CompletedTask;
					}, 60, symbols
				);
			}

			await RunWebSocket(ExchangeName, GetWebSocket);
		}

		public string ExchangeName { get; set; }

		public IEnumerable<string> MarketSymbols { get; set; }
	}
}
