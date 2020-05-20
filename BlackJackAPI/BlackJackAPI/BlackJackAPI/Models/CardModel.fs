namespace BlackJackAPI.Models

open System
open BlackJackAPI.Enums

type Card =
    {
        Name: string
        Symbol: string
        FirstValue: int
        SecondValue: int
    }