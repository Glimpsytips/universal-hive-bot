module WritePost

open System
open PipelineResult
open Types 
open Pipeline
open PipelineProcessData
open PostId
open FSharp.Control

[<Literal>]
let ModuleName = "WritePost"

let private createPost username body title (tags: string array) = 
    let tagsList = String.Join("\",\"", tags)
    let metadata = $"""{{"app":"universalbot/0.12.0", "tags": ["{tagsList}"]}}"""
    let permlink = title |> String.replace ' ' '-' |> String.toLower
    Hive.createComment username body metadata "" tags.[0] permlink title

let private getTemplate templateId (entity: PipelineProcessData<UniversalHiveBotResutls>) = 
    (Map.getValueWithDefault entity.properties templateId ("" :> obj)).ToString()

let action hiveUrl content title tags username (entity: PipelineProcessData<UniversalHiveBotResutls>) = 
    task {
        let! body = Template.replaceMustache entity (getTemplate content entity)
        let! title = Template.replaceMustache entity title
        return 
            createPost username body  title tags 
            |> Hive.scheduleSinglePostingOperation ModuleName "vote"
            |> withResult entity
    }

let bind urls (parameters: Map<string, string>) = 
    let template = Map.getValueWithDefault parameters "template" ""
    let title = Map.getValueWithDefault parameters "title" ""
    let tags  = Map.getValueWithDefault parameters "tags" "" |> String.split ","

    Action.bindAsyncAction ModuleName (action urls.hiveNodeUrl template title tags)
