using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetGame : MonoBehaviour
{
    public Enemy enemy; 
    // Start is called before the first frame update
  void OnControllerColliderHit(ControllerColliderHit hit) {
    
    if (hit.gameObject.CompareTag("Enemy")){
            enemy.health = 3; 
            FindObjectOfType<ScoreManager>().ResetScore();
            CharacterController charController = GetComponent<CharacterController>();
            charController.enabled = false;
            transform.position = new Vector3(0, 0, 0);
            charController.enabled = true;
            int newX = Random.Range(0, 100);
            int newZ = Random.Range(0, 100);
            enemy.gameObject.transform.position = new Vector3(newX, 0, newZ);
        }
    }
  }
