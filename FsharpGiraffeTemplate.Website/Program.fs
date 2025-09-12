module FsharpGiraffeTemplate.Website.Program

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =
        let app =
            WebApplication.CreateBuilder(args)
            |> Startup.configureHost
            |> Startup.configureServices
            |> Startup.build
            |> Startup.configureApp

        app.Run()

        exitCode
