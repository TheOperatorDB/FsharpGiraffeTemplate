module FsharpGiraffeTemplate.Website.Startup

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Mvc.ApplicationParts
open Microsoft.AspNetCore.Routing
open Microsoft.Extensions.DependencyInjection
open System.IO
open System.Reflection

open FsharpGiraffeTemplate.Website.ExceptionHandling
open FsharpGiraffeTemplate.Website.Utils.ModelBinding

let registerCompiledViewsAssembly (mvcBuilder: IMvcBuilder) =
    let currentAssembly = Assembly.GetExecutingAssembly()
    let folderPath = Path.GetDirectoryName(currentAssembly.Location)

    let assemblyPath =
        Path.Combine(folderPath, currentAssembly.GetName().Name + ".Views.dll")

    let viewsAssembly = Assembly.LoadFrom(assemblyPath)
    let viewsApplicationPart = CompiledRazorAssemblyPart(viewsAssembly)

    mvcBuilder.ConfigureApplicationPartManager(fun manager -> manager.ApplicationParts.Add(viewsApplicationPart))
    |> ignore

let configureHost (builder: WebApplicationBuilder) : WebApplicationBuilder =
    let urls =
        [| "http://0.0.0.0:5002/"
           "https://0.0.0.0:5001/" |]

    builder.WebHost.UseUrls(urls) |> ignore

    builder

let configureServices (builder: WebApplicationBuilder) : WebApplicationBuilder =
    builder.Services
        .Configure<RouteOptions>(fun (options: RouteOptions) -> options.LowercaseUrls <- true)
    |> ignore

    builder.Services.AddHttpContextAccessor()
    |> ignore

    let mvcBuilder =
        builder.Services
            .AddControllersWithViews(fun options ->
                options.ModelBinderProviders.Insert(0, EmptyResizeArrayModelBinderProvider())

                options.Filters.Add<DomainErrorExceptionFilter>()
                |> ignore)

    // tell Razor Engine where to look for compiled assemblies
    registerCompiledViewsAssembly mvcBuilder

#if DEBUG
    // only do runtime compilation when developing
    mvcBuilder.AddRazorRuntimeCompilation() |> ignore
#endif

    builder.Services.AddRazorPages() |> ignore

    builder

let configureApp (app: WebApplication) : WebApplication =
    app
        .UseHttpMethodOverride(
            let o = HttpMethodOverrideOptions()
            o.FormFieldName <- "X-Http-Method-Override"
            o
        )
        .UseHttpsRedirection()
        .UseForwardedHeaders()
        .UseStaticFiles()
        .UseRouting()
    |> ignore

    app.MapDefaultControllerRoute() |> ignore

    app

let build (builder: WebApplicationBuilder) : WebApplication =
    builder.Build()
