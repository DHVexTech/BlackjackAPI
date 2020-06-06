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
            PlayerOneScore = ""
            PlayerTwoScore = ""
            PlayerTwoName = ""
            PlayerTwoHand = hand
            PlayerTwoState = ""
            Deck = hand
        }
        let games = GameHelper.AddGameToGames(game)
        game

        // write into json

    let GetGames(_:int) : Game[] = 
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
        let mutable currentGames = ResizeArray<Game>()
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

        let rec loopingJoinGame'(count:int) : Game =
            match games.[count] with
            | game when count < games.Length && game.Id = item.Id ->
                game
            | game when count < games.Length && game.Id <> item.Id ->
                loopingJoinGame'(count+1)
 
        let currentGame = loopingJoinGame' 0

        match RemoveGame(item) with 
        | true -> 
            let editedGame = {
                Id = currentGame.Id
                PlayerOneName = currentGame.PlayerOneName
                PlayerTwoName = item.PlayerTwoName
                PlayerOneHand = currentGame.PlayerOneHand
                PlayerOneScore = currentGame.PlayerOneScore
                PlayerTwoScore = currentGame.PlayerTwoScore
                PlayerTwoHand = currentGame.PlayerTwoHand
                PlayerOneState = "Play"
                PlayerTwoState = "Play"
                Deck = CardService.Shuffle CardService.CreateDeck
                State = "PlayerOneTurn"
            }
            GameHelper.AddGameToGames(editedGame) |> ignore
            editedGame
        | false ->
            item

        

    let DrawCard(id:string) : Game =
    // Get CurrentGame
        let games = GameHelper.GetGames 0
        
        let rec loopingJoinGame'(count:int) : Game =
            match games.[count] with
            | game when count < games.Length && game.Id = id ->
                game
            | game when count < games.Length && game.Id <> id ->
                loopingJoinGame'(count+1)
         
        let currentGame = loopingJoinGame' 0
        RemoveGame(currentGame)

        // Get Card
        let deck = ResizeArray<Card>()

        let rec loopingCard'(count:int) =
            match currentGame.Deck.[count] with
            | card when count+1 = currentGame.Deck.Length ->
                deck.Add(card)
            | card when count < currentGame.Deck.Length ->
                deck.Add(card)
                loopingCard'(count+1)
            | _ -> ignore()
            
        loopingCard' 1 |> ignore 
        let cardToAdd = currentGame.Deck.[0]


       
        // Add Card To player Hand

        match currentGame.State with
        | "PlayerOneTurn" ->
            match currentGame.PlayerOneState with
            | "Play" ->
                let handCards = ResizeArray<Card>()
            
                let rec loopingPlayerOneHand'(count:int) =
                    match currentGame.PlayerOneHand.[count] with
                    | card when count+1 = currentGame.PlayerOneHand.Length ->
                        handCards.Add(card)
                    | card when count < currentGame.PlayerOneHand.Length ->
                        handCards.Add(card)
                        loopingPlayerOneHand'(count+1)
                    | _ -> ignore()

                match currentGame.PlayerOneHand.Length with
                | result when result <> 0 ->
                    loopingPlayerOneHand' 0
                | _ -> ()

                handCards.Add(cardToAdd)

                let getState(_:int) : string =
                    match currentGame with
                        | game when game.PlayerOneState = "Stop" && game.PlayerTwoState = "Stop" ->
                            "Ended"
                        | game when game.PlayerTwoState = "Play" ->
                            "PlayerTwoTurn"
                        | game when game.PlayerTwoState = "Stop" ->
                            "PlayerOneTurn"

                let state = getState 0
                let newGame = {
                    Id = currentGame.Id
                    PlayerOneName = currentGame.PlayerOneName
                    PlayerTwoName = currentGame.PlayerTwoName
                    PlayerOneHand = handCards.ToArray()
                    PlayerOneScore = currentGame.PlayerOneScore
                    PlayerTwoScore = currentGame.PlayerTwoScore
                    PlayerTwoHand = currentGame.PlayerTwoHand
                    PlayerOneState = currentGame.PlayerOneState
                    PlayerTwoState = currentGame.PlayerTwoState
                    Deck = deck.ToArray()
                    State = state
                }
                GameHelper.AddGameToGames newGame
                newGame
            | _ -> currentGame 

        | "PlayerTwoTurn" ->
            match currentGame.PlayerTwoState with 
            | "Play" ->
                let handCards = ResizeArray<Card>()
                let rec loopingPlayerTwoHand'(count:int) =
                    match currentGame.PlayerTwoHand.[count] with
                    | card when count+1 = currentGame.PlayerTwoHand.Length ->
                        handCards.Add(card)
                    | card when count < currentGame.PlayerTwoHand.Length ->
                        handCards.Add(card)
                        loopingPlayerTwoHand'(count+1)
                    | _ -> ignore()

                match currentGame.PlayerTwoHand.Length with
                | result when result <> 0 ->
                    loopingPlayerTwoHand' 0
                | _ -> ()

                handCards.Add(cardToAdd)

                let getState(_:int) : string =
                    match currentGame with
                        | game when game.PlayerOneState = "Stop" && game.PlayerTwoState = "Stop" ->
                            "Ended"
                        | game when game.PlayerOneState = "Play" ->
                            "PlayerOneTurn"
                        | game when game.PlayerOneState = "Stop" ->
                            "PlayerTwoTurn"

                let state = getState 0
                let newGame = {
                    Id = currentGame.Id
                    PlayerOneName = currentGame.PlayerOneName
                    PlayerTwoName = currentGame.PlayerTwoName
                    PlayerOneHand = currentGame.PlayerOneHand
                    PlayerOneScore = currentGame.PlayerOneScore
                    PlayerTwoScore = currentGame.PlayerTwoScore
                    PlayerTwoHand = handCards.ToArray()
                    PlayerOneState = currentGame.PlayerOneState
                    PlayerTwoState = currentGame.PlayerTwoState
                    Deck = deck.ToArray()
                    State = state
                }
                GameHelper.AddGameToGames newGame
                newGame
            | _ -> currentGame
        | _ -> currentGame


    let Stop(id:string) : Game =
        let games = GameHelper.GetGames 0
        
        let rec loopingJoinGame'(count:int) : Game =
            match games.[count] with
            | game when count < games.Length && game.Id = id ->
                game
            | game when count < games.Length && game.Id <> id ->
                loopingJoinGame'(count+1)
         
        let currentGame = loopingJoinGame' 0
        RemoveGame(currentGame)

        match currentGame.State with
        | state when state = "PlayerOneTurn" ->
            match currentGame.PlayerTwoState with
            | "Play" ->
                let newGame = {
                    Id = currentGame.Id
                    PlayerOneName = currentGame.PlayerOneName
                    PlayerTwoName = currentGame.PlayerTwoName
                    PlayerOneHand = currentGame.PlayerOneHand
                    PlayerOneScore = currentGame.PlayerOneScore
                    PlayerTwoScore = currentGame.PlayerTwoScore
                    PlayerTwoHand = currentGame.PlayerTwoHand
                    PlayerOneState = "Stop"
                    PlayerTwoState = currentGame.PlayerTwoState
                    Deck = currentGame.Deck
                    State = "PlayerTwoTurn"
                }
                GameHelper.AddGameToGames newGame
                newGame
            | "Stop" ->
                let newGame = {
                    Id = currentGame.Id
                    PlayerOneName = currentGame.PlayerOneName
                    PlayerTwoName = currentGame.PlayerTwoName
                    PlayerOneHand = currentGame.PlayerOneHand
                    PlayerOneScore = currentGame.PlayerOneScore
                    PlayerTwoScore = currentGame.PlayerTwoScore
                    PlayerTwoHand = currentGame.PlayerTwoHand
                    PlayerOneState = "Stop"
                    PlayerTwoState = currentGame.PlayerTwoState
                    Deck = currentGame.Deck
                    State = "Ended"
                }
                GameHelper.AddGameToGames newGame
                newGame
        | state when state = "PlayerTwoTurn" ->
            match currentGame.PlayerOneState with
            | "Play" ->
                let newGame = {
                    Id = currentGame.Id
                    PlayerOneName = currentGame.PlayerOneName
                    PlayerTwoName = currentGame.PlayerTwoName
                    PlayerOneHand = currentGame.PlayerOneHand
                    PlayerOneScore = currentGame.PlayerOneScore
                    PlayerTwoScore = currentGame.PlayerTwoScore
                    PlayerTwoHand = currentGame.PlayerTwoHand
                    PlayerOneState = currentGame.PlayerOneState
                    PlayerTwoState = "Stop"
                    Deck = currentGame.Deck
                    State = "PlayerOneTurn"
                }
                GameHelper.AddGameToGames newGame
                newGame
            | "Stop" ->
                let newGame = {
                    Id = currentGame.Id
                    PlayerOneName = currentGame.PlayerOneName
                    PlayerTwoName = currentGame.PlayerTwoName
                    PlayerOneHand = currentGame.PlayerOneHand
                    PlayerOneScore = currentGame.PlayerOneScore
                    PlayerTwoScore = currentGame.PlayerTwoScore
                    PlayerTwoHand = currentGame.PlayerTwoHand
                    PlayerOneState = currentGame.PlayerOneState
                    PlayerTwoState = "Stop"
                    Deck = currentGame.Deck
                    State = "Ended"
                }
                GameHelper.AddGameToGames newGame
                newGame

    //let CountScore(item:Game) : Game =
    //    let mutable playerOneScoreNumber = 0
    //    let rec loopingCardsPlayerOne'(count:int) =
    //        match item.PlayerOneHand.[count] with
    //        | card when count < item.PlayerOneHand.Length ->
    //            playerOneScoreNumber = playerOneScoreNumber + card.FirstValue
    //            match card.SecondValue with
    //            | value when value <> 0 ->
    //            | _ -> ignore()
    //        | card when (count+1) = item.PlayerOneHand.Length ->
    //        | _ -> ignore()
    //        ""
        
    //    let rec loopingCardsPlayerTwo'(count:int) : string =
    //        ""
        
    //    let playerOneScore = loopingCardsPlayerOne'(0)
    //    let playerTwoScore = loopingCardsPlayerTwo'(0)

    //    item