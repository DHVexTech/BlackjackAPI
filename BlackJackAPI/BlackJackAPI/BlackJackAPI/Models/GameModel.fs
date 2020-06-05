namespace BlackJackAPI.Models

open System
open BlackJackAPI.Enums
open System.Runtime.Serialization

[<CLIMutable>]
[<DataContract>]
type Game =
    {
        [<DataMember>] mutable Id : string
        [<DataMember>] mutable State: string // Created / Started / Player1Turn / Player2Turn / Finished
        [<DataMember>] mutable PlayerOneName: string
        [<DataMember>] mutable PlayerOneHand: Card[]
        [<DataMember>] mutable PlayerOneState: string // Active / Stopped
        [<DataMember>] mutable PlayerTwoName: string
        [<DataMember>] mutable PlayerTwoHand: Card[]
        [<DataMember>] mutable PlayerTwoState: string // Active / Stopped
        [<DataMember>] mutable Deck: Card[]
    }