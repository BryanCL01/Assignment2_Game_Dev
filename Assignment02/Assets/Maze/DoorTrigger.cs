using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public MazeManager mazeManager;  // Reference to MazeManager to call OnPlayerEnterDoor

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Call OnPlayerEnterDoor to save position and load the next scene
            mazeManager.OnPlayerEnterDoor();
        }
    }
}
