namespace Client

open Feliz
open Feliz.DaisyUI
open Elmish
open Fable.FontAwesome

module Drawer =

    type SelectPage =
        | Home | SignalR

    type State = {
        DrawerIsOpen : bool
        SelectedPage : SelectPage
    }

    type Message =
    | ToggleDrawer
    | PageSelected of SelectPage

    let init () : State * Cmd<Message> =
        {
            DrawerIsOpen = false
            SelectedPage = Home
        }, Cmd.ofMsg ToggleDrawer

    let update (msg : Message) (state: State) =
        match msg with
        | ToggleDrawer -> { state with DrawerIsOpen = not state.DrawerIsOpen }, Cmd.none
        | PageSelected p -> { state with SelectedPage = p }, Cmd.none

    let view (state : State) (dispatch : Message -> unit) =
        Html.div [
                prop.className "navbar mb-40 shadow-xl bg-primary square-box"
                prop.children [
                    Html.div [
                        prop.className "navbar-start"
                        prop.children [
                        Html.div [
                            prop.className "dropdown"
                            prop.children [
                                Html.label [
                                    prop.className "btn btn-ghost m-1"
                                    prop.tabIndex 0
                                    prop.children [ Fa.i [ Fa.Solid.Bars ] [ ] ] ]
                                Html.ul [
                                    prop.className "mt-2 shadow menu dropdown-content bg-secondary rounded-box w-52"
                                    prop.tabIndex 0
                                    prop.children [
                                        Html.li [
                                            Html.a [
                                                prop.text "Home"
                                                prop.onClick (fun _ -> dispatch (PageSelected Home)) ] ]
                                        Html.li [
                                            Html.a [
                                                prop.text "SignalR"
                                                prop.onClick (fun _ -> dispatch (PageSelected SignalR)) ] ]
                                    ] ] ] ] ] ]
                    Html.div [
                        prop.className "navbar-center"
                        prop.children [ Html.span "Agrico Style guide" ]
                    ]
                    Html.div [
                        prop.className "navbar-end"
                        prop.children[
                        Daisy.button.button [
                            prop.className "btn btn-ghost"
                            prop.children [ Fa.i [ Fa.Solid.Search ] [ ] ]
                            ] ] ] ] ]