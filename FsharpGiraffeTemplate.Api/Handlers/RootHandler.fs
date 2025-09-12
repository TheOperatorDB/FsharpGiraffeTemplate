module FsharpGiraffeTemplate.Api.Handlers.RootHandler

open Giraffe

open FsharpGiraffeTemplate.Api

let routes: HttpHandler =
    choose [
        route "/" >=> GET >=> PingHandler.getPingMessage()
        RequestErrors.NOT_FOUND "Not Found"
    ]
