namespace Functional.UniversalBot.CLI2

open System
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Serilog
open Serilog.Settings.Configuration
open Microsoft.Extensions.Configuration
open System.IO

module Program =
    let private createLogger (hostingContext: HostBuilderContext) (logger: LoggerConfiguration) = 
        logger
            .ReadFrom
            .Configuration(hostingContext.Configuration, ConfigurationAssemblySource.AlwaysScanDllFiles)
        |> ignore

    let private addMultipleConfigFiles path (builder: IConfigurationBuilder)  =
        if Directory.Exists path 
        then
            Directory.GetFiles(path, "*.json")
            |> Seq.map (fun file -> builder.AddJsonFile(file, true))
            |> ignore

    let createHostBuilder args =
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(fun builder ->
                builder.AddJsonFile("configuration.json", true) |> ignore
                builder.AddUserSecrets() |> ignore
                builder |> addMultipleConfigFiles "actions")
            .ConfigureServices(fun hostContext services ->
                let configuration =
                    hostContext.Configuration
                    |> Configuration.getConfiguration
                services.AddSingleton (configuration) |> ignore
                let timeProvider () = DateTime.Now
                services.AddSingleton<unit -> DateTime> (timeProvider) |> ignore
                services.AddHostedService<Workers.ScheduleWorker>() |> ignore
                services.AddHostedService<Workers.ContinousWorker>() |> ignore)
            .UseSerilog(createLogger)

    [<EntryPoint>]
    let main args =
        createHostBuilder(args)
            .Build()
            .Run()

        0 // exit code
