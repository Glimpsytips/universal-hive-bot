﻿module PipelineFixture

open System
open Xunit
open FsUnit.Xunit
open Pipeline
open FSharp.Control

let private hiveNodeUrl = "http://engine.alamut.uk"
let private port = "5000"

let extractSome (option: Option<obj>) =
    option.Value

[<Fact>]
let ``Can combine multiple using decoration pattern`` () =
    let converter1 entity =
        PipelineProcessData.withProperty entity "b" 2
    let converter2 entity =
        PipelineProcessData.withProperty entity "c" true

    let testEntity = 
        {
            index = 1
            properties = Map ["a", "a";]
            results = list.Empty
        }

    let combinedConverter = 
        converter1 >> converter2

    let result = combinedConverter testEntity

    PipelineProcessData.readProperty result "a" |> extractSome |> should equal "a"
    PipelineProcessData.readProperty result "b" |> extractSome |> should equal 2
    PipelineProcessData.readProperty result "c" |> extractSome |> should equal true
