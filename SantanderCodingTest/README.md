## Running the Application

- Download
- Can be run in Visual Studio (load solution and run in debug)
- Or from the command line

## Changes given additional time

- Likely changes would be required to fit in with existing API style (e.g. A controller style may be prefered over minimal API)
- Unit test would likely be required which would require mocking framework to mock the backend and a dependency injection framework to switch between mock / live
- The performance of cache fill could be greatly improved by parralising the story fetches
