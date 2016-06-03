namespace FunctionalMeetup

open System.Threading.Tasks

/// Repo is the one source of mutation in the app.
/// Data is updated in transactions with potential rollback.
/// This version is missing history and messages from the
/// presentation.
type AppRepo (initialData : AppData) =

    let mutable data = initialData
    let dataLock = new obj()

    let updatedData = Event<unit> ()

    member this.Data = data

    /// Fired whenever the data changes
    member this.UpdatedData = updatedData.Publish
    
    /// The only way to change the app data
    member this.UpdateData (update : AppData -> AppData option) =
        let mutable updated = false
        lock dataLock (fun () ->
            match update data with
            | None -> ()
            | Some newData ->
                data <- newData
                updated <- true)
        if updated then
            Task.Run updatedData.Trigger |> ignore
            


