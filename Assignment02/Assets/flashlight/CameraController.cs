using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform _camera;
    public Transform playerBody;
    public Transform hand;
    public float cameraSensitivity = 200.0f;
    public float cameraAcceleration = 5.0f;

    public float sensitivity = 100f;  // Unified sensitivity for mouse and controller
    private Player_InputActions inputActions;
    private InputAction lookAction;
    private float xRotation = 0f;

    public float rotation_x_axis;
    public float rotation_y_axis;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Awake()
    {
        inputActions = new Player_InputActions();
    }
    private void OnEnable()
    {
        lookAction = inputActions.Player.Look; // Assuming 'Player' is the action map and 'Look' is the action name
        lookAction.Enable();
    }

    private void OnDisable()
    {
        lookAction.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        // rotation_x_axis += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
        // rotation_y_axis += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        // rotation_x_axis = Mathf.Clamp(rotation_x_axis, -90.0f, 90.0f);

         hand.localRotation = Quaternion.Euler(-rotation_x_axis, rotation_y_axis, 0);

        // transform.localRotation = Quaternion.Lerp(transform.localRotation,
        // Quaternion.Euler(0, rotation_y_axis, 0), cameraAcceleration  * Time.deltaTime);
        // _camera.localRotation = Quaternion.Lerp(_camera.localRotation,
        // Quaternion.Euler(-rotation_x_axis, 0, 0), cameraAcceleration  * Time.deltaTime);
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        float mouseX = lookInput.x * sensitivity * Time.deltaTime;
        float mouseY = lookInput.y * sensitivity * Time.deltaTime;

        // Vertical rotation (looking up and down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal rotation (rotating the player body)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
