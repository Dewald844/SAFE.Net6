namespace Client

module Home =

    open Fable.Core.JsInterop
    open Feliz
    open Feliz.DaisyUI
    importAll "./tailwind.css"

    open Elmish
    open Fable.Remoting.Client
    open Shared

    type Model = { Todos: Todo list; Input: string; Drawer : bool }

    type Message =
        | GotTodos of Todo list
        | SetInput of string
        | AddTodo
        | ToggleDrawer
        | AddedTodo of Todo

    let todosApi =
        Remoting.createApi ()
        |> Remoting.withRouteBuilder Route.builder
        |> Remoting.buildProxy<ITodosApi>

    let init () : Model * Cmd<Message> =
        let model = { Todos = []; Input = ""; Drawer = false }

        let cmd =
            Cmd.OfAsync.perform todosApi.getTodos () GotTodos

        model, cmd

    let update (msg: Message) (model: Model) : Model * Cmd<Message> =
        match msg with
        | GotTodos todos -> { model with Todos = todos }, Cmd.none
        | SetInput value -> { model with Input = value }, Cmd.none
        | AddTodo ->
            let todo = Todo.create model.Input
            let cmd = Cmd.OfAsync.perform todosApi.addTodo todo AddedTodo
            { model with Input = "" }, cmd
        | AddedTodo todo -> { model with Todos = model.Todos @ [ todo ] }, Cmd.none
        | ToggleDrawer -> { model with Drawer = not model.Drawer }, Cmd.none

    let daisyContainer (model:Model) (dispatch: Message -> unit)  =
                Daisy.card [
                    prop.className "bg-color-neutral"
                    prop.style [
                        style.paddingLeft 50
                        style.paddingTop 50
                        style.paddingRight 50
                        style.border (3, borderStyle.solid, "#43b02a") ]
                    prop.children [
                      Daisy.cardBody [
                        prop.children [
                        Daisy.cardTitle [
                          Daisy.button.button [
                              prop.style [
                                  style.backgroundImageUrl "./AgricoSmallLogo.png"
                                  style.backgroundPosition "center"
                                  style.backgroundSize.cover
                                  style.boxShadow (5,5,"##43d02a" )
                                  style.width 550
                                  style.height 170
                                  style.paddingRight 100
                                  style.position.inheritFromParent
                              ]
                              button.glass
                              prop.onClick (fun _ -> ())
                          ]
                        ]
                        Html.orderedList [
                            prop.className "p-5"
                            prop.children (
                            model.Todos
                            |> List.map (fun x -> Html.li [ prop.text x.Description ]))
                        ]
                        Daisy.formControl [
                            Daisy.input [
                                prop.className "p-5 input-accent input-lg"
                                prop.placeholder "Add todo"
                                prop.onChange (fun s -> dispatch (SetInput s))
                            ] ]
                        Html.div [
                              prop.className "p-5 bg-cover card bg-base-50"
                              prop.children [
                                  Daisy.buttonGroup [
                                    Daisy.button.button [
                                        prop.className "btn btn-primary"
                                        prop.text "Primary " ]
                                    Daisy.button.button [
                                        prop.className "btn btn-secondary"
                                        prop.text "Secondary" ]
                                    Daisy.button.button [
                                        prop.className "btn btn-accent"
                                        prop.text "Accent" ]
                                    Daisy.button.button [
                                        prop.className "btn btn-neutral"
                                        prop.text "neutral" ] ]
                              ] ] ] ] ] ]

    let view (model: Model) (dispatch: Message -> unit) =

        Html.div [
            prop.className "bg-lightgrey"
            prop.children [
                Daisy.hero [
                    prop.children [
                        Html.div [ prop.className "" ]
                        Daisy.heroContent [
                            prop.className "text-center text-lightgrey-content"
                            prop.children [
                                Html.div [
                                    prop.children [
                                        Html.p [
                                            prop.children [
                                            daisyContainer model dispatch
                                            ] ] ] ] ] ] ] ] ] ]