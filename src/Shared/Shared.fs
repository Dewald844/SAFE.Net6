namespace Shared

open System

type Todo = { Id: Guid; Description: string }

type CSVData =
  {
      RowId : string
      Description : string
      Name : string
      NumberValue1 : string
      NumberValue2 : string
      NumberValue3 : string
      NumberValue4 : string
      Reference : string
      RowGroup : string
      NumberValue5 : string
  }

module Todo =
    let isValid (description: string) =
        String.IsNullOrWhiteSpace description |> not

    let create (description: string) =
        { Id = Guid.NewGuid()
          Description = description }

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type ITodosApi =
    {
      getTodos: unit -> Async<Todo list>
      addTodo: Todo -> Async<Todo>
      read5000RowCsv : unit -> Async<List<CSVData>>
    }
