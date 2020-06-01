namespace BlackJackAPI.Services

open BlackJackAPI.Models
open BlackJackAPI.Enums
open System

module GameService =
    let CreatedGame : Game =  
        let hand = ResizeArray<Card>().ToArray()
        {
            Id = Guid.NewGuid().ToString()
            State = "Created"
            PlayerOneName = ""
            PlayerOneHand = hand
            PlayerOneState = ""
            PlayerTwoName = ""
            PlayerTwoHand = hand
            PlayerTwoState = ""
            Deck = hand
            //Deck = CardService.Shuffle

        }


    //match theGame with

    //let id = 1
    //let State = Created
    // enum 2 joueurs
    // objet contient 2 joueurs tableau avec carte en main avec statut
    //    let state = "Created"

    //return

    //let PickCard :



   //createGame(2id joeuurs et renvoie id a partir du front + deck + state)
   //pickCard
   //etat

   //cas où ça dépasse 21



   //doturn (requetes http?/validation)
   //result (after turn)


   (*
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
     *)