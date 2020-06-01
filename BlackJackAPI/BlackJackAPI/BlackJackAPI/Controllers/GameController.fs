namespace BlackJackAPI.Controllers

open System
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open BlackJackAPI.Models
open BlackJackAPI.Enums
open BlackJackAPI.Services

[<ApiController>]
[<Route("game")>]
type GameController (logger : ILogger<GameController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member __.Get() : Game =
        GameService.CreatedGame

