namespace BlackJackAPI.Controllers

open System
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open BlackJackAPI.Models
open BlackJackAPI.Enums
open BlackJackAPI.Services

[<ApiController>]
[<Route("card")>]
type CardController (logger : ILogger<CardController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member __.Get() : Card[] =
        CardService.CreateDeck