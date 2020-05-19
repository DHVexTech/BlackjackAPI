namespace BlackJackAPI.Controllers

open System
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

[<ApiController>]
[<Route("")>]
type BaseController (logger : ILogger<BaseController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member __.Get() : string =
        "Server started"