using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    public GameObject player;  // Reference to the player object
    public string sceneToLoad; // Scene to load when the player enters the door
    public Transform Player;
    private Vector3 lastPosition;  // To store the player's last position

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player enters the door
        {
            // Save the player's current position
            lastPosition = player.transform.position;
            PlayerPrefs.SetFloat("PlayerPosX", lastPosition.x);
            PlayerPrefs.SetFloat("PlayerPosY", lastPosition.y);
            PlayerPrefs.SetFloat("PlayerPosZ", lastPosition.z);

            // Optionally save the maze state here if needed
            PlayerPositionManager.Instance.SavePlayerPosition(player.transform.position);

            // Load the next scene (or reload the current scene)
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void Start()
    {
        // When the player returns to the scene, load their saved position
        if (PlayerPrefs.HasKey("PlayerPosX"))
        {
            float x = PlayerPrefs.GetFloat("PlayerPosX");
            float y = PlayerPrefs.GetFloat("PlayerPosY");
            float z = PlayerPrefs.GetFloat("PlayerPosZ");

            player.transform.position = PlayerPositionManager.Instance.GetSavedPosition();
        }
    }
}
