using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Range(5, 500)]
    public int mazeWidth = 5, mazeHeight = 5;  // Maze dimensions

    public int startX, startY;  // Starting position for the maze

    [SerializeField] GameObject doorPrefab;       // Door prefab reference
    [SerializeField] Vector2Int doorPosition;     // Position for the door

    MazeCell[,] maze;  // Maze cell array
    Vector2Int currentCell;  // Current position in the maze

    // Get the generated maze
    public MazeCell[,] GetMaze()
    {
        if (maze == null) // Ensure maze is only generated once
        {
            maze = new MazeCell[mazeHeight, mazeWidth];
            for (int x = 0; x < mazeWidth; x++)
            {
                for (int y = 0; y < mazeHeight; y++)
                {
                    maze[x, y] = new MazeCell(x, y);
                }
            }
            CarvePath(startX, startY); // Generate the maze
        }

        return maze;
    }

    // Directions for maze carving
    List<Direction> directions = new List<Direction>
    {
        Direction.Up, Direction.Down, Direction.Left, Direction.Right,
    };

    List<Direction> GetRandomDirection()
    {
        List<Direction> dir = new List<Direction>(directions);
        List<Direction> rndDir = new List<Direction>();

        while (dir.Count > 0)
        {
            int rnd = Random.Range(0, dir.Count);
            rndDir.Add(dir[rnd]);
            dir.RemoveAt(rnd);
        }

        return rndDir;
    }

    bool IsCellValid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < mazeWidth && y < mazeHeight && !maze[x, y].visited;
    }

    Vector2Int CheckNeighbours()
    {
        List<Direction> rndDir = GetRandomDirection();

        for (int i = 0; i < rndDir.Count; i++)
        {
            Vector2Int neighbour = currentCell;

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

            if (IsCellValid(neighbour.x, neighbour.y)) return neighbour;
        }

        return currentCell;
    }

    void BreakWalls(Vector2Int primaryCell, Vector2Int secondaryCell)
    {
        if (primaryCell.x > secondaryCell.x)
        {
            maze[primaryCell.x, primaryCell.y].leftWall = false;
        }
        else if (primaryCell.x < secondaryCell.x)
        {
            maze[secondaryCell.x, secondaryCell.y].leftWall = false;
        }
        else if (primaryCell.y < secondaryCell.y)
        {
            maze[primaryCell.x, primaryCell.y].topWall = false;
        }
        else if (primaryCell.y > secondaryCell.y)
        {
            maze[secondaryCell.x, secondaryCell.y].topWall = false;
        }
    }

    void CarvePath(int x, int y)
    {
        currentCell = new Vector2Int(x, y);
        List<Vector2Int> path = new List<Vector2Int>();
        bool deadEnd = false;

        while (!deadEnd)
        {
            Vector2Int nextCell = CheckNeighbours();

            if (nextCell == currentCell)
            {
                for (int i = path.Count - 1; i >= 0; i--)
                {
                    currentCell = path[i];
                    path.RemoveAt(i);
                    nextCell = CheckNeighbours();

                    if (nextCell != currentCell) break;
                }

                if (nextCell == currentCell)
                    deadEnd = true;
            }
            else
            {
                BreakWalls(currentCell, nextCell);
                maze[currentCell.x, currentCell.y].visited = true;
                currentCell = nextCell;
                path.Add(currentCell);
            }
        }
    }

    private void PlaceDoor()
    {
        Vector3 doorPositionWorld = new Vector3(doorPosition.x * 5f, 1f, doorPosition.y * 5f); // Adjust the position as needed
        Instantiate(doorPrefab, doorPositionWorld, Quaternion.identity);
    }

    public void GenerateMaze()
    {
        maze = new MazeCell[mazeHeight, mazeWidth]; // Reset the maze

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                maze[x, y] = new MazeCell(x, y);
            }
        }

        CarvePath(startX, startY); // Generate the paths
        PlaceDoor();  // Place the door at the specified location
    }

    private void Start()
    {
        GenerateMaze(); // Start generating the maze
    }
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class MazeCell
{
    public bool visited;
    public int x, y;
    public bool topWall;
    public bool leftWall;

    public Vector2Int position
    {
        get
        {
            return new Vector2Int(x, y);
        }
    }

    public MazeCell(int x, int y)
    {
        this.x = x;
        this.y = y;
        visited = false;
        topWall = leftWall = true;
    }
}
