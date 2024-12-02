using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeftWallLogic : MonoBehaviour
{
    // Start is called before the first frame update
    private int playerTwoScore;
    public Text playerTwoText;
    public Text gameOverText;
    private bool gameEnded = false;
    public Transform spawnPoint;
    [SerializeField] private string sceneToLoad;
    void Start()
    {
        playerTwoScore = 0;
        gameOverText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        playerTwoText.text = "Player 2 Score: " + playerTwoScore.ToString();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (gameEnded) return;
        if (col.gameObject.name == ("Ball"))
        {
            col.gameObject.transform.position = spawnPoint.position;
            playerTwoScore++;
            if (playerTwoScore >= 5)
            {
                EndGame();
                return;
            }
        }
    }
    private void EndGame()
    {
        gameEnded = true; // Set the gameEnded flag to true
        GameObject ball = GameObject.Find("Ball");
        if (PlayerPrefs.HasKey("doorPosX"))
        {
            float doorPosX = PlayerPrefs.GetFloat("doorPosX");
            float doorPosY = PlayerPrefs.GetFloat("doorPosY");
            float doorPosZ = PlayerPrefs.GetFloat("doorPosZ");

            // Set the player's position to the door's position
            transform.position = new Vector3(doorPosX, doorPosY, doorPosZ);
        }
        SceneManager.LoadScene(sceneToLoad);
        gameOverText.gameObject.SetActive(true);
        gameOverText.text = "Game Over! PLayer 2 Wins!";
        Debug.Log("Player 2 Wins!");

    }
}
