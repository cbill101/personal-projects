Design Choices:

CLIENT:
We decided to have a menu strip for having control help and the about instead of a button because it feels more natural to access tabs in a menu at the top.
Also, the GUI should adjust to the world size, and the form shouldn't be resized during play, so we fixed the form after connection, and resized the main components depending on the world size.

For drawing the images, we decided that accessing the images would be much eeasier if wwe added them into the View project's Resources properties folder.
This made it much easier to get images. Also, the images are arranged into Image arrays so if more models are made, they can be added to the arrays and code can
be modified easily for accomodation.

For the scoreboard, we made the font slightly bigger to make it easier to read during play. Otherwise, it's roughly the same as the provided client in functionality (Sorts by score, using Graphics instead of labels, etc.)

SERVER:
Using a LinkedList to manage clients helps make graceful disconnections possible.

SpaceWarsSettings serves as the main settings hub. Everything on the server is modular. The defaults can be edited, as well as the XML.

Managing teams and adapting ID's for the mode through the ships helps simplify team implementation. Adding the team ID to the username label helps the user to identify their team.

Working inside out helps with collision detection. World deals with collisions on borders and some objects, and objects deal with collisions on other objects.

The testing strategy is straightforward. There's a lot of simple tests, then there are some tests that try to simulate a real game situation, which is necessary for the big world functions.
NOTE: YOU MUST RUN ALL TESTS, DO NOT TEST THEM STANDALONE, AS GLOBAL ID'S CARRY OVER FROM TEST TO TEST.

Features:

Client: See design choices -> Client.

Server:

The server is very technical and has little in the way of variation. The position of the objects on screen is calculated by using Vector2d
vectors and adding all forces together. This is done for all objects except stars.

We added the feature to play on teams. This can be turned on/off in the settings xml file with the by changing <Team> to true or false.
This will cause players to vary being on a team as every other player to join will be separated. Teammates share a score, cannot shoot each other
and are the same color/ship model.

