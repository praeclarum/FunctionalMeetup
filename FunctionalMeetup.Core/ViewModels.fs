module FunctionalMeetup.ViewModels


type MeetupList =
    {
        Title : string
        Items : MeetupListItem[]
    }

and MeetupListItem =
    {
        Title : string
        BackgroundColor : Color
    }

and Color = { Red : float; Green : float; Blue : float; Alpha : float }

type Map =
    {
        NextMeetup : Meetup option
    }


module Colors =
    let Red = { Red = 1.0; Green = 0.0; Blue = 0.0; Alpha = 1.0 }
    let Green = { Red = 0.0; Green = 1.0; Blue = 0.0; Alpha = 1.0 }
    let Clear = { Red = 0.0; Green = 0.0; Blue = 0.0; Alpha = 0.0 }


type AppRepo with
    member this.MakeMeetupList () =
        let data = this.Data
        let makeItem (m : Meetup) =
            {
                Title = m.Name
                BackgroundColor =
                    let distance = m.Location - data.Location
                    if distance.IsNear then Colors.Green
                    else Colors.Clear
            }            
        {
            Title = "My Meetups"
            Items = data.Meetups |> Array.map makeItem
        }
    
