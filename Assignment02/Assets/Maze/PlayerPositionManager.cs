using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPositionManager : MonoBehaviour
{
    public static PlayerPositionManager Instance { get; private set; }

    private Vector3 savedPosition = Vector3.zero; // Default position if not set

    private void Awake()
    {
        // Ensure a single instance (Singleton pattern)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Save the player's position
    public void SavePlayerPosition(Vector3 position)
    {
        savedPosition = position;
    }

    // Restore the player's position
    public Vector3 GetSavedPosition()
    {
        return savedPosition;
    }
}
