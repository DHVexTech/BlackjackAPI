namespace BlackJackAPI.Helpers

open FSharp.Data
open BlackJackAPI.Models
open FSharp.Data.JsonExtensions
open System

type gamesProvider = JsonProvider<"""[{"id": "00ef34af-6e55-48b9-b5d5-e4d69283df8e","state": "Created","playerOneName": "Bob","playerOneHand": [],"playerOneState": "","playerTwoName": "","playerTwoHand": [],"playerTwoState": "","deck": []}]""">
type gameProvider = JsonProvider<"""{"id": "","state": "","playerOneName": "","playerOneHand": [],"playerOneState": "","playerTwoName": "","playerTwoHand": [],"playerTwoState": "","deck": []}""">


module GameHelper = 
    let ObjToGameParser(value:JsonValue) : TestPostModel[] = 
        let games = ResizeArray<TestPostModel>()
        let game = gamesProvider.Parse(value.ToString())


        match game with
        | data -> 
            let gameP = gameProvider.Parse(data.[0].ToString())
            match gameP with
            | info ->
                //let playerOne = CardHelper.ObjToCardParser(info?PlayerOneHand)
                //let playerTwo = CardHelper.ObjToCardParser(info?PlayerTwoHand)
                //let Deck = CardHelper.ObjToCardParser(info?Deck)

                games.Add({
                    Name = info.Id.ToString().Replace("\"", "")
                })

        games.ToArray()


    let GetGames : TestPostModel[] = 
        let datasDirectory = __SOURCE_DIRECTORY__.Replace("Helpers", "")
        let value = JsonValue.Load(datasDirectory + "datas\Games.json")
        ObjToGameParser(value)