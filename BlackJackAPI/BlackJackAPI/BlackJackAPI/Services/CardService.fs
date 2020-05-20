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
        let cutNumber = 50

        let rec CutDeck' (cutCount : int, cutLimit : int, cards : Card[]) : Card[] =
            let resizedDeck = ResizeArray()
            let cutIndex = rnd.Next(1, cards.Length-1)
            let firstPartDeck = cards.[0..cutIndex-1]
            let secondPartDeck = cards.[cutIndex..(cards.Length-1)]

            let rec loopingDeck' = fun (countCard : int, deck : Card[]) ->
                match deck.[countCard] with
                | value when countCard+1 < deck.Length -> 
                    resizedDeck.Add(value)
                    loopingDeck' (countCard+1, deck)
                | value when countCard < deck.Length ->
                    resizedDeck.Add(value)
                | _ -> ()

            loopingDeck' (0, secondPartDeck)
            loopingDeck' (0, firstPartDeck)
            
            let deckShuffle = resizedDeck.ToArray()
            let doubleShuffledDeck = ResizeArray()

            let firsPartDeck = deckShuffle.[0..((cards.Length / 2)-1)]
            let secondPartDeck = deckShuffle.[((cards.Length/2))..(cards.Length-1)]

            let rec loopingDeckSecond' = fun (countCard : int, firstDeck : Card[], secondDeck : Card[]) ->
                match (secondDeck.[countCard], firstDeck.[countCard]) with
                | second, first when (countCard+1) < firstDeck.Length ->
                    doubleShuffledDeck.Add(second)
                    doubleShuffledDeck.Add(first)
                    loopingDeckSecond' (countCard+1, firstDeck, secondDeck)
                | second, first when countCard < firstDeck.Length -> 
                    doubleShuffledDeck.Add(second)
                    doubleShuffledDeck.Add(first)
                | _ , _ -> ()

            loopingDeckSecond' (0, firsPartDeck, secondPartDeck)

            let deckDoubleShuffled = doubleShuffledDeck.ToArray()

            match cutCount with
            | _ when cutCount < cutLimit -> CutDeck' (cutCount+1, cutLimit, deckDoubleShuffled)
            | _ -> deckDoubleShuffled
            
        CutDeck' (0, cutNumber, cards)

