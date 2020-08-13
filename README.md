# Ticker
⌚ 🙉
# What, why and how
`Ticker` let's you listen for changes in a series of objects implementing ITrack
```
public interface ITrack
{
    public DateTime Start { get; }
    public DateTime Stop { get; }
}
```
Internally this is implemented by using a `Queue<T>` so any expired ITrack is gone, `Ticker` is not tracking expired `ITrack` objects

## Basic usage
First you need to create a `Ticker`, using `TickerBuilder` in this example
```
var builder = new TickerBuilder();
var ticker = builder
                .OnTrackChanged(Console.WriteLine) //any failure in any handler will cause the chain to break
                .OnTrackChanged(Console.WriteLine) //you can use sync
                .OnTrackChanged(async e => await OnChanged(e)) //or async handlers
                .Precision(TimeSpan.FromSeconds(1)) //indicates how fast will internal clock tick
                .Build();

```
To track objects, you need to create a channel, channel contains objects of a given type implementing `ITrack`
You can have many channels, all tracking different type of objects
```
var tracks = new List<ExampleTrack>
{
    new ExampleTrack {Start = DateTime.Now.AddSeconds(-1), Stop = DateTime.Now.AddSeconds(3), Title = "1"},
    new ExampleTrack {Start = DateTime.Now.AddSeconds(3), Stop = DateTime.Now.AddSeconds(5), Title = "2"},
    new ExampleTrack {Start = DateTime.Now.AddSeconds(5), Stop = DateTime.Now.AddSeconds(7), Title = "3"}
};
ticker.AddChannel("identifier", tracks); //identifier needs to be unique, otherwise you'll overwrite an existing channel
```
If you're happy with your setup you can start `Ticker`
```
ticker.Start(); //this method will not block
```
