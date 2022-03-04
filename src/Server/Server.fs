namespace Server

module APIInterface =

    open Fable.Remoting.Server
    open Fable.Remoting.Giraffe
    open Saturn
    open Fable.SignalR.SignalRExtension

    open Shared

    type Storage() =
        let todos = ResizeArray<_>()

        member __.GetTodos() = List.ofSeq todos

        member __.AddTodo(todo: Todo) =
            if Todo.isValid todo.Description then
                todos.Add todo
                Ok()
            else
                Error "Invalid todo"

    let storage = Storage()

    storage.AddTodo(Todo.create "Create new SAFE project")
    |> ignore

    storage.AddTodo(Todo.create "Write your app")
    |> ignore

    storage.AddTodo(Todo.create "Ship it !!!")
    |> ignore

    let todosApi =
        { getTodos = fun () -> async { return storage.GetTodos() }
          addTodo =
              fun todo ->
                  async {
                      match storage.AddTodo todo with
                      | Ok () -> return todo
                      | Error e -> return failwith e
                  } }


    let webApp =
        Remoting.createApi ()
        |> Remoting.withRouteBuilder Route.builder
        |> Remoting.fromValue todosApi
        |> Remoting.buildHttpHandler

    let app =
        application {
            use_signalr (
                configure_signalr {
                    endpoint SignalRApp.EndPoints.Root
                    send SignalRHub.send
                    invoke SignalRHub.invoke } )
            url "http://0.0.0.0:8085"
            use_json_serializer (Thoth.Json.Giraffe.ThothSerializer())
            use_router webApp
            memory_cache
            use_static "public"
            use_gzip
        }

    run app
