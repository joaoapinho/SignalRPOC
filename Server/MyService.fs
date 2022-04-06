module Giraffe1.MyService

open System.Threading
open System.Threading.Tasks
open Giraffe1.ClientApi
open Giraffe1.MyHub
open Microsoft.AspNetCore.SignalR
open Microsoft.Extensions.Hosting

type MyService (hubContext :IHubContext<MyHub, IClientApi>) =
  inherit BackgroundService ()
  
  member this.HubContext :IHubContext<MyHub, IClientApi> = hubContext
  
  override this.ExecuteAsync (stoppingToken :CancellationToken) =
    let pingTimer = new System.Timers.Timer(10.)
    pingTimer.Elapsed.Add(fun _ -> 
      //updateState ()
      //let stateSerialized = serializeState gState
      this.HubContext.Clients.All.State("it's on") |> ignore)

    pingTimer.Start()
    Task.CompletedTask
