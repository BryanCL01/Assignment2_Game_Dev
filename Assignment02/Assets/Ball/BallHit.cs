using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHit : MonoBehaviour
{
    public Transform hitSphere;
    public AudioClip collisionSound; // Assign the sound clip in the inspector
    private AudioSource audioSource;

    Ray ray;
    RaycastHit hitInfo;
    public LayerMask layerMask;

    void Start()
    {
        // Get the AudioSource component attached to the same GameObject
        audioSource = GetComponent<AudioSource>();
        
        // Check if AudioSource component is missing
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on " + gameObject.name);
        }

        // Check if the collision sound is assigned
        if (collisionSound == null)
        {
            Debug.LogError("Collision sound clip is not assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Set up the ray to start shooting from the camera
        ray.origin = Camera.main.transform.position;
        ray.direction = Camera.main.transform.forward;
        float distance = 30f;

        // If ray hits something, put the hitSphere at the location the ray hit
        if (Physics.Raycast(ray, out hitInfo, distance, layerMask))
        {
            // We hit something!
            hitSphere.gameObject.SetActive(true);
            hitSphere.position = hitInfo.point;

            // Play the collision sound
            if (audioSource != null && collisionSound != null)
            {
                audioSource.PlayOneShot(collisionSound);
            }
            else
            {
                Debug.LogWarning("AudioSource or collisionSound is missing.");
            }
        }
        else
        {
            // Optionally, you could hide the hitSphere when there's no hit.
            hitSphere.gameObject.SetActive(false);
            hitSphere.position = ray.origin + ray.direction * distance;
        }

        Debug.DrawLine(ray.origin, hitSphere.position, Color.red, 3f);
    }
}
