using Discord;
using Discord.Commands;
using Discord.WebSocket;

public class Program
{
    public static Task Main(string[] args) => new Program().MainAsync();

    private readonly DiscordSocketClient _client;

    public Program()
    {
        _client = new DiscordSocketClient();
        _client.Log += Log;
    }

    public async Task MainAsync()
    {

        await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("FitiToken"));
        await _client.StartAsync();

        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}