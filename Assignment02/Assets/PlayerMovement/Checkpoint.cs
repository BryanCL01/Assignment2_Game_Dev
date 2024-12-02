using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public ScoreManager scoreManager;  // Reference to the ScoreManager script
    public GameObject player; // The player GameObject

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Save the player's position when they reach this checkpoint
            Debug.Log(player.transform.position);
            scoreManager.SavePlayerPosition();
            Debug.Log("Player position saved!");


        }
    }
}
