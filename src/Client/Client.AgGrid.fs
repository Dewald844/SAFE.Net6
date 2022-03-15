namespace Client

open Elmish
open Feliz
open Feliz.AgGrid
open Shared
open Fable.Remoting.Client

module AgGrid =

    type State = {
        IsLoading : bool
        RowL : List<CSVData>
        IsError : bool
        ErrorExceptionString : string
        CardHeaderString : string
    }

    type Message =
        | Read5000Rows
        | RowsReceived of List<CSVData>
        | HeaderSelected of string
        | ExceptionReceived of exn

    let todosApi =
        Remoting.createApi ()
        |> Remoting.withRouteBuilder Route.builder
        |> Remoting.buildProxy<ITodosApi>

    let init () =
        {
            IsLoading = false
            RowL = List.empty
            IsError = false
            ErrorExceptionString = ""
            CardHeaderString = ""
        }, Cmd.none

    let update (state : State) (msg : Message) =
        match msg with
        | Read5000Rows   -> { state with IsLoading = true }, Cmd.OfAsync.either todosApi.read5000RowCsv () RowsReceived ExceptionReceived
        | RowsReceived l -> { state with IsLoading = false; RowL = l }, Cmd.none
        | HeaderSelected s -> { state with CardHeaderString = s }, Cmd.none
        | ExceptionReceived x -> { state with IsLoading = false; IsError = true; ErrorExceptionString = x.Message }, Cmd.none

    let view (state : State) (dispatch : Message -> unit) =
        Html.div [
            prop.className "p-5 card"
            prop.children [
                Html.div [
                    prop.className "card-header bg-primary p-5 text-black"
                    prop.text state.CardHeaderString
                ]
                if not (List.isEmpty state.RowL) then
                    Html.div [
                        prop.className ThemeClass.Balham
                        prop.children [
                            AgGrid.grid [
                                AgGrid.rowData (state.RowL |> List.toArray)
                                AgGrid.pagination true
                                AgGrid.defaultColDef [
                                    ColumnDef.resizable true
                                    ColumnDef.sortable true
                                    ColumnDef.editable (fun _ -> false)
                                ]
                                AgGrid.domLayout AutoHeight

                                AgGrid.paginationPageSize 20
                                AgGrid.onColumnGroupOpened ( fun x -> x.AutoSizeGroupColumns())
                                AgGrid.columnDefs [
                                    ColumnDef.create<string> [
                                        ColumnDef.filter RowFilter.Text
                                        ColumnDef.headerName "Row id"
                                        ColumnDef.valueGetter (fun x -> x.RowId)
                                    ]
                                    ColumnDef.create<string> [
                                        ColumnDef.filter RowFilter.Text
                                        ColumnDef.headerName "Description"
                                        ColumnDef.valueGetter (fun x -> x.Description)
                                    ]
                                    ColumnDef.create<string> [
                                        ColumnDef.filter RowFilter.Text
                                        ColumnDef.onCellClicked(fun _ y -> dispatch (HeaderSelected <| y.Name))
                                        ColumnDef.headerName "Name"
                                        ColumnDef.valueGetter (fun x -> x.Name)
                                    ]
                                    ColumnDef.create<string> [
                                        ColumnDef.filter RowFilter.Text
                                        ColumnDef.headerName "Reference"
                                        ColumnDef.valueGetter (fun x -> x.Reference)
                                    ]
                                    ColumnGroup.create [
                                        ColumnGroup.headerName "Value's"
                                        ColumnGroup.marryChildren true
                                        ColumnGroup.openByDefault true ] [
                                            ColumnDef.create<string> [
                                                ColumnDef.filter RowFilter.Number
                                                ColumnDef.headerName "Value 1"
                                                ColumnDef.columnType ColumnType.NumericColumn
                                                ColumnDef.valueGetter (fun x -> x.NumberValue1)
                                                ColumnDef.columnGroupShow true
                                            ]
                                            ColumnDef.create<string> [
                                                ColumnDef.filter RowFilter.Number
                                                ColumnDef.headerName "Value 2"
                                                ColumnDef.columnType ColumnType.NumericColumn
                                                ColumnDef.valueGetter (fun x -> x.NumberValue2)
                                                ColumnDef.columnGroupShow false
                                            ]
                                            ColumnDef.create<string> [
                                                ColumnDef.filter RowFilter.Number
                                                ColumnDef.headerName "Value 3"
                                                ColumnDef.columnType ColumnType.NumericColumn
                                                ColumnDef.valueGetter (fun x -> x.NumberValue3)
                                                ColumnDef.columnGroupShow false
                                            ]
                                            ColumnDef.create<string> [
                                                ColumnDef.filter RowFilter.Number
                                                ColumnDef.headerName "Value 4"
                                                ColumnDef.columnType ColumnType.NumericColumn
                                                ColumnDef.valueGetter (fun x -> x.NumberValue4)
                                                ColumnDef.columnGroupShow false
                                            ]
                                            ColumnDef.create<string> [
                                                ColumnDef.filter RowFilter.Number
                                                ColumnDef.headerName "Value 5"
                                                ColumnDef.columnType ColumnType.NumericColumn
                                                ColumnDef.valueGetter (fun x -> x.NumberValue5)
                                                ColumnDef.columnGroupShow false
                                            ] ] ] ] ] ]
                else
                    Html.span "Click button to read data"
                Html.label [
                    prop.onClick (fun _ -> dispatch Read5000Rows)
                    prop.className "btn btn-secondary btn-block rounded-none"
                    prop.text "Read CSV Data from server"
                ]
            ]
        ]


