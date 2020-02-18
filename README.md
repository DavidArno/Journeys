# Journeys
This is my take on the [Journeys coding challenge](https://github.com/mikehadlow/Journeys). 

I recently met up with [Mike Hadlow](https://github.com/mikehadlow) and he spoke of this challenge and how much he enjoyed tackling it. Looking at it, it seemed to me to be an ideal way of showcasing some of the features of C# 8 that support a more functional style of programming.

The challenge is to parse a file to obtain a set of journeys that a robot goes on and to validate those journeys. Copying Mike's words here, an example journey would be:
```
1 1 E
RFRFRFRF
1 1 E
```
Each journey starts with the initial coordinates of the robot (`1 1` in this case) and the direction it is pointing in. In this case `E` (East). The directions are as follows:
```
N = North
E = East
S = South
W = West
```
Following the starting conditions are a list of commands:
```
RFRFRFRF
```
Each character is a command, either to turn (`L` = left, `R` = right) or to move forwards (`F`).
Finally the journey ends with another set of coordinates and a direction. This is the expected position and orientation of the robot at the end of the journey.

I set myself the following aims with the code:
* Aside from in the "composition root" of `Main()`, there should be no dependencies on code with side effects. So for example, the file parser is not allowed direct access to the file system.
* Where possible, all methods should be pure functions. In other words, they should be static methods in static classes with no access to state other than what's encapsulated by the method.
* Use C# 8's pattern matching.
* Use C# 8's nullable reference types feature and avoid nullable references.

To avoid the parser having direct access to the file system, I used the brilliant [System.IO.Abstractions library](https://github.com/System-IO-Abstractions/System.IO.Abstractions), allowing the parser to just know about `IFileSystem`, with `Main` injecting the real file system at runtime.

The only mutable state in the system is within the parser. A private class there implements `IEnumerator<>` and handles the state machine around reading the file, parsing the contents and handling any errors.

I also used my own [SuccincT library](https://github.com/DavidArno/SuccincT/wiki) to provide an `Either<Journey, ParserError>` discriminated union (DU), so the parser can yield either a successfully parsed journey or an error. The downside to such "roll your own" DUs is that it just has the properties `IsLeft`, `Left` and `Right`, which can hinder the readability of the code. So I added some extension methods here (`IsJourney`, `AsJourney` and `AsParserError` respectively) that I feel make the code clearer. 

To try out my solution, ensure you have .NET Core 3.1 installed and then either clone this repo into VS2019 and hit F5 or use the .NET Core command line tools.

