using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeManager : MonoBehaviour
{
    public GameObject player;
    public GameObject doorPrefab; // Reference to door prefab
    public string sceneToLoad; // Name of the scene to load
    private Vector3 savedPlayerPosition;

    private void Start()
    {
        // Check if there's a saved player position and set it
        if (PlayerPrefs.HasKey("PlayerPosX") && PlayerPrefs.HasKey("PlayerPosY") && PlayerPrefs.HasKey("PlayerPosZ"))
        {
            float x = PlayerPrefs.GetFloat("PlayerPosX");
            float y = PlayerPrefs.GetFloat("PlayerPosY");
            float z = PlayerPrefs.GetFloat("PlayerPosZ");
            savedPlayerPosition = new Vector3(x, y, z);
            player.transform.position = savedPlayerPosition;
        }
        else
        {
            savedPlayerPosition = player.transform.position; // Use starting position if no save exists
        }

        // If the door prefab is not instantiated in the scene, instantiate it here (optional)
        if (doorPrefab != null)
        {
            // You can add logic to position the door within the maze
            Instantiate(doorPrefab, savedPlayerPosition + new Vector3(5, 0, 0), Quaternion.identity);
        }
    }

    public void SavePlayerPosition()
    {
        PlayerPrefs.SetFloat("PlayerPosX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", player.transform.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", player.transform.position.z);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad); // Load the next scene
    }

    public void OnPlayerEnterDoor()
    {
        SavePlayerPosition();
        LoadScene();
    }
}
