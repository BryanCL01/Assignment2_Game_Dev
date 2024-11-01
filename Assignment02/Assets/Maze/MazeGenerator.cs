using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Range(5, 500)]

    //The maze dimensions
    public int mazeWidth = 5, mazeHeight = 5; 

    //Start position
    public int startX, startY; 

    //Array of maze cells
    MazeCell[,] maze; 

    //current maze cell
    Vector2Int currentCell; 

    public MazeCell[,] GetMaze() 
    {
        maze = new MazeCell[mazeHeight, mazeWidth];

        for (int x = 0; x < mazeWidth; x++) 
        {
            for (int y= 0; y < mazeHeight; y++)
            {
                maze[x,y] = new MazeCell(x,y); 
            }
        }

        CarvePath(startX, startY);
        
        return maze; 
    }

    List<Direction> direction = new List<Direction> 
    {
        Direction.Up, Direction.Down, Direction.Left, Direction.Right,
    };

    List<Direction> GetRandomDirection() 
    {
        //Making copy of list
        List<Direction> dir = new List<Direction>(direction);
        //List to put random directions in
        List<Direction> rndDir = new List<Direction>(); 

        // Loop until rndDir is empty
        while (dir.Count > 0)
        {
            //Get random index in list
            int rnd = Random.Range(0, dir.Count);
            //Add the random direction to our list
            rndDir.Add(dir[rnd]); 
            //Remove the direction so we can't choose it again
            dir.RemoveAt(rnd); 
        }
        //When we have all four directions in a random order, return it
        return rndDir; 
    }


    bool IsCellValid (int x, int y) 
    {
        if (x < 0 || y < 0 || x > mazeWidth - 1 || y > mazeHeight - 1 || maze[x, y].visited) 
            return false; 
        else 
            return true; 
    }

    Vector2Int CheckNeighbour() 
    {
        List<Direction> rndDir = GetRandomDirection(); 

        for (int i = 0; i < rndDir.Count; i++) 
        {

            //Set neighnour coordinates to current cell for now. 
            Vector2Int neighbour = currentCell; 

            //Modify neighbour coordinates based on the radnom directions
            switch (rndDir[i]) 
            {
                case Direction.Up:
                    neighbour.y++;
                    break; 
                case Direction.Down:
                    neighbour.y--;
                    break;
                case Direction.Right:
                    neighbour.x++;
                    break; 
                case Direction.Left:
                    neighbour.x--;
                    break; 

            }
            // If the neighour we just tried is valid, we can return it
            if (IsCellValid(neighbour.x, neighbour.y)) return neighbour; 
        } 
        // If we tried all directions and didn't find a valid neighbour, return currectCell
        return currentCell; 
    }

    void BreakWalls (Vector2Int primaryCell, Vector2Int secondaryCell) 
    {
        //We can only go in one direction at a time 
        //Primary Cells's Left Wall
        if (primaryCell.x > secondaryCell.x) {
            maze[primaryCell.x, primaryCell.y].leftWall = false; 
        } else 
        if (primaryCell.x < secondaryCell.x) {
            maze[secondaryCell.x, secondaryCell.y].leftWall = false; 
        } else 
        if (primaryCell.y < secondaryCell.y) {
            maze[primaryCell.x, primaryCell.y].topWall = false;
        } else 
        if (primaryCell.y > secondaryCell.y) {
            maze[secondaryCell.x, secondaryCell.y].topWall = false; 
        }
    }

    //Carves a path through the maze until it encounters a dead end
    void CarvePath(int x, int y) 
    {
        if (x < 0 || y < 0 ||x > mazeWidth -1 || y > mazeHeight -1)
        {
            x = y = 0;
            Debug.LogWarning("Starting position is out of bounds, defaulting to 0,0");
        }

        currentCell = new Vector2Int(x, y); 

        //A list to keep track of our current path
        List<Vector2Int> path = new List<Vector2Int>(); 

        bool deadEnd = false;
        while (!deadEnd) 
        {
            //Get the next cell w're going to try
            Vector2Int nextCell = CheckNeighbour(); 

            if (nextCell == currentCell) 
            {

                //If cell has no valid neighbours, set deadend to true
                for (int i = path.Count - 1; i >= 0; i--) 
                {
                    //Set currentCell to the next step
                    currentCell = path[i];
                    //Remove this step from the path
                    path.RemoveAt(i); 
                    //Check that cell to see if any other neighbours are valid
                    nextCell = CheckNeighbour(); 

                    if (nextCell != currentCell) break;
                }

                if (nextCell == currentCell)
                    deadEnd = true;

            } else {
                // Set wall flags on these cells 
                BreakWalls(currentCell, nextCell);
                //Set cell to visited 
                maze[currentCell.x, currentCell.y].visited = true; 
                //Set the current cell to valid neighbour
                currentCell = nextCell; 
                //Add this cell to our path
                path.Add(currentCell); 
            }
        }
    }

}


public enum Direction 
{
    Up, 
    Down, 
    Left, 
    Right
}

//Holds data for each indiviual maze cell.
public class MazeCell 
{
    public bool visited; 
    public int x, y; 
    public bool topWall; 
    public bool leftWall;

    // Return x and y as a Vector2Int for conveience. 
    public Vector2Int position {
        get {
            return new Vector2Int(x,y); 
        }
    }

    public MazeCell(int x, int y) 
    {
        // coordinates of the cell in the maze grid. 
        this.x = x; 
        this.y = y;
        
        //Whether the algorithm has visited this cell or not 
        visited = false; 
        topWall = leftWall = true; 
    }
}

