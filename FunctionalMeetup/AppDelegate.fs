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

        let repo = AppRepo AppData.Initial

        let tabs = new UITabBarController ()

        let meetupList = new MeetupListController (repo)

        tabs.SetViewControllers ([|
                                    new UINavigationController (meetupList)
                                 |], false)

        this.Window <- new UIWindow (UIScreen.MainScreen.Bounds)
        this.Window.RootViewController <- tabs
        this.Window.MakeKeyAndVisible ()
        true
