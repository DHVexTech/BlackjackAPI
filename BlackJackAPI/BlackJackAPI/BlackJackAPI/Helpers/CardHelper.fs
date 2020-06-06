namespace BlackJackAPI.Helpers

open BlackJackAPI.Models
open FSharp.Data
open FSharp.Data.Runtime.BaseTypes

type CardsProvider = JsonProvider<"""[{"Name": "","Symbol": "","FirstValue": 0,"SecondValue": 0}]""">
type CardProvider = JsonProvider<"""{"Name": "","Symbol": "","FirstValue": 0,"SecondValue": 0}""">

module CardHelper = 
    let ObjToCardParser(cards:IJsonDocument[]) : Card[] =
        let cardsArray = ResizeArray<Card>()
        let count = 0

        let rec loopingCard' = fun (count:int) ->
            let cardP = CardProvider.Parse(cards.[count].ToString())
            match cardP with
            |card when (count+1) = cards.Length ->
                cardsArray.Add({
                    Name = card.Name.ToString().Replace("\"", "")
                    Symbol = card.Symbol.ToString().Replace("\"", "")
                    FirstValue = card.FirstValue |> int
                    SecondValue = card.SecondValue |> int
                })
            | card when (count) < cards.Length  ->
                cardsArray.Add({
                    Name = card.Name.ToString().Replace("\"", "")
                    Symbol = card.Symbol.ToString().Replace("\"", "")
                    FirstValue = card.FirstValue |> int
                    SecondValue = card.SecondValue |> int
                })
                loopingCard'(count+1)
            | _ -> printfn "failed parse Card"

        match cards.Length with
        | x when x > 0 ->
            loopingCard'(count)
        | x -> ignore()

        cardsArray.ToArray()
        
            

