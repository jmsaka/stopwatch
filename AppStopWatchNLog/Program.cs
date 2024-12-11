using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System.Diagnostics;

namespace AppStopWatchNLog;

public static class Program
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureLogging((context, logging) =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                logging.AddNLog(); 
            })
            .ConfigureServices((context, services) =>
            {
                // IoC de serviços extras
            })
            .Build();

        await RunStopwatchSimulationAsync();
        await host.RunAsync();
    }

    private static async Task RunStopwatchSimulationAsync()
    {
        var stopwatch = new Stopwatch();

        var temposExecucao = new Dictionary<string, long>();

        try
        {
            // Medir a Rotina 1
            stopwatch.Start();
            await Rotina1();
            stopwatch.Stop();
            temposExecucao["Rotina1"] = stopwatch.ElapsedMilliseconds;
            Logger.Info("Tempo de execução da Rotina1: {0} ms", stopwatch.ElapsedMilliseconds);

            // Medir a Rotina 2
            stopwatch.Reset();
            stopwatch.Start();
            await Rotina2();
            stopwatch.Stop();
            temposExecucao["Rotina2"] = stopwatch.ElapsedMilliseconds;
            Logger.Info("Tempo de execução da Rotina2: {0} ms", stopwatch.ElapsedMilliseconds);

            // Medir a Rotina 3
            stopwatch.Reset();
            stopwatch.Start();
            await Rotina3();
            stopwatch.Stop();
            temposExecucao["Rotina3"] = stopwatch.ElapsedMilliseconds;
            Logger.Info("Tempo de execução da Rotina3: {0} ms", stopwatch.ElapsedMilliseconds);

            Logger.Info("Resumo dos tempos de execução:");
            foreach (var tempo in temposExecucao)
            {
                Logger.Info("{0}: {1} ms", tempo.Key, tempo.Value);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Ocorreu um erro durante a medição de tempos.");
        }
    }

    private static async Task Rotina1() => await Task.Delay(500);

    private static async Task Rotina2() => await Task.Delay(300);

    private static async Task Rotina3() => await Task.Delay(700);
}
