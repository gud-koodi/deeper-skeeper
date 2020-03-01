# Technical Design

## Used Technology

Game was made with Unity version `2019.2.11f1` which had available containers for CI building via GitHub actions (Thanks @GabLeRoux). [DarkRift Networking](https://www.darkriftnetworking.com/darkrift2) was used for sending messages between players over network. The game is built for Linux, Windows & Mac, though we have been unable to test out the Mac version.

At the current state, players can connect to the game and advance through a dungeon that consists of 10 similar rooms that are aligned differently each time and have a varying enemy setup.

## Game Architecture

Following Unity's component-based system, most of the hierarchy in this project is relatively simple;
components are designed to work individually, and at most they will search for other components from the same object.
Communication between objects is handled indirectly with events and event listeners.

Network code, however, required a remarkably different approach.
Server and clients must identify and keep a track of multiple different types of objects,
and some of the editor side configuration, such as configuring event references, wasn't directly available.
In addition, similar but not quite equivalent code in both server and client controllers
caused a need for more rigid approach to this part of the project.

The following is the class diagram of the current network hierarchy.
Unity-originated classes are colored with green, and DarkRift-originated classes are colored with orange.

![Network class diagram](Images/network-uml.png)

* At the root is `MonoBehaviour`, the base class for all Unity's components.
First it is inherited by both `ClientController` and `ServerController`,
that control their respective DarkRift components.
This includes sending and responding to network messages.
* `NetworkEvents` is a container for different kinds of named events,
that the server and clients may need to listen to.
For example, creation of a new object must always be requested from the server,
to guarantee that its network ID is unique and same for all clients.
Clients on their part need to report back when their respective player avatar is moving in the game.
* Server generates IDs to objects using `NetworkIDPool`,
and they are mapped to an `ObjectManager` indexed by this ID.
In the current state of the project,
there exists two separate and specialized *ObjectManagers* for both players and enemies.
Generic type parameter `T` must implement `INetworkSerializable`.
* `ObjectManager` uses `NetworkObjectList` internally for two-way mapping between
actual `GameObject` objects in the game and `INetworkSerializable` implementing serialization objects.
* `INetworkSerializable` is slightly extended version of `IDarkRiftSerializable` to guarantee,
that each implementation has the property `NetworkID`.

One detail missing from this diagram is where the reference for a prototype object comes from,
when a new one needs to be created.
At the moment there exists a slightly different approach for both players and enemies,
because before implementing bot synchronization it wasn't clear yet as to how NavMesh agents should be handled.
Though the main idea is very similar to how `NetworkEvents` works as a preconfigured container class.

Sending and replicating received states also use different approaches between players and enemies.
For a player there exists two different components that are used to either send or receive updates.
Enemies, however, have only one component
where a strategy pattern is used to toggle between these two modes at a request.
The latter approach seems to work better especially with more complicated components.
In addition, any serialization is currently handled in `ObjectManager`,
when giving that responsibility to the component itself would also make client/server more scalable.

Towards the end of the course, few shortcuts had to be taken to get this game to its currently functional state.
These were few examples of that, but a lot could also be done to clean up server/client specific code.
For example, adding a boss network logic to the game would currently require implementation of its specific
component(s), events, `ObjectManager`, serialization class, network message tags
and partially identical code handlers to both `ServerController` and `ClientController`.
Some of these are inherently necessary, but with above ideas a lot of extra weight could be lifted.


## Technical challenges

### Networking
We decided to implement online Co-Op for our game. One of our team members had previously done an online 
PvP shooter with Unreal Engine and thought it would be as straightforward to accomplish here.
Unfortunately Unity's seemingly similar feature had been deprecated a good while ago,
and instead they were only offering commercial cloud server solutions.
Fortunately we managed to find a third party C# plugin called DarkRift,
that allowed us to send TCP/UDP messages over network.
However, game-logic-wise we had to implement everything ourselves: Server, Client, what to
send, when and how to serialize the data etc. In the end networking was the biggest
overhead in our project and the reason why we didn't get that far with the gameplay.
Yet, we would argue that the experience was very valuable and worth the effort.

### Dynamic navigation mesh links
Our game has randomly generated levels. AI pathfinding is dependent on navigational
meshes and moving between these meshes is done via offmesh links. Our problem was that
Unity's default navigation meshes are pre-baked and cannot be applied to prefabs.

Luckily we found Unity's own NavMeshComponents scripts from their github (for some reason
they are not part of the main engine) and were able to use those for our needs. NavMeshComponents
are components that can be added to game objects and thus allows us to use them in prefabs.
Then in runtime when we generate a level, we add NavMeshLink components to each floor and
calculate start and end point and update the link.

These links were working and enemies traversed them, but rather than jumping to a lower floor,
they instead were slowly walking through the air towards the ground.
We had to disable the default behavior and implement our own method to achieve the desired effect.

### CI, Git and LFS
Even though many indie game developers surely overlook having working Continuous Integration and version
control, we wanted to have a working and automated commit-push-build pipeline. We use GitHub Actions as
our CI platform.

Getting Unity build and run tests on CI was hard, but some preparation had been done beforehand.
We actually had used many hours before the project solely for this purpose,
so that we had a working CI at the start of the project
(Those hours naturally are not counted on our project hours).

We got Unity to build in a Docker container with a help of examples by GitHub user @GabLeRoux. We had to modify them
for our purposes and to get it run on GitHub Actions but those examples were very helpful!

One big problem was licensing Unity in container. We use offline licensing with following workflow:
`Load license from env -> store to file -> give file to unity -> restart unity` 
For some reason licensing did not work without restart even though that was not needed in the examples.

Git was never meant to be used with large binary assets and with Unity we have plenty of those. We use
Git Large File Storage to handle those. GitHub gives 1GB of free bandwidth for LFS per month. After
I pushed some high resolution material components to our repository out bandwidth needs exploded and
1GB was full very fast. This is because out CI pulls the LFS assets four times per push (test, build
Linux, build Windows and Build OSX). We don't change these binary assets that often so I setup caching
for LFS files so we need to pull those only if they have changes.

### Getting Unity run on Linux
Since Unity does not support Linux officially (though they provide unofficial Linux installations), 
one of our team members had problems to get Unity working on their Linux machine.
After giving up, a switch to Windows was made.

## Testing

Due to the nature of server/client relationship in our game,
it would've been very difficult to come up with proper tests for the game.
Currently only `NetworkIDPool` and `NetworkObjectList` utilize unit testing to make sure that they work properly.
Otherwise, manual testing has been the dominant way to test our project by printing debug error messages
at places where things had a chance to break, or to provide insight when things seemingly broke down.

One more sneaky bug in the project occurred, when a certain reference in Unity Editor had broken for some reason.
This made the client always connect to *locahost*,
which allowed it to go unnoticed for a while before connecting to a remote host was attempted.
