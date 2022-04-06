// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.Threading.Tasks
open Microsoft.AspNetCore.SignalR.Client

// Define a function to construct a message to print
let from whom =
    sprintf "from %s" whom

let mutable currentId = String.Empty
    
let loginResponseHandler (connection :HubConnection) (success :bool) (id :string) =
  currentId <- id
  printfn $"Login: success? %b{success} with id %s{id}"

let messageHandler (message :string) =
  printfn $"Property %s{message}"

let stateHandler (connection :HubConnection) (state :string) =
  printfn $"State: %s{state}"
  //connection.InvokeAsync("Turn", playerId, move.ToString()) |> ignore

let rec reconnect (connection :HubConnection) (error :'a) =
  printfn "reconnected"
  Task.Delay 1
  

[<EntryPoint>]
let main argv =
    let connection =
        HubConnectionBuilder()
            .WithUrl("http://localhost:5000/myHub")
            .Build()
    connection.On<bool, string>("LoginResponse", fun success id ->
      loginResponseHandler connection success id
      ) |> ignore
    connection.On<string>("Message", fun message -> messageHandler message) |> ignore
    connection.On<string>("State", fun gameState -> stateHandler connection gameState) |> ignore
    connection.add_Closed(fun error -> reconnect connection error)
    
    // Start connection and login
    try
      connection.StartAsync().Wait()
      connection.InvokeAsync("login", "myName").Wait()
      connection.InvokeAsync("send", "hello!").Wait()
      connection.InvokeAsync("logout", currentId).Wait()
    with
    | ex -> printfn "Connection error %s" (ex.ToString())
            Environment.Exit(1)
    
    
    // Listen for 'q' to quit
   // getCommand connection

    0