
# FunctionalMeetup

This app demonstrates an architecture to integrate functional programming
techniques in mobile apps.

It demonstrates:

1. An immutable, rooted object `AppData` that contains all the data
needed by the app.

2. An object that manages transactional changes to that data called `AppRepo`

3. `ViewModels` are created from the `AppData`

4. UI objects refresh their view models when the data changes

5. Services also modify the data

Platform independent code is separated into a `Core` library.


