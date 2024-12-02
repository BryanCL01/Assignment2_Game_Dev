using UnityEngine;

public class FogController : MonoBehaviour
{
    public GameObject targetObject; // Reference to the object to enable/disable

    private bool isObjectActive = false; // Tracks the object's active state

    void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target Object is not assigned!");
        }
        targetObject.SetActive(false);
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) // Detect when the F key is pressed
        {
            ToggleObject();
        }
    }

    void ToggleObject()
    {
        if (targetObject != null)
        {
            isObjectActive = !isObjectActive; // Toggle the state
            targetObject.SetActive(isObjectActive); // Enable or disable the object
        }
    }
}
