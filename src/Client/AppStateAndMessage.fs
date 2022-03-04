namespace Client
module App =

    type PageState =
      | HomeState of Home.Model
      | SignalRState of SignalR.State
      | DrawerState of Drawer.State

    type Message =
      | HomeMessage of Home.Message
      | DrawerMessage of Drawer.Message
      | SignalRMessage of SignalR.Message

    type State = {
        IsLoading : bool
        PageState : PageState
        DrawerState : Option<Drawer.State>
    }
