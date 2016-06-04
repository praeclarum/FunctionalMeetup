namespace FunctionalMeetup.iOS

open System

open UIKit
open Foundation

open FunctionalMeetup

[<Register ("AppDelegate")>]
type AppDelegate () =
    inherit UIApplicationDelegate ()

    override val Window = null with get, set

    override this.FinishedLaunching (app, options) =

        //
        // Init data
        //
        let user : Person = { Name = "Sally"; Id = "1" }
        let client = TestMeetupClient ([| user |])
        let repo = AppRepo { AppData.Initial with User = user }
        let cmds = UserCommands (client, repo)

        let tabs = new UITabBarController ()
        let meetupList = new MeetupListController (cmds)

        tabs.SetViewControllers ([|
                                    new UINavigationController (meetupList)
                                 |], false)

        this.Window <- new UIWindow (UIScreen.MainScreen.Bounds)
        this.Window.RootViewController <- tabs
        this.Window.MakeKeyAndVisible ()
        true
