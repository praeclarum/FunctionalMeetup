namespace FunctionalMeetup

open System

type RefreshService (client:IMeetupClient, repo:AppRepo) =
    let updatePeriod = 5000<ms>
    
    member this.RefreshAsync () =
        async {
            let! newMeetups =
                client.GetMeetupsAsync repo.Data.User.Id
            repo.UpdateData (fun data ->
                Some { data with Meetups = newMeetups }) 
        }
    
    member this.Start (cancelToken) =
        let loop =
            async {                
                while true do
                    do! Async.Sleep (int updatePeriod)
                    try do! this.RefreshAsync ()
                    with ex -> this.LogError ex
            }
        Async.Start (loop, cancelToken)

    member this.LogError (e : exn) =
        printfn "ERROR: %O" e

