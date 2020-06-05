namespace BlackJackAPI.Helpers

open FSharp.Data
open BlackJackAPI.Models
open System.IO
open System.Runtime.Serialization.Json

type gamesProvider = JsonProvider<"""[{"Id": "","State": "","PlayerOneName": "","PlayerOneHand": [],"PlayerOneState": "","PlayerTwoName": "","PlayerTwoHand": [],"PlayerTwoState": "","Deck": []}]""">
type gameProvider = JsonProvider<"""{"Id": "","State": "","PlayerOneName": "","PlayerOneHand": [],"PlayerOneState": "","PlayerTwoName": "","PlayerTwoHand": [],"PlayerTwoState": "","Deck": []}""">


module GameHelper = 
    let ObjToGameParser(value:JsonValue) : Game[] = 
        let games = ResizeArray<Game>()
        let game = gamesProvider.Parse(value.ToString())
        let count = 0

        let rec loopingGames' = fun (count:int) ->
            match game with
            | data -> 
                match data.Length with
                | x when x <> 0 ->
                    let gameP = gameProvider.Parse(data.[count].ToString())
                    match gameP with
                    | info when (count+1) = data.Length ->
                        let playerOne = CardHelper.ObjToCardParser(info.PlayerOneHand)
                        let playerTwo = CardHelper.ObjToCardParser(info.PlayerTwoHand)
                        let deck = CardHelper.ObjToCardParser(info.Deck)
                        let actualGame = {
                            Id = info.Id.ToString().Replace("\"", "")
                            State = info.State.ToString().Replace("\"", "")
                            PlayerOneName = info.PlayerOneName.ToString().Replace("\"", "")
                            PlayerOneHand = playerOne
                            PlayerOneState = info.PlayerOneState.ToString().Replace("\"", "")
                            PlayerTwoName = info.PlayerTwoName.ToString().Replace("\"", "")
                            PlayerTwoHand = playerTwo
                            PlayerTwoState = info.PlayerTwoState.ToString().Replace("\"", "")
                            Deck = deck
                        }
                        games.Add(actualGame)

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
                | _ -> ignore()

        loopingGames'(count)
        games.ToArray()


    let GetGames(_:int) : Game[] = 
        let datasDirectory = __SOURCE_DIRECTORY__.Replace("Helpers", "")
        let value = JsonValue.Load(datasDirectory + "datas\Games.json")
        ObjToGameParser(value)



    let ObjToJson<'t> (myObj:'t) =   
        let datasDirectory = __SOURCE_DIRECTORY__.Replace("Helpers", "")
        File.WriteAllText(datasDirectory + "datas\Games.json", "[]");
        let fs = new FileStream("datas\Games.json",FileMode.Create)
        (new DataContractJsonSerializer(typeof<'t>)).WriteObject(fs,myObj)
        fs.Dispose()
        

    let AddGameToGames(gameEdited:Game) : bool =
        let games = GetGames 0
        let gamesCopy = ResizeArray<Game>()
        let rec loopingGame' = fun (count:int) ->
            match games.[count] with
            | game when (count+1) = games.Length ->
                gamesCopy.Add(game)
            | game when count < games.Length ->
                gamesCopy.Add(game)
                loopingGame'(count+1)

        match games.Length with
        | x when x <> 0 ->
            loopingGame'(0)
        | _ -> ignore()

        gamesCopy.Add(gameEdited)
        ObjToJson(gamesCopy.ToArray())
        true


