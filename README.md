# stackman-prototype-monogame
A prototype I made over a weekend. Uses C# and MonoGame Library (XNA4 fork)
This is an experimental hybrid of Pac-Man and 2048. 

There is a compiled debug binary in \Game1\bin\DesktopGL\AnyCPU\Debug\

Instructions:
1. Use directional keys to move all pellets in a direction. 
2. Yellow pellet is StackMan. Blue pellets are ghosts. 
3. StackMan can eat all pellets. Eating a ghost pellet will result in GameOver.
4. Combine two ghosts to turn them Green (weakened ghost). Green ghosts can now be for higher score.
5. When a row or column is full press the opposite direction key to open a black-hole. 
6. Once open all pellets can be pushed into the black-hole. Pushing a Blue ghost into a black-hole will close it.
7. Once Four blue ghosts are spawned, the portals will open.
8. When next to a Portal, press W,A,S,D to teleport to the oppostite side.
9. Movement of the pellets starts from the last row/col in the direction of key pressed.
10. For example if Up key is pressed the Bottom most row pellets will first move one row above then the third row and so on. 

