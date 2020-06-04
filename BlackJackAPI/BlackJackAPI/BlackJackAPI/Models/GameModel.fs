namespace BlackJackAPI.Models

open System
open BlackJackAPI.Enums

[<CLIMutable>]
type Game =
    {
        Id : string
        State: string // Created / Started / Player1Turn / Player2Turn / Finished
        PlayerOneName: string
        PlayerOneHand: Card[]
        PlayerOneState: string // Active / Stopped
        PlayerTwoName: string
        PlayerTwoHand: Card[]
        PlayerTwoState: string // Active / Stopped
        Deck: Card[]
    }