# Ticker
⌚ 🙉
## Basic usage
```
var builder = new TickerBuilder();
var ticker = builder
                .OnTrackChanged(Console.WriteLine) //any failure in any handler will cause the chain to break
                .OnTrackChanged(Console.WriteLine) //you can use sync
                .OnTrackChanged(async e => await OnChanged(e)) //or async handlers
                .Precision(TimeSpan.FromSeconds(1)) //indicates how fast will internal clock tick
                .Build();
ticker.Start(); //this method will not block
```
