![Screenshot of project](https://user-images.githubusercontent.com/48601026/176002007-5c0bf8d7-1c4e-4911-8d0a-fe985bc2589a.png)

# Explanation
This project generates a perfect maze with the width and height given by the user.
A perfect maze has every path connected to every other path, so there are no unreachable areas. 
The algorithm used is known as the "recursive backtracker" algorithm. 

Consider the space for a maze being a large grid of cells. 
Each cell starting with four walls. Starting from a random cell, the computer then selects a random neighbouring cell that has not yet been visited. 
The computer removes the wall between the two cells and marks the new cell as visited, and adds it to the stack to facilitate backtracking. 
The computer continues this process, with a cell that has no unvisited neighbours being considered a dead-end. 
When at a dead-end it backtracks through the path until it reaches a cell with an unvisited neighbour, continuing the path generation by visiting this new, unvisited cell. 
This process continues until every cell has been visited, causing the computer to backtrack all the way back to the beginning cell. 
We can be sure every cell is visited.

