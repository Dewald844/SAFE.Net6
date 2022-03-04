namespace Client

open Fable
open Elmish
open Fable.SignalR
open SignalRApp
open SignalR.Elmish


module SignalR =

    type State =
        {
            Count : int
            Text  : string
            Hub   : Option<Elmish.Hub<Action,Response>>
        }

        interface System.IDisposable with
            member this.Dispose() =
                this.Hub |> Option.iter (fun hub -> hub.Dispose())

    type Message =
        | SignalRMsg of Response
        | IncrementCount
        | DecrementCount
        | CreateHub
        | RegisterHub of Elmish.Hub<Action,Response>

    let init() =
        {
            Count = 0
            Text  = ""
            Hub   = None
        }
        , Cmd.none

    let registerHub =
        Cmd.SignalR.connect RegisterHub (fun hub ->
            hub.withUrl("http://localhost:8085/SignalR")
                .withAutomaticReconnect()
                .configureLogging(LogLevel.Debug) )

    let update msg model =
        match msg with
        | CreateHub ->  model , registerHub
        | RegisterHub hub -> { model with Hub = Some hub }, Cmd.none
        | SignalRMsg rsp ->
            match rsp with | Response.NewCount i -> { model with Count = i }, Cmd.none
        | IncrementCount -> model, Cmd.SignalR.perform model.Hub (Action.IncrementCount model.Count) SignalRMsg
        | DecrementCount -> model, Cmd.SignalR.perform model.Hub (Action.DecrementCount model.Count) SignalRMsg

    open Feliz

    let view (state: State) (dispatch : Message -> unit) =
        Html.div [
            prop.className "p-5 bg-cover card bg-base-50"
            prop.children [
                Html.p [ prop.text $"COUNT : {state.Count}   RandomCharacter : {state.Text}" ]
                Html.label [
                    prop.className "btn btn-neutral rounded-none"
                    prop.onClick (fun _ -> dispatch CreateHub)
                    prop.text "Connect Signal R"
                ]
                Html.label [
                    prop.className "btn btn-primary rounded-none btn-outline"
                    prop.onClick (fun _ -> dispatch IncrementCount)
                    prop.text "Increment"
                ]
                Html.label [
                    prop.className "btn btn-secondary rounded-none btn-outline"
                    prop.onClick (fun _ -> dispatch DecrementCount)
                    prop.text "Decrement"
                ]
            ]
        ]