## Running the Application
- Clone the repository
- Open the solution in Visual Studio
- Run the solution (F5)
- The code can be tested by hitting the best-stories endpoint in the Swagger interface
## Changes given additional time
- Changes would likely be required to fit in with existing API style (e.g. a controller style may be preferred over minimal API style used)
- Unit test would likely be required which in turn would ideally require a mocking framework (e.g. MOQ) and a dependency injection framework (e.g. NINJECT)
- The performance of cache fill could be greatly improved by performing story fetches concurrently (e.g. 10 at a time)
- This solution has a simple cache which is flushed every five minutes. I would investigate the use of the notification API’s to allow cache updates only when cached items change
