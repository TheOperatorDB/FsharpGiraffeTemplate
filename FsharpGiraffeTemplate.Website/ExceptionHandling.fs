module FsharpGiraffeTemplate.Website.ExceptionHandling

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc.Filters

exception Unauthorized
exception EntityNotFound

type DomainErrorExceptionFilter() =
    interface IExceptionFilter with
        member this.OnException(context) =
            match context.Exception with
            | Unauthorized -> context.Result <- ForbidResult()
            | EntityNotFound -> context.Result <- NotFoundResult()
            | _ -> ()
