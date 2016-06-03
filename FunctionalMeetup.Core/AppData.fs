namespace FunctionalMeetup

open System

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
        Latitude : float
        Longitude : float
    }

type AppData =
    {
        User : Person
        AuthToken : string
        
        Settings : AppSettings
        
        Location : Location
        Friends : Person[]
        Meetups : Meetup[]
    }
    
and AppSettings =
    {
        ShareLocation : bool
    }    

