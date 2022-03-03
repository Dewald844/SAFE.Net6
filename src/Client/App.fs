namespace Client

open Client.App

module Root =

    open Elmish
    open Feliz

    let init () =
        {
            IsLoading = false
            PageState = (PageState.HomeState <| (Home.init () |> fst))
            DrawerState = Some (Drawer.init () |> fst )
        }, Cmd.none


    let  update (msg : Message) (state : State) =
        match msg , state.PageState with
        | HomeMessage homeMsg , HomeState homeState ->
            let nextState, nextMsg = Home.update homeMsg homeState
            { state with PageState = HomeState <| nextState }, Cmd.map HomeMessage nextMsg
        | DrawerMessage drawerMsg, DrawerState drawerState ->
            let nextState, nextMsg = Drawer.update drawerMsg drawerState
            { state with PageState = DrawerState nextState ; DrawerState = Some nextState},Cmd.map DrawerMessage nextMsg
        | AlertMessage, AlertState ->
            state, Cmd.none
        | _ -> state, Cmd.none


    let view (pageState: State) (dispatch : Message -> unit) =
        Html.div [
            prop.className "bg-lightgrey min-h-screen"
            prop.children [
            match pageState.DrawerState with
            | Some ds -> Drawer.view ds (DrawerMessage >> dispatch)
            | None -> Html.p [ prop.text "Navbar not showing" ]

            Html.div [
                match pageState.PageState with
                | PageState.HomeState   state -> yield Home.view   state (HomeMessage   >> dispatch)
                | PageState.DrawerState state -> yield Drawer.view state (DrawerMessage >> dispatch)
                | PageState.AlertState        -> yield Html.p [ prop.text "Page Not Available yet" ]
                ] ] ]

    open Elmish.React

    #if DEBUG
    open Elmish.Debug
    open Elmish.HMR
    #endif

    Program.mkProgram init update view
    #if DEBUG
    |> Program.withConsoleTrace
    #endif
    |> Program.withReactSynchronous "elmish-app"
    #if DEBUG
    |> Program.withDebugger
    #endif
    |> Program.run
