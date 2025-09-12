module FsharpGiraffeTemplate.Api.Handlers.PingHandler

open Giraffe
open Thoth.Json.Giraffe

let getPingMessage () : HttpHandler =
    fun next ctx ->
        task {
            let response = ThothSerializer.RespondRawJson "Hello Edelweiss Connect!"

            return! response next ctx
        }
