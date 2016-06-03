namespace FunctionalMeetup.iOS

open System

open UIKit
open Foundation

[<Register ("AppDelegate")>]
type AppDelegate () =
    inherit UIApplicationDelegate ()

    override val Window = null with get, set

    override this.FinishedLaunching (app, options) =

        let tabs = new UITabBarController ()

        this.Window <- new UIWindow (UIScreen.MainScreen.Bounds)
        this.Window.RootViewController <- tabs

        true
