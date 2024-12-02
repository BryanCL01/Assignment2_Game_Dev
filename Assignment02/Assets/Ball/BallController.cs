using UnityEngine;

public class BallController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit");
            // Update the score
            FindObjectOfType<ScoreManager>().UpdateScore();

            // Destroy the ball
            gameObject.transform.position = new Vector3(0,-50, 0);

            // Reduce enemy's health
            collision.gameObject.GetComponent<Enemy>().TakeDamage();
        }
    }
}
