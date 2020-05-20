namespace BlackJackAPI.Services

open BlackJackAPI.Models
open BlackJackAPI.Enums
open System

module CardService =
    let CreateDeck : Card[] (*: Card[]*) = 
        let names = Enum.GetNames(typeof<CardsNames>)
        let symbols = Enum.GetNames(typeof<CardsSymbols>)
        let resizeArrayCards = ResizeArray<Card>()

        let CreateCard = fun (symbol : string, name : string) ->
            resizeArrayCards.Add {
                Name = name
                Symbol = symbol
                FirstValue = 0
                SecondValue = 0
            }
        
        let rec loopingNames' (countName : int) =
            let rec loopingSymbols' = fun (countSymbol : int, name : string) ->
                match symbols.[countSymbol] with
                | value when (countSymbol+1) = symbols.Length -> CreateCard (value, name)
                | value when countSymbol < symbols.Length -> 
                    CreateCard (value, name)
                    loopingSymbols' (countSymbol+1, name)

                
            match names.[countName] with
            | value when (countName+1) = names.Length -> loopingSymbols' (0, value)
            | value when countName < names.Length -> 
                loopingSymbols' (0, value)
                loopingNames' (countName+1)
                
        let countName = 0
        loopingNames' (countName)
        resizeArrayCards.ToArray()
        