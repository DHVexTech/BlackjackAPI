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
                PlayerOneScore = currentGame.PlayerOneScore
                PlayerTwoScore = currentGame.PlayerTwoScore
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