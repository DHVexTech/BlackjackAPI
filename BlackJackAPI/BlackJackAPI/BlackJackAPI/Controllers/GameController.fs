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

    [<HttpPost>]
    member __.Create(item:Game) : Game =
        GameService.CreatedGame(item.PlayerOneName)
        //GameService.CreatedGame(item.PlayerOneName)

    [<HttpGet>]
    member __.GetGames() : TestPostModel[] =
        GameService.GetGames


    //[<HttpPut("Join")>]
    //member __.JoinGame(item:Game) : Game =
    //    GameService.JoinGame

    //[<HttpGet("Start")>]
    //member __.StartGame(id:string) : Game =
    //    GameService.InitializeGame

    //[<HttpPut("Draw")>]
    //member __.Draw(item:Game) : Game =
    //    GameService.Draw

    //[<HttpPut("StopDraw")>]
    //member __.StopDraw(item:Game) : Game =
    //    GameService.StopDraw





