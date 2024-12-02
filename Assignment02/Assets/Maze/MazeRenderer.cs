using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.SceneManagement;

public class MazeRenderer : MonoBehaviour
{   
    [SerializeField] MazeGenerator mazeGenerator;
    [SerializeField] GameObject MazeCellPrefab;

    [SerializeField] GameObject startPlatform;
    [SerializeField] GameObject EndPlatform;
    

    public float CellSize = 5f;
    private static bool isGenned = false; // Static to persist across scenes

    private void Awake()
    {
        // Make sure this GameObject persists across scenes
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        // Generate the maze only once
        if (!isGenned)
        {
            GenerateMaze();
            isGenned = true; // Mark as generated
        }

        // Rebuild the NavMesh for navigation
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    private void GenerateMaze()
    {
        MazeCell[,] maze = mazeGenerator.GetMaze();

        for (int x = 0; x < mazeGenerator.mazeWidth; x++)
        {
            for (int y = 0; y < mazeGenerator.mazeHeight; y++)
            {
                // Instantiate start and end platforms
                if (x == 0 && y == 0)
                {
                    Vector3 startPosition = new Vector3(x, 0, y);
                    Instantiate(startPlatform, startPosition, Quaternion.identity);
                }
                else if (x == (mazeGenerator.mazeWidth - 1) && y == (mazeGenerator.mazeHeight - 1))
                {
                    Vector3 endPosition = new Vector3(
                        (mazeGenerator.mazeWidth - 1) * CellSize,
                        1,
                        (mazeGenerator.mazeHeight - 1) * CellSize
                    );
                    Instantiate(EndPlatform, endPosition, Quaternion.identity);
                }

                // Instantiate maze cells
                GameObject newCell = Instantiate(
                    MazeCellPrefab,
                    new Vector3((float)x * CellSize, 0f, (float)y * CellSize),
                    Quaternion.identity,
                    transform
                );

                MazeCellObject mazeCell = newCell.GetComponent<MazeCellObject>();

                bool top = maze[x, y].topWall;
                bool left = maze[x, y].leftWall;

                bool bottom = false;
                bool right = false;
                if (x == mazeGenerator.mazeWidth - 1) right = true;
                if (y == 0) bottom = true;

                mazeCell.Init(top, bottom, right, left);
            }
        }
    }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the current scene is the maze scene
        if (scene.name == "Maze") // Replace "MazeScene" with your actual maze scene name
        {
            this.gameObject.SetActive(true); // Enable the maze in the maze scene
        }
        else
        {
            this.gameObject.SetActive(false); // Disable the maze in other scenes
        }
    }
}
