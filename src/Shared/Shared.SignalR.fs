namespace SignalRApp

[<RequireQualifiedAccess>]

type Action =
    | IncrementCount of int
    | DecrementCount of int

[<RequireQualifiedAccess>]

type Response =
    | NewCount of int

module EndPoints =
    let [<Literal>] Root = "/SignalR"

module SignalRHub =
    open Fable.SignalR
    open FSharp.Control.Tasks.V2

    let update (msg  : Action) =
        printf "Here i am updating from the signalR"
        match msg with
        | Action.IncrementCount i -> Response.NewCount(i + 1)
        | Action.DecrementCount i -> Response.NewCount(i - 1)

    let invoke (msg: Action) (hubContext: FableHub) =
        task { return update msg }

    let send (msg : Action) (hubContext: FableHub<Action,Response>) =
        update msg
        |> hubContext.Clients.Caller.Send


