namespace FunctionalMeetup

open System

type Validation =
    | Valid
    | Invalid of string
    member this.ThrowIfInvalid () =
        match this with
        | Invalid m -> raise (new Exception (m))
        | _ -> ()
        
type UserCommands (client : IMeetupClient, repo:AppRepo) =

    member this.Repo = repo

    member this.ValidateCreateMeetup (name : string) =
        if String.IsNullOrWhiteSpace (name) then
            Invalid "Name must be given"
        else Valid
    
    member this.CreateMeetupAsync (userId:string)
                                  (name:string) (loc:Location) =
        async {
            let v = this.ValidateCreateMeetup name
            v.ThrowIfInvalid ()
            
            let! serverMeetup = client.CreateMeetupAsync userId name loc
            
            repo.UpdateData (fun data ->
                let newMeetups = Array.append [|serverMeetup|]
                                              data.Meetups
                Some { data with Meetups = newMeetups }) 
        }                


