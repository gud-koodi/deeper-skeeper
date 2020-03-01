# Technical Design

## Used Technology

Game was made with Unity version `2019.2.11f1` which had available containers for CI building via GitHub actions. (Thanks @GabLeRoux). [DarkRift Networking](https://www.darkriftnetworking.com/darkrift2) was used for sending messages between players over network. The game is built for Linux, Windows & Mac, though we are unable to test out the Mac version.

At the current state, players can connect to the game and advance through a dungeon that consists of 10 similar rooms that are aligned differently and have a varying enemy setup.

## Game Architecture

Unity uses components, UML, scalability, solved challenges like dynamic nav mesh links, shortcomings

## Technical challenges

### Networking
    We decided that our game need Online co-op. One of our team members has done online 
    PvP shooter with Unreal Engine and thought it is a good idea! Unfortunately Unity
    had dropped their support for networking and were offering just paid Unity server
    based solutions. We said no thanks Unity and found third party C# plugin for sending
    TCP/UDP messages over network, DarkRift. And that is what it did, sent messages and
    received messages. Everything else we had to code ourselves: Server, Client, what to
    send, when and how to serialize the data etc. In the end networking was the biggest
    overhead in our project and reason why we did not get more done in gameplay. Still,
    worth it.
    
### Dynamic navigation mesh links
    Our game has randomly generated levels. AI pathfinding id dependent on navigational
    meshes and moving between those meshes is done with offmesh links. Our problem was that
    Unity's default navigation meshes are pre-baked and cannot be applied to prefabs.

    Luckily we found Unity's own NavMeshComponents scripts from their github (for some reason
    they are not part of the main engine) and were able to use those for our needs. NavMeshComponents
    are components that can be addes to game objects and thus allows us to use them in prefabs.
    Then in runtime when we generate the levels we add NavMeshLink components to each floor and
    calculate start point and end point and update the link. The links were working, enemies did
    traverse the links but unfortunately they did not jump from one floor to another but
    slowly walked in the air towards the lower point. We had to disable Nav mesh link autotraverse
    and implement out own method for that.
### CI, git and LFS
    Even though many indie game developers surely overlook having working Continuous Integration and version
    control, we wanted to have a working and automated commit-push-build pipeline. We use GitHub Actions as
    our CI platform

    Getting Unity build and run tests on CI was hard but we were prepared. We actually used many hours before
    the project for this purpose only so we could have a working CI at the start of the projects. (Those hours
    naturally are not counted on our project hours.)

    We got Unity build in Docker container with help of GitHub User @GabLeRoux examples. We had to modify them
    for our purposes and to get it run on GitHub Actions but those examples were very helpful!

    One big problem was licensing Unity in container. We use offline licensing with following workflow:
    `Load license from env -> store to file -> give file to unity -> restart unity` 
    For some reason licensing did not work without restart even though that was not needed in the examples.

    Git was never meant to be used with large binary assets and with Unity we have plenty of those. We use
    Git Large File Storage to handle those. GitHub gives 1GB of free bandwidth for LFS per month. After
    I pushed some high resolution material components to our repository out bandwidth needs exploded and
    1GB was full very fast. This is because out CI pulls the LFS assets four times per push (test, build
    Linux, build Windows and Build OSX). We don't change the binary assets that often so I setup caching
    for LFS files so we need to pull those only if they have changes.

### Getting unity run on Linux
    Since Unity does not support Linux officially (though they provide unofficial Linux installations), 
    one of our team member had problems to get Unity working on his Linux machine. He switched to Windows.

## Testing

Mostly manual, some structures have actual tests.
