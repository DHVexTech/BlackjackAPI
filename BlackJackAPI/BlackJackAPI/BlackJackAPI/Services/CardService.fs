namespace BlackJackAPI.Services

open BlackJackAPI.Models
open BlackJackAPI.Enums
open System

module CardService =
    let CreateDeck : Card[] = 
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
        

    let Shuffle (cards : Card[]) : Card[] = 
        let rnd = Random()
        let cutNumber = rnd.Next(1000, 2000)

        let rec CutDeck' (cutCount : int, cutLimit : int, cards : Card[]) : Card[] =
            let resizedDeck = ResizeArray()
            
            let firsPartDeck = cards.[0..((cards.Length / 2)-1)]
            let secondPartDeck = cards.[((cards.Length/2))..(cards.Length-1)]

            let rec loopingDeck' = fun (countCard : int, firstDeck : Card[], secondDeck : Card[]) ->
                match (secondDeck.[countCard], firstDeck.[countCard]) with
                | second, first when (countCard+1) < firstDeck.Length ->
                    resizedDeck.Add(second)
                    resizedDeck.Add(first)
                    loopingDeck' (countCard+1, firstDeck, secondDeck)
                | second, first when countCard < firstDeck.Length -> 
                    resizedDeck.Add(second)
                    resizedDeck.Add(first)
                | _ , _ -> ()

            loopingDeck' (0, firsPartDeck, secondPartDeck)
            
            let deckShuffle = resizedDeck.ToArray()
            match cutCount with
            | _ when cutCount < cutLimit -> CutDeck' (cutCount+1, cutLimit, deckShuffle)
            | _ -> deckShuffle
            
        CutDeck' (0, cutNumber, cards)

