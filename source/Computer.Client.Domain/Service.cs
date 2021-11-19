using Microsoft.Extensions.Hosting;

namespace Computer.Client.Domain
{
    public class Service : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Starting service...");
            var tSource = new TaskCompletionSource();
            stoppingToken.Register(() => tSource.TrySetCanceled());
            await tSource.Task;
        }
    }
}