using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField] MazeGenerator mazeGenerator;
    [SerializeField] GameObject MazeCellPrefab;

    [SerializeField] GameObject startPlatform;

    [SerializeField] GameObject EndPlatform;

    public float CellSize = 5f;

    private void Start()
    {

        MazeCell[,] maze = mazeGenerator.GetMaze();

        for (int x = 0; x < mazeGenerator.mazeWidth; x++)
        {
            for (int y = 0; y < mazeGenerator.mazeHeight; y++)
            {
                if (x == 0 && y == 0)
                {
                    Vector3 startPosition = new Vector3(x, 0, y); // or wherever your start position is
                    Instantiate(startPlatform, startPosition, Quaternion.identity);
                }
                else if (x == (mazeGenerator.mazeWidth - 1) & y == (mazeGenerator.mazeHeight - 1))
                {
                    Vector3 endPosition = new Vector3(mazeGenerator.mazeWidth - 1, 0, mazeGenerator.mazeHeight - 1);
                    Instantiate(EndPlatform, endPosition, Quaternion.identity);
                }

                GameObject newCell = Instantiate(MazeCellPrefab, new Vector3((float)x * CellSize, 0f, (float)y * CellSize), Quaternion.identity, transform);

                MazeCellObject mazeCell = newCell.GetComponent<MazeCellObject>();

                bool top = maze[x, y].topWall;
                bool left = maze[x, y].leftWall;

                bool botttom = false;
                bool right = false;
                if (x == mazeGenerator.mazeWidth - 1) right = true;
                if (y == 0) botttom = true;

                mazeCell.Init(top, botttom, right, left);
            }
        }


    }
}
