namespace Server

#if INTERACTIVE
#r "FSharp.Data.dll"
#endif

open System
open Shared
open FSharp.Data

module ReadCSV =

    let rec readCsvFile retry (source: string) =

        if retry > 0  then
            async {
            try
               return CsvFile.Load(source, hasHeaders = true)
            with
            | :? SystemException as x ->
                printfn $"System Exception %A{x}"
                return! readCsvFile (retry - 1) source
            | x ->
                printfn $"System Exception %A{x}"
                return! readCsvFile (retry - 1) source
                }
        else
            failwith "Command ran out of retrys"

    let read5000PathString = "../../../public/5000Rows.csv"

    let parseCSVToType (file : CsvFile) : List<CSVData> =
        try
        printfn "Parsing file . . ."
        file.Rows
        |> List.ofSeq
        |> List.map ( fun row ->
            {
                RowId         = (row.GetColumn "RowId")
                Description   = (row.GetColumn "Description")
                Name          = (row.GetColumn "Name")
                NumberValue1  = (row.GetColumn "NumberValue1")
                NumberValue2  = (row.GetColumn "NumberValue2")
                NumberValue3  = (row.GetColumn "NumberValue3")
                NumberValue4  = (row.GetColumn "NumberValue4")
                Reference     = (row.GetColumn "Reference")
                RowGroup      = (row.GetColumn "RowGroup")
                NumberValue5  = (row.GetColumn "NumberValue5")
            } )

        with
        | ex ->
            printfn $"{ex.Message}"
            failwith ex.Message

    let readCsvApi () =
        async {
            printfn "Reading CSV File"
            let! file = readCsvFile 5 read5000PathString
            printfn $"NUMBER OF COLUMNS : {file.NumberOfColumns}"
            printfn $"Headers %A{file.Headers}"
            printfn $"File Read %A{file}"
            return parseCSVToType file
        }

