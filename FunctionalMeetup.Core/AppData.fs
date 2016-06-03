namespace FunctionalMeetup

open System

[<Measure>]
type km
[<Measure>]
type deg

type Meetup =
    {
        Id : string
        Name : string
        Location : Location
        Invitations : (Person * InvitationStatus)[]
    }

and InvitationStatus = Accepted of Location | NotAccepted

and Person =
    {
        Id : string
        Name : string
    }

and Location =
    {
        Time : DateTimeOffset
        Latitude : float<deg>
        Longitude : float<deg>
    }
    static member ( - ) (x : Location, y : Location) =
        {
            TimeSpan = x.Time - y.Time
            Distance = 10.0<km> // TODO: Calc distance
        }

and LocationDelta =
    {
        TimeSpan : TimeSpan
        Distance : float<km>
    }
    member this.IsNear = this.TimeSpan.TotalMinutes < 5.0

type AppData =
    {
        User : Person
        AuthToken : string
        
        Settings : AppSettings
        
        Location : Location
        Friends : Person[]
        Meetups : Meetup[]
    }
    static member Initial =
        {
            User = { Id = ""; Name = "" }
            AuthToken = ""
            Settings = { ShareLocation = false }
            Location = { Time = DateTimeOffset.Now; Latitude = 0.0<deg>; Longitude = 0.0<deg> }
            Friends = [||]
            Meetups = [||]
        }
    
and AppSettings =
    {
        ShareLocation : bool
    }    

