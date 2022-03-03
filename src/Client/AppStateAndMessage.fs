namespace Client
module App =

    type PageState =
      | HomeState of Home.Model
      | AlertState
      | DrawerState of Drawer.State

    type Message =
      | HomeMessage of Home.Message
      | DrawerMessage of Drawer.Message
      | AlertMessage

    type State = {
        IsLoading : bool
        PageState : PageState
        DrawerState : Option<Drawer.State>
    }

