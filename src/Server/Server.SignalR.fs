namespace Server

open System
open SignalRApp

module ServerStreamer  =
    type ServerSubscriber<'T> =
        {
            next: 'T -> unit
            error: exn option -> unit
            complete: unit -> unit
        }

module SignalRHub =
    open Fable.SignalR
    open FSharp.Control.Tasks.V2

    let update (msg  : Action) =
        match msg with
        | Action.IncrementCount i -> Response.NewCount(i + 1)
        | Action.DecrementCount i -> Response.NewCount(i - 1)

    let invoke (msg: Action) (hubContext: FableHub) =
        task { return update msg }

    let send (msg : Action) (hubContext: FableHub<Action,Response>) =
        update msg
        |> hubContext.Clients.Caller.Send

