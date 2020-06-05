namespace BlackJackAPI.Services

open BlackJackAPI.Models
open BlackJackAPI.Helpers
open BlackJackAPI.Enums
open System
open FSharp.Data

module GameService =
    let CreatedGame(playerOne:string) : Game =  
        let hand = ResizeArray<Card>().ToArray()
        let game = {
            Id = Guid.NewGuid().ToString()
            State = "Created"
            PlayerOneName = playerOne
            PlayerOneHand = hand
            PlayerOneState = ""
            PlayerTwoName = ""
            PlayerTwoHand = hand
            PlayerTwoState = ""
            Deck = hand
        }
        let games = GameHelper.AddGameToGames(game)
        game

        // write into json

    let GetGames : Game[] = 
        GameHelper.GetGames 0

    let GetGamesByUsername(username:string) : Game[] = 
        let games = GameHelper.GetGames 0
        let userGames = ResizeArray<Game>()
        let count = 0
        let rec loopingGameFindUser' = fun (count:int) ->
            match games.[count] with 
            | game when count < games.Length && (game.PlayerOneName = username || game.PlayerTwoName = username) ->
                userGames.Add(game)
                loopingGameFindUser'(count+1)
            | game when (count+1) = games.Length && (game.PlayerOneName = username || game.PlayerTwoName = username) ->
                userGames.Add(game)
            | _ -> ignore()

        loopingGameFindUser'(count)
        userGames.ToArray()


    let RemoveGame(item:Game) : bool =
        let games = GameHelper.GetGames 0
        let currentGames = ResizeArray<Game>()
        let rec loopingRemoveGame' = fun (count:int) ->
            match games.[count] with
            | game when count < games.Length && game.Id <> item.Id ->
                currentGames.Add(game)
                loopingRemoveGame'(count+1)
            | game when count < games.Length && game.Id <> item.Id ->
                currentGames.Add(game)
            | _ -> ignore()
        
        loopingRemoveGame'(0)
        GameHelper.ObjToJson(currentGames.ToArray())
        true

    let JoinGame(item:Game) : Game = 
        let games = GameHelper.GetGames 0
        let nullArray = ResizeArray<Card>().ToArray()
        let currentGame = games.[0]

        let rec loopingJoinGame'(count:int) : Game =
            match games.[count] with
            | game when (count+1) = games.Length && game.Id = item.Id ->
                currentGame = game |> ignore
                currentGame
            | game when count < games.Length && game.Id = item.Id ->
                currentGame = game |> ignore
                currentGame
            | game when count < games.Length && (count+1) < games.Length && game.Id <> item.Id ->
                loopingJoinGame'(count+1)
            | _ -> currentGame
 
        loopingJoinGame'(0) |> ignore

        match RemoveGame(item) with 
        | true -> 
            let editedGame = {
                Id = currentGame.Id
                PlayerOneName = currentGame.PlayerOneName
                PlayerTwoName = item.PlayerTwoName
                PlayerOneHand = currentGame.PlayerOneHand
                PlayerTwoHand = currentGame.PlayerTwoHand
                PlayerOneState = "Play"
                PlayerTwoState = "Wait"
                Deck = CardService.Shuffle CardService.CreateDeck
                State = "Started"
            }
            GameHelper.AddGameToGames(editedGame) |> ignore
            editedGame
        | false ->
            item

        

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