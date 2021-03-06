# causation-game
A minor refactor of the 2D game project for the Collaborative Game Development Davenport University class made in Unity version 2020.1.2f1 (version 2019.4.9f1 of Unity was used for the class, but an upgrade was necessary for some bugs.)
PS. Previous teammates your names are still in the in-game credits but if you want credits here at the top, please reach out to me and let me know :)

Postmortem and Reflection:
A postmortem was already created for the class but here are some of my thoughts and reflections not adhering to a classroom format.

Good:
The game, Causation, is a "full" featured video game that has a start, middle, and an end. It was developed within the course of a semester and while the conceptual stage had more promise (as do most games), the end product is indeed a video game.

Bad:
There are a few design quirks with the actual gameplay. Components that were once requirements had to be cut out due to being out of scope and a lack of time. So the bad could be not having a realistic scope given the time and team availability, and not having a coherent design principle to stick to.

Ugly:
Code that takes the path of least resistance to achieve its goal. Basically, the time constraints were heavily factored and the code quality suffered as a result. And here's where this refactor fork comes in.

This repo is an attempt to rein in the code and clean it up. Objects and functions that are too tightly wound up could be decoupled, certain game components might need complete reworking, polishing and adjusting the gameplay elements, etc.

Why? As someone who's passionate about game development and programming in general, I love being able to program and... well, make games. So this is a chance for me to be able to keep programming while also practicing and exercising SOLID (literally and figuratively) design principles. Or utilize new and possibly smarter design patterns to improvise this student project.

As a warning: This is an attempt to fix the programming and code that I had some control over, not the entire project. Nor is this an attempt to re-design the entire game (meant to be a team project) alone and from the ground-up!

