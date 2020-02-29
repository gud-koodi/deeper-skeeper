# Technical Design

## Used Technology

Game was made with Unity version `2019.2.11f1` which had available containers for CI building via GitHub actions. [DarkRift Networking](https://www.darkriftnetworking.com/darkrift2) was used for sending messages between players over network. The game is built for Linux, Windows & Mac, though we are unable to test out the Mac version.

At the current state, players can connect to the game and advance through a dungeon that consists of 10 similar rooms that are aligned differently and have a varying enemy setup.

## Game Architechture

Unity uses components, UML, scalability, solved challenges like dynamic nav mesh links, shortcomings

## Testing

Mostly manual, some structures have actual tests.
