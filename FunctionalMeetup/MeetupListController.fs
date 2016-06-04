namespace FunctionalMeetup.iOS

open System

open UIKit

open FunctionalMeetup
open FunctionalMeetup.ViewModels

type MeetupListController (cmds : UserCommands) =
    inherit UITableViewController (Title = "Meetups")

    let repo = cmds.Repo

    let mutable viewModel = None

    let mutable sub = None
    
    member this.UpdateViewModel () =
        let newViewModel = Some (repo.MakeMeetupList ())
        if viewModel <> newViewModel then
            this.BeginInvokeOnMainThread (fun () ->
                viewModel <- newViewModel
                this.TableView.ReloadData ())
                
    override this.ViewDidAppear (a) =
        base.ViewDidAppear (a)
        sub <- Some (repo.UpdatedData.Subscribe
                    this.UpdateViewModel)
        this.UpdateViewModel ()

    override this.ViewDidDisappear (a) =
        base.ViewDidDisappear (a)
        sub |> Option.iter (fun x -> x.Dispose ())
        sub <- None

    override this.ViewDidLoad () =
        base.ViewDidLoad ()
        this.NavigationItem.RightBarButtonItem <-
            new UIBarButtonItem (UIBarButtonSystemItem.Add, fun s e ->
                let vc = new CreateMeetupController (cmds)
                let nc = new UINavigationController (vc)
                this.PresentViewController (nc, true, null)
                ())

    override this.NumberOfSections (tv) = nint 1

    override this.RowsInSection (tv, s) =
        match viewModel with
        | Some vm -> nint vm.Items.Length
        | None -> nint 0

    override this.GetCell (tv, ip) =
        let c =
            match tv.DequeueReusableCell ("M") with
            | null -> new UITableViewCell (UITableViewCellStyle.Default, "M")
            | c -> c
        match viewModel, int ip.Row with
        | Some vm, i when i < vm.Items.Length ->
            c.TextLabel.Text <- vm.Items.[i].Title
        | _ -> ()
        c

