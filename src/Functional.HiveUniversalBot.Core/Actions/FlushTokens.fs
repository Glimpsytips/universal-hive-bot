﻿module FlushTokens 

open Core
open PipelineResult
open Functional.ETL.Pipeline

[<Literal>]
let private ModuleName = "Flush"
 
let private isHiveOperation result = 
    match result with 
    | HiveOperation _ -> true
    | _ -> false

let private selectHiveOperationOnly result = 
    match result with 
    | HiveOperation (moduleRequesting, token, requiredKey, customJson) -> Some (moduleRequesting, token, requiredKey, customJson :> obj)
    | _ -> None

let private extractOperations entity =
    entity.results
    |> Seq.map selectHiveOperationOnly
    |> Seq.filter (fun x -> x.IsSome)
    |> Seq.map (fun x -> x.Value)
    |> Seq.groupBy (fun (_, _, requiredKey, _) -> requiredKey)

let private executeOperations (hive: Hive) keyRequired operations = 
    hive.brodcastTransactions operations keyRequired

let private extractCustomJson hiveOperationRequest = 
    let (_, _, _, customJson) = hiveOperationRequest
    customJson

let private processHiveOperations hive requiredKey key (operations: Map<KeyRequired,seq<Module * Token * KeyRequired * obj>>) = 
    if operations.ContainsKey (requiredKey)
    then 
        operations.[requiredKey] |> Seq.map extractCustomJson |> Array.ofSeq |> executeOperations hive key |> ignore

let action (hive: Hive) (entity: PipelineProcessData<UniversalHiveBotResutls>) = 
    let userDetails: (string * string * string) option = PipelineProcessData.readPropertyAsType entity "userdata" 

    match userDetails with 
    | Some (username, activeKey, postingKey) -> 
        let operations = extractOperations entity |> Map.ofSeq

        operations |> processHiveOperations hive KeyRequired.Active activeKey 
        operations |> processHiveOperations hive KeyRequired.Posting postingKey

        let results = 
            entity.results 
            |> Seq.filter (fun x -> not (isHiveOperation x))
            |> List.ofSeq

        { entity with results = results }
    | _ -> 
        NoUserDetails ModuleName |> PipelineProcessData.withResult entity

let bind hive urls (parameters: Map<string, string>) = 
    action hive
