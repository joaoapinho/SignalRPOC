module Giraffe1.MyHub

open System
open System.Collections.Generic
open Giraffe1.ClientApi
open Microsoft.AspNetCore.SignalR


type User =
    { Id: Guid
      Name: string
      LoggedAt: DateTime }
    
let userList = Map.empty
    

let addUser name =
    let newUser =
        { Id = Guid.NewGuid()
          Name = name
          LoggedAt = DateTime.UtcNow }
    userList|> Map.add (newUser.Id, newUser)
    (true, newUser.Id)

let removeUser (id:Guid) =
    userList.Remove id
        
type MyHub () =
    inherit Hub<IClientApi> ()
    
    //login
    member this.Login (name : string) =
        let connectionId = this.Context.ConnectionId
        let success, userId = addUser name
        if success then
            this.Clients.Client(connectionId).LoginResponse(true, userId |> string) |> ignore
            this.Clients.All.Message(sprintf "New User: %s (%O)" name userId )
        else
            this.Clients.Client(connectionId).LoginResponse(false, "")

    member this.Logout (userId : string) =
        removeUser (userId |> Guid)
        this.Clients.All.Message(sprintf "User left: %s" userId)

    member this.Send (message: string) =
        this.Clients.All.Message(message)