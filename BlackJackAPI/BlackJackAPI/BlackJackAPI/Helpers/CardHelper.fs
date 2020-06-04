namespace BlackJackAPI.Helpers

open BlackJackAPI.Models
open FSharp.Data
open FSharp.Data.JsonExtensions

module CardHelper = 
    let ObjToCardParser(cards:JsonValue) : Card[] =
        let cardsArray = ResizeArray<Card>()
        match cards with
        | card ->
            match card with
            | info ->
                cardsArray.Add({
                    Name = info?Name.ToString()
                    Symbol = info?Symbol.ToString()
                    FirstValue = info?FirstValue.AsInteger()
                    SecondValue = info?SecondValue.AsInteger()
                })
        | _ -> printfn "failed parse Card"

        cardsArray.ToArray()
        
            

