namespace Client

open Client.App
open Thoth.Json

module Root =

    open Elmish
    open Feliz

    let init () =
        {
            IsLoading = false
            PageState = (PageState.HomeState <| (Home.init () |> fst))
            DrawerState = Some (Drawer.init () |> fst )
        }, Cmd.map HomeMessage (Home.init () |> snd)


    let  update (msg : Message) (state : State) =
        match msg , state.PageState with
        | HomeMessage homeMsg , HomeState homeState ->
            let nextState, nextMsg = Home.update homeMsg homeState
            { state with PageState = HomeState <| nextState }, Cmd.map HomeMessage nextMsg
        | SignalRMessage signalMsg, SignalRState signalState ->
            let nextState, nextMsg = SignalR.update signalMsg signalState
            { state with PageState = SignalRState nextState }, Cmd.map SignalRMessage nextMsg
        | DrawerMessage drawerMsg, _ ->
            match state.DrawerState with
            | Some ds ->
                let nextDrawerState, nextDrawerMsg = Drawer.update drawerMsg ds
                match ds.SelectedPage with
                | Drawer.SelectPage.Home ->
                    let nextState, nextMsg = Home.init()
                    { state with PageState = HomeState nextState ; DrawerState = Some nextDrawerState}
                    ,Cmd.batch [ Cmd.map HomeMessage nextMsg; Cmd.map DrawerMessage nextDrawerMsg ]
                | Drawer.SelectPage.SignalR ->
                    let nextState, nextMsg = SignalR.init()
                    { state with PageState = SignalRState nextState; DrawerState = Some nextDrawerState }
                    , Cmd.batch [ Cmd.map SignalRMessage nextMsg; Cmd.map DrawerMessage nextDrawerMsg ]
            | None ->
                let initialState, initialCmd = Drawer.init()
                let nextState, nextMsg = Drawer.update drawerMsg initialState
                { state with DrawerState = Some nextState }, Cmd.map DrawerMessage nextMsg
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
                | PageState.HomeState    state -> yield Home.view    state (HomeMessage    >> dispatch)
                | PageState.DrawerState  state -> yield Drawer.view  state (DrawerMessage  >> dispatch)
                | PageState.SignalRState state -> yield SignalR.view state (SignalRMessage >> dispatch)
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
