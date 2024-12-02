using UnityEngine;
using TMPro; // If using TextMeshPro

public class EndPlatform : MonoBehaviour
{
    public GameObject winText; // Reference to the "You Won!" text GameObject

    private void Start()
    {
        if (winText == null)
        {
            winText = GameObject.Find("WinText"); // Use the name of your Text object
            winText.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Check if the collision was with a specific object (optional)
        if (collision.CompareTag("Player"))
        {
            // Enable the winText GameObject to show the message
            winText.SetActive(true);
            Debug.Log("Step!");
        }
    }
}