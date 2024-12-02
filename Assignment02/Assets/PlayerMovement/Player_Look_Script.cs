using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook_Script : MonoBehaviour
{
    public Transform playerBody;
    public float sensitivity = 100f;  // Unified sensitivity for mouse and controller
    private Player_InputActions inputActions;
    public float rotation_x_axis;
    public float rotation_y_axis;
    public Transform hand;
    private InputAction lookAction;
    private float xRotation = 0f;

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

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Read look input as a Vector2 (e.g., from right stick on gamepad or mouse delta)
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        float mouseX = lookInput.x * sensitivity * Time.deltaTime;
        float mouseY = lookInput.y * sensitivity * Time.deltaTime;
        rotation_x_axis += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        rotation_y_axis += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        rotation_x_axis = Mathf.Clamp(rotation_x_axis, -90.0f, 90.0f);

        hand.localRotation = Quaternion.Euler(-rotation_x_axis, rotation_y_axis, 0);
        // Vertical rotation (looking up and down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal rotation (rotating the player body)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
