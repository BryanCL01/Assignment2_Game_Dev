using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText; // Assign a UI Text element for displaying the score
    private int score;
    public GameObject player; // The player GameObject

    void Start()
    {
        // Load the saved score and player position from PlayerPrefs
        score = PlayerPrefs.GetInt("Score", 0);
        float playerX = PlayerPrefs.GetFloat("PlayerPosX", player.transform.position.x);
        float playerY = PlayerPrefs.GetFloat("PlayerPosY", player.transform.position.y);
        float playerZ = PlayerPrefs.GetFloat("PlayerPosZ", player.transform.position.z);

        // Set the player's position to the saved values
        player.transform.position = new Vector3(playerX, playerY, playerZ);
        Debug.Log(player.transform.position);

        // Display the score
        DisplayScore();
    }

    // Update the score
    public void UpdateScore()
    {
        score++;
        PlayerPrefs.SetInt("Score", score); // Save the current score
        PlayerPrefs.Save(); // Ensure data is written to disk

        DisplayScore();
    }

    // Reset the score to 0
    public void ResetScore()
    {
        score = 0;
        PlayerPrefs.SetInt("Score", score); // Save the reset score
        PlayerPrefs.Save();

        DisplayScore();
    }

    // Display the current score
    private void DisplayScore()
    {
        scoreText.text = "Score: " + score;
    }

    // Save the player position
    public void SavePlayerPosition()
    {
        PlayerPrefs.SetFloat("PlayerPosX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", player.transform.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", player.transform.position.z);
        PlayerPrefs.Save(); // Ensure data is saved
    }
}
