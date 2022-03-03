namespace Client
open Feliz
open Feliz.DaisyUI
open Elmish
open Fable.FontAwesome

module Drawer =

    type State = {
        DrawerIsOpen : bool
    }

    type Message =
    | ToggleDrawer

    let init () =
        {
            DrawerIsOpen = false
        }, Cmd.none

    let update (msg : Message) (state: State) =
        match msg with
        | ToggleDrawer -> { state with DrawerIsOpen = not state.DrawerIsOpen }, Cmd.none

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
                                        Html.li [ Html.a [  prop.text "Home" ] ]
                                        Html.li [ Html.a [  prop.text "Alert" ] ]
                                        ] ] ]
                                ] ] ]
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