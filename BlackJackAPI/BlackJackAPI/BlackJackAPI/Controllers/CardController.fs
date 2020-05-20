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

    [<HttpPost("shuffle")>]
    member __.Shuffle(body : obj) : string =
        let cards = JsonValue.Parse(body.ToString())
        //CardService.Shuffle 
            

        "get"
        //CardService.Shuffle cards

