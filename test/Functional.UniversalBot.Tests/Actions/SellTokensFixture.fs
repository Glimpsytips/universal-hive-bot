﻿module SellTokensFixture

open Xunit
open FsUnit.Xunit
open Pipeline
open TestingStubs
open PipelineResult
open FSharp.Control

let private hiveNodeUrl = "https://anyx.io"
let private hiveEngineNode = "http://engine.alamut.uk:5000"

let testData =
    [|
        [| ~~123M; ~~"*"; ~~"123" |]
        [| ~~123M; ~~"100"; ~~"100" |]
        [| ~~99M; ~~"100"; ~~"99" |]
    |]
    
[<Theory>]
[<MemberData("testData")>]
let ``Can sell tokens`` (oneUpBalance:decimal) (amountToBind: string) (result: string) =
    task {
        let transformer = 
            [|
                TestingStubs.mockedBalanceAction [| ("ONEUP", oneUpBalance) |]
                SellToken.action hiveNodeUrl hiveEngineNode "ONEUP" (AmountCalator.bind amountToBind) "universal-bot" |> TestingStubs.taskDecorator
            |] |> TaskSeq.ofSeq
        let pipelineDefinition = Pipeline.bind TestingStubs.reader transformer
   
        processPipeline pipelineDefinition
        |> Seq.item 0
        |> fun entity -> entity.results
        |> Seq.item 0
        |> TestingStubs.extractCustomJson 
        |> should startWith """{"contractName":"market","contractAction":"sell","contractPayload":{"""
    }

[<Fact>]
let ``Check that balance is too low`` () =
    task { 
        let transformer = 
            [|
                (TestingStubs.mockedDelegatedStakedBalanceAction [| ("ONEUP", 0M) |])
                (UndelegateStake.action "ONEUP" "delegation-target-user" (AmountCalator.bind "100") "universal-bot") |> TestingStubs.taskDecorator 
            |] |> TaskSeq.ofSeq
        let pipelineDefinition = Pipeline.bind TestingStubs.reader transformer

        processPipeline pipelineDefinition
        |> Seq.item 0
        |> fun entity -> entity.results
        |> Seq.item 0
        |> should equal (TokenBalanceTooLow ("UndelegateStake", "universal-bot", "ONEUP"))
    }
