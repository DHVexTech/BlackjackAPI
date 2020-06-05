namespace BlackJackAPI.Helpers

open FSharp.Data
open BlackJackAPI.Models

type gamesProvider = JsonProvider<"""[{"id": "","state": "","playerOneName": "","playerOneHand": [],"playerOneState": "","playerTwoName": "","playerTwoHand": [],"playerTwoState": "","deck": []}]""">
type gameProvider = JsonProvider<"""{"id": "","state": "","playerOneName": "","playerOneHand": [],"playerOneState": "","playerTwoName": "","playerTwoHand": [],"playerTwoState": "","deck": []}""">


module GameHelper = 
    let ObjToGameParser(value:JsonValue) : Game[] = 
        let games = ResizeArray<Game>()
        let game = gamesProvider.Parse(value.ToString())
        let count = 0

        let rec loopingGames' = fun (count:int) ->
            match game with
            | data -> 
                let gameP = gameProvider.Parse(data.[count].ToString())
                match gameP with
                | info when (count+1) = data.Length ->
                    let playerOne = CardHelper.ObjToCardParser(info.PlayerOneHand)
                    let playerTwo = CardHelper.ObjToCardParser(info.PlayerTwoHand)
                    let deck = CardHelper.ObjToCardParser(info.Deck)

                    games.Add({
                        Id = info.Id.ToString().Replace("\"", "")
                        State = info.State.ToString().Replace("\"", "")
                        PlayerOneName = info.PlayerOneName.ToString().Replace("\"", "")
                        PlayerOneHand = playerOne
                        PlayerOneState = info.PlayerOneState.ToString().Replace("\"", "")
                        PlayerTwoName = info.PlayerTwoName.ToString().Replace("\"", "")
                        PlayerTwoHand = playerTwo
                        PlayerTwoState = info.PlayerTwoState.ToString().Replace("\"", "")
                        Deck = deck
                    })
                | info when (count) < data.Length ->
                    let playerOne = CardHelper.ObjToCardParser(info.PlayerOneHand)
                    let playerTwo = CardHelper.ObjToCardParser(info.PlayerTwoHand)
                    let deck = CardHelper.ObjToCardParser(info.Deck)

                    games.Add({
                        Id = info.Id.ToString().Replace("\"", "")
                        State = info.State.ToString().Replace("\"", "")
                        PlayerOneName = info.PlayerOneName.ToString().Replace("\"", "")
                        PlayerOneHand = playerOne
                        PlayerOneState = info.PlayerOneState.ToString().Replace("\"", "")
                        PlayerTwoName = info.PlayerTwoName.ToString().Replace("\"", "")
                        PlayerTwoHand = playerTwo
                        PlayerTwoState = info.PlayerTwoState.ToString().Replace("\"", "")
                        Deck = deck
                    })
                    loopingGames'(count+1)
                | _ -> printfn "Error while parsing Game Object"

        loopingGames'(count)
        games.ToArray()


    let GetGames : Game[] = 
        let datasDirectory = __SOURCE_DIRECTORY__.Replace("Helpers", "")
        let value = JsonValue.Load(datasDirectory + "datas\Games.json")
        ObjToGameParser(value)

    let AddGameToGames(gameEdited:Game) : bool =
        let games = GetGames
        let gamesCopy = ResizeArray<Game>()
        let rec loopingGame' = fun (count:int) ->
            match games.[count] with
            | game when (count+1) = games.Length ->
                gamesCopy.Add(game)
            | game when count < games.Length ->
                gamesCopy.Add(game)
                loopingGame'(count+1)

        loopingGame'(0)
        gamesCopy.Add(gameEdited)
        // Write into json
        true