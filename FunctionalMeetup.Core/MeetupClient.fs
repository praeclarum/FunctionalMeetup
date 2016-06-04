namespace FunctionalMeetup



type IMeetupClient =

    abstract CreateMeetupAsync : string -> string -> Location -> Async<Meetup>
    abstract GetMeetupsAsync : string -> Async<Meetup[]>
    
    abstract FindPersonAsync : string -> Async<Person option>
    //abstract AddFriendAsync : string -> string -> Async<Person>
    //abstract GetFriendsAsync : string -> Async<Person[]>


type TestMeetupClient (initialPeople) =
    let mutable meetups = [||]
    let mutable people = initialPeople

    let findPersonAsync personId =
        async {
            let person =
                people
                |> Array.tryFind (fun x -> x.Id = personId)
            return person
        }
    let createMeetupAsync userId name loc =
        async {
            let! user = findPersonAsync userId
            let newMeetup =
                match user with
                | Some user ->
                    let newMeetup =
                        {
                            Id = (meetups.Length + 1).ToString()
                            Name = name
                            Location = loc
                            Invitations = [| (user, Accepted None) |]
                        }
                    let newMeetups = Array.append meetups [| newMeetup |]
                    meetups <- newMeetups
                    newMeetup
                | None -> failwith "Unknown user"
            return newMeetup
        }
    let getMeetupsAsync userId =
        async {
            let userMeetups =
                meetups
                |> Array.filter (fun x ->
                    x.Invitations
                    |> Array.exists (fun (p, _) -> p.Id = userId))
            return userMeetups
        }

    interface IMeetupClient with
        member this.CreateMeetupAsync userId name location =
            createMeetupAsync userId name location
        member this.GetMeetupsAsync userId =
            getMeetupsAsync userId
        member this.FindPersonAsync personId =
            findPersonAsync personId
