namespace Client

open Elmish
open Feliz

module AgGrid =

    type State = {
        IsLoading : bool
        RowL : List<string>
    }

    type Message =
        | Read5000Rows
        | Read50000Rows
        | Read100000Rows
        | RowsReceived of List<string>

    let init () =
        {
            IsLoading = false
            RowL = List.empty
        }, Cmd.none

    let update (state : State) (msg : Message) =
        match msg with
        | Read5000Rows   -> { state with IsLoading = true }, Cmd.none
        | Read50000Rows  -> { state with IsLoading = true }, Cmd.none
        | Read100000Rows -> { state with IsLoading = true }, Cmd.none
        | RowsReceived l -> { state with IsLoading = false; RowL = l }, Cmd.none

    let view (state : State) (dispatch : Message -> unit) =
        Html.div [
            prop.className "p-5 bg-cover card bg-base-50"
            prop.children [
                Html.span "Welcome to Ag grid"
            ]
        ]

