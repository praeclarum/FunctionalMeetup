namespace FunctionalMeetup.iOS

open System

open UIKit
open Foundation

open FunctionalMeetup

[<Register ("AppDelegate")>]
type AppDelegate () =
    inherit UIApplicationDelegate ()

    let initialUser : Person = { Name = "Sally"; Id = "1" }
    let client = TestMeetupClient ([| initialUser |])
    let repo = AppRepo { AppData.Initial with User = initialUser }
    let cmds = UserCommands (client, repo)

    let refresherCancel = new Threading.CancellationTokenSource ()
    let refresher = RefreshService (client, repo)

    override val Window = null with get, set

    override this.FinishedLaunching (app, options) =
        //
        // Init services
        //
        refresher.Start (refresherCancel.Token)

        //
        // Construct UI
        //
        let tabs = new UITabBarController ()
        let meetupList = new MeetupListController (cmds)

        tabs.SetViewControllers ([|
                                    new UINavigationController (meetupList)
                                 |], false)

        this.Window <- new UIWindow (UIScreen.MainScreen.Bounds)
        this.Window.RootViewController <- tabs
        this.Window.MakeKeyAndVisible ()
        true
