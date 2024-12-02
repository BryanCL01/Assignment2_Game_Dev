using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RightWallLogic : MonoBehaviour
{
    // Start is called before the first frame update
    private int playerOneScore;
    public Text playerOneText;
    public Text gameOverText;
    private bool gameEnded = false;
    public Transform spawnPoint;
    [SerializeField] private string sceneToLoad;
    void Start()
    {
        playerOneScore = 0;
        gameOverText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        playerOneText.text = "Player 1 Score: " + playerOneScore.ToString();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (gameEnded) return;
        if (col.gameObject.name == ("Ball"))
        {
            col.gameObject.transform.position = spawnPoint.position;
            playerOneScore++;
            if (playerOneScore >= 5)
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
        gameOverText.text = "Game Over! PLayer 1 Wins!";
        Debug.Log("Player 1 Wins!");

    }
}
