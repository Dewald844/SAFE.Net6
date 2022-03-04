namespace Server

module APIInterface =

    open Fable.Remoting.Server
    open Fable.Remoting.Giraffe
    open Saturn
    open Fable.SignalR.SignalRExtension
    open Microsoft.AspNetCore.Cors.Infrastructure

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

    let corsConfig (config: CorsPolicyBuilder) =
        printfn $"Setting CORS config"
        let originArray = ["http://0.0.0.0:8080"; "http://0.0.0.0:8085"; "http://localhost:8080"] |> List.toArray
        config.AllowAnyOrigin()
            .WithOrigins(originArray)
            .AllowCredentials()
            .AllowAnyHeader()
            .Build() |> ignore
        printfn $"CORS config set"

    let app =
        application {
            use_signalr (
                configure_signalr {
                    endpoint SignalRApp.EndPoints.Root
                    send SignalRHub.send
                    invoke SignalRHub.invoke } )
            url "http://0.0.0.0:8085"
            use_json_serializer (Thoth.Json.Giraffe.ThothSerializer())
            memory_cache
            use_static "public"
            use_gzip
            use_developer_exceptions
            use_cors "true" corsConfig
            use_router webApp
        }

    run app
