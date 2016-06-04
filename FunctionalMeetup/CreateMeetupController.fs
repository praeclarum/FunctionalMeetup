namespace FunctionalMeetup.iOS

open System

open UIKit
open MonoTouch.Dialog

open FunctionalMeetup
open FunctionalMeetup.ViewModels

type CreateMeetupController (cmds : UserCommands) =
    inherit DialogViewController (new RootElement("New Meetup"))

    let nameEntry = new EntryElement ("Name", "Fun Time", "")
    let nameSec = new Section ()

    do
        nameSec.Add (nameEntry)
        base.Root.Add (nameSec)

    member this.OnClickCreate () =
        async {
            try
                let name, loc = this.GetMeetupFromUI ()
                do! cmds.CreateMeetupAsync cmds.Repo.Data.User.Id 
                                           name loc
                this.DismissViewController (true, null)
            with e ->
                this.ShowError ("Create Meetup Failed", e)
        }
        |> Async.StartImmediate

    member this.GetMeetupFromUI () =
        nameEntry.Value, {Time=DateTimeOffset.Now;Latitude=0.0<deg>;Longitude=0.0<deg>}

    member this.ShowError (m : string, e : exn) =
        Console.WriteLine ("ERROR {0}: {1}", m, e)

    override this.ViewDidLoad () =
        base.ViewDidLoad ()
        this.NavigationItem.LeftBarButtonItem <-
            new UIBarButtonItem (UIBarButtonSystemItem.Cancel, fun s e ->
                this.DismissViewController (true, null))
        this.NavigationItem.RightBarButtonItem <-
            new UIBarButtonItem (UIBarButtonSystemItem.Done, fun s e ->
                this.OnClickCreate ())
