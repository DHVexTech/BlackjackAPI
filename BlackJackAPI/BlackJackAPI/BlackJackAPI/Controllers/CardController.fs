namespace BlackJackAPI.Controllers

open System
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open BlackJackAPI.Models
open BlackJackAPI.Enums

[<ApiController>]
[<Route("card")>]
type CardController (logger : ILogger<CardController>) =
    inherit ControllerBase()

    //[<HttpGet>]
    //member __.Get() : Card[] = 
        //let test = Enum.GetValues(typeof<CardsNames>)
        //let test2 = Enum.GetValues(typeof<CardsSymbols>)
        //[|
        //[0..test.Length]
        //    |>Seq.map(fun x ->
        //        [0..test2.Length]
        //            |> Seq.map (fun y ->
        //                match y with
        //                | s -> 
        //                {
        //                    let card = { 
        //                        Name = enum<CardsNames> x
        //                        Symbols = enum<CardsSymbols> y 
        //                        firstValue = 0
        //                        secondValue = 0
        //                    }
        //                }
        //            )
        //            |> Seq.iter(printfn )
        //    )
        //    |> Seq.iter(stdout )
        //|]

