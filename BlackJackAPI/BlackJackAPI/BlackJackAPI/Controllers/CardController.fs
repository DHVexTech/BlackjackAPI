namespace BlackJackAPI.Controllers

open System
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open BlackJackAPI.Models
open BlackJackAPI.Enums
open BlackJackAPI.Services
open FSharp.Data

[<ApiController>]
[<Route("card")>]
type CardController (logger : ILogger<CardController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member __.Get() : Card[] =
        CardService.CreateDeck

    [<HttpGet("shuffle")>]
    member __.Shuffle() : Card[] =
        CardService.Shuffle CardService.CreateDeck
        

