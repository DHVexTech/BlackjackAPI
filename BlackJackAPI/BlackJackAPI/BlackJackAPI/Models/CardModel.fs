namespace BlackJackAPI.Models

open System
open BlackJackAPI.Enums

type Card =
    {
        Name: CardsNames
        Symbols: CardsSymbols
        firstValue: int
        secondValue: int
    }