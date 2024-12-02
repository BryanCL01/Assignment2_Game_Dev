using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 3; // The enemy can take 3 hits before disappearing
    private float timer = 0f; 
    public float respawnTime = 5.0f;
    public GameObject player; 


    void Update() {
        if (health == 0) {

            timer += Time.deltaTime;
             
             if (timer < respawnTime) {
                health = 3; 
                timer = 0; 
                int newX = Random.Range(0, 100);
                int newZ = Random.Range(0, 100);
                gameObject.transform.position = new Vector3(newX, 0, newZ);

             }
        }

    }
    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            // Reset score when the enemy dies
            gameObject.transform.position = new Vector3(0, -70, 0);
        }
    }

}

