namespace BlackJackAPI.Controllers

open System
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open BlackJackAPI.Models

[<ApiController>]
[<Route("user")>]
type UserController (logger : ILogger<UserController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member __.Get() : User[] =
        let rng = System.Random()
        [|
            for index in 0..4 ->
                { Date = DateTime.Now.AddDays(float index)
                  UserName = Guid.NewGuid().ToString()
                  credit = rng.Next(0, 1000) }
        |]
