public static class MazeManager
{
    // Store the maze layout, with each cell's visited state or other relevant data.
    public static int[,] mazeLayout;

    // Method to save the maze data (for example, the visited state of cells).
    public static void SaveMaze(int[,] layout)
    {
        mazeLayout = layout;
    }

    // Method to load the maze data.
    public static int[,] LoadMaze()
    {
        return mazeLayout; // Return the saved layout (null if not saved yet).
    }
}
