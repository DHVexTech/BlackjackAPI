namespace BlackJackAPI.Models

open System.Runtime.Serialization

[<CLIMutable>]
[<DataContract>]
type Card =
    {
        [<DataMember>] mutable Name: string
        [<DataMember>] mutable Symbol: string
        [<DataMember>] mutable FirstValue: int
        [<DataMember>] mutable SecondValue: int
    }