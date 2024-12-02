using UnityEngine;

public class BallController : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        Debug.Log(collision.gameObject);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Update the score
            FindObjectOfType<ScoreManager>().UpdateScore();

            // Destroy the ball
            gameObject.transform.position = new Vector3(0, -50, 0);

            // Reduce enemy's health
            collision.gameObject.GetComponent<Enemy>().TakeDamage();
        }
    }
}
