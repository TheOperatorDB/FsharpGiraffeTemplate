module FsharpGiraffeTemplate.Api.Handlers.Errors

open Giraffe
open Microsoft.Extensions.Logging
open System

exception MissingRequiredQueryParam of Name: string

let errorHandler (exn: Exception) (logger: ILogger) : HttpHandler =
    fun next ctx ->
        match exn with
        | MissingRequiredQueryParam name -> RequestErrors.BAD_REQUEST $"Missing query parameter '{name}'" next ctx
        | other ->
            logger.LogError(other, "Uncaught exception when handling request")

            (clearResponse
             >=> ServerErrors.INTERNAL_ERROR "Internal Server Error")
                next
                ctx
