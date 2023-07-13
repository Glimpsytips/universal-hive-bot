module LoadTemplate

open System
open System.Threading
open PipelineResult
open Pipeline
open PipelineProcessData
open Nettle
open Nettle.Functions

[<Literal>]
let private ModuleName = "LoadTemplate" 

let action templateId label username (entity: PipelineProcessData<UniversalHiveBotResutls>) = 
    TemplateAPI.getTemplate templateId
    |> withProperty entity label 
    |>= Loaded (sprintf "Template with id %s" templateId)

let bind urls (parameters: Map<string, string>) = 
    let templateId = Map.getValueWithDefault parameters "templateId" ""
    let label = Map.getValueWithDefault parameters "label" "template"
    Action.bindAction ModuleName (action templateId label)

