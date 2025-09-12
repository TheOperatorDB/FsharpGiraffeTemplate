module FsharpGiraffeTemplate.Api.App

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open System
open System.IO

let configuration =
    ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional = false)
        .AddJsonFile("appsettings.Development.json", optional = true)
        .AddEnvironmentVariables()
        .Build()

let configureCors (app: WebApplication) =
    app.UseCors(fun builder ->
        builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
        |> ignore)
    |> ignore

let configureApp (app: IApplicationBuilder) =
    app.UseCors(fun corsPolicyBuilder -> corsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod() |> ignore)
    |> ignore

    app
        .UseGiraffeErrorHandler(Handlers.Errors.errorHandler)
        .UseGiraffe(Handlers.RootHandler.routes)

let configureServices (services: IServiceCollection) =
    services.AddCors() |> ignore
    services.AddGiraffe() |> ignore

let configureLogging (builder: ILoggingBuilder) =
    builder.AddConsole().AddDebug() |> ignore

[<EntryPoint>]
let main args =
    let contentRoot = Directory.GetCurrentDirectory()

    Host
        .CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(fun webHostBuilder ->
            webHostBuilder
                .UseContentRoot(contentRoot)
                .Configure(Action<IApplicationBuilder> configureApp)
                .ConfigureServices(configureServices)
                .ConfigureLogging(configureLogging)
            |> ignore)
        .Build()
        .Run()

    0
