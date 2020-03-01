# Technical Design

## Used Technology

Game was made with Unity version `2019.2.11f1` which had available containers for CI building via GitHub actions. (Thanks @GabLeRoux). [DarkRift Networking](https://www.darkriftnetworking.com/darkrift2) was used for sending messages between players over network. The game is built for Linux, Windows & Mac, though we are unable to test out the Mac version.

At the current state, players can connect to the game and advance through a dungeon that consists of 10 similar rooms that are aligned differently and have a varying enemy setup.

## Game Architecture

Unity uses components, UML, scalability, solved challenges like dynamic nav mesh links, shortcomings

## Technical challenges

### Networking

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
### CI

### Getting unity run on Linux

## Testing

Mostly manual, some structures have actual tests.
